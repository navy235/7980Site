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

namespace PadSite.Controllers
{
    public class BizVerifyController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        private ICityCateService CityCateService;
        public BizVerifyController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService,
            ICityCateService CityCateService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
            this.CityCateService = CityCateService;
        }

        public ActionResult Index()
        {
            ViewBag.CompanyStatus = UIHelper.CompanyStatusList;
            return View();
        }

        public ActionResult Authed()
        {
            ViewBag.CompanyStatus = UIHelper.CompanyStatusList;
            return View();
        }

        public ActionResult Company_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(CompanyService.GetVerifyList(CompanyStatus.CompanyApply).ToDataSourceResult(request));
        }

        public ActionResult Company_ReadAuthed([DataSourceRequest] DataSourceRequest request)
        {
            return Json(CompanyService.GetVerifyList(CompanyStatus.CompanyAuth).ToDataSourceResult(request));
        }

        public ActionResult VerifyPass(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                CompanyService.ChangeStatus(ids, CompanyStatus.CompanyAuth);
                LogHelper.WriteLog("信息审核通过成功");
                result.Message = "信息审核通过成功！";
            }
            catch (DbUnexpectedValidationException ex)
            {
                result.Message = "信息审核通过失败！";
                result.AddServiceError("信息审核通过失败!");
                LogHelper.WriteLog("信息审核通过失败", ex);
            }
            return Json(result);
        }

        public ActionResult VerifyFailed(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                CompanyService.ChangeStatus(ids, CompanyStatus.CompanyFailed);
                LogHelper.WriteLog("信息审核未通过成功");
                result.Message = "信息审核未通过成功！";
            }
            catch (DbUnexpectedValidationException ex)
            {
                result.Message = "信息审核未通过失败！";
                result.AddServiceError("信息审未核通过失败!");
                LogHelper.WriteLog("信息审核未通过失败", ex);
            }
            return Json(result);
        }

        public ActionResult Details(int id)
        {
            Company cpy = CompanyService.Find(id);
            CompanyRegViewModel cpr = new CompanyRegViewModel()
            {
                Address = cpy.Address,

                CityCode = cpy.CityCodeValue,
                CredentialsImg = cpy.CredentialsImg,
                IdentityCard = cpy.IdentityCard,
                LinkManImg = cpy.LinkManImg,
                LogoImg = cpy.LogoImg,
                Description = cpy.Description,
                Fax = cpy.Fax,
                LinkMan = cpy.LinkMan,
                Mobile = cpy.Mobile,
                MSN = cpy.MSN,
                Name = cpy.Name,
                Phone = cpy.Phone,
                Position = cpy.Lat + "|" + cpy.Lng,
                QQ = cpy.QQ,
                Sex = cpy.Sex
            };
            var cityIds = cpy.CityCodeValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            ViewBag.Data_CityCode = cityValues;

            return View(cpr);
        }

    }
}
