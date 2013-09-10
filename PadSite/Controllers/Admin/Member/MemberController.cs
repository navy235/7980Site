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
using PadSite.Filters;
namespace Maitonn.Web
{
    public class MemberController : Controller
    {
        //

        private IMemberService MemberService;
        private IGroupService GroupService;
        public MemberController(
          IMemberService MemberService
            , IGroupService GroupService)
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
        }

        #region KendoGrid Action
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var members = MemberService.GetKendoALL().ToList();
            return Json(members.ToDataSourceResult(request));
        }

        #endregion

        //#region Create Edit
        public ActionResult Create()
        {
            var groups = GetForeignData();
            ViewBag.Data_GroupID = groups;
            return View(new DetailsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailsViewModel model)
        {
            var groups = GetForeignData();
            ViewBag.Data_GroupID = groups;
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.Create(model);
                    result.Message = "添加会员信息成功！";
                    LogHelper.WriteLog("添加会员信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加会员信息错误", ex);
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

        public ActionResult Edit(int id)
        {

            var model = new EditViewModel();
            Member member = MemberService.GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == id);
            model.MemberID = member.MemberID;
            model.Email = member.Email;
            model.NickName = member.NickName;
            model.GroupID = member.GroupID;
            model.AvtarUrl = member.AvtarUrl;
            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }
            model.CityCode = member.Member_Profile.CityCodeValue;
            model.Sex = member.Member_Profile.Sex;
            model.Borthday = member.Member_Profile.Borthday;
            model.Description = member.Member_Profile.Description;

            List<int> GroupList = new List<int>();
            GroupList.Add(model.GroupID);
            ViewBag.Data_GroupID = GetForeignData(GroupList);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            List<int> GroupList = new List<int>();
            GroupList.Add(model.GroupID);
            var groups = GetForeignData(GroupList);
            ViewBag.Data_GroupID = groups;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.Update(model);
                    result.Message = "添加会员信息成功！";
                    LogHelper.WriteLog("添加会员信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加会员信息错误", ex);
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

        public List<SelectListItem> GetForeignData(List<int> selectIdList)
        {

            List<SelectListItem> data = new List<SelectListItem>();
            data = GroupService.GetALL().ToList().Select(x => new SelectListItem
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

        //#endregion

    }
}

