﻿using cms.dbModel.entity;
using Disly.Areas.Admin.Models;
using Disly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Disly.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Pager(Pager Model, string startUrl, string viewName = "Services/Pager")
        {
            ViewBag.PagerSize = string.IsNullOrEmpty(Request.QueryString["size"]) ? Model.size.ToString() : Request.QueryString["size"];
            string qwer = String.Empty;

            int PagerLinkSize = 2;

            int FPage = (Model.page - PagerLinkSize < 1) ? 1 : Model.page - PagerLinkSize;
            int LPage = (Model.page + PagerLinkSize > Model.page_count) ? Model.page_count : Model.page + PagerLinkSize;

            if (String.IsNullOrEmpty(startUrl)) startUrl = Request.Url.Query;

            if (FPage > 1)
            {
                qwer = qwer + "1,";
            }
            if (FPage > 2)
            {
                qwer = qwer + "*,";
            }
            for (int i = FPage; i < LPage + 1; i++)
            {
                qwer = (@i < Model.page_count) ? qwer + @i + "," : qwer + @i;
            }
            if (LPage < Model.page_count - 1)
            {
                qwer = qwer + "*,";
            }
            if (Model.page_count > LPage)
            {
                qwer = qwer + @Model.page_count;
            }


            var viewModel = qwer.Split(',').
                Where(w => w != String.Empty).
                Select(s => new PagerFront
                {
                    text = (s == "*") ? "..." : s,
                    url = (s == "*") ? String.Empty : addFiltrParam(startUrl, "page", s),
                    isChecked = (s == Model.page.ToString())
                }).ToArray();

            if (viewModel.Length < 2) viewModel = null;

            return View(viewName, viewModel);
        }
        public string addFiltrParam(string query, string name, string val)
        {
            //string search_Param = @"\b" + name + @"=[\w]*[\b]*&?";
            string search_Param = @"\b" + name + @"=(.*?)(&|$)";
            string normal_Query = @"&$";

            Regex delParam = new Regex(search_Param, RegexOptions.CultureInvariant);
            Regex normalQuery = new Regex(normal_Query);
            query = delParam.Replace(query, String.Empty);
            query = normalQuery.Replace(query, String.Empty);

            if (val != String.Empty)
            {
                if (query.IndexOf("?") > -1) query += "&" + name + "=" + val;
                else query += "?" + name + "=" + val;
            }

            query = query.Replace("?&", "?").Replace("&&", "&");

            return query;
        }
    }
}