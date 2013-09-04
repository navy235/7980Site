using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Maitonn.Core;
using PadSite.Service.Interface;
using PadSite.Models;
using PadSite.Utils;

namespace PadSite.Controllers
{
    public class CityCateController : Controller
    {
        //
        // GET: /Area/
        private ICityCateService cityService;
        public CityCateController(
             ICityCateService _cityService
          )
        {
            cityService = _cityService;
        }

        #region KendoGrid Action

        public ActionResult Index()
        {
            ViewBag.PID = Utilities.CreateSelectList(
                cityService.GetALL().ToList()
                , item => item.ID
                , item => item.CateName, true);
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var citys = cityService.GetKendoALL().OrderBy(x => x.ID);
            return Json(citys.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityCate> citys)
        {
            var results = new List<CityCate>();

            if (citys != null && ModelState.IsValid)
            {
                foreach (var city in citys)
                {
                    if (city.PID.Value == 0)
                    {
                        city.PID = null;
                    }
                    cityService.Create(city);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityCate> citys)
        {
            if (citys != null && ModelState.IsValid)
            {
                foreach (var city in citys)
                {
                    cityService.Update(city);
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CityCate> citys)
        {
            if (citys.Any())
            {
                foreach (var city in citys)
                {
                    cityService.Delete(city);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

    }
}

