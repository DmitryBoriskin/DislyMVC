﻿using cms.dbModel.entity;
using Disly.Models;
using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Disly.Controllers
{
    public class PortalDoctorsController : RootController
    {
        private PortalDoctorsViewModel model;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            model = new PortalDoctorsViewModel
            {
                SitesInfo = siteModel,
                SiteMapArray = siteMapArray,
                Breadcrumbs = breadcrumb,
                BannerArray = bannerArray,
                CurrentPage = currentPage
            };

            #region Создаем переменные (значения по умолчанию)
            ViewBag.Title = "Страница";
            ViewBag.Description = "Страница без названия";
            ViewBag.KeyWords = "";
            #endregion
        }

        // GET: PortalDoctors
        public ActionResult Index()
        {
            #region currentPage
            currentPage = _repository.getSiteMap("PortalDoctors");
            if (currentPage == null)
                throw new Exception("model.CurrentPage == null");

            if (currentPage != null)
            {
                ViewBag.Title = currentPage.Title;
                ViewBag.Description = currentPage.Desc;
                ViewBag.KeyWords = currentPage.Keyw;

                model.CurrentPage = currentPage;
            }
            #endregion

            var filter = getFilter();
            model.DoctorsList = _repository.getDoctorsList(filter);
            model.PeoplePosts = _repository.getPeoplePosts();

            ViewBag.SearchText = filter.SearchText;
            ViewBag.Position = filter.Type;

            return View(model);
        }

        // GET: portaldoctors/id
        public ActionResult Item(Guid id)
        {
            #region currentPage
            currentPage = _repository.getSiteMap("PortalDoctors");
            //if (currentPage == null)
                //throw new Exception("model.CurrentPage == null");

            if (currentPage != null)
            {
                ViewBag.Title = currentPage.Title;
                ViewBag.Description = currentPage.Desc;
                ViewBag.KeyWords = currentPage.Keyw;

                model.CurrentPage = currentPage;
            }
            #endregion

            string _ViewName = (ViewName != String.Empty) ? ViewName : "~/Views/Error/CustomError.cshtml";

            model.DoctorsItem = _repository.getPeopleItem(id);

            // десериализация xml
            XmlSerializer serializer = new XmlSerializer(typeof(Employee));

            using (TextReader reader = new StringReader(model.DoctorsItem.XmlInfo))
            {
                var result = (Employee)serializer.Deserialize(reader);
                model.DoctorsItem.EmployeeInfo = result;
            }

            return View("Index", model);
        }
    }
}