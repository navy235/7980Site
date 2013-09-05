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
    public class DepartmentController : Controller
    {
        //
        // GET: /Area/
        private IDepartmentService DepartmentService;
        public DepartmentController(
             IDepartmentService _DepartmentService
          )
        {
            DepartmentService = _DepartmentService;
        }


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = DepartmentService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {

            return View(new DepartmentViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Department entity = new Department();
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Leader = model.Leader;
                    DepartmentService.Create(entity);
                    result.Message = "添加部门成功！";
                    LogHelper.WriteLog("添加部门成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加部门错误", ex);
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

            DepartmentViewModel model = new DepartmentViewModel();
            var entity = DepartmentService.Find(ID);
            model.Name = entity.Name;
            model.ID = entity.ID;
            model.Description = entity.Description;
            model.Leader = entity.Leader;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentViewModel model)
        {

            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Department entity = new Department();
                    entity.ID = model.ID;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Leader = model.Leader;
                    DepartmentService.Update(entity);
                    result.Message = "编辑部门成功！";
                    LogHelper.WriteLog("编辑部门成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加部门错误", ex);
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
                    var model = DepartmentService.Find(IdArr[i]);
                    DepartmentService.Delete(model);
                }
                LogHelper.WriteLog("删除部门成功");
                result.Message = "删除部门成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除部门错误！";
                result.AddServiceError("删除部门错误!");
                LogHelper.WriteLog("删除部门错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}

