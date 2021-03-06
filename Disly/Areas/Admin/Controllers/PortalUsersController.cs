﻿using cms.dbModel.entity;
using Disly.Areas.Admin.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Disly.Areas.Admin.Controllers
{
    public class PortalUsersController : CoreController
    {
        PortalUsersViewModel model;
        FilterParams filter;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            model = new PortalUsersViewModel()
            {
                Account = AccountInfo,
                Settings = SettingsInfo,
                UserResolution = UserResolutionInfo,
                ControllerName = ControllerName,
                ActionName = ActionName,
                GroupList = _cmsRepository.getUsersGroupList()
            };
            if (AccountInfo != null)
            {
                model.Menu = _cmsRepository.getCmsMenu(AccountInfo.Id);
            }
            ViewBag.DataPath = Settings.UserFiles + Domain + "/" + (String)RouteData.Values["controller"] + "/";

            #region Метатеги
            ViewBag.Title = UserResolutionInfo.Title;
            ViewBag.Description = "";
            ViewBag.KeyWords = "";
            #endregion
        }

        /// <summary>
        /// Страница по умолчанию (Список)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Наполняем фильтр значениями
            filter = getFilter(page_size);
            filter.Domain = null;
            // Наполняем модель данными
            model.List = _cmsRepository.getUsersList(filter);

            //Фильтр на странице

            if(model.GroupList!= null && model.GroupList.Count()> 0)
            {
                var alias = "group";
                var groupLink = "/admin/portalusers/";
                var editGroupUrl = "/admin/services/groupclaims/";

                string Link = Request.Url.Query;
                string active = Request.QueryString[alias];

                model.Filter = new FiltrModel()
                {
                    Title = "Группы",
                    Icon = "icon-users-3",
                    BtnName = "Новая группа пользователей",
                    Alias = alias,
                    Url = editGroupUrl,
                    ReadOnly = false,
                    AccountGroup = (model.Account != null) ? model.Account.Group : "",
                    Items = model.GroupList.Select(p =>
                        new Catalog_list()
                        {
                            Text = p.Text,
                            Value = p.Value,
                            Link = AddFiltrParam(Link, alias, p.Value),
                            Url = editGroupUrl + p.Value + "/",
                            Selected = (active == p.Value.ToLower()) ? true : false
                        })
                        .ToArray(),
                    Link = groupLink
                };
            }

            return View(model);
        }

        /// <summary>
        /// Форма редактирования записи
        /// </summary>
        /// <returns></returns>
        public ActionResult Item(Guid Id)
        {
            model.Item = _cmsRepository.getUser(Id);

            return View("Item", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchtext"></param>
        /// <param name="disabled"></param>
        /// <param name="size"></param>
        /// <param name="date"></param>
        /// <param name="dateend"></param>
        /// <returns></returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "search-btn")]
        public ActionResult Search(string searchtext, string group, bool enabled, string size)
        {
            string query = HttpUtility.UrlDecode(Request.Url.Query);
            query = AddFiltrParam(query, "searchtext", searchtext);
            query = AddFiltrParam(query, "disabled", (!enabled).ToString().ToLower());
            query = AddFiltrParam(query, "page", String.Empty);
            query = AddFiltrParam(query, "size", size);

            return Redirect(StartUrl + query);
        }

        /// <summary>
        /// Очищаем фильтр
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "clear-btn")]
        public ActionResult ClearFiltr()
        {
            return Redirect(StartUrl);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "insert-btn")]
        public ActionResult Insert()
        {
            //  При создании записи сбрасываем номер страницы
            string query = HttpUtility.UrlDecode(Request.Url.Query);
            query = AddFiltrParam(query, "page", String.Empty);

            return Redirect(StartUrl + "Item/" + Guid.NewGuid() + "/" + query);
        }

        [HttpPost]
        [ValidateInput(false)]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "save-btn")]
        public ActionResult Save(Guid Id, PortalUsersViewModel back_model)
        {
            ErrorMessage userMassege = new ErrorMessage();
            userMassege.title = "Информация";

            if (ModelState.IsValid)
            {
                if (_cmsRepository.check_user(Id))
                {
                    _cmsRepository.updateUser(Id, back_model.Item); //, AccountInfo.id, RequestUserInfo.IP
                    userMassege.info = "Запись обновлена";
                }
                else if (!_cmsRepository.check_user(back_model.Item.EMail))
                {
                    char[] _pass = back_model.Password.Password.ToCharArray();
                    Cripto password = new Cripto(_pass);
                    string NewSalt = password.Salt;
                    string NewHash = password.Hash;

                    back_model.Item.Salt = NewSalt;
                    back_model.Item.Hash = NewHash;

                    _cmsRepository.createUser(Id, back_model.Item);//, AccountInfo.id, RequestUserInfo.IP

                    userMassege.info = "Запись добавлена";
                }
                else
                {
                    userMassege.info = "Пользователь с таким EMail адресом уже существует.";
                }

                userMassege.buttons = new ErrorMassegeBtn[]{
                    new ErrorMassegeBtn { url = StartUrl + Request.Url.Query, text = "вернуться в список" },
                    new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                };
            }
            else
            {
                userMassege.info = "Ошибка в заполнении формы. Поля в которых допушены ошибки - помечены цветом.";

                userMassege.buttons = new ErrorMassegeBtn[]{
                    new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                };
            }

            model.Item = _cmsRepository.getUser(Id);
            model.ErrorInfo = userMassege;

            return View("Item", model);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "cancel-btn")]
        public ActionResult Cancel()
        {
            return Redirect(StartUrl + Request.Url.Query);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "delete-btn")]
        public ActionResult Delete(Guid Id)
        {
            _cmsRepository.deleteUser(Id); //, AccountInfo.id, RequestUserInfo.IP

            // записываем информацию о результатах
            ErrorMessage userMassege = new ErrorMessage();
            userMassege.title = "Информация";
            userMassege.info = "Запись Удалена";
            userMassege.buttons = new ErrorMassegeBtn[]{
                new ErrorMassegeBtn { url = StartUrl + Request.Url.Query, text = "ок", action = "false" }
            };

            model.Item = _cmsRepository.getUser(Id);
            model.ErrorInfo = userMassege;

            return View("Item", model);
        }
    }
}