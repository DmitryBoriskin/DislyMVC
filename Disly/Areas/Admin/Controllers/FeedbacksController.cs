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
 
             ViewBag.ControllerName = (String) RouteData.Values["controller"];
             ViewBag.ActionName = RouteData.Values["action"].ToString().ToLower();
 
             ViewBag.HttpKeys = Request.QueryString.AllKeys;
             ViewBag.Query = Request.QueryString;
 
             filter = getFilter();
 
              model = new FeedbacksViewModel()
             {
                 Account = AccountInfo,
                 Settings = SettingsInfo,
                 UserResolution = UserResolutionInfo
             };
 
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
             // Наполняем модель данными
             model.List = _cmsRepository.getFeedbacksList(filter);
 
             return View(model);
         }
 
         /// <summary>
         /// GET: Форма редактирования/добавления события
         /// </summary>
         /// <returns></returns>
         public ActionResult Item(Guid Id)
         {
             model.Item = _cmsRepository.getFeedback(Id);
 
             return View("Item", model);
         }
 
 
         /// <summary>
         /// Формируем строку фильтра
         /// </summary>
         /// <param name="title_serch">Поиск по названию</param>
         /// <returns></returns>
         [HttpPost]
         [MultiButton(MatchFormKey = "action", MatchFormValue = "search-btn")]
         public ActionResult Search(string filter, bool enabeld, string size)
         {
             string query = HttpUtility.UrlDecode(Request.Url.Query);
             query = addFiltrParam(query, "filter", filter);
             if (enabeld) query = addFiltrParam(query, "enabeld", String.Empty);
             else query = addFiltrParam(query, "enabeld", enabeld.ToString().ToLower());
 
             query = addFiltrParam(query, "page", String.Empty);
             query = addFiltrParam(query, "size", size);
 
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
             query = addFiltrParam(query, "page", String.Empty);
 
             var id = Guid.NewGuid();
             ViewBag.ItemId = id;
             return Redirect(StartUrl + "Item/" + id + "/" + query);
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
         public ActionResult Save(Guid Id, EventsViewModel bindData)
         {
             ErrorMassege userMessage = new ErrorMassege();
             userMessage.title = "Информация";
 
             if (ModelState.IsValid)
             {
                 var res = false;
                 var getEvent = _cmsRepository.getEvent(Id);
 
                 bindData.Item.Id = Id;
                 //Определяем Insert или Update
                 if (getEvent != null)
                     res = _cmsRepository.updateCmsEvent(bindData.Item);
                 else
                     res = _cmsRepository.insertCmsEvent(bindData.Item);
                 //Сообщение пользователю
                 if (res)
                     userMessage.info = "Запись обновлена";
                 else
                     userMessage.info = "Произошла ошибка";
 
                 userMessage.buttons = new ErrorMassegeBtn[]{
                     new ErrorMassegeBtn { url = StartUrl + Request.Url.Query, text = "Вернуться в список" },
                     new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                 };
             }
             else
             {
                 userMessage.info = "Ошибка в заполнении формы. Поля в которых допушены ошибки - помечены цветом.";
 
                 userMessage.buttons = new ErrorMassegeBtn[]{
                     new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
                 };
             }
 
             //model.Item = _cmsRepository.getUser(Id);
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
             //_cmsRepository.deleteUser(Id, AccountInfo.id, RequestUserInfo.IP);
 
             // записываем информацию о результатах
             ErrorMassege userMassege = new ErrorMassege();
             userMassege.title = "Информация";
             userMassege.info = "Запись Удалена";
             userMassege.buttons = new ErrorMassegeBtn[]{
                 new ErrorMassegeBtn { url = "#", text = "ок", action = "false" }
             };
 
             //model.Item = _cmsRepository.getUser(Id);
             model.ErrorInfo = userMassege;
 
             return View("Item", model);
         }
     }
 }