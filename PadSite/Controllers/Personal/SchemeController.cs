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
    public class SchemeController : Controller
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
        private IFavoriteService FavoriteService;
        private ISchemeItemService SchemeItemService;
        private ISchemeService SchemeService;
        public SchemeController(
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
            IPeriodCateService PeriodCateService,
            IFavoriteService FavoriteService,
            ISchemeItemService SchemeItemService,
            ISchemeService SchemeService

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
            this.FavoriteService = FavoriteService;
            this.SchemeItemService = SchemeItemService;
            this.SchemeService = SchemeService;
        }

        public ActionResult Index()
        {
            ViewBag.MenuItem = "scheme-media";
            return View();
        }

        public ActionResult Scheme_Read([DataSourceRequest] DataSourceRequest request)
        {
            var memberID = CookieHelper.MemberID;
            var model = SchemeService.GetKendoALL().Where(x => x.MemberID == memberID);
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult SchemeMedia_Read(int schemeID, [DataSourceRequest] DataSourceRequest request)
        {
            var memberID = CookieHelper.MemberID;

            var model = (from s in SchemeItemService.GetALL()
                         join o in OutDoorService.GetALL().Where(x => x.MemberID == memberID && x.Status >= (int)OutDoorStatus.ShowOnline) on s.MediaID equals o.ID
                         where s.SchemeID == schemeID
                         select new SchemeMediaViewModel()
                         {
                             StartTime = s.StartTime,
                             SchemeID = schemeID,
                             Price = s.Price,
                             MediaID = s.MediaID,
                             ID = s.ID,
                             EndTime = s.EndTime,
                             ImgUrl = o.MediaFoucsImg,
                             Name = o.Name
                         });

            return Json(model.ToDataSourceResult(request));
        }

        [LoginAuthorize]
        public ActionResult GetSchemeForm()
        {
            var MemberID = CookieHelper.MemberID;
            var memberSchemeList = SchemeService.GetALL().Where(x => x.MemberID == MemberID);
            ViewBag.schemeId = Utilities.GetSelectListData(memberSchemeList, x => x.ID, x => x.Name, false);
            return View();
        }


        [HttpPost]
        public ActionResult Scheme_Delete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var idsList = Utilities.GetIdList(ids);

                foreach (var id in idsList)
                {
                    var model = SchemeService.Find(id);
                    SchemeService.Delete(model);
                }

                result.Message = "删除方案成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除方案失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除方案失败!", ex);
            }
            return Json(result);
        }

        public ActionResult Print(int id)
        {

            var scheme = SchemeService.GetALL().Include(x => x.SchemeItem).Single(x => x.ID == id);
            return View(scheme);

        }


        public ActionResult Add()
        {
            ViewBag.MenuItem = "scheme-add";
            return View(new SchemeAddViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SchemeAddViewModel model)
        {
            ViewBag.MenuItem = "scheme-add";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var memberID = Convert.ToInt32(CookieHelper.UID);

                    Scheme entity = new Scheme()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        MemberID = memberID,
                        AddTime = DateTime.Now,
                        LastTime = DateTime.Now,
                    };
                    SchemeService.Create(entity);
                    result.Message = "媒体方案保存成功！";
                    return Redirect(Url.Action("index"));
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体方案保存失败!", ex);
                }
            }
            else
            {
                result.Message = "表单输入有误！";
                result.AddServiceError("表单输入有误！");
            }
            return View(model);

        }


        public ActionResult Edit(int id)
        {
            ViewBag.MenuItem = "scheme-index";

            var scheme = SchemeService.Find(id);

            var model = new SchemeAddViewModel()
            {
                ID = scheme.ID,
                Name = scheme.Name,
                Description = scheme.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SchemeAddViewModel model)
        {
            ViewBag.MenuItem = "scheme-index";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Scheme();
                    entity.ID = model.ID;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    SchemeService.Update(entity);
                    result.Message = "媒体方案修改成功！";
                    return Redirect(Url.Action("index"));
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体方案修改失败!", ex);
                }
            }
            else
            {
                result.Message = "表单输入有误！";
                result.AddServiceError("表单输入有误！");
            }
            return View(model);

        }

        public JsonResult SchemeNameExists(string Name)
        {
            var result = !SchemeService.GetALL().Any(x =>
                x.MemberID == CookieHelper.MemberID &&
                x.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEditSchemeForm(int id)
        {
            var model = (from s in SchemeItemService.GetALL()
                         join o in OutDoorService.GetALL().Where(x => x.MemberID == CookieHelper.MemberID && x.Status >= (int)OutDoorStatus.ShowOnline) on s.MediaID equals o.ID
                         select new EditSchemeMediaViewModel()
                         {
                             ID = s.ID,
                             MediaID = s.MediaID,
                             Name = o.Name,
                             PeriodCode = s.PeriodCode,
                             PeriodCount = s.PeriodCount,
                             Price = s.Price,
                             SchemeID = s.SchemeID,
                             StartTime = s.StartTime,
                             Uprice = o.Price

                         }).Single(x => x.ID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SchemeMedia_Delete(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var model = SchemeItemService.Find(id);
                SchemeItemService.Delete(model);
            }
            catch (Exception ex)
            {
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "媒体方案修改失败!", ex);
            }
            result.Message = "删除媒体方案" + (result.Success ? "成功！" : "失败！");
            return Json(result);
        }

    }
}
