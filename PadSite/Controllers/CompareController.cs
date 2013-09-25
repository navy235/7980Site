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

namespace PadSite.Controllers
{
    public class CompareController : Controller
    {
        //
        // GET: /Compare/

        private IOutDoorLuceneService OutDoorLuceneService;

        public CompareController(
            IOutDoorLuceneService OutDoorLuceneService
            )
        {
            this.OutDoorLuceneService = OutDoorLuceneService;
        }

        public ActionResult Index(string id)
        {
            var reg = new Regex("(\\d+)(,(\\d+)){0,4}");
            if (!reg.IsMatch(id))
            {
                return Content("<script>alert('您输入的地址有误!');window.histroy.go(-1);</script>");
            }
            var IdArr = Utilities.GetIdList(id).Distinct();
            var model = OutDoorLuceneService.Search(IdArr);
            return View(model);
        }

    }
}
