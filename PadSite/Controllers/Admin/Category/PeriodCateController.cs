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
    public class PeriodCateController : Controller
    {
        //
        // GET: /Area/
        private IPeriodCateService PeriodCateService;
        public PeriodCateController(
             IPeriodCateService _PeriodCateService
          )
        {
            PeriodCateService = _PeriodCateService;
        }


        public ActionResult Index()
        {
            ViewBag.PID = GetSelectList();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = PeriodCateService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_PID = GetSelectList();
            return View(new PeriodCateViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PeriodCateViewModel model)
        {
            ViewBag.Data_PID = GetSelectList();
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    PeriodCate entity = new PeriodCate();
                    entity.CateName = model.CateName;
                    entity.PID = model.PID == 0 ? null : model.PID;
                    entity.Level = model.Level;
                    entity.OrderIndex = model.OrderIndex;

                    entity.Code = model.Code;
                    PeriodCateService.Create(entity);
                    result.Message = "添加购买周期成功！";
                    LogHelper.WriteLog("添加购买周期成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加购买周期错误", ex);
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

            PeriodCateViewModel model = new PeriodCateViewModel();
            var entity = PeriodCateService.Find(ID);
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
        public ActionResult Edit(PeriodCateViewModel model)
        {

            ViewBag.Data_PID = GetSelectList(model.PID.HasValue ? model.PID.Value : 0);
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    PeriodCate entity = new PeriodCate();
                    entity.ID = model.ID;
                    entity.CateName = model.CateName;
                    entity.PID = model.PID == 0 ? null : model.PID;
                    entity.Level = model.Level;
                    entity.OrderIndex = model.OrderIndex;

                    entity.Code = model.Code;
                    PeriodCateService.Update(entity);
                    result.Message = "编辑购买周期成功！";
                    LogHelper.WriteLog("编辑购买周期成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加购买周期错误", ex);
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


        [HttpPost]
        public ActionResult Delete(string ids)
        {
            ServiceResult result = new ServiceResult();
            var IdArr = ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            try
            {
                for (var i = 0; i < IdArr.Count; i++)
                {
                    var model = PeriodCateService.Find(IdArr[i]);
                    PeriodCateService.Delete(model);
                }
                LogHelper.WriteLog("删除购买周期成功");
                result.Message = "删除购买周期成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除购买周期错误！";
                result.AddServiceError("删除购买周期错误!");
                LogHelper.WriteLog("删除购买周期错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region private Method

        private List<SelectListItem> GetSelectList(int value = 0)
        {

            var list = Utilities.GetSelectListData(
                    PeriodCateService.GetALL().ToList()
                    , item => item.ID
                    , item => item.CateName, true).ToList();

            if (value != 0)
            {
                list.Single(x => x.Value == value.ToString()).Selected = true;
            }

            return list;
        }
        #endregion
    }
}

