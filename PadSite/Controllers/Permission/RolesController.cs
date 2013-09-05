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
    public class RolesController : Controller
    {
        //
        // GET: /Area/
        private IRolesService RolesService;
        private IPermissionsService PermissionsService;
        private IDepartmentService DepartmentService;
        public RolesController(
             IRolesService _RolesService,
         IPermissionsService _PermissionsService,
             IDepartmentService _DepartmentService
          )
        {
            RolesService = _RolesService;
            PermissionsService = _PermissionsService;
            DepartmentService = _DepartmentService;
        }


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = RolesService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            var permissions = GetGroupForeignData();
            ViewBag.Data_Permissions = permissions;
            return View(new RolesViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolesViewModel model)
        {
            var permissions = GetGroupForeignData();
            ViewBag.Data_Permissions = permissions;
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Roles entity = new Roles();
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    var permissionsArray = model.Permissions.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    var PermissionList = PermissionsService.GetALL().Where(x => permissionsArray.Contains(x.ID));
                    entity.Permissions.AddRange(PermissionList);
                    RolesService.Create(entity);
                    result.Message = "添加角色成功！";
                    LogHelper.WriteLog("添加角色成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加角色错误", ex);
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

            RolesViewModel model = new RolesViewModel();
            var entity = RolesService.GetALL().Include(x => x.Permissions).Single(x => x.ID == ID);
            model.Name = entity.Name;
            model.ID = entity.ID;
            model.Description = entity.Description;
            List<int> PermissionsList = new List<int>();
            PermissionsList = entity.Permissions.Select(x => x.ID).ToList();
            var permissions = GetGroupForeignData(PermissionsList);
            ViewBag.Data_Permissions = permissions;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolesViewModel model)
        {
            var permissionsArray = model.Permissions.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var permissions = GetGroupForeignData(permissionsArray);
            ViewBag.Data_Permissions = permissions;
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Roles entity = new Roles();
                    entity.ID = model.ID;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    var PermissionList = PermissionsService.GetALL().Where(x => permissionsArray.Contains(x.ID)).ToList();
                    entity.Permissions = PermissionList;
                    RolesService.Update(entity);
                    result.Message = "编辑角色成功！";
                    LogHelper.WriteLog("编辑角色成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加角色错误", ex);
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
                    var model = RolesService.Find(IdArr[i]);
                    RolesService.Delete(model);
                }
                LogHelper.WriteLog("删除角色成功");
                result.Message = "删除角色成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除角色错误！";
                result.AddServiceError("删除角色错误!");
                LogHelper.WriteLog("删除角色错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region private Method

        private Dictionary<string, List<SelectListItem>> GetGroupForeignData(List<int> selectIdList)
        {
            Dictionary<string, List<SelectListItem>> data = new Dictionary<string, List<SelectListItem>>();
            var parentList = DepartmentService.GetALL().Include(x => x.Permissions).ToList();
            foreach (var p in parentList)
            {
                List<SelectListItem> clist = new List<SelectListItem>();
                clist = p.Permissions.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ID.ToString(),
                    Selected = selectIdList.Contains(x.ID)

                }).ToList();
                data.Add(p.Name, clist);
            }
            return data;
        }

        private Dictionary<string, List<SelectListItem>> GetGroupForeignData()
        {
            return GetGroupForeignData(new List<int>());
        }

        #endregion

    }
}

