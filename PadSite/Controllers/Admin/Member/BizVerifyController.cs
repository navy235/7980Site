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
        public BizVerifyController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
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
            var success = CompanyService.ChangeStatus(ids,
                CompanyStatus.CompanyAuth);
            return Json(success);
        }

        public ActionResult VerifyFailed(string ids)
        {
            var success = CompanyService.ChangeStatus(ids,
             CompanyStatus.CompanyFailed);
            return Json(success);
        }

        public ActionResult Details(int id)
        {

            Company cpy = companyService.IncludeFindByCompanyID(id);

            CompanyReg cpr = new CompanyReg()
            {
                Address = cpy.Address,
                BussinessCode = cpy.BussinessCode,
                CityCode = cpy.CityCode,
                CompanyImg = cpy.CompanyImg.ImgUrls,
                Logo = cpy.CompanyLogoImg.FocusImgUrl,
                Description = cpy.Description,
                Fax = cpy.Fax,
                FundCode = cpy.FundCode,
                LinkMan = cpy.LinkMan,
                LinManImg = cpy.LinkManImg.ImgUrls,
                Mobile = cpy.Mobile,
                MSN = cpy.MSN,
                Name = cpy.Name,
                Phone = cpy.Phone,
                Position = cpy.Lat + "|" + cpy.Lng,
                QQ = cpy.QQ,
                ScaleCode = cpy.ScaleCode,
                Sex = cpy.Sex

            };
            return View(cpr);
        }

    }
}
