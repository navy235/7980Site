using System;
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

namespace PadSite.Controllers
{
    public class ArticleCateController : Controller
    {
        //
        // GET: /Area/
        private IArticleCateService articleCateService;
        public ArticleCateController(
             IArticleCateService _articleCateService
          )
        {
            articleCateService = _articleCateService;
        }


        public ActionResult Index()
        {
            ViewBag.PID = GetSelectList();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var articles = articleCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(articles.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_PID = GetSelectList();
            return View(new ArticleCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleCateViewModel model)
        {
            ViewBag.Data_PID = GetSelectList();
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    ArticleCate entity = new ArticleCate();
                    entity.CateName = model.CateName;
                    entity.PID = model.PID;
                    entity.Level = model.Level;
                    entity.OrderIndex = model.OrderIndex;
                    entity.IsSingle = model.IsSingle;
                    entity.Code = model.Code;
                    articleCateService.Create(entity);
                    result.Message = "添加文章分类成功！";
                    LogHelper.WriteLog("添加文章分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加文章分类错误", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");
                return View(model);
            }
        }


        #region private Method

        private SelectList GetSelectList()
        {
            var list = Utilities.CreateSelectList(
                    articleCateService.GetALL().ToList()
                    , item => item.ID
                    , item => item.CateName, true);
            return list;
        }
        #endregion
    }
}

