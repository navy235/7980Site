using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Entity;
using PadSite.ViewModels;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Filters;
using PadSite.Utils;
using PadSite.Service.Interface;
using PadSite.Setting;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PadSite.Service;
namespace Maitonn.Web
{
    public class MapController : Controller
    {


        private IMediaCateService MediaCateService;


        public MapController(
         IMediaCateService MediaCateService
            )
        {
            this.MediaCateService = MediaCateService;
        }


        public ActionResult Index()
        {

            var categoryList = Utilities.GetSelectListData(MediaCateService.GetALL().Where(x => x.PID.Equals(null)), x => x.ID, x => x.CateName, false);

            categoryList.Insert(0, new SelectListItem()
            {
                Selected = true,
                Text = "媒体分类",
                Value = "0"
            });

            var priceList = UIHelper.PriceList;

            priceList.Single(x => x.Value == "0").Selected = true;

            ViewBag.categoryList = categoryList;

            ViewBag.priceList = priceList;

            return View();
        }



    }
}
