using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

namespace Maitonn.Web
{
    [LoginAuthorize]
    public class FavoriteController : Controller
    {
        //
        // GET: /Favorite/



        private IFavoriteService FavoriteService;
        private IOutDoorService OutDoorService;
        public FavoriteController(
            IFavoriteService FavoriteService
            , IOutDoorService OutDoorService
            )
        {
            this.FavoriteService = FavoriteService;
            this.OutDoorService = OutDoorService;
        }

        public ActionResult Index()
        {
            ViewBag.MenuItem = "favorite-media";
            return View();
        }

        public ActionResult Favorite_Read([DataSourceRequest] DataSourceRequest request)
        {
            var memberID = CookieHelper.MemberID;

            var model = (from f in FavoriteService.GetALL()
                         join o in OutDoorService.GetALL().Where(x => x.Status >= (int)OutDoorStatus.ShowOnline) on f.MediaID equals o.ID
                         where f.MemberID == memberID
                         orderby f.AddTime descending
                         select new FavoriteViewModel
                         {
                             ID = f.ID,
                             MediaID = o.ID,
                             MemberID = f.MemberID,
                             Name = o.Name,
                             ImgUrl = o.MediaFoucsImg,
                             AddTime = f.AddTime
                         });
            return Json(model.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult FavoriteDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var idsList = Utilities.GetIdList(ids);
                foreach (var id in idsList)
                {
                    var model = FavoriteService.Find(id);
                    FavoriteService.Delete(model);
                }
                result.Message = "删除收藏成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除收藏失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除收藏失败!", ex);
            }
            return Json(result);
        }


    }
}
