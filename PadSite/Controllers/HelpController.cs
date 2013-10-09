﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Maitonn.Core;
using PadSite.Service.Interface;
using PadSite.Models;
using PadSite.ViewModels;
using PadSite.Utils;
using PadSite.Filters;

namespace PadSite.Controllers
{
    public class HelpController : Controller
    {
        private IArticleService ArticleService;
        private IArticleCateService ArticleCateService;
        public HelpController(
             IArticleService _ArticleService,
             IArticleCateService _ArticleCateService
          )
        {
            ArticleService = _ArticleService;
            ArticleCateService = _ArticleCateService;
        }


        public ActionResult Index(int id = 1)
        {
            HelpViewModel model = new HelpViewModel();

            model.Article = ArticleService.GetALL().Include(x => x.ArticleCate).Single(x => x.ID == id);
            if (model.Article == null)
            {
                return HttpNotFound();
            }
            model.HelpNav = GetHelpNav(id);
            return View(model);
        }


        private List<HelpNavViewModel> GetHelpNav(int articleID)
        {
            List<HelpNavViewModel> model = new List<HelpNavViewModel>();

            var categorys = ArticleCateService.GetKendoALL().ToList();

            foreach (var category in categorys)
            {
                HelpNavViewModel item = new HelpNavViewModel();
                item.Name = category.CateName;
                item.Items = ArticleService.GetALL().Where(x => x.ArticleCode == category.ID).Select(x => new HelpNavItemViewModel()
                {
                    Name = x.Name,
                    ID = x.ID

                }).ToList();
                model.Add(item);
            }

            return model;

        }

    }
}
