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


namespace PadSite.Controllers
{
    [LoginAuthorize]
    public class BizController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        private ICityCateService CityCateService;
        public BizController(
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
            ViewBag.MenuItem = "company-baseinfo";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            else
            {
                var company = CompanyService.Find(CookieHelper.MemberID);
                var model = new CompanyBaseInfoViewModel()
                {
                    CityCode = company.CityCodeValue,
                    Description = company.Description,
                    Name = company.Name,
                    CredentialsImg = company.CredentialsImg,
                    LinkManImg = company.LinkManImg,
                    LogoImg = company.LogoImg
                };
                var cityIds = company.CityCodeValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                ViewBag.Data_CityCode = cityValues;
                return View(model);
            }
        }

    }
}
