﻿using System;
using System.Collections.Generic;
using System.Linq;
using cms.dbModel;
using cms.dbModel.entity;
using cms.dbase.models;
using LinqToDB;

namespace cms.dbase
{
    /// <summary>
    /// Репозиторий для работы с организациями
    /// </summary>
    public partial class cmsRepository : abstract_cmsRepository
    {

        /// <summary>
        /// Строим запрос на основе фильтра
        /// </summary>
        /// <param name="db"></param>
        /// <param name="filtr"></param>
        /// <returns></returns>
        private IQueryable<content_orgs> queryByOrgFilter(CMSdb db, OrgFilter filtr)
        {
            if (filtr == null)
                throw new Exception("cmsRepository > queryByOrgFilter: Filter is null");

            var query = db.content_orgss
                               .OrderBy(o => o.n_sort)
                               .AsQueryable();

            if (!string.IsNullOrEmpty(filtr.Domain))
            {
#warning  Дописать потом что должно происходить!!!
            }

            if (filtr.Disabled != null && (bool)filtr.Disabled)
                query = query.Where(w => w.b_disabled);

            if (!string.IsNullOrEmpty(filtr.SearchText))
            {
                query = query.Where(w => w.c_title.Contains(filtr.SearchText));
            }

            if (filtr.RelId.HasValue && filtr.RelId.Value != Guid.Empty)
            {
                //В таблице ищем связи оранизация - контент (новость/событие)
                var objctLinks = db.content_content_links
                    .Where(s => s.f_content == filtr.RelId.Value)
                    .Where(s => s.f_link_type == "org")
                    .Where(s => s.f_content_type == filtr.RelType.ToString().ToLower());

                if (!objctLinks.Any())
                    query = query.Where(o => o.id == Guid.Empty); //Делаем заранее ложный запрос
                else
                {
                    var objctsId = objctLinks.Select(o => o.f_link);
                    query = query.Where(o => objctsId.Contains(o.id));
                }
            }

            if(filtr.PeopleId.HasValue && filtr.PeopleId.Value != Guid.Empty)
            {
                query = query.Where(p => p.contentorgpeoplelinks.Any(s => s.f_people == filtr.PeopleId.Value));
            }

            return query;
        }

        private bool ContentLinkExists(Guid contentId, ContentType contentType, Guid linkId, ContentLinkType linkType)
        {
            using (var db = new CMSdb(_context))
            {
                var links = db.content_content_links
                    .Where(s => s.f_content == contentId)
                    .Where(s => s.f_content_type == contentType.ToString().ToLower())
                    .Where(s => s.f_link == linkId)
                    .Where(s => s.f_link_type == linkType.ToString().ToLower())
                    .Select(s => s.f_link);
                if (links.Any())
                    return true;

                return false;
            }
        }

        private bool ContentLinkOrigin(Guid contentId, ContentType contentType, Guid linkId, ContentLinkType linkType)
        {
            using (var db = new CMSdb(_context))
            {
                var links = db.content_content_links
                    .Where(s => s.f_content == contentId)
                    .Where(s => s.f_content_type == contentType.ToString().ToLower())
                    .Where(s => s.f_link == linkId)
                    .Where(s => s.f_link_type == linkType.ToString().ToLower())
                    .Where(s => s.b_origin == true)
                    .Select(s => s.f_link);
                if (links.Any())
                    return true;

                return false;
            }
        }

