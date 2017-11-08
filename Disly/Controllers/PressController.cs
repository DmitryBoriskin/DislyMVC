﻿using cms.dbase;
using Disly.Models;
using System;
using System.Web.Mvc;

namespace Disly.Controllers
{
    public class PressController : RootController
    {
        private NewsViewModel model;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            model = new NewsViewModel
            {
                SitesInfo = siteModel,
                SiteMapArray = siteMapArray,
                BannerArray = bannerArray,
                Group=_repository.getMaterialsGroup()
            };
        }

        /// <summary>
        /// Сраница по умолчанию
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Category = (RouteData.Values["category"] != null) ? RouteData.Values["category"] : String.Empty;
            var filter = getFilter();
            filter.Disabled = false;
            model.List = _repository.getMaterialsList(filter);

            #region Создаем переменные (значения по умолчанию)            
            string _ViewName = (ViewName != String.Empty) ? ViewName : "~/Views/Error/CustomError.cshtml";

            string PageTitle = "новости";
            string PageDesc = "описание страницы";
            string PageKeyw = "ключевые слова";
            #endregion            
            #region Метатеги
            ViewBag.Title = PageTitle;
            ViewBag.Description = PageDesc;
            ViewBag.KeyWords = PageKeyw;
            #endregion
            return View(_ViewName, model);
        }

        public ActionResult Item(string year, string month, string day, string alias)
        {
            ViewBag.Day = day.ToString();
            ViewBag.Alias = (RouteData.Values["alias"] != null) ? RouteData.Values["alias"] : String.Empty;
            model.Item = _repository.getMaterialsItem(year, month, day, alias,Domain);

            #region Создаем переменные (значения по умолчанию)            
            string _ViewName = (ViewName != String.Empty) ? ViewName : "~/Views/Error/CustomError.cshtml";

            string PageTitle = "новости";
            string PageDesc = "описание страницы";
            string PageKeyw = "ключевые слова";
            #endregion


            #region Метатеги
            ViewBag.Title = PageTitle;
            ViewBag.Description = PageDesc;
            ViewBag.KeyWords = PageKeyw;
            #endregion

            return View(_ViewName, model);
        }


        public ActionResult Category(string category)
        {
            ViewBag.CurrentCategory = category;         
            var filter = getFilter();
            filter.Disabled = false;
            filter.Category = category;
            model.List = _repository.getMaterialsList(filter);

            #region Создаем переменные (значения по умолчанию)            
            string _ViewName = (ViewName != String.Empty) ? ViewName : "~/Views/Error/CustomError.cshtml";

            string PageTitle = "новости";
            string PageDesc = "описание страницы";
            string PageKeyw = "ключевые слова";
            #endregion            
            #region Метатеги
            ViewBag.Title = PageTitle;
            ViewBag.Description = PageDesc;
            ViewBag.KeyWords = PageKeyw;
            #endregion
            return View(_ViewName, model);
        }

    }
}

