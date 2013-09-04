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
    public class MediaCateController : Controller
    {
        //
        // GET: /Area/
        private IMediaCateService MediaCateService;
        public MediaCateController(
             IMediaCateService _MediaCateService
          )
        {
            MediaCateService = _MediaCateService;
        }


        public ActionResult Index()
        {
            ViewBag.PID = GetSelectList();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var articles = MediaCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(articles.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_PID = GetSelectList();
            return View(new MediaCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MediaCateViewModel model)
        {
            ViewBag.Data_PID = GetSelectList();
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MediaCate entity = new MediaCate();
                    entity.CateName = model.CateName;
                    entity.PID = model.PID == 0 ? null : model.PID;
                    entity.Level = model.Level;
                    entity.OrderIndex = model.OrderIndex;
              
                    entity.Code = model.Code;
                    MediaCateService.Create(entity);
                    result.Message = "添加媒体分类成功！";
                    LogHelper.WriteLog("添加媒体分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加媒体分类错误", ex);
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



        public ActionResult Edit(int ID)
        {

            MediaCateViewModel model = new MediaCateViewModel();
            var entity = MediaCateService.Find(ID);
            model.CateName = entity.CateName;
            model.ID = entity.ID;
            model.Code = entity.Code;
        
            model.Level = entity.Level;
            model.OrderIndex = entity.OrderIndex;
            model.PID = entity.PID;
            ViewBag.Data_PID = GetSelectList(entity.PID.HasValue ? entity.PID.Value : 0);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MediaCateViewModel model)
        {

            ViewBag.Data_PID = GetSelectList(model.PID.HasValue ? model.PID.Value : 0);
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MediaCate entity = MediaCateService.Find(model.ID);
                    entity.CateName = model.CateName;
                    entity.PID = model.PID == 0 ? null : model.PID;
                    entity.Level = model.Level;
                    entity.OrderIndex = model.OrderIndex;
               
                    entity.Code = model.Code;
                    MediaCateService.Update(entity);
                    result.Message = "编辑媒体分类成功！";
                    LogHelper.WriteLog("编辑媒体分类成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加媒体分类错误", ex);
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

        private List<SelectListItem> GetSelectList(int value = 0)
        {
            var query = MediaCateService.GetALL().ToList();

            var list = Utilities.GetSelectListData(
                    MediaCateService.GetALL().ToList()
                    , item => item.ID
                    , item => item.CateName, true).ToList();

            list.Single(x => x.Value == value.ToString()).Selected = true;

            return list;
        }
        #endregion
    }
}