        public int getCountOrg()
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgss;
                if (query.Any()) return query.Count();
                return 0;
            }
        }

        /// <summary>
        /// Постраничный список организаций
        /// </summary>
        /// <param name="filtr"></param>
        /// <returns></returns>
        public override OrgsList getOrgsList(OrgFilter filtr)
        {
            using (var db = new CMSdb(_context))
            {
                //Получаем сформированный запрос по фильтру
                var query = queryByOrgFilter(db, filtr);

                var itemCount = query.Count();

                var data = query.Select(s => new OrgsModel()
                {
                    Id = s.id,
                    Title = s.c_title,
                    Sort = s.n_sort,
                    Types = s.contentorgstypeslinkorgs.Select(t => t.f_type).ToArray()
                });
                if (!data.Any())
                    return null;

                return new OrgsList()
                {
                    Data = data.ToArray(),
                    Pager = new Pager()
                    {
                        Page = filtr.Page,
                        Size = filtr.Size,
                        ItemsCount = itemCount,
                        //PageCount = (itemCount % filtr.Size > 0) ? (itemCount / filtr.Size) + 1 : itemCount / filtr.Size
                    }
                };
            }
        }

        /// <summary>
        /// Получаем список организаций по фильтру
        /// </summary>
        /// <param name="filtr">Фильтр</param>
        /// <returns></returns> 
        public override OrgsModel[] getOrgs(OrgFilter filtr)
        {
            using (var db = new CMSdb(_context))
            {
                //Получаем сформированный запрос по фильтру
                var query = queryByOrgFilter(db, filtr);

                //data.OrderBy(o => o.n_sort); ХЗ почему эта строка нормально не сортирует

                if (filtr.Except.HasValue && filtr.Except.Value != Guid.Empty)
                    query = query.Where(w => w.id != filtr.Except.Value);

                var data = query.Select(s => new OrgsModel()
                {
                    Id = s.id,
                    Title = s.c_title,
                    Sort = s.n_sort,
                    Address = s.c_adress,
                    Types = s.contentorgstypeslinkorgs.Select(t => t.f_type).ToArray()
                });

                if (data.Any())
                    return data.ToArray();

                return null;
            }
        }

        /// <summary>
        /// Получаем полный список доступных организаций с отмеченными значениями(для кот есть связи для объекта)
        /// </summary>
        /// <param name="filtr">Фильтр</param>
        /// <returns></returns>
        public override OrgsShortModel[] getOrgsListWhithChekedFor(OrgFilter filtr)
        {
            using (var db = new CMSdb(_context))
            {
                //Получаем сформированный запрос по фильтру
                var query = db.content_orgss.AsQueryable();

                if (!string.IsNullOrEmpty(filtr.Domain))
                {
                    //query = query;
                }
                if (filtr.RelId.HasValue && filtr.RelId.Value != Guid.Empty)
                {
                    var data = query
                        .Select(s => new OrgsShortModel()
                        {
                            Id = s.id,
                            Title = !string.IsNullOrEmpty(s.c_title_short)? s.c_title_short: s.c_title,
                            Types = (s.contentorgstypeslinkorgs.Select(t => t.f_type).Any()) ?
                                s.contentorgstypeslinkorgs.Select(t => t.f_type).ToArray() : null,
                            Checked = ContentLinkExists(filtr.RelId.Value, filtr.RelType, s.id, ContentLinkType.ORG),
                            Origin = ContentLinkOrigin(filtr.RelId.Value, filtr.RelType, s.id, ContentLinkType.ORG)
                        });

                    if (data.Any())
                        return data.ToArray();

                }

                return null;
            }
        }

        /// <summary>
        /// Получаем организацию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public override OrgsModel getOrgItem(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                Guid[] types = null;
                var getTypes = getOrgTypesList(new OrgTypeFilter { OrgId = id });
                if (getTypes != null)
                    types = getTypes.Select(t => t.Id).ToArray();

                var querySite = db.cms_sitess.Where(w => w.f_content == id).Select(s => s.id).SingleOrDefault();
                String siteGuid = (querySite != null) ? querySite.ToString() : String.Empty;
                
                var services = getOrgMedicalServicesLinks(id);

                var data = db.content_orgss
                    .Where(w => w.id.Equals(id) || w.f_guid.Equals(id))
                    .Select(s => new OrgsModel
                    {
                        Id = s.id,
                        Title = s.c_title,
                        ShortTitle = s.c_title_short,
                        Phone = s.c_phone,
                        PhoneReception = s.c_phone_reception,
                        Fax = s.c_fax,
                        Email = s.c_email,
                        ExtUrl = s.c_www,
                        DirecorPost = s.c_director_post,
                        DirectorF = s.f_director,
                        Contacts = s.c_contacts,
                        Address = s.c_adress,
                        GeopointX = s.n_geopoint_x,
                        GeopointY = s.n_geopoint_y,
                        Structure = getStructureList(s.id),
                        Administrativ = getAdministrativList(s.id),
                        Oid = s.f_oid,
                        Disabled = s.b_disabled,
                        Types = types,
                        Services = services,
                        Affiliation = s.f_department_affiliation,
                        SiteGuid =(siteGuid!=String.Empty)? siteGuid: String.Empty,
                        Logo = new Photo
                        {
                            Url = s.c_logo
                        }
                    });

                if (!data.Any()) return null;
                else return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Добавляем организацию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="model">Организация</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool insertOrg(Guid id, OrgsModel model)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_orgss.Where(w => w.id == id);
                    if (!data.Any())
                    {
                        int MaxSort = 0;
                        try
                        {
                            MaxSort = db.content_orgss.Max(m => m.n_sort);
                        }
                        catch { }
                        MaxSort++;
                        string _Logo = (model.Logo != null) ? model.Logo.Url : null;
                        db.content_orgss
                            .Value(s => s.id, id)
                            .Value(s => s.n_sort, MaxSort)
                            .Value(s => s.c_title, model.Title)
                            .Value(s => s.c_title_short, model.ShortTitle)
                            .Value(s => s.c_phone, model.Phone)
                            .Value(s => s.c_phone_reception, model.PhoneReception)
                            .Value(s => s.c_fax, model.Fax)
                            .Value(s => s.c_email, model.Email)
                            .Value(s=> s.c_www, model.ExtUrl)
                            .Value(s => s.c_director_post, model.DirecorPost)
                            .Value(s => s.f_director, model.DirectorF)
                            .Value(s => s.c_contacts, model.Contacts)
                            .Value(s => s.c_adress, model.Address)
                            .Value(s => s.n_geopoint_x, model.GeopointX)
                            .Value(s => s.n_geopoint_y, model.GeopointY)
                            .Value(s => s.f_oid, model.Oid)
                            .Value(s => s.f_department_affiliation, model.Affiliation)
                            .Value(s => s.c_logo, _Logo)
                            .Value(s => s.b_disabled, model.Disabled)
                            .Insert();

                        // обновляем типы мед. учреждений
                        if (model.Types != null)
                        {
                            // удаляем старые типы
                            db.content_orgs_types_links.Where(w => w.f_org.Equals(id)).Delete();

                            foreach (var t in model.Types)
                            {
                                var maxSortQuery = db.content_orgs_types_links
                                    .Where(w => w.f_org.Equals(id)).Select(s => s.n_sort);

                                int maxSort = maxSortQuery.Any() ? maxSortQuery.Max() : 0;

                                db.content_orgs_types_links
                                    .Value(v => v.f_org, id)
                                    .Value(v => v.f_type, t)
                                    .Value(v => v.n_sort, maxSort + 1)
                                    .Insert();
                            }
                        }

                        // обновляем медицинские услуги
                        if (model.Services != null)
                        {
                            // удаляем старые услуги
                            db.content_orgs_medical_services_linkss
                                .Where(w => w.f_org.Equals(id)).Delete();

                            foreach (var s in model.Services)
                            {
                                db.content_orgs_medical_services_linkss
                                    .Value(v => v.f_org, id)
                                    .Value(v => v.f_medical_service, s)
                                    .Insert();
                            }
                        }

                        //логирование
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Orgs,
                            Action = LogAction.insert,
                            PageId = id,
                            PageName = model.Title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Обновляем организацию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="model">Организация</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool updateOrg(Guid id, OrgsModel model)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_orgss.Where(w => w.id == id);
                    if (data.Any())
                    {
                        var url = string.Empty;
                        if ((model.Logo != null))
                        {
                            url = model.Logo.Url;
                        }
                        else
                        {
                            url = null;
                        }
                             

                        data
                            .Set(s => s.c_title, model.Title)
                            .Set(s => s.c_title_short, model.ShortTitle)
                            .Set(s => s.c_phone, model.Phone)
                            .Set(s => s.c_phone_reception, model.PhoneReception)
                            .Set(s => s.c_fax, model.Fax)
                            .Set(s => s.c_email, model.Email)
                            .Set(s => s.c_www, model.ExtUrl)
                            .Set(s => s.c_director_post, model.DirecorPost)
                            .Set(s => s.f_director, model.DirectorF)
                            .Set(s => s.c_contacts, model.Contacts)
                            .Set(s => s.c_adress, model.Address)
                            .Set(s => s.n_geopoint_x, model.GeopointX)
                            .Set(s => s.n_geopoint_y, model.GeopointY)
                            .Set(s => s.f_oid, model.Oid)
                            .Set(s => s.f_department_affiliation, model.Affiliation)
                            .Set(s => s.c_logo, url)
                            .Set(s => s.b_disabled, model.Disabled)
                            .Update();

                        // обновляем типы мед. учреждений
                        if (model.Types != null)
                        {
                            // удаляем старые типы
                            db.content_orgs_types_links.Where(w => w.f_org.Equals(id)).Delete();

                            foreach (var t in model.Types)
                            {
                                var maxSortQuery = db.content_orgs_types_links
                                    .Where(w => w.f_org.Equals(id)).Select(s => s.n_sort);

                                int maxSort = maxSortQuery.Any() ? maxSortQuery.Max() : 0;

                                db.content_orgs_types_links
                                    .Value(v => v.f_org, id)
                                    .Value(v => v.f_type, t)
                                    .Value(v => v.n_sort, maxSort + 1)
                                    .Insert();
                            }
                        }

                        // обновляем медицинские услуги
                        if (model.Services != null)
                        {
                            // удаляем старые услуги
                            db.content_orgs_medical_services_linkss
                                .Where(w => w.f_org.Equals(id)).Delete();

                            foreach (var s in model.Services)
                            {
                                db.content_orgs_medical_services_linkss
                                    .Value(v => v.f_org, id)
                                    .Value(v => v.f_medical_service, s)
                                    .Insert();
                            }
                        }

                        //логирование
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Orgs,
                            Action = LogAction.update,
                            PageId = id,
                            PageName = model.Title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                    return false;
                }

            }
        }

        /// <summary>
        /// Удаляем организацию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool deleteOrg(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_orgss.Where(w => w.id == id);
                    if (data.Any())
                    {

                        string logTitle = data.FirstOrDefault().c_title;
                        int ThisSort = data.FirstOrDefault().n_sort;
                        db.content_orgss.Where(w => w.n_sort > ThisSort).Set(p => p.n_sort, p => p.n_sort - 1).Update();//смещение n_sort
                        data.Delete();

                        //логирование
                        //insertLog(UserId, IP, "delete", id, String.Empty, "Orgs", logTitle);
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Orgs,
                            Action = LogAction.delete,
                            PageId = id,
                            PageName = logTitle,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Сортировка организаций
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="new_num">Новое значение сортировки</param>
        /// <returns></returns>
        public override bool sortOrgs(Guid id, int new_num)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var actual_num = db.content_orgss.Where(w => w.id == id).FirstOrDefault().n_sort;
                    if (new_num > actual_num)
                    {
                        db.content_orgss
                            .Where(w => (w.n_sort > actual_num && w.n_sort <= new_num))
                            .Set(p => p.n_sort, p => p.n_sort - 1)
                            .Update();
                    }
                    else
                    {
                        db.content_orgss
                            .Where(w => w.n_sort < actual_num && w.n_sort >= new_num)
                            .Set(p => p.n_sort, p => p.n_sort + 1)
                            .Update();
                    }
                    db.content_orgss
                        .Where(w => w.id == id)
                        .Set(s => s.n_sort, new_num)
                        .Update();

                    tran.Commit();

                    return true;
                }
            }
        }

        /// <summary>
        /// Получаем список структурных подразделений
        /// </summary>
        /// <param name="id">Организация</param>
        /// <returns></returns>
        public override StructureModel[] getStructureList(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_org_structures.Where(w => w.f_ord == id).OrderBy(o => o.n_sort);
                if (data.Any())
                {
                    var List = data
                                .Select(s => new StructureModel()
                                {
                                    Id = s.id,
                                    Title = s.c_title,
                                    Ovp = s.b_ovp
                                });
                    return List.ToArray();
                }
                return null;
            }
        }


        /// <summary>
        /// Получаем структурное подразделение
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public override StructureModel getStructure(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_org_structures.Where(w => w.id == id);
                if (data.Any())
                {
                    return data.Select(s => new StructureModel
                    {
                        Id = s.id,
                        OrgId = s.f_ord,
                        Title = s.c_title,
                        TitleShort=s.c_title_short,
                        Adress = s.c_adress,
                        GeopointX = s.n_geopoint_x,
                        GeopointY = s.n_geopoint_y,
                        Phone = s.c_phone,
                        PhoneReception = s.c_phone_reception,
                        Fax = s.c_fax,
                        Email = s.c_email,
                        Routes = s.c_routes,
                        Schedule = s.c_schedule,
                        DirecorPost = s.c_director_post,
                        Ovp = s.b_ovp,
                        Departments = getDepartmentsList(s.id),
                        DopAddres=getDopAddres(s.id)

                        //f_direcor
                    }).FirstOrDefault();
                }
                return null;
            }
        }

        /// <summary>
        /// Добавляем структуру
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="OrgId">Организация</param>
        /// <param name="insert">Структура</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool insertStructure(Guid id, Guid OrgId, StructureModel insert)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    int MaxSort = db.content_org_structures
                    .Where(w => w.f_ord == OrgId)
                    .Any() ? db.content_org_structures.Where(w => w.f_ord == OrgId).Max(m => m.n_sort) : 0;
                    MaxSort++;

                    db.content_org_structures
                      .Value(v => v.id, id)
                      .Value(v => v.n_sort, MaxSort)
                      .Value(v => v.f_ord, OrgId)
                      .Value(v => v.c_title, insert.Title)
                      .Value(v => v.c_adress, insert.Adress)
                      .Value(v => v.n_geopoint_x, insert.GeopointX)
                      .Value(v => v.n_geopoint_y, insert.GeopointY)
                      .Value(v => v.c_phone, insert.Phone)
                      .Value(v => v.c_phone_reception, insert.PhoneReception)
                      .Value(v => v.c_fax, insert.Fax)
                      .Value(v => v.c_email, insert.Email)
                      .Value(v => v.c_routes, insert.Routes)
                      .Value(v => v.c_schedule, insert.Schedule)
                      .Value(v => v.c_director_post, insert.DirecorPost)
                      .Value(v => v.f_director, insert.DirectorF)
                      .Insert();

                    //логирование
                    //insertLog(UserId, IP, "insert", id, String.Empty, "Orgs", insert.Title);
                    var log = new LogModel()
                    {
                        Site = _domain,
                        Section = LogSection.Orgs,
                        Action = LogAction.insert,
                        PageId = id,
                        PageName = insert.Title,
                        UserId = _currentUserId,
                        IP = _ip,
                    };
                    insertLog(log);

                    tran.Commit();
                    return true;
                }
            }
        }

        /// <summary>
        /// Обновляем структуру
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="insert">Структура</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool updateStructure(Guid id, StructureModel insert)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_org_structures.Where(w => w.id == id);
                    if (data.Any())
                    {
                        data
                        .Set(v => v.c_title, insert.Title)
                        .Set(v => v.c_adress, insert.Adress)
                        .Set(v => v.n_geopoint_x, insert.GeopointX)
                        .Set(v => v.n_geopoint_y, insert.GeopointY)
                        .Set(v => v.c_phone, insert.Phone)
                        .Set(v => v.c_phone_reception, insert.PhoneReception)
                        .Set(v => v.c_fax, insert.Fax)
                        .Set(v => v.c_email, insert.Email)
                        .Set(v => v.c_routes, insert.Routes)
                        .Set(v => v.c_schedule, insert.Schedule)
                        .Set(v => v.c_director_post, insert.DirecorPost)
                        .Set(v => v.f_director, insert.DirectorF)
                        .Update();

                        //логирование
                        //insertLog(UserId, IP, "update", id, String.Empty, "Orgs", insert.Title);
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Orgs,
                            Action = LogAction.update,
                            PageId = id,
                            PageName = insert.Title,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Удаляем структуру
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool deleteStructure(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_org_structures.Where(w => w.id == id);
                    Guid IdOrg = data.FirstOrDefault().f_ord;
                    int ThisSort = data.FirstOrDefault().n_sort;
                    string logTitle = data.FirstOrDefault().c_title;
                    if (data.Any())
                    {
                        data.Delete();
                        db.content_org_structures.Where(w => w.f_ord == IdOrg && w.n_sort > ThisSort)
                            .Set(p => p.n_sort, p => p.n_sort - 1)
                            .Update();//смещение n_sort

                        //логирование
                        //insertLog(UserId, IP, "delete", id, String.Empty, "Orgs", logTitle);
                        var log = new LogModel()
                        {
                            Site = _domain,
                            Section = LogSection.Orgs,
                            Action = LogAction.delete,
                            PageId = id,
                            PageName = logTitle,
                            UserId = _currentUserId,
                            IP = _ip,
                        };
                        insertLog(log);

                        tran.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Сортировка структуры
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="new_num">Новое значение сортировки</param>
        /// <returns></returns>
        public override bool sortStructure(Guid id, int new_num)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var thisdata = db.content_org_structures.Where(w => w.id == id).FirstOrDefault();
                    int actual_num = thisdata.n_sort;
                    Guid OrgId = thisdata.f_ord;
                    if (new_num > actual_num)
                    {
                        db.content_org_structures
                            .Where(w => w.f_ord == OrgId && w.n_sort > actual_num && w.n_sort <= new_num)
                            .Set(p => p.n_sort, p => p.n_sort - 1)
                            .Update();
                    }
                    else
                    {
                        db.content_org_structures
                            .Where(w => w.f_ord == OrgId && w.n_sort < actual_num && w.n_sort >= new_num)
                            .Set(p => p.n_sort, p => p.n_sort + 1)
                            .Update();
                    }
                    db.content_org_structures
                        .Where(w => w.f_ord == OrgId && w.id == id)
                        .Set(s => s.n_sort, new_num)
                        .Update();

                    tran.Commit();
                    return true;
                }
            }
        }


        public override bool addAddressStructure(DopAddres addres)
        {
            using (var db = new CMSdb(_context))
            {
                db.content_org_structure_adresss
                  .Value(v => v.c_adress, addres.Address)
                  .Value(v => v.n_geopoint_x, addres.GeopointX)
                  .Value(v => v.n_geopoint_y, addres.GeopointY)
                  .Value(v => v.f_org_structure, addres.IdStructure)
                  .Value(v => v.c_title, addres.Title)
                  .Insert();
                return true;
            }
        }
        public override DopAddres[] getDopAddres(Guid StrucId)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_org_structure_adresss.Where(w => w.f_org_structure == StrucId);
                if (query.Any())
                {
                    return query
                            .OrderBy(o=>o.c_title)
                            .Select(s => new DopAddres()
                            {
                                Id=s.id,
                                Address=s.c_adress,
                                Title=s.c_title
                            }).ToArray();
                }
                return null;
            }
        }
        public override bool delAddressStructure(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_org_structure_adresss.Where(w => w.id == id);
                if (data.Any())
                {                    
                    data.Delete();
                }
                return true;
            }
        }

        /// <summary>
        /// Добавляем ОВП
        /// </summary>        
        /// <returns></returns>
        public override bool insOvp(Guid IdStructure, Guid OrgId, StructureModel insertStructure)
        {
            using (var db = new CMSdb(_context))
            {
                content_org_structure cdStructur = db.content_org_structures.Where(w => w.id == IdStructure).SingleOrDefault();
                if (cdStructur != null)
                {
                    throw new Exception("Запись с таким Id уже существует");
                }
                int MaxSort = 0;
                try
                {
                    MaxSort = db.content_org_structures.Where(w => w.f_ord == OrgId).Max(m => m.n_sort);
                }
                catch { }
                MaxSort++;

                cdStructur = new content_org_structure
                {
                    id = IdStructure,
                    f_ord = OrgId,
                    n_sort = MaxSort,
                    c_title = insertStructure.Title, 
                    c_title_short= insertStructure.TitleShort,
                    c_adress = insertStructure.Adress,
                    c_phone = insertStructure.PhoneReception,
                    c_fax = insertStructure.Fax,
                    c_email = insertStructure.Email,
                    n_geopoint_x = insertStructure.GeopointX,
                    n_geopoint_y = insertStructure.GeopointY,
                    c_schedule = insertStructure.Schedule,
                    c_routes = insertStructure.Routes,
                    c_director_post = insertStructure.DirecorPost,
                    f_director = insertStructure.DirectorF,
                    b_ovp = true
                };

                content_departments cdDepart = new content_departments
                {
                    id = Guid.NewGuid(),
                    n_sort = 1,
                    f_structure = IdStructure,
                    c_title = insertStructure.Title,
                    c_adress = insertStructure.Adress
                };
                string logTitle = insertStructure.Title;
                using (var tran = db.BeginTransaction())
                {
                    db.Insert(cdStructur);
                    db.Insert(cdDepart);
                    tran.Commit();

                    //логирование
                    //insertLog(UserId, IP, "insert", IdStructure, String.Empty, "Orgs", logTitle);
                    var log = new LogModel()
                    {
                        Site = _domain,
                        Section = LogSection.Orgs,
                        Action = LogAction.insert,
                        PageId = IdStructure,
                        PageName = logTitle,
                        UserId = _currentUserId,
                        IP = _ip,
                    };
                    insertLog(log);

                    return true;
                }
            }
        }

        /// <summary>
        /// Обновляем ОВП
        /// </summary>
        /// <param name="IdStructure">Идентификатор структура</param>
        /// <param name="updStructure">Структура</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool setOvp(Guid IdStructure, StructureModel updStructure, Departments updDepart)
        {
            using (var db = new CMSdb(_context))
            {
                content_org_structure cdStructur = db.content_org_structures.Where(w => w.id == IdStructure).SingleOrDefault();
                if (cdStructur == null)
                {
                    throw new Exception("Запись с таким Id не существует");
                }
                cdStructur.c_title = updStructure.Title;
                cdStructur.c_title_short = updStructure.TitleShort;
                cdStructur.c_adress = updStructure.Adress;
                cdStructur.n_geopoint_x = updStructure.GeopointX;
                cdStructur.n_geopoint_y = updStructure.GeopointY;
                cdStructur.c_phone = updStructure.PhoneReception;
                cdStructur.c_fax = updStructure.Fax;
                cdStructur.c_email = updStructure.Email;
                cdStructur.c_schedule = updStructure.Schedule;
                cdStructur.c_routes = updStructure.Routes;
                cdStructur.c_director_post = updStructure.DirecorPost;
                cdStructur.f_director = updStructure.DirectorF;

                content_departments cdDepart = db.content_departmentss.Where(w => w.f_structure == IdStructure).FirstOrDefault();
                if (cdDepart == null)
                {
                    throw new Exception("У данного ОВП в базе не существует отдела");
                }
                cdDepart.c_title = updStructure.Title;
                cdDepart.c_adress = updDepart.Text;                
                using (var tran = db.BeginTransaction())
                {
                    db.Update(cdStructur);
                    db.Update(cdDepart);
                    tran.Commit();
                    //логирование
                    //insertLog(UserId, IP, "update", IdStructure, String.Empty, "Orgs", updStructure.Title);
                    var log = new LogModel()
                    {
                        Site = _domain,
                        Section = LogSection.Orgs,
                        Action = LogAction.update,
                        PageId = IdStructure,
                        PageName = updStructure.Title,
                        UserId = _currentUserId,
                        IP = _ip,
                    };
                    insertLog(log);

                    return true;
                }
            }
        }

        /// <summary>
        /// Отделение
        /// </summary>
        /// <param name="id">идентификатор структурного подразделения</param>
        /// <returns>отделения входящие в струкутрное подразделенеи</returns>
        public override Departments[] getDepartmentsList(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_departmentss.Where(w => w.f_structure == id).OrderBy(o => o.n_sort);
                if (data.Any())
                {
                    return data.Select(s => new Departments()
                    {
                        Id = s.id,
                        Title = s.c_title
                    }).ToArray();
                }
                return null;
            }
        }

        /// <summary>
        /// Возвращает департамент, если подходящего значения нет. то возвращает пустую модель Departments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Departments getDepartamentItem(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_departmentss.Where(w => w.id == id);
                if (data.Any())
                {
                    return data.Select(s => new Departments
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Text = s.c_adress,
                        StructureF = s.f_structure,
                        Phones = getDepartmentsPhone(s.id),
                        Peoples = getPeopleDepartment(s.id)

                    }).First();
                }
                return null;
            }
        }

        /// <summary>
        /// Телефонные номера департамента
        /// </summary>
        /// <param name="id">идентификатор отделения</param>
        /// <returns></returns>
        public override DepartmentsPhone[] getDepartmentsPhone(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_departments_phones.Where(w => w.f_department == id);
                if (data.Any())
                    return data
                            .Select(s => new DepartmentsPhone()
                            {
                                Id = s.id,
                                Label = s.c_key,
                                Value = s.c_val
                            })
                                        .ToArray();
            }
            return null;
        }

        /// <summary>
        /// Хлебные крошки раздела Orgs
        /// </summary>
        /// <param name="id">идентификатор элемента относительно которого нужно построить хлебные крошки</param>
        /// <param name="type">тип раздела orgs- в качество него скорее всего будем брать action name</param>
        /// <returns></returns>
        public override Breadcrumbs[] getBreadCrumbOrgs(Guid id, string type)
        {
            using (var db = new CMSdb(_context))
            {
                var MyBread = new Stack<Breadcrumbs>();
                MyBread.Push(new Breadcrumbs
                {
                    Title = "Организации",
                    Url = "/admin/orgs/"
                });
                #region item
                if (type == "item")
                {
                    var data = db.content_departmentss.Where(w => w.id == id).FirstOrDefault();
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = data.c_title,
                        Url = "/admin/orgs/item/" + data.id
                    });
                }
                #endregion
                #region structure
                if (type == "structure")
                {
                    var data = db.content_org_structures.Where(w => w.id == id).FirstOrDefault();
                    var ParentStructure = db.content_orgss.Where(w => w.id == data.f_ord).FirstOrDefault();

                    MyBread.Push(new Breadcrumbs
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/item/" + ParentStructure.id
                    });
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = data.c_title,
                        Url = "/admin/orgs/structure/" + data.id
                    });
                }
                #endregion
                #region administrativ
                if (type == "administrativ")
                {
                    var data = db.content_orgs_adminstrativs.Where(w => w.id == id).FirstOrDefault();
                    var Parent = db.content_orgss.Where(w => w.id == data.f_org).FirstOrDefault();

                    MyBread.Push(new Breadcrumbs
                    {
                        Title = Parent.c_title,
                        Url = "/admin/orgs/item/" + Parent.id
                    });
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = data.c_surname + " " + data.c_name,
                        Url = "/admin/orgs/structure/" + data.id
                    });
                }
                #endregion
                #region ovp
                if (type == "ovp")
                {
                    var data = db.content_org_structures.Where(w => w.id == id).FirstOrDefault();
                    var ParentStructure = db.content_orgss.Where(w => w.id == data.f_ord).FirstOrDefault();

                    MyBread.Push(new Breadcrumbs
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/item/" + ParentStructure.id
                    });
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = data.c_title,
                        Url = "/admin/orgs/ovp/" + data.id
                    });
                }
                #endregion
                #region department
                if (type == "department")
                {
                    var data = db.content_departmentss.Where(w => w.id == id).FirstOrDefault();
                    var ParentStructure = db.content_org_structures.Where(w => w.id == data.f_structure).FirstOrDefault();
                    var ParentOrg = db.content_orgss.Where(w => w.id == ParentStructure.f_ord).FirstOrDefault();

                    MyBread.Push(new Breadcrumbs
                    {
                        Title = ParentOrg.c_title,
                        Url = "/admin/orgs/item/" + ParentOrg.id
                    });
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/structure/" + ParentStructure.id
                    });
                    MyBread.Push(new Breadcrumbs
                    {
                        Title = data.c_title,
                        Url = "/admin/orgs/department/" + data.id
                    });
                }
                #endregion
                return MyBread.Reverse().ToArray();
            }
        }

        /// <summary>
        /// Добавляет значение в список телефонов отдела
        /// </summary>
        /// <param name="idDepart"></param>
        /// <param name="Label"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override bool insDepartmentsPhone(Guid idDepart, string Label, string Value)
        {
            using (var db = new CMSdb(_context))
            {
                int Sort = 1;
                var data = db.content_departments_phones.Where(w => w.f_department == idDepart);
                if (data.Any()) Sort = data.Max(m => m.n_sort) + 1;
                db.content_departments_phones
                   .Value(v => v.f_department, idDepart)
                   .Value(v => v.c_key, Label)
                   .Value(v => v.c_val, Value)
                   .Value(v => v.n_sort, Sort)
                   .Insert();

                //логирование
                //insertLog(UserId, IP, "insert_phone_depart", idDepart, String.Empty, "Orgs", Label);
                var log = new LogModel()
                {
                    Site = _domain,
                    Section = LogSection.Orgs,
                    Action = LogAction.insert,
                    PageId = idDepart,
                    PageName = Label,
                    UserId = _currentUserId,
                    IP = _ip,
                };
                insertLog(log);
                return true;
            }
        }

        /// <summary>
        /// Удаление телефона
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool delDepartmentsPhone(int id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_departments_phones.Where(w => w.id == id);
                if (data.Any())
                {
                    string logtitle = data.FirstOrDefault().c_val;
                    data.Delete();
                }
            }
            return true;
        }

        /// <summary>
        /// Получаем список сотрудников по департаменту
        /// </summary>
        /// <param name="idDepart">Департамент</param>
        /// <returns></returns>
        public override PeopleModel[] getPeopleDepartment(Guid idDepart)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_sv_employees_departments
                             .Where(w => w.f_department == idDepart)
                             .Select(s => new PeopleModel()
                             {
                                 Id = s.f_employee,
                                 FIO = s.c_surname + " " + s.c_name + " " + s.c_patronymic,
                                 IdLinkOrg = s.idOrgLink,
                                 Post= s.c_post,
                                 Status = (s.c_status == "boss") ? "Руководитель" : (s.c_status == "sister") ? "Старшая медсестра" : null
                             });
                if (data.Any()) return data.ToArray();
                return null;
            }
        }

        /// <summary>
        /// Добавляем департамент
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="Structure">Структура</param>
        /// <param name="insert">Департамент</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool insDepartament(Guid id, Guid Structure, Departments insert)
        {
            using (var db = new CMSdb(_context))
            {
                content_departments cdDepart = db.content_departmentss
                                                 .Where(p => p.id == id)
                                                 .SingleOrDefault();
                if (cdDepart != null)
                {
                    throw new Exception("Запись с таким Id уже существует");
                }
                int MaxSort = 0;
                try
                {
                    MaxSort = db.content_departmentss.Where(w => w.f_structure == Structure).Max(m => m.n_sort);
                }
                catch { }
                MaxSort++;

                cdDepart = new content_departments
                {
                    id = id,
                    f_structure = Structure,
                    c_title = insert.Title,
                    c_adress = insert.Text,
                    n_sort = MaxSort
                };

                using (var tran = db.BeginTransaction())
                {
                    db.Insert(cdDepart);
                    tran.Commit();

                    //логирование
                    // insertLog(UserId, IP, "insert", id, String.Empty, "Site", insert.Title);
                    var log = new LogModel()
                    {
                        Site = _domain,
                        Section = LogSection.Sites,
                        Action = LogAction.insert,
                        PageId = id,
                        PageName = insert.Title,
                        UserId = _currentUserId,
                        IP = _ip,
                    };
                    insertLog(log);
                }
                return true;
            }
        }

        /// <summary>
        /// Обновляем департамент
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="insert">Департамент</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool updDepartament(Guid id, Departments insert)
        {
            using (var db = new CMSdb(_context))
            {
                content_departments cdDepart = db.content_departmentss
                                              .Where(p => p.id == id)
                                              .SingleOrDefault();
                if (cdDepart == null)
                {
                    throw new Exception("Запись с таким Id не существует");
                }
                cdDepart.c_title = insert.Title;
                cdDepart.c_adress = insert.Text;                

                using (var tran = db.BeginTransaction())
                {
                    db.Update(cdDepart);
                    tran.Commit();
                }
                return true;
            }
        }

        /// <summary>
        /// Удаляем департамент
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="UserId">Пользователь</param>
        /// <param name="IP">ip-адрес</param>
        /// <returns></returns>
        public override bool delDepartament(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                content_departments cdDepart = db.content_departmentss
                                                .Where(p => p.id == id)
                                                .SingleOrDefault();
                if (cdDepart == null)
                {
                    throw new Exception("Запись с таким Id не найдена");
                }
                Guid IdStruct = cdDepart.f_structure;
                int ThisSort = cdDepart.n_sort;
                using (var tran = db.BeginTransaction())
                {
                    db.content_departmentss.Where(w => w.f_structure == IdStruct && w.n_sort > ThisSort).Set(p => p.n_sort, p => p.n_sort - 1).Update();//смещение n_sort
                    db.Delete(cdDepart);

                    tran.Commit();
                }
                return true;
            }
        }

        /// <summary>
        /// Сортируем департаменты
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="new_num">Новое значение сортировки</param>
        /// <returns></returns>
        public override bool sortDepartament(Guid id, int new_num)
        {
            using (var db = new CMSdb(_context))
            {
                var thisdata = db.content_departmentss.Where(w => w.id == id).FirstOrDefault();
                int actual_num = thisdata.n_sort;
                Guid OrgId = thisdata.f_structure;
                if (new_num > actual_num)
                {
                    db.content_departmentss
                        .Where(w => w.f_structure == OrgId && w.n_sort > actual_num && w.n_sort <= new_num)
                        .Set(p => p.n_sort, p => p.n_sort - 1)
                        .Update();
                }
                else
                {
                    db.content_departmentss
                        .Where(w => w.f_structure == OrgId && w.n_sort < actual_num && w.n_sort >= new_num)
                        .Set(p => p.n_sort, p => p.n_sort + 1)
                        .Update();
                }
                db.content_departmentss
                    .Where(w => w.f_structure == OrgId && w.id == id)
                    .Set(s => s.n_sort, new_num)
                    .Update();

                return true;
            }
        }

        /// <summary>
        /// Получаем список сотрудников по департаменту
        /// </summary>
        /// <param name="idDepar">Департамент</param>
        /// <returns></returns>
        public override PeopleModel[] getPersonsThisDepartment(Guid idDepar)
        {
            using (var db = new CMSdb(_context))
            {
                var data_dep = db.content_departmentss.Where(w => w.id == idDepar);
                if (data_dep.Any())
                {
                    Guid idStructure = data_dep.First().f_structure;

                    var data_str = db.content_org_structures.Where(w => w.id == idStructure);
                    if (data_str.Any())
                    {
                        //нужно показать только персон не добавленных в отделение
                        Guid OrgId = data_str.First().f_ord;
                        var PeopleList = db.content_org_employeess
                                           .Where(w => w.f_org == OrgId)
                                           //.Where(w => (w.fkcontentpeopleorgdepartmentlinks == null || w.fkcontentpeopleorgdepartmentlinks.FirstOrDefault().f_department != idDepar))
                                           .Select(s => new PeopleModel
                                           {
                                               FIO = s.contentpeopleorglink.c_surname + " " + s.contentpeopleorglink.c_name + " " + s.contentpeopleorglink.c_patronymic,
                                               Id = s.id,
                                               IdLinkOrg = s.f_people
                                           }).ToArray();
                        return PeopleList.Any() ? PeopleList : null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Цепяет врача к департаменту(отделу)
        /// </summary>
        /// <param name="idDepart">id департамента(отдела)</param>
        /// <param name="IdLinkPeopleForOrg">идентификатор связи пользователя с организацией</param>
        /// <param name="status">стаус: старшая медсестра, начальник отделени ....</param>
        /// <param name="post">Должность</param>
        /// <returns></returns>
        public override bool insPersonsThisDepartment(Guid idDepart, Guid IdLinkPeopleForOrg, string status, string post)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    //проверям подключен ли данный пользователь к данному отделу
                    var data = db.content_department_employeess.Where(w => (w.f_department == idDepart && w.f_employee == IdLinkPeopleForOrg));
                    if (!data.Any())
                    {
                        content_department_employees newdata = new content_department_employees
                        {
                            id = Guid.NewGuid(),
                            f_department = idDepart,
                            f_employee = IdLinkPeopleForOrg,
                            c_status = status,
                            c_post = post
                        };
                        db.Insert(newdata);
                        tran.Commit();
                        return true;
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// Удаляем связь сотрудника из департамента
        /// </summary>
        /// <param name="idDep">Департамент</param>
        /// <param name="idPeople">Сотрудник</param>
        /// <returns></returns>
        public override bool delPersonsThisDepartment(Guid idDep, Guid idPeople)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_department_employeess.Where(w => w.f_department == idDep && w.f_employee == idPeople);

                if (data.Any())
                {
                    data.Delete();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получаем список типов организаций
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override OrgType[] getOrgTypesList(OrgTypeFilter filter)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_typess.AsQueryable();


                if (filter.Id.HasValue && filter.Id.Value != Guid.Empty)
                {
                    query = query.Where(s => s.id == filter.Id.Value);
                }

                if (filter.OrgId.HasValue && filter.OrgId.Value != Guid.Empty)
                {
                    query = query.Where(s => s.contentorgstypeslinkorgtypess.Any(o => o.f_org == filter.OrgId.Value));
                }

                var data = query
                    .OrderBy(s => s.n_sort)
                    .Select(s => new OrgType
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Sort = s.n_sort
                    });

                if (!data.Any()) return null;
                else return data.ToArray();
            }
        }


        /// <summary>
        /// Получим список типов организаций с привязанными к ним организациями
        /// </summary>
        /// <returns></returns>
        public override List<OrgType> getOrgByType(Guid material)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgs_typess
                    .OrderBy(o => o.n_sort)
                    .Select(s => new OrgType
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Sort = s.n_sort,
                        Orgs = getOrgSmall(s.id, material)
                    });

                if (!data.Any()) return null;
                else return data.ToList();
            }
        }

        /// <summary>
        /// Получим список организаций по типу
        /// </summary>
        /// <param name="id">Тип</param>
        /// <returns></returns>
        public override OrgsShortModel[] getOrgSmall(Guid id, Guid material)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_sv_orgs_by_types
                    .Where(w => w.f_type.Equals(id))
                    .OrderBy(o => o.n_sort)
                    .Select(s => new OrgsShortModel
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Sort = s.n_sort,
                        Checked = setCheckedOrgs(s.id, material)
                    });

                if (!data.Any()) return null;
                else return data.ToArray();
            }
        }

        /// <summary>
        /// Отметим выбранные организации
        /// <param name="id">Идентификатор</param>
        /// </summary>
        public override bool setCheckedOrgs(Guid id, Guid material)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_content_links
                    .Where(w => w.f_content.Equals(material))
                    .Where(w => w.f_link.Equals(id));

                return data.Any();
            }
        }

        /// <summary>
        /// Получаем список организаций, прикреплённых к каким-то типам
        /// </summary>
        /// <returns></returns>
        public override OrgsShortModel[] getOrgAttachedToTypes(Guid material)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_sv_orgs_not_attacheds
                    .OrderBy(o => o.c_title)
                    .Select(s => new OrgsShortModel
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Checked = setCheckedOrgs(s.id, material)
                    });

                if (!data.Any()) return null;
                else return data.ToArray();
            }
        }
        /// <summary>
        /// Административный персонал организации
        /// </summary>
        /// <param name="id">идентификатор организации</param>
        /// <returns></returns>
        public override OrgsAdministrative[] getAdministrativList(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_adminstrativs.Where(w => w.f_org == id).AsQueryable();
                if (query.Any())
                {
                    query = query.OrderBy(o => o.n_sort);
                    return query
                                .Select(s => new OrgsAdministrative()
                                {
                                    id = s.id,
                                    Surname = s.c_surname,
                                    Name = s.c_name,
                                    Patronymic = s.c_patronymic,
                                    Photo = new Photo { Url = s.c_photo }
                                })
                                .ToArray();
                }
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override OrgsAdministrative getAdministrativ(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_adminstrativs
                              .Where(w => w.id == id)
                              .Select(s => new OrgsAdministrative
                              {
                                  id = s.id,
                                  Surname = s.c_surname,                                  
                                  Name = s.c_name,
                                  Patronymic = s.c_patronymic,
                                  Phone = s.c_phone,
                                  Photo = new Photo { Url = s.c_photo },
                                  Post = s.c_post,
                                  Text = s.c_text,
                                  OrgId = s.f_org,
                                  PeopleId = s.f_people,
                                  Leader = s.b_leader
                              });
                if (query.Any())
                {
                    return query.Single();
                }
                return null;
            }
        }

        /// <summary>
        /// Добавляем административный персонал
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="ins">Модель для вставки</param>
        /// <returns></returns>
        public override bool insAdministrativ(Guid id, OrgsAdministrative ins)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var queryMaxSort = db.content_orgs_adminstrativs
                    .Where(w => w.f_org == ins.OrgId)
                    .Select(s => s.n_sort);

                    int maxSort = queryMaxSort.Any() ? queryMaxSort.Max() + 1 : 1;

                    db.content_orgs_adminstrativs
                        .Value(v => v.id, id)
                      .Value(v => v.c_surname, ins.Surname)
                      .Value(v => v.c_name, ins.Name)
                      .Value(v => v.c_patronymic, ins.Patronymic)
                      .Value(v => v.c_phone, ins.Phone)
                      .Value(v => v.c_photo, ins.Photo != null ? ins.Photo.Url : null)
                      .Value(v => v.c_post, ins.Post)
                      .Value(v => v.c_text, ins.Text)
                      .Value(v => v.f_org, ins.OrgId)
                      .Value(v => v.f_people, ins.PeopleId)
                      .Value(v => v.n_sort, maxSort)
                      .Value(v => v.b_leader, ins.Leader)
                      .Insert();
                    tran.Commit();
                }

                return true;
            }
        }

        /// <summary>
        /// Обновляем административный персонал
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="upd">Модель для обновления</param>
        /// <returns></returns>
        public override bool updAdministrativ(Guid id, OrgsAdministrative upd)
        {
            using (var db = new CMSdb(_context))
            {
                using (var tran = db.BeginTransaction())
                {
                    var data = db.content_orgs_adminstrativs.Where(w => w.id == id);
                    if (data.Any())
                    {
                        var item = data.Single();

                        if (upd.Leader)
                        {
                            var lider = db.content_orgs_adminstrativs
                                .Where(p => p.f_org == item.f_org && p.b_leader==true);
                            if (lider.Any())
                            {
                                lider.Set(p => p.b_leader, false)
                                .Update();
                            }
                            
                        }

                        data.Set(s => s.c_surname, upd.Surname)
                            .Set(s => s.c_name, upd.Name)
                            .Set(s => s.c_patronymic, upd.Patronymic)
                            .Set(s => s.c_phone, upd.Phone)
                            .Set(s => s.c_photo, upd.Photo.Url)
                            .Set(s => s.c_post, upd.Post)
                            .Set(s => s.c_text, upd.Text)
                            .Set(s => s.f_people, upd.PeopleId)
                            .Set(s => s.b_leader, upd.Leader)
                            .Update();

                        tran.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Удаляем административный персонал
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public override bool delAdministrativ(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgs_adminstrativs.Where(w => w.id == id);
                if (data.Any())
                {
                    data.Delete();
                    return true;
                }
                return false;

            }
        }

        /// <summary>
        /// Сортировка административного персонала
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="OrgId">Организация</param>
        /// <param name="num">Сортировка</param>
        /// <returns></returns>
        public override bool permit_OrgsAdminstrativ(Guid id, Guid OrgId, int num)
        {
            using (var db = new CMSdb(_context))
            {
                //текущее значение элемента чей приоритет меняется
                var query = db.content_orgs_adminstrativs
                    .Where(w => w.id==id);

                if (query.Any())
                {
                    int CurrentSort = query.Single().n_sort;
                    var q = db.content_orgs_adminstrativs
                        .Where(w => w.f_org.Equals(OrgId) && (w.n_sort < CurrentSort && w.n_sort >= num));


                    if (num > CurrentSort)
                    {
                        db.content_orgs_adminstrativs
                            .Where(w => w.f_org == OrgId)
                            .Where(w => w.n_sort > CurrentSort && w.n_sort <= num)
                            .Set(u => u.n_sort, u => u.n_sort - 1)
                            .Update();
                    }
                    else
                    {
                        db.content_orgs_adminstrativs
                            .Where(w => w.f_org == OrgId)
                            .Where(w => w.n_sort < CurrentSort && w.n_sort >= num)
                            .Set(u => u.n_sort, u => u.n_sort + 1)
                            .Update();
                    }
                    db.content_orgs_adminstrativs
                        .Where(w => w.id == id)
                        .Set(u => u.n_sort, num)
                        .Update();

                    return true;
                }
                return false;


                //try
                //{
                //    if (query.Any())
                //    {
                //        var data = query.Single();
                //        if (num > data.n_sort)
                //        {
                //            db.content_orgs_adminstrativs
                //                .Where(w => w.f_org==OrgId)
                //                .Where(w => w.n_sort > data.n_sort && w.n_sort <= num)
                //                .Set(u => u.n_sort, u => u.n_sort - 1)
                //                .Update();
                //        }
                //        else
                //        {
                //            db.content_orgs_adminstrativs
                //                .Where(w => w.f_org==OrgId)
                //                .Where(w => w.n_sort < data.n_sort && w.n_sort >= num)
                //                .Set(u => u.n_sort, u => u.n_sort + 1)
                //                .Update();
                //        }
                //        db.content_orgs_adminstrativs
                //            .Where(w => w.id==id)
                //            .Set(u => u.n_sort, num)
                //            .Update();

                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }

                //}
                //catch(Exception e)
                //{
                //    var r = e.Message;
                //    int g = 0;
                //    return false;
                //}

            }
        }

        /// <summary>
        /// Получим список ведомственных принадлежностей
        /// </summary>
        /// <returns></returns>
        public override DepartmentAffiliationModel[] getDepartmentAffiliations()
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_department_affiliations
                    .OrderBy(o => o.n_sort)
                    .Select(s => new DepartmentAffiliationModel
                    {
                        Key = s.id,
                        Value = s.c_title
                    });

                if (!query.Any()) return null;
                return query.ToArray();
            }
        }


        /// <summary>
        /// список сотрудников организации
        /// </summary>
        /// <param name="idOrg">идентификатор организации</param>
        /// <returns></returns>
        public override PeopleModel[] getPersonsThisOrg(Guid idOrg)
        {
            using (var db = new CMSdb(_context))
            {
                var PeopleList = db.content_org_employeess
                                           .Where(w => w.f_org == idOrg)
                                           .Select(s => new PeopleModel
                                           {
                                               FIO = s.contentpeopleorglink.c_surname + " " + s.contentpeopleorglink.c_name + " " + s.contentpeopleorglink.c_patronymic,
                                               Id = s.contentpeopleorglink.id
                                               //Id = s.id,
                                               //IdLinkOrg = s.f_people
                                           }).ToArray();
                return PeopleList.Any() ? PeopleList : null;
            }
        }

        /// <summary>
        /// Получим список медицинских услуг
        /// </summary>
        /// <returns></returns>
        public override MedServiceModel[] getMedicalServices()
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_medical_servicess
                    .OrderBy(o => o.n_sort)
                    .Select(s => new MedServiceModel
                    {
                        Id = s.id,
                        Title = s.c_title,
                        Sort = s.n_sort
                    });

                if (!query.Any()) return null;
                return query.ToArray();
            }
        }

        /// <summary>
        /// Получаем связи медицинские услуги привязанные к организации
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public override Guid[] getOrgMedicalServicesLinks(Guid org)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_medical_services_linkss
                    .Where(w => w.f_org.Equals(org))
                    .Select(s => s.f_medical_service);

                if (!query.Any()) return null;
                return query.ToArray();
            }
        }

        /// <summary>
        /// Получим ссылку для редактирования организации
        /// </summary>
        /// <param name="domain">Домен</param>
        /// <returns></returns>
        public override Guid? getOrgLinkByDomain()
        {
            using (var db = new CMSdb(_context))
            {
                var query = (from s in db.cms_sitess
                             join o in db.content_orgss on s.f_content equals o.id
                             where s.c_alias.ToLower().Equals(_domain)
                             select o.id);

                if (!query.Any()) return null;
                return query.SingleOrDefault();
            }
        }

        /// <summary>
        /// Доступна ли структура для админа сайта
        /// </summary>
        /// <param name="structureId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public override bool IsStructureAllowedToOrg(Guid structureId, Guid orgId)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_org_structures
                    .Where(w => w.f_ord.Equals(orgId))
                    .Where(w => w.id.Equals(structureId));

                return query.Any();
            }
        }

        /// <summary>
        /// Доступен ли административный персонал для организации
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public override bool IsAdministrativeAllowedToOrg(Guid id, Guid orgId)
        {
            using (var db = new CMSdb(_context))
            {
                var query = db.content_orgs_adminstrativs
                    .Where(w => w.f_org.Equals(orgId))
                    .Where(w => w.id.Equals(id));

                return query.Any();
            }
        }

        /// <summary>
        /// Доступен ли департамент для организации
        /// </summary>
        /// <param name="id"></param>
        /// <param name="structure"></param>
        /// <returns></returns>
        public override bool IsDepartmentAllowedToOrg(Guid id, Guid orgId)
        {
            using (var db = new CMSdb(_context))
            {
                // список структур организации
                var structureIds = db.content_org_structures
                    .Where(w => w.f_ord.Equals(orgId))
                    .Select(s => s.id);

                if (!structureIds.Any()) return false;

                


                
                var query1 = db.content_departmentss
                    .Where(w => w.id.Equals(id));
                //случай когда департамент создается т.е. его не существует в базе
                if (!query1.Any()) return true;
                // структура владеющая департаментом
                var query =query1.Select(s => s.f_structure);

                if (!query.Any()) return false;

                return structureIds.Contains(query.SingleOrDefault());
            }
        }


        public override bool NormalizeDepartamnt()
        {
            using (var db = new CMSdb(_context))
            {
                var structure = db.content_org_structures;
                foreach (var item in structure.ToArray())
                {
                    db.normolize_sort_departamts(item.id);
                }
                return true;
            }
        }
    }
}
