﻿using PhotoImport.Models;
using PhotoImport.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PhotoImport
{
    class Program
    {
        // разрешённые расширения
        static string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        // параметры из конфига
        static ImportParams _params = new ImportParams();

        // репозиторий
        private static Repository repository = new Repository();

        /// <summary>
        /// Точка входа
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Import photogallery app");

            // список идентификаторов организаций
            int[] orgIds = repository.GetOrgIds();

            int id = 0;

            do
            {
                Console.WriteLine("Enter organisation id: ");
                try
                {
                    id = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Not corrected input!");
                    continue;
                }
            } while (!orgIds.Contains(id));

            Execute(id);

            Console.WriteLine("Import completed");
            Console.ReadKey();
        }

        /// <summary>
        /// Основная логика
        /// </summary>
        /// <param name="org">Идентификатор организации</param>
        private static void Execute(int org)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // список фотоальбомов
            IEnumerable<PhotoAlbumOld> albums = repository.GetAlbums(org, null);

            // текущий порядковый номер альбома
            int currentAlbumNumber = 0;

            // общее кол-во альбомов
            int countAlbums = albums.Count();

            foreach (var album in albums)
            {
                currentAlbumNumber++;
                try
                {
                    // идентификатор нового альбома
                    Guid id = Guid.NewGuid();

                    // путь до старой директории
                    string oldDirectory = (_params.OldDirectory + album.Org + album.Folder.Replace(".", "")).Replace("/", "\\");
                    
                    ServiceLogger.Debug("{work}", String.Format("oldDirectory: {0}", oldDirectory));

                    if (Directory.Exists(oldDirectory))
                    {
                        // информация по полученной директории
                        DirectoryInfo di = new DirectoryInfo(oldDirectory);

                        // список файлов в директории
                        FileInfo[] fi = di.GetFiles();

                        string datePath = "/" + album.Time.ToString("yyyy_MM") + "/" + album.Time.ToString("dd") + "/";

                        // путь в приложении
                        string localPath = _params.UserFiles + album.Domain + "/Photo" + datePath + id + "/";

                        // путь для сохранения альбома
                        string savePath = _params.NewDirectory + localPath;
                        
                        ServiceLogger.Debug("{work}", String.Format("savePath: {0}", savePath));

                        // создадим папку для нового альбома
                        if (!Directory.Exists(savePath))
                        {
                            DirectoryInfo _di = Directory.CreateDirectory(savePath);
                        }

                        // добавим в бд запись по фотоальбому
                        string _title = album.Name.Length > 512 ?
                            album.Name.Substring(0, 512) : album.Name;

                        PhotoAlbumNew newAlbum = new PhotoAlbumNew
                        {
                            Id = id,
                            Title = _title,
                            Text = album.Description,
                            Date = album.Time,
                            Domain = album.Domain,
                            Path = localPath,
                            Disabled = false,
                            OldId = album.Link
                        };

                        // добавление альбома
                        repository.InsertPhotoAlbum(newAlbum);

                        // номер фотки
                        int count = 0;

                        // превьюшка для альбома
                        string _previewAlbum = null;

                        foreach (var img in fi)
                        {
                            if (allowedExtensions.Contains(img.Extension.ToLower()))
                            {
                                count++;
                                
                                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 70L);
                                
                                // превьюшка для фотки
                                Bitmap imgPrev = (Bitmap)Bitmap.FromFile(img.FullName);
                                imgPrev = Imaging.Resize(imgPrev, 120, 120, "center", "center");
                                imgPrev.Save(savePath + "prev_" + count + ".jpg", myImageCodecInfo, myEncoderParameters);

                                // основное изображение
                                Bitmap imgReal = (Bitmap)Bitmap.FromFile(img.FullName);
                                imgReal = Imaging.Resize(imgReal, 2000, "width");
                                imgReal.Save(savePath + count + ".jpg", myImageCodecInfo, myEncoderParameters);

                                // сохраняем превьюшку для альбома
                                if (count == 1)
                                {
                                    Bitmap albumPrev = (Bitmap)Bitmap.FromFile(img.FullName);
                                    albumPrev = Imaging.Resize(albumPrev, 540, 360, "center", "center");
                                    albumPrev.Save(savePath + "albumprev" + ".jpg", myImageCodecInfo, myEncoderParameters);

                                    _previewAlbum = localPath + "albumprev" + ".jpg";
                                    repository.UpdatePreviewAlbum(id, _previewAlbum);

                                    albumPrev.Dispose();
                                }

                                // новая фотка
                                Photo photo = new Photo
                                {
                                    Id = Guid.NewGuid(),
                                    AlbumId = id,
                                    Title = count + img.Extension,
                                    Date = img.LastWriteTime,
                                    Preview = localPath + "prev_" + count + ".jpg",
                                    Original = localPath + count + ".jpg",
                                    Sort = count
                                };

                                // добавление фото
                                repository.InsertPhotoItem(photo);

                                imgPrev.Dispose();
                                imgReal.Dispose();

                            }
                        }

                        try
                        {
                            //MaterialsUpdate(newAlbum, org);
                        }
                        catch (Exception e)
                        {
                            ServiceLogger.Debug("{work}", String.Format("Error updating materials with album id: {0}", newAlbum.Id));
                            ServiceLogger.Debug("{work}", e.ToString());
                        }
                    }

                    // сколько альбомов обработанно
                    if (currentAlbumNumber % 10 == 0)
                    {
                        ServiceLogger.Debug("{work}", String.Format("Processed {0} of {1}", currentAlbumNumber, countAlbums));
                    }
                }
                catch (Exception e)
                {
                    ServiceLogger.Debug("{work}", String.Format("Error on iteration: {0} of {1}", currentAlbumNumber, countAlbums));
                    ServiceLogger.Debug("{work}", e.ToString());
                }
            }
        }

        /// <summary>
        /// Обновление новостей
        /// </summary>
        private static void MaterialsUpdate(PhotoAlbumNew album, int org)
        {
            // список новостей
            var list = repository.GetMaterials(org);

            // iframe
            string iframe = "<p><iframe width=\"100%\" height=\"150\" src=\"/Photolist/" + album.Id 
                                + " class=\"photo-gallery\" frameborder=\"0\" allowfullscreen=\"allowfullscreen\"></iframe></p>";

            foreach (var item in list)
            {
                item.Text += iframe;
                repository.UpdateMaterial(item);
            }
        }

        /// <summary>
        /// Получает инфу для кодирования
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            foreach (var enc in ImageCodecInfo.GetImageEncoders())
                if (enc.MimeType.ToLower() == mimeType.ToLower())
                    return enc;
            return null;
        }
    }
}
