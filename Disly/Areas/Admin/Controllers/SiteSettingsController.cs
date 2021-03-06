﻿using cms.dbModel.entity;
using Disly.Areas.Admin.Models;
using Disly.Areas.Admin.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Disly.Areas.Admin.Controllers
{
    public class SiteSettingsController : CoreController
    {
        // модель для вывода в представлении
        private SitesViewModel model;


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            model = new SitesViewModel
            {
                Account = AccountInfo,
                Settings = SettingsInfo,
                UserResolution = UserResolutionInfo,
                ControllerName = ControllerName,
                ActionName = ActionName
            };
            if (AccountInfo != null)
            {
                model.Menu = _cmsRepository.getCmsMenu(AccountInfo.Id);
            }

            #region Метатеги
            ViewBag.Title = UserResolutionInfo.Title;
            ViewBag.Description = "";
            ViewBag.KeyWords = "";
            #endregion

            model.Themes = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = "Бирюзовая", Value = "turquoise" },
                    new SelectListItem { Text = "Синяя", Value = "blue" },
                    new SelectListItem { Text = "Фиолетовая", Value = "purple" },
                    new SelectListItem { Text = "Зеленая", Value = "green" },
                    new SelectListItem { Text = "Зеленая2", Value = "green2" },
                    new SelectListItem { Text = "Темно синяя", Value = "blue_dark" },
                }, "Value", "Text");
        }

        // GET: Admin/SiteSettings
        [HttpGet]
        public ActionResult Index()
        {
            model.Item = _cmsRepository.getSite(Domain);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "save-btn")]
        public ActionResult Index(SitesViewModel backModel, HttpPostedFileBase upload, HttpPostedFileBase uploadBack)
        {
            ErrorMessage userMassege = new ErrorMessage();
            userMassege.title = "Информация";

            var old = _cmsRepository.getSite(Domain);

            if (ModelState.IsValid)
            {
                #region Сохранение изображений

                #region сохраняем логотип
                if (upload != null && upload.ContentLength > 0)
                {
                    if (AttachedPicExtAllowed(upload.FileName))//убеждаемся что закачивается изображение
                    {
                        string savePath = Settings.UserFiles + Domain + "/logo/";

                        if (!Directory.Exists(Server.MapPath(savePath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(savePath));
                        }
                        int idx = upload.FileName.LastIndexOf('.');
                        string Title = upload.FileName.Substring(0, idx);

                        string TransTitle = Transliteration.Translit(Title);
                        string FileName = TransTitle + Path.GetExtension(upload.FileName);


                        //сохраняем оригинал
                        upload.SaveAs(Server.MapPath(Path.Combine(savePath, FileName)));
                        string FullName = savePath + FileName;
                        backModel.Item.Logo = new Photo {Url= FullName };
                    }
                    //var photo = imageWorker(upload, 1);
                    //if (photo == null)
                    //    return View("Item", model);

                    
                } 
                #endregion

                #region Изображени под слайдером
                if (uploadBack != null && uploadBack.ContentLength > 0)
                {
                    string SavePath = Settings.UserFiles + Domain + "/logo/";
                    int idx = uploadBack.FileName.LastIndexOf('.');
                    string Title = uploadBack.FileName.Substring(0, idx);
                    string TransTitle = Transliteration.Translit(Title);
                    string FileName = TransTitle + Path.GetExtension(uploadBack.FileName);


                    string FullName = SavePath + FileName;
                    if (!Directory.Exists(Server.MapPath(SavePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(SavePath));
                    }
                    uploadBack.SaveAs(Server.MapPath(Path.Combine(SavePath, FileName)));
                    backModel.Item.BackGroundImg = new Photo { Url = FullName };
                }

                #endregion


                #endregion

                _cmsRepository.updateSiteInfo(backModel.Item);
                userMassege.info = "Запись обновлена";
                userMassege.buttons = new ErrorMassegeBtn[]
            {
                    new ErrorMassegeBtn { url = "/Admin/sitesettings", text = "ок"}
            };
            }
            else
            {

                userMassege.info = "Ошибка в заполнении формы. Поля в которых допушены ошибки - помечены цветом.";

                userMassege.buttons = new ErrorMassegeBtn[]
                {
                    new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                };
            }
            model.ErrorInfo = userMassege;
            model.Item = _cmsRepository.getSite(Domain);
            return View(model);
        }

        /// <summary>
        /// Сохранение изображений
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Photo imageWorker(HttpPostedFileBase upload, int type)
        {
            // путь для сохранения изображения
            string savePath = Settings.UserFiles + Domain + Settings.LogoDir;

            int width = 0; // ширина
            int height = 0; // высота

            switch (type)
            {
                case 1:
                    width = 80;
                    break;
                case 2:
                    height = 290;
                    break;
            }

            // var validExtension = (!string.IsNullOrEmpty(Settings.PicTypes)) ? Settings.PicTypes.Split(',') : "jpg,jpeg,png,gif".Split(',');
            //if (!validExtension.Contains(fileExtension.Replace(".", "")))
            if (!AttachedPicExtAllowed(upload.FileName))
            {
                return null;
            }

            string fileName = Transliteration.Translit(upload.FileName.Substring(0, upload.FileName.LastIndexOf(".")));

            string fileExtension = upload.FileName.Substring(upload.FileName.LastIndexOf(".")).ToLower();

            Photo photoNew = new Photo()
            {
                Name = fileName + fileExtension,
                Size = Files.FileAnliz.SizeFromUpload(upload),
                Url = Files.SaveImageResizeRename(upload, savePath, fileName, width, height)
            };

            return photoNew;
        }
    }
}