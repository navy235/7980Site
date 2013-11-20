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
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
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
            ViewBag.MemberType = UIHelper.MemberTypeList;
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var members = MemberService.GetKendoALL().ToList();
            return Json(members.ToDataSourceResult(request));
        }

        #endregion


        public ActionResult Export()
        {

            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 10 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 30 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 30 * 256);
            sheet.SetColumnWidth(9, 10 * 256);

            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("会员 ID");
            headerRow.CreateCell(1).SetCellValue("邮箱地址");
            headerRow.CreateCell(2).SetCellValue("昵称");
            headerRow.CreateCell(3).SetCellValue("手机");
            headerRow.CreateCell(4).SetCellValue("用户类型");
            headerRow.CreateCell(5).SetCellValue("注册IP");
            headerRow.CreateCell(6).SetCellValue("注册时间");
            headerRow.CreateCell(7).SetCellValue("最后登录IP");
            headerRow.CreateCell(8).SetCellValue("最后登录时间");
            headerRow.CreateCell(9).SetCellValue("登录次数");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (Member member in MemberService.GetALL())
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(member.MemberID);
                row.CreateCell(1).SetCellValue(member.Email);
                row.CreateCell(2).SetCellValue(member.NickName);
                row.CreateCell(3).SetCellValue(member.Mobile);
                row.CreateCell(4).SetCellValue(UIHelper.MemberTypeList.Single(x => x.Value == member.MemberType.ToString()).Text);
                row.CreateCell(5).SetCellValue(member.AddIP);
                row.CreateCell(6).SetCellValue(member.AddTime.ToString("yyyy-MM-dd hh:mm:ss"));
                row.CreateCell(7).SetCellValue(member.LastIP);
                row.CreateCell(8).SetCellValue(member.LastTime.ToString("yyyy-MM-dd hh:mm:ss"));
                row.CreateCell(9).SetCellValue(member.LoginCount);
            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "会员信息.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

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

