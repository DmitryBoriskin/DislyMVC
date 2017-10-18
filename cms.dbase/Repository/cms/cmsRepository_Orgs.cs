﻿using System;
using System.Collections.Generic;
using System.Linq;
using cms.dbModel.entity.cms;
using System.Text;
using System.Threading.Tasks;
using cms.dbModel;
using cms.dbModel.entity;
using cms.dbase.models;
using LinqToDB;
using System.Web.Mvc;

namespace cms.dbase
{
    /// <summary>
    /// Репозиторий для работы с картой сайта
    /// </summary>
    public partial class cmsRepository : abstract_cmsRepository
    {        
        public override OrgsModel[] getOrgs(FilterParams filtr)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgss.OrderBy(o => o.n_sort).AsQueryable();
                if (filtr.SearchText != null)
                {
                    data = data.Where(w => (w.c_title.Contains(filtr.SearchText)));
                }
                //data.OrderBy(o => o.n_sort); ХЗ почему эта строка нормально не сортирует                
                var list = data.Select(s => new OrgsModel()
                {
                    Id = s.id,
                    Title = s.c_title,
                    Sort = s.n_sort
                });
                if (list.Any()) return list.ToArray();
                return null;
            }
        }
        public override OrgsModel getOrgItem(Guid id)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgss.Where(w => w.id == id)
                            .Select(s => new OrgsModel
                            {
                                Id = s.id,
                                Title = s.c_title,
                                ShortTitle = s.c_title_short,
                                Phone = s.c_phone,
                                PhoneReception = s.c_phone_reception,
                                Fax = s.c_fax,
                                Email = s.c_email,
                                DirecorPost = s.c_director_post,
                                DirectorF = s.f_director,
                                Contacts = s.c_contacts,
                                Address = s.c_adress,
                                GeopointX = s.n_geopoint_x,
                                GeopointY = s.n_geopoint_y,
                                Structure = getStructureList(s.id),
                                Frmp=s.f_frmp
                            })
                            .FirstOrDefault();
                if (data != null) return data;
                return null;
            }
        }
        public override bool insOrgs(Guid id, OrgsModel model, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
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
                    db.content_orgss
                        .Value(s => s.id, id)
                        .Value(s => s.n_sort, MaxSort)
                        .Value(s => s.c_title, model.Title)
                        .Value(s => s.c_title_short, model.ShortTitle)
                        .Value(s => s.c_phone, model.Phone)
                        .Value(s => s.c_phone_reception, model.PhoneReception)
                        .Value(s => s.c_fax, model.Fax)
                        .Value(s => s.c_email, model.Email)
                        .Value(s => s.c_director_post, model.DirecorPost)
                        .Value(s => s.f_director, model.DirectorF)
                        .Value(s => s.c_contacts, model.Contacts)
                        .Value(s => s.c_adress, model.Address)
                        .Value(s => s.n_geopoint_x, model.GeopointX)
                        .Value(s => s.n_geopoint_y, model.GeopointY)
                        .Value(s => s.f_frmp, model.Frmp)
                        .Insert();
                    //логирование
                    insertLog(UserId, IP, "insert", id, String.Empty, "Orgs", model.Title);
                    return true;
                }
                return false;
            }
        }
        public override bool setOrgs(Guid id, OrgsModel model, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgss.Where(w => w.id == id);
                if (data.Any())
                {
                    data
                        .Set(s => s.c_title, model.Title)
                        .Set(s => s.c_title_short, model.ShortTitle)
                        .Set(s => s.c_phone, model.Phone)
                        .Set(s => s.c_phone_reception, model.PhoneReception)
                        .Set(s => s.c_fax, model.Fax)
                        .Set(s => s.c_email, model.Email)
                        .Set(s => s.c_director_post, model.DirecorPost)
                        .Set(s => s.f_director, model.DirectorF)
                        .Set(s => s.c_contacts, model.Contacts)
                        .Set(s => s.c_adress, model.Address)
                        .Set(s => s.n_geopoint_x, model.GeopointX)
                        .Set(s => s.n_geopoint_y, model.GeopointY)
                        .Set(s => s.f_frmp, model.Frmp)
                        .Update();
                    //логирование
                    insertLog(UserId, IP, "update", id, String.Empty, "Orgs", model.Title);
                    return true;
                }
                return false;
            }
        }
        public override bool delOrgs(Guid id, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_orgss.Where(w => w.id == id);
                if (data.Any())
                {
                    string logTitle = data.FirstOrDefault().c_title;
                    int ThisSort = data.FirstOrDefault().n_sort;
                    db.content_orgss.Where(w => w.n_sort > ThisSort).Set(p => p.n_sort, p => p.n_sort - 1).Update();//смещение n_sort
                    data.Delete();
                    //логирование
                    insertLog(UserId, IP, "delete", id, String.Empty, "Orgs", logTitle);
                    return true;
                }
                return false;
            }
        }
        public override bool sortOrgs(Guid id, int new_num)
        {
            using (var db = new CMSdb(_context))
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

                return true;
            }
        }
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
                        Departments = getDepartmentsList(s.id)

                        //f_direcor
                    }).FirstOrDefault();
                }
                return null;
            }
        }
        public override bool insStructure(Guid id, Guid OrgId, StructureModel insert, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
            {
                int MaxSort = 0;
                try
                {
                    MaxSort = db.content_org_structures.Where(w => w.f_ord == OrgId).Max(m => m.n_sort);
                }
                catch { }
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
                insertLog(UserId, IP, "insert", id, String.Empty, "Orgs", insert.Title);
                return true;
            }
        }
        public override bool setStructure(Guid id, StructureModel insert, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
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
                    insertLog(UserId, IP, "update", id, String.Empty, "Orgs", insert.Title);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public override bool delStructure(Guid id, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_org_structures.Where(w => w.id == id);
                Guid IdOrg = data.FirstOrDefault().f_ord;
                int ThisSort = data.FirstOrDefault().n_sort;
                string logTitle = data.FirstOrDefault().c_title;
                if (data.Any())
                {

                    data.Delete();
                    db.content_org_structures.Where(w => w.f_ord == IdOrg && w.n_sort > ThisSort).Set(p => p.n_sort, p => p.n_sort - 1).Update();//смещение n_sort
                    //логирование
                    insertLog(UserId, IP, "delete", id, String.Empty, "Orgs", logTitle);
                    return true;
                }
                return false;
            }
        }
        public override bool sortStructure(Guid id, int new_num)
        {
            using (var db = new CMSdb(_context))
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

                return true;
            }
        }
        /// <summary>
        /// Добавляем ОВП
        /// </summary>        
        /// <returns></returns>
        public override bool insOvp(Guid IdStructure, Guid OrgId, StructureModel insertStructure, Guid UserId, String IP)
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
                    insertLog(UserId, IP, "insert", IdStructure, String.Empty, "Orgs", logTitle);
                    return true;
                }
            }
        }
        public override bool setOvp(Guid IdStructure, StructureModel updStructure, Guid UserId, String IP)
        {
            using (var db = new CMSdb(_context))
            {
                content_org_structure cdStructur = db.content_org_structures.Where(w => w.id == IdStructure).SingleOrDefault();
                if (cdStructur == null)
                {
                    throw new Exception("Запись с таким Id не существует");
                }
                cdStructur.c_title = updStructure.Title;
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
                using (var tran = db.BeginTransaction())
                {
                    db.Update(cdStructur);
                    db.Update(cdDepart);
                    tran.Commit();
                    //логирование
                    insertLog(UserId, IP, "update", IdStructure, String.Empty, "Orgs", updStructure.Title);
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
        public override BreadCrumb[] getBreadCrumbOrgs(Guid id, string type)
        {
            using (var db = new CMSdb(_context))
            {
                var MyBread = new Stack<BreadCrumb>();
                MyBread.Push(new BreadCrumb
                {
                    Title = "Организации",
                    Url = "/admin/orgs/"
                });
                #region item
                if (type == "item")
                {
                    var data = db.content_departmentss.Where(w => w.id == id).FirstOrDefault();
                    MyBread.Push(new BreadCrumb
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

                    MyBread.Push(new BreadCrumb
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/item/" + ParentStructure.id
                    });
                    MyBread.Push(new BreadCrumb
                    {
                        Title = data.c_title,
                        Url = "/admin/orgs/structure/" + data.id
                    });
                }
                #endregion
                #region ovp
                if (type == "ovp")
                {
                    var data = db.content_org_structures.Where(w => w.id == id).FirstOrDefault();
                    var ParentStructure = db.content_orgss.Where(w => w.id == data.f_ord).FirstOrDefault();

                    MyBread.Push(new BreadCrumb
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/item/" + ParentStructure.id
                    });
                    MyBread.Push(new BreadCrumb
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

                    MyBread.Push(new BreadCrumb
                    {
                        Title = ParentOrg.c_title,
                        Url = "/admin/orgs/item/" + ParentOrg.id
                    });
                    MyBread.Push(new BreadCrumb
                    {
                        Title = ParentStructure.c_title,
                        Url = "/admin/orgs/structure/" + ParentStructure.id
                    });
                    MyBread.Push(new BreadCrumb
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
        public override bool insDepartmentsPhone(Guid idDepart, string Label, string Value, Guid UserId, String IP)
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
                insertLog(UserId, IP, "insert_phone_depart", idDepart, String.Empty, "Orgs", Label);
                return true;
            }
        }
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
        public override People[] getPeopleDepartment(Guid idDepart)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_sv_people_departments
                           .Where(w => w.f_department == idDepart)
                           .Select(s => new People()
                           {
                               Id = s.id,
                               FIO = s.c_surname + " " + s.c_name + " " + s.c_patronymic
                               //,
                               //IdLinkOrg=s.
                           });
                if (data.Any()) return data.ToArray();
                return null;
            }
        }
        public override bool insDepartament(Guid id, Guid Structure, Departments insert, Guid UserId, String IP)
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
                    n_sort = MaxSort
                };
                using (var tran = db.BeginTransaction())
                {
                    db.Insert(cdDepart);
                    tran.Commit();
                }
                return true;
            }
        }
        public override bool updDepartament(Guid id, Departments insert, Guid UserId, String IP)
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
                using (var tran = db.BeginTransaction())
                {
                    db.Update(cdDepart);
                    tran.Commit();
                }
                return true;
            }
        }
        public override bool delDepartament(Guid id, Guid UserId, String IP)
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


        public override People[] getPersonsThisDepartment(Guid idDepar)
        {
            using (var db = new CMSdb(_context))
            {
                var data_dep = db.content_departmentss.Where(w => w.id == idDepar);
                if (data_dep.Any())
                {
                    Guid idStructure=data_dep.First().f_structure;

                    var data_str = db.content_org_structures.Where(w => w.id == idStructure);
                    if (data_str.Any())
                    {
                        //нужно показать только персон не добавленных в отделение
                        Guid OrgId = data_str.First().f_ord;
                        var PeopleList = db.content_people_org_links
                                           .Where(w => w.f_org == OrgId)
                                           .Where(w =>(w.fkcontentpeopleorgdepartmentlinks == null || w.fkcontentpeopleorgdepartmentlinks.FirstOrDefault().f_department!=idDepar))                                           
                                           .Select(s => new People
                                           {
                                               FIO = s.fkcontentpeopleorglink.c_surname+" "+ s.fkcontentpeopleorglink.c_name + " " + s.fkcontentpeopleorglink.c_patronymic,
                                               Id = s.id
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
                    var data = db.content_people_department_links.Where(w => (w.f_department == idDepart && w.f_people == IdLinkPeopleForOrg));
                    if (!data.Any())
                    {
                        content_people_department_link newdata = new content_people_department_link {
                            f_department=idDepart,
                            f_people= IdLinkPeopleForOrg,
                            c_status=status,
                            c_post=post
                        };
                        db.Insert(newdata);
                        tran.Commit();
                        return true;
                    }
                }
                    
            }
            return false;
        }

        public override bool delPersonsThisDepartment(Guid idDep, Guid idPeople)
        {
            using (var db = new CMSdb(_context))
            {
                var data = db.content_people_department_links.Where(w => w.f_department == idDep && w.f_people== idPeople);

                if (data.Any())
                {                    
                    data.Delete();
                }
            }
            return true;
        }
    }
}