using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PadSite.ViewModels;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Filters;
using PadSite.Utils;
using PadSite.Service.Interface;
using PadSite.Setting;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace PadSite.Controllers
{
    [LoginAuthorize]
    public class OutDoorController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        private ICityCateService CityCateService;
        private ICompanyCredentialsImgService CompanyCredentialsImgService;
        private ICompanyNoticeService CompanyNoticeService;
        private ICompanyMessageService CompanyMessageService;
        private IOutDoorService OutDoorService;
        private IIndustryCateService IndustryCateService;
        private ICrowdCateService CrowdCateService;
        private IOwnerCateService OwnerCateService;
        private IAreaCateService AreaCateService;
        private IPurposeCateService PurposeCateService;
        private IFormatCateService FormatCateService;
        private IPeriodCateService PeriodCateService;

        public OutDoorController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService,
            ICityCateService CityCateService,
            ICompanyCredentialsImgService CompanyCredentialsImgService,
            ICompanyNoticeService CompanyNoticeService,
            ICompanyMessageService CompanyMessageService,
            IOutDoorService OutDoorService,
            IIndustryCateService IndustryCateService,
            ICrowdCateService CrowdCateService,
            IOwnerCateService OwnerCateService,
            IAreaCateService AreaCateService,
            IPurposeCateService PurposeCateService,
            IFormatCateService FormatCateService,
            IPeriodCateService PeriodCateService


            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
            this.CityCateService = CityCateService;
            this.CompanyCredentialsImgService = CompanyCredentialsImgService;
            this.CompanyNoticeService = CompanyNoticeService;
            this.CompanyMessageService = CompanyMessageService;
            this.OutDoorService = OutDoorService;
            this.IndustryCateService = IndustryCateService;
            this.CrowdCateService = CrowdCateService;
            this.OwnerCateService = OwnerCateService;
            this.AreaCateService = AreaCateService;
            this.PurposeCateService = PurposeCateService;
            this.FormatCateService = FormatCateService;
            this.PeriodCateService = PeriodCateService;
        }

        private bool CheckMemberStatus()
        {
            var member = MemberService.Find(CookieHelper.MemberID);
            return member.Status >= (int)MemberStatus.CompanyAuth;
        }

        public ActionResult Index()
        {
            ViewBag.MenuItem = "media-list";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult OutDoor_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
                .Where(x => x.MemberID == CookieHelper.MemberID
                    && x.Status >= (int)OutDoorStatus.ShowOnline)
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


        public ActionResult Preverify()
        {
            ViewBag.MenuItem = "media-preverify";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult OutDoor_Preverify([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
             .Where(x => x.MemberID == CookieHelper.MemberID
                 && x.Status == (int)OutDoorStatus.PreVerify)
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

        public ActionResult VerifyFailed()
        {
            ViewBag.MenuItem = "media-verifyfailed";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult OutDoor_VerifyFailed([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
              .Where(x => x.MemberID == CookieHelper.MemberID
                  && x.Status == (int)OutDoorStatus.VerifyFailed)
                  .Select(x => new OutDoorItemViewModel()
                  {
                      AddTime = x.AddTime,
                      ID = x.ID,
                      MediaFocusImg = x.MediaFoucsImg,
                      Name = x.Name,
                      Status = x.Status,
                      Unapprovedlog = x.Unapprovedlog
                  });
            return Json(model.ToDataSourceResult(request));
        }


        public ActionResult Delete()
        {
            ViewBag.MenuItem = "media-delete";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult OutDoor_Delete([DataSourceRequest] DataSourceRequest request)
        {

            var model = OutDoorService.GetKendoALL()
              .Where(x => x.MemberID == CookieHelper.MemberID
                  && x.Status == (int)OutDoorStatus.Deleted)
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

        public ActionResult NotShow()
        {
            ViewBag.MenuItem = "media-notshow";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult OutDoor_NoShow([DataSourceRequest] DataSourceRequest request)
        {
            var model = OutDoorService.GetKendoALL()
               .Where(x => x.MemberID == CookieHelper.MemberID
                   && x.Status == (int)OutDoorStatus.NoShow)
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

        public ActionResult Add()
        {
            ViewBag.MenuItem = "media-publish";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }

            //ViewBag.Data_CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.Data_IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.Data_PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.Data_AreaCate = Utilities.GetSelectListData(AreaCateService.GetALL(), x => x.ID, x => x.CateName, false);
            ViewBag.Data_FormatCode = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, true);
            //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, false);
            return View(new OutDoorViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(OutDoorViewModel model)
        {
            ViewBag.MenuItem = "media-publish";

            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
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
                    result.Message = "添加户外成功！";
                    return RedirectToAction("Preverify");
                }
                catch (Exception ex)
                {
                    result.Message = "添加户外失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加户外失败!", ex);
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
            ViewBag.MenuItem = "media-list";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "register"));
            }
            if (OutDoorService.GetALL().Any(x => x.ID == id && x.MemberID == CookieHelper.MemberID))
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
                    PeriodCode = entity.PeriodCode,
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
                ViewBag.Data_PeriodCode = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.ID, x => x.CateName, model.PeriodCode, true);
                //ViewBag.Data_OwnerCode = Utilities.GetSelectListData(OwnerCateService.GetALL(), x => x.ID, x => x.CateName, model.OwnerCode, false);
                return View(model);
            }
            else
            {
                return Content("<script>alert('非法操作！');window.history.go(-1);</script>");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OutDoorViewModel model)
        {
            ViewBag.MenuItem = "media-list";
            if (!CheckMemberStatus())
            {
                return Redirect(Url.Action("openbiz", "register"));
            }

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
                    result.Message = "编辑户外成功！";
                    return RedirectToAction("preverify");
                }
                catch (Exception ex)
                {
                    result.Message = "编辑户外失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑户外失败!", ex);
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

        #region Set OutDoorStatus

        [HttpPost]
        public ActionResult SetNotShow(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {

                OutDoorService.ChangeStatus(ids, OutDoorStatus.NoShow);
                result.Message = "设置媒体信息未显示成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置媒体信息未显示失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置媒体信息未显示失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SetDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeStatus(ids, OutDoorStatus.Deleted);
                result.Message = "删除媒体信息成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除媒体信息失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除媒体信息失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SetShow(string ids)
        {

            ServiceResult result = new ServiceResult();
            try
            {
                OutDoorService.ChangeStatus(ids, OutDoorStatus.ShowOnline);
                result.Message = "设置媒体信息显示成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置媒体信息显示失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置媒体信息显示失败!", ex);
            }
            return Json(result);

        }

        #endregion





    }



}
