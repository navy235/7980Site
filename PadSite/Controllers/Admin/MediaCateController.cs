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
    public class MediaCateController : Controller
    {
        //
        // GET: /Area/
        private IMediaCateService mediaService;
        public MediaCateController(
             IMediaCateService _mediaService
          )
        {
            mediaService = _mediaService;
        }

        #region KendoGrid Action

        public ActionResult Index()
        {
            ViewBag.PID = Utilities.CreateSelectList(
                mediaService.GetALL().ToList()
                , item => item.ID
                , item => item.CateName, true);
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var medias = mediaService.GetKendoALL().OrderBy(x => x.ID);
            return Json(medias.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<MediaCate> medias)
        {
            var results = new List<MediaCate>();

            if (medias != null && ModelState.IsValid)
            {
                foreach (var media in medias)
                {
                    if (media.PID.Value == 0)
                    {
                        media.PID = null;
                    }
                    mediaService.Create(media);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<MediaCate> medias)
        {
            if (medias != null && ModelState.IsValid)
            {
                foreach (var media in medias)
                {
                    mediaService.Update(media);
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<MediaCate> medias)
        {
            if (medias.Any())
            {
                foreach (var media in medias)
                {
                    mediaService.Delete(media);
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

    }
}

