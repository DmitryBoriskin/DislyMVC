﻿using cms.dbModel.entity;
using Disly.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Disly.Areas.Admin.Controllers
{
    public class FeedbacksController : CoreController
    {
        FeedbacksViewModel model;
        FilterParams filter;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.ControllerName = (String)RouteData.Values["controller"];
            ViewBag.ActionName = RouteData.Values["action"].ToString().ToLower();

            ViewBag.HttpKeys = Request.QueryString.AllKeys;
            ViewBag.Query = Request.QueryString;
            ViewBag.DataPath = Settings.UserFiles + Domain+ "/"+ ViewBag.ControllerName + "/";

            filter = getFilter();

            model = new FeedbacksViewModel()
            {
                Account = AccountInfo,
                Settings = SettingsInfo,
                UserResolution = UserResolutionInfo,
                ControllerName = ControllerName,
                ActionName = ActionName,
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
        }


        /// <summary>
        /// GET: Список событий
        /// </summary>
        /// <param name="category"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(string category, string type)
        {
            #region Group filter
            var alias = "group";
            var groupLink = "/admin/feedbacks/"; 
            //var editGroupUrl = "";

            string query = Request.Url.Query;
            string active = Request.QueryString[alias];

            var types = new Catalog_list[]
            {
                  new Catalog_list()
                {
                    Text = "Вопросы",
                    Value = "appeal"
                },
                new Catalog_list()
                {
                    Text = "Отзывы",
                    Value = "review"
                }
            };

            if (types != null && types.Count() > 0)
            {
                model.Filtr = new FiltrModel()
                {
                    Title = "Группы",
                    Icon = "icon-th-list-3",
                    BtnName = "Новая группа",
                    Alias = alias,
                    //Url = editGroupUrl,
                    ReadOnly = true,
                    AccountGroup = (model.Account != null) ? model.Account.Group : "",
                    Items = types.Select(p =>
                        new Catalog_list()
                        {
                            Text = p.Text,
                            Value = p.Value,
                            Link = AddFiltrParam(query, alias, p.Value),
                            //Url = editGroupUrl + p.Value + "/",
                            Selected = (active == p.Value) ? true : false
                        })
                        .ToArray(),
                    Link = groupLink
                };
            }
            #endregion

            model.List = _cmsRepository.getFeedbacksList(filter);
            //Domain

            return View(model);
        }

        /// <summary>
        /// GET: Форма редактирования/добавления записи
        /// </summary>
        /// <returns></returns>
        public ActionResult Item(Guid Id)
        {
            model.Item = _cmsRepository.getFeedbackItem(Id);
            if (model.Item == null)
                model.Item = new FeedbackModel()
                {
                    Date = DateTime.Now,
                    Disabled = true
                };
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
        public ActionResult Search(string searchtext, string group, bool enabled, string size, DateTime? datestart, DateTime? dateend)
        {
            string query = HttpUtility.UrlDecode(Request.Url.Query);
            query = AddFiltrParam(query, "searchtext", searchtext);
            query = AddFiltrParam(query, "group", group);
            query = AddFiltrParam(query, "disabled", (!enabled).ToString().ToLower());
            if (datestart.HasValue)
                query = AddFiltrParam(query, "datestart", datestart.Value.ToString("dd.MM.yyyy").ToLower());
            if (dateend.HasValue)
                query = AddFiltrParam(query, "dateend", dateend.Value.ToString("dd.MM.yyyy").ToLower());
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

        /// <summary>
        /// Создание новой записи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "insert-btn")]
        public ActionResult Insert()
        {
            //  При создании записи сбрасываем номер страницы
            string query = HttpUtility.UrlDecode(Request.Url.Query);
            query = AddFiltrParam(query, "page", String.Empty);

            return Redirect(StartUrl + "Item/" + Guid.NewGuid() + "/" + query);
        }

        /// <summary>
        /// Сохранение изменений в записи
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="bindData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "save-btn")]
        public ActionResult Save(Guid Id, FeedbacksViewModel bindData)
        {
            ErrorMessage userMessage = new ErrorMessage();
            userMessage.title = "Информация";

            if (ModelState.IsValid)
            {
                var res = false;
                var getFeedback = _cmsRepository.getFeedbackItem(Id);

                bindData.Item.Id = Id;
                //Определяем Insert или Update
                if (getFeedback != null)
                {
                    userMessage.info = "Запись обновлена";
                    res = _cmsRepository.updateCmsFeedback(bindData.Item);
                }

                else
                {
                    userMessage.info = "Запись добавлена";
                    res = _cmsRepository.insertCmsFeedback(bindData.Item);
                }
                //Сообщение пользователю
                if (res)
                {
                    string currentUrl = Request.Url.PathAndQuery;
                    userMessage.buttons = new ErrorMassegeBtn[]
                    {
                     new ErrorMassegeBtn { url = StartUrl + Request.Url.Query, text = "Вернуться в список" },
                     new ErrorMassegeBtn { url = currentUrl, text = "ок" }
                 };
                }
                else
                {
                    userMessage.info = "Произошла ошибка";
                    userMessage.buttons = new ErrorMassegeBtn[]
                    {
                     new ErrorMassegeBtn { url = StartUrl + Request.Url.Query, text = "Вернуться в список" },
                     new ErrorMassegeBtn { url = "#", text = "ок", action = "false"  }
                    };
                }
            }
            else
            {
                userMessage.info = "Ошибка в заполнении формы. Поля в которых допушены ошибки - помечены цветом.";

                userMessage.buttons = new ErrorMassegeBtn[]{
                     new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                 };
            }

            //model.Item = _cmsRepository.getFeedback(Id);
            model.ErrorInfo = userMessage;

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
            // записываем информацию о результатах
            ErrorMessage userMessage = new ErrorMessage();
            userMessage.title = "Информация";
            //В случае ошибки
            userMessage.info = "Ошибка, Запись не удалена";
            userMessage.buttons = new ErrorMassegeBtn[]{
                new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
            };

            var res = _cmsRepository.deleteCmsFeedback(Id);

            if (res)
            {
                // записываем информацию о результатах
                userMessage.title = "Информация";
                userMessage.info = "Запись удалена";

                userMessage.buttons = new ErrorMassegeBtn[]
                {
                        new ErrorMassegeBtn { url = StartUrl, text = "Перейти в список" }
                };
                //return RedirectToAction("Index", new { id = model.Item.Section });
            }

            model.ErrorInfo = userMessage;

            return View(model);
        }
    }
}