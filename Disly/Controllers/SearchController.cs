﻿using cms.dbase;
using Disly.Models;
using System;
using System.Web.Mvc;

namespace Disly.Controllers
{
    public class SearchController : RootController
    {
        public const String Name = "Error";
        public const String ActionName_Custom = "Custom";
        private TypePageViewModel model;


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            model = new TypePageViewModel
            {
                SitesInfo = siteModel,
                SiteMapArray = siteMapArray,
                Breadcrumbs= breadcrumb,
                BannerArray = bannerArray
            };
        }

        /// <summary>
        /// Сраница по умолчанию
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string searchtext)
        {
            #region Получаем данные из адресной строки
            string UrlPath = "/" + (String)RouteData.Values["path"];
            if (UrlPath.LastIndexOf("/") > 0 && UrlPath.LastIndexOf("/") == UrlPath.Length - 1) UrlPath = UrlPath.Substring(0, UrlPath.Length - 1);

            string _path = UrlPath.Substring(0, UrlPath.LastIndexOf("/") + 1);
            string _alias = UrlPath.Substring(UrlPath.LastIndexOf("/") + 1);
            #endregion
            ViewBag.SearchText = (searchtext!=null)?searchtext.Replace("%20", " "):String.Empty;

            model.Item = _repository.getSiteMap(_path,_alias,Domain);
            if (model.Item != null)
            {
                if (model.Item.FrontSection.ToLower() != "page")
                {
                    return Redirect("/" + model.Item.FrontSection);
                }
                model.Child = _repository.getSiteMapChild(model.Item.Id);
                model.Documents = _repository.getAttachDocuments(model.Item.Id);
            }

            #region Создаем переменные (значения по умолчанию)
            PageViewModel Model = new PageViewModel();
            string _ViewName = (ViewName != String.Empty) ? ViewName : "~/Views/Error/CustomError.cshtml";
            string PageTitle = (model.Item != null)? model.Item.Title: null;
            string PageDesc = "описание страницы";
            string PageKeyw = "ключевые слова";
            #endregion            
          
            #region Метатеги
            ViewBag.Title = "Поиск";
            ViewBag.Description = PageDesc;
            ViewBag.KeyWords = PageKeyw;
            #endregion

            return View(_ViewName,model);            
        }
    }
}
