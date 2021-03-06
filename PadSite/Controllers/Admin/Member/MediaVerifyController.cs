﻿using System;
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

namespace PadSite.Controllers
{


    public class MediaVerifyController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private IMember_ActionService Member_ActionService;
        private IMediaCateService MediaCateService;
        private ICityCateService CityCateService;
        private IOutDoorService OutDoorService;
        private IIndustryCateService IndustryCateService;
        private ICrowdCateService CrowdCateService;
        private IOwnerCateService OwnerCateService;
        private IAreaCateService AreaCateService;
        private IPurposeCateService PurposeCateService;
        private IFormatCateService FormatCateService;
        private IPeriodCateService PeriodCateService;
        private ICompanyService CompanyService;
        public MediaVerifyController(
              IMemberService MemberService
            , IEmailService EmailService
            , IMember_ActionService Member_ActionService
            , IOutDoorService OutDoorService
            , IIndustryCateService IndustryCateService,
            ICrowdCateService CrowdCateService,
            IOwnerCateService OwnerCateService,
            IAreaCateService AreaCateService,
            IPurposeCateService PurposeCateService,
            IFormatCateService FormatCateService,
            IPeriodCateService PeriodCateService,
            ICityCateService CityCateService,
            IMediaCateService MediaCateService,
            ICompanyService CompanyService)
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.Member_ActionService = Member_ActionService;
            this.OutDoorService = OutDoorService;
            this.IndustryCateService = IndustryCateService;
            this.CrowdCateService = CrowdCateService;
            this.OwnerCateService = OwnerCateService;
            this.AreaCateService = AreaCateService;
            this.PurposeCateService = PurposeCateService;
            this.FormatCateService = FormatCateService;
            this.PeriodCateService = PeriodCateService;
            this.CityCateService = CityCateService;
            this.MediaCateService = MediaCateService;
            this.CompanyService = CompanyService;
        }

        public ActionResult Index()
        {
            ViewBag.OutDoorStatus = UIHelper.OutDoorStatusList;
            return View();
        }


        public ActionResult OutDoor_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
                .Where(x => x.Status == (int)OutDoorStatus.PreVerify)
                .Select(x => new OutDoorItemViewModel()
                 {
                     AddTime = x.AddTime,
                     ID = x.ID,
                     MediaFocusImg = x.MediaFoucsImg,
                     Name = x.Name,
                     Status = x.Status
                 });
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult Authed()
        {
            ViewBag.OutDoorStatus = UIHelper.OutDoorStatusList;
            ViewBag.SuggestStatus = UIHelper.SuggestStatusList;
            return View();
        }

        public ActionResult OutDoor_ReadAuthed([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
            .Where(x => x.Status >= (int)OutDoorStatus.Verified)
            .Select(x => new OutDoorItemViewModel()
            {
                AddTime = x.AddTime,
                ID = x.ID,
                MediaFocusImg = x.MediaFoucsImg,
                Name = x.Name,
                SuggestStatus = x.SuggestStatus,
                Status = x.Status
            });
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult VerifyPass(string ids)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeStatus(ids, OutDoorStatus.Verified);
                result.Message = "媒体信息审核通过成功！";
            }
            catch (Exception ex)
            {
                result.Message = "媒体信息审核通过失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体信息审核通过失败!", ex);
            }
            return Json(result);

        }

        public ActionResult VerifyFailed(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeStatus(ids, OutDoorStatus.VerifyFailed);
                result.Message = "媒体信息审核不通过操作成功！";
            }
            catch (Exception ex)
            {
                result.Message = "媒体信息审核不通过操作失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体信息审核不通过操作失败!", ex);
            }
            return Json(result);
        }
        public ActionResult VerifyFailedWithReason(string ids, string reason)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeStatus(ids, OutDoorStatus.VerifyFailed, reason);
                result.Message = "媒体信息审核不通过操作成功！";
            }
            catch (Exception ex)
            {
                result.Message = "媒体信息审核不通过操作失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体信息审核不通过操作失败!", ex);
            }
            return Json(result);
        }

        public ActionResult SetSuggest(string ids, int reason)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeSuggestStatus(ids, reason);
                result.Message = "设置媒体信息推荐状态操作成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置媒体信息推荐状态操作失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置媒体信息推荐状态操作失败!", ex);
            }
            return Json(result);
        }

        public ActionResult Details(int id)
        {
            OutDoor entity = OutDoorService.GetALL()
                //.Include(x => x.AreaCate)
                //.Include(x => x.CrowdCate)
                //.Include(x => x.IndustryCate)
                //.Include(x => x.PurposeCate)
                    .Single(x => x.ID == id);

            OutDoorViewModel model = new OutDoorViewModel()
            {
                Name = entity.Name,
                MediaCode = entity.MediaCodeValue,
                CityCode = entity.CityCodeValue,
                //CredentialsImg = entity.CredentialsImg,
                MediaImg = entity.MediaImg,
                Description = entity.Description,
                Deadline = entity.Deadline,
                HasLight = entity.HasLight,
                //AreaCate = String.Join(",", entity.AreaCate.Select(x => x.ID)),
                //CrowdCate = String.Join(",", entity.CrowdCate.Select(x => x.ID)),
                //IndustryCate = String.Join(",", entity.IndustryCate.Select(x => x.ID)),
                //PurposeCate = String.Join(",", entity.PurposeCate.Select(x => x.ID)),
                FormatCode = entity.FormatCode,
                //OwnerCode = entity.OwnerCode,
                Price = entity.Price,
                RealPrice = entity.RealPrice,
                //PriceExten = entity.PriceExten,
                ID = entity.ID,
                Location = entity.Location,
                //PeriodCode = entity.PeriodCode,
                Position = entity.Lat + "|" + entity.Lng,
                TrafficAuto = entity.TrafficAuto,
                TrafficPerson = entity.TrafficPerson,
                VideoUrl = entity.VideoUrl
            };

            if (entity.HasLight)
            {
                model.LightTime = entity.LightStart + "|" + entity.LightEnd;
            }
            if (entity.IsRegular)
            {
                model.MediaArea = "true|" + entity.Wdith + "|" + entity.Height + "|" + entity.TotalFaces;
            }
            else
            {
                model.MediaArea = entity.IrRegularArea;
            }

            var AreaCateArray = new List<int>();
            var CrowdCateArray = new List<int>();
            var IndustryCateArray = new List<int>();
            var PurposeCateArray = new List<int>();
            //if (!string.IsNullOrEmpty(model.AreaCate))
            //{
            //    AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            //if (!string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //ViewBag.Data_CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, CrowdCateArray, false);
            //ViewBag.Data_IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, IndustryCateArray, false);
            //ViewBag.Data_PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, PurposeCateArray, false);
            //ViewBag.Data_AreaCate = Utilities.GetSelectListData(AreaCateService.GetALL(), x => x.ID, x => x.CateName, AreaCateArray, false);
            var cityIds = model.CityCode.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            ViewBag.Data_CityCode = cityValues;

            var meidaIds = model.MediaCode.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var meidaValues = MediaCateService.GetALL().Where(x => meidaIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            ViewBag.Data_MediaCode = meidaValues;
            return View(model);
        }


        public ActionResult Add()
        {
            ViewBag.Data_FormatCode = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, true);
            //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, false);
            return View(new OutDoorAdminViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(OutDoorAdminViewModel model)
        {

            ServiceResult result = new ServiceResult();

            var AreaCateArray = new List<int>();
            var CrowdCateArray = new List<int>();
            var IndustryCateArray = new List<int>();
            var PurposeCateArray = new List<int>();
            //if (!string.IsNullOrEmpty(model.AreaCate))
            //{
            //    AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            //if (!string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            TempData["Service_Result"] = result;

            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    OutDoorService.Create(model);
                    result.Message = "后台添加户外成功！";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = "后台添加户外失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("后台用户:" + CookieHelper.MemberID + "添加户外失败!", ex);
                }

            }

            //ViewBag.Data_CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, CrowdCateArray, false);
            //ViewBag.Data_IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, IndustryCateArray, false);
            //ViewBag.Data_PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, PurposeCateArray, false);
            //ViewBag.Data_AreaCate = Utilities.GetSelectListData(AreaCateService.GetALL(), x => x.ID, x => x.CateName, AreaCateArray, false);
            ViewBag.Data_FormatCode = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, true);
            //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, false);

            return View(model);
        }


        public ActionResult Edit(int id)
        {
            OutDoor entity = OutDoorService.GetALL()
                //.Include(x => x.AreaCate)
                //.Include(x => x.CrowdCate)
                //.Include(x => x.IndustryCate)
                //.Include(x => x.PurposeCate)
                       .Single(x => x.ID == id);

            OutDoorAdminViewModel model = new OutDoorAdminViewModel()
            {
                Name = entity.Name,
                MemberID = entity.MemberID,
                MediaCode = entity.MediaCodeValue,
                CityCode = entity.CityCodeValue,
                //CredentialsImg = entity.CredentialsImg,
                MediaImg = entity.MediaImg,
                Description = entity.Description,
                Deadline = entity.Deadline,
                HasLight = entity.HasLight,
                //AreaCate = String.Join(",", entity.AreaCate.Select(x => x.ID)),
                //CrowdCate = String.Join(",", entity.CrowdCate.Select(x => x.ID)),
                //IndustryCate = String.Join(",", entity.IndustryCate.Select(x => x.ID)),
                //PurposeCate = String.Join(",", entity.PurposeCate.Select(x => x.ID)),
                FormatCode = entity.FormatCode,
                //OwnerCode = entity.OwnerCode,
                Price = entity.Price,
                RealPrice = entity.RealPrice,
                //PriceExten = entity.PriceExten,
                ID = entity.ID,
                Location = entity.Location,
                //PeriodCode = entity.PeriodCode,
                Position = entity.Lat + "|" + entity.Lng,
                TrafficAuto = entity.TrafficAuto,
                TrafficPerson = entity.TrafficPerson,
                VideoUrl = entity.VideoUrl
            };

            if (entity.HasLight)
            {
                model.LightTime = entity.LightStart + "|" + entity.LightEnd;
            }
            if (entity.IsRegular)
            {
                model.MediaArea = "true|" + entity.Wdith + "|" + entity.Height + "|" + entity.TotalFaces;
            }
            else
            {
                model.MediaArea = entity.IrRegularArea;
            }

            var AreaCateArray = new List<int>();
            var CrowdCateArray = new List<int>();
            var IndustryCateArray = new List<int>();
            var PurposeCateArray = new List<int>();
            //if (!string.IsNullOrEmpty(model.AreaCate))
            //{
            //    AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            //if (!string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}


            //ViewBag.Data_CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, CrowdCateArray, false);
            //ViewBag.Data_IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, IndustryCateArray, false);
            //ViewBag.Data_PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, PurposeCateArray, false);
            //ViewBag.Data_AreaCate = Utilities.GetSelectListData(AreaCateService.GetALL(), x => x.ID, x => x.CateName, AreaCateArray, false);
            ViewBag.Data_FormatCode = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, model.FormatCode, true);
            //ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, model.PeriodCode, true);
            //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, model.OwnerCode, false);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OutDoorAdminViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            var AreaCateArray = new List<int>();
            var CrowdCateArray = new List<int>();
            var IndustryCateArray = new List<int>();
            var PurposeCateArray = new List<int>();
            //if (!string.IsNullOrEmpty(model.AreaCate))
            //{
            //    AreaCateArray = model.AreaCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.CrowdCate))
            //{
            //    CrowdCateArray = model.CrowdCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}
            //if (!string.IsNullOrEmpty(model.IndustryCate))
            //{
            //    IndustryCateArray = model.IndustryCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            //if (!string.IsNullOrEmpty(model.PurposeCate))
            //{
            //    PurposeCateArray = model.PurposeCate.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            //}

            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    OutDoorService.Update(model);
                    result.Message = "后台编辑户外成功！";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = "后台编辑户外失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("后台用户:" + CookieHelper.MemberID + "编辑户外失败!", ex);
                }
            }
            //ViewBag.Data_CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, CrowdCateArray, false);
            //ViewBag.Data_IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, IndustryCateArray, false);
            //ViewBag.Data_PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, PurposeCateArray, false);
            //ViewBag.Data_AreaCate = Utilities.GetSelectListData(AreaCateService.GetALL(), x => x.ID, x => x.CateName, AreaCateArray, false);
            ViewBag.Data_FormatCode = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, true);
            //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, false);
            return View(model);
        }


        public ActionResult getcompanylist([DataSourceRequest] DataSourceRequest request)
        {
            var memberID = Convert.ToInt32(CookieHelper.UID);
            var model = CompanyService.GetVerifyList(CompanyStatus.CompanyAuth).Select(x => new CompanyListItemViewModel
            {
                MemberID = x.MemberID,
                Name = x.Name
            });
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult getcompanyName(int id)
        {
            return Content(CompanyService.Find(id).Name);
        }
    }
}
