using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.Utils;

namespace PadSite.Controllers
{
    public class AjaxServiceController : Controller
    {

        // GET: /Area/
        private ICityCateService cityCateService;
        private IArticleCateService articleCateService;
        public AjaxServiceController(
             ICityCateService _cityCateService,
             IArticleCateService _articleCateService
          )
        {
            cityCateService = _cityCateService;
            articleCateService = _articleCateService;
        }

        public ActionResult CityCode(int pid = 0)
        {
            var query = cityCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticleCode(int pid = 0)
        {
            var query = articleCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

    }
}
