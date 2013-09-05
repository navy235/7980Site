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
    public class GroupController : Controller
    {
        //
        // GET: /Area/
        private IGroupService GroupService;
        private IRolesService RolesService;

        public GroupController(
             IGroupService _GroupService,
             IRolesService _RolesService

          )
        {
            GroupService = _GroupService;
            RolesService = _RolesService;
        }


        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var lists = GroupService.GetKendoALL().OrderBy(x => x.ID);
            return Json(lists.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            var Roles = GetForeignData();
            ViewBag.Data_Roles = Roles;
            return View(new GroupViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupViewModel model)
        {
            var Roles = GetForeignData();
            ViewBag.Data_Roles = Roles;
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Group entity = new Group();
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    var RolesArray = model.Roles.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    var RoleList = RolesService.GetALL().Where(x => RolesArray.Contains(x.ID));
                    entity.Roles.AddRange(RoleList);
                    GroupService.Create(entity);
                    result.Message = "添加群组成功！";
                    LogHelper.WriteLog("添加群组成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加群组错误", ex);
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

            GroupViewModel model = new GroupViewModel();
            var entity = GroupService.GetALL().Include(x => x.Roles).Single(x => x.ID == ID);
            model.Name = entity.Name;
            model.ID = entity.ID;
            model.Description = entity.Description;
            List<int> RolesList = new List<int>();
            RolesList = entity.Roles.Select(x => x.ID).ToList();
            var Roles = GetForeignData(RolesList);
            ViewBag.Data_Roles = Roles;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupViewModel model)
        {
            var RolesArray = model.Roles.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var Roles = GetForeignData(RolesArray);
            ViewBag.Data_Roles = Roles;
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    Group entity = new Group();
                    entity.ID = model.ID;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    var RoleList = RolesService.GetALL().Where(x => RolesArray.Contains(x.ID)).ToList();
                    entity.Roles = RoleList;
                    GroupService.Update(entity);
                    result.Message = "编辑群组成功！";
                    LogHelper.WriteLog("编辑群组成功");
                    return RedirectToAction("index");
                }
                catch (DbEntityValidationException ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加群组错误", ex);
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
                    var model = GroupService.Find(IdArr[i]);
                    GroupService.Delete(model);
                }
                LogHelper.WriteLog("删除群组成功");
                result.Message = "删除群组成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除群组错误！";
                result.AddServiceError("删除群组错误!");
                LogHelper.WriteLog("删除群组错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region private Method

        public List<SelectListItem> GetForeignData(List<int> selectIdList)
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data = RolesService.GetALL().ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString(),
                Selected = selectIdList.Contains(x.ID)
            }).ToList();
            return data;
        }

        public List<SelectListItem> GetForeignData()
        {
            return GetForeignData(new List<int>());
        }
        #endregion

    }
}

