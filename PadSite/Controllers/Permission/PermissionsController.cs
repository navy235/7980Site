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
    public class PermissionsController : Controller
    {
        //
        // GET: /Area/
        private IPermissionsService PermissionsService;
        private IDepartmentService DepartmentService;
        public PermissionsController(
             IPermissionsService _PermissionsService,
             IDepartmentService _DepartmentService
          )
        {
            PermissionsService = _PermissionsService;
            DepartmentService = _DepartmentService;
        }


        public ActionResult Index()
        {
            ViewBag.DepartmentID = GetSelectList();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = PermissionsService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_DepartmentID = GetSelectList();
            return View(new PermissionsViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PermissionsViewModel model)
        {
            ViewBag.Data_DepartmentID = GetSelectList();
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Permissions entity = new Permissions();
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Controller = model.Controller;
                    entity.Action = model.Action;
                    entity.Namespace = model.Namespace;
                    entity.DepartmentID = model.DepartmentID;
                    PermissionsService.Create(entity);
                    result.Message = "添加权限成功！";
                    LogHelper.WriteLog("添加权限成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加权限错误", ex);
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

            PermissionsViewModel model = new PermissionsViewModel();
            var entity = PermissionsService.Find(ID);
            model.Name = entity.Name;
            model.ID = entity.ID;
            model.Description = entity.Description;
            model.Action = entity.Action;
            model.Controller = entity.Controller;
            model.Namespace = entity.Namespace;
            model.DepartmentID = entity.DepartmentID;
            ViewBag.Data_DepartmentID = GetSelectList(entity.DepartmentID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PermissionsViewModel model)
        {
            ViewBag.Data_DepartmentID = GetSelectList(model.DepartmentID);
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Permissions entity = new Permissions();
                    entity.ID = model.ID;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Controller = model.Controller;
                    entity.Action = model.Action;
                    entity.Namespace = model.Namespace;
                    entity.DepartmentID = model.DepartmentID;
                    PermissionsService.Update(entity);
                    result.Message = "编辑权限成功！";
                    LogHelper.WriteLog("编辑权限成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加权限错误", ex);
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
                    var model = PermissionsService.Find(IdArr[i]);
                    PermissionsService.Delete(model);
                }
                LogHelper.WriteLog("删除权限成功");
                result.Message = "删除权限成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除权限错误！";
                result.AddServiceError("删除权限错误!");
                LogHelper.WriteLog("删除权限错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region private Method

        private List<SelectListItem> GetSelectList(int value = 0)
        {
            var list = Utilities.GetSelectListData(
                    DepartmentService.GetALL().ToList()
                    , item => item.ID
                    , item => item.Name, true).ToList();

            list.Single(x => x.Value == value.ToString()).Selected = true;

            return list;
        }
        #endregion
    }
}

