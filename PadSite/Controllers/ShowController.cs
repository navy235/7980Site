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
using PadSite.Service;

namespace PadSite.Controllers
{
    public class ShowController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        private IMediaCateService MediaCateService;
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
        private IOutDoorLuceneService OutDoorLuceneService;
        public ShowController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService,
            IMediaCateService MediaCateService,
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
            IOutDoorLuceneService OutDoorLuceneService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
            this.MediaCateService = MediaCateService;
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
            this.OutDoorLuceneService = OutDoorLuceneService;
        }
        public ActionResult Index(int id)
        {
            var outdoor = OutDoorLuceneService.Search(id);

            var company = CompanyService.Find(outdoor.MemberID);
            if (company == null || outdoor == null)
            {
                return HttpNotFound();
            }
            CompanyIndexViewModel model = new CompanyIndexViewModel();
            model.BannerImg = company.BannerImg;
            model.LogoImg = company.LogoImg;
            model.ID = company.ID;
            model.Name = company.Name;
            var cityIds = Utilities.GetIdList(company.CityCodeValue);
            var cityName = string.Join(" - ",
                CityCateService.GetALL()
                .Where(x => cityIds.Contains(x.ID))
                .ToList().Select(x => x.CateName));
            model.CityName = cityName;
            model.Description = company.Description;
            model.LinkMan = company.LinkMan;
            model.Sex = company.Sex;
            model.Lat = company.Lat;
            model.Lng = company.Lng;
            model.Mobile = company.Mobile;
            model.Phone = company.Phone;
            model.QQ = company.QQ;
            model.Links = outdoor;
            model.Categories = GetCompanyCategorise(outdoor.MemberID);
            return View(model);
        }
        private List<CompanyCategoryViewModel> GetCompanyCategorise(int id)
        {
            List<CompanyCategoryViewModel> result = new List<CompanyCategoryViewModel>();
            List<CompanyCategoryViewModel> model = new List<CompanyCategoryViewModel>();
            result = OutDoorService.GetALL()
                .Where(x => x.MemberID == id
                    && x.Status >= (int)OutDoorStatus.ShowOnline)
                    .GroupBy(x => x.MediaCodeValue)
                    .Select(x => new CompanyCategoryViewModel()
                    {
                        Count = x.Count(),
                        Code = x.Key
                    }).ToList();

            foreach (var item in result)
            {
                var codeId = Convert.ToInt32(item.Code.Split(',').First());
                item.Code = codeId.ToString();
            }

            model = result.GroupBy(x => x.Code).Select(x => new CompanyCategoryViewModel()
            {
                Code = x.Key,
                Count = x.Count()
            }).ToList();

            foreach (var item in model)
            {
                var codeId = Convert.ToInt32(item.Code);
                item.Name = MediaCateService.Find(codeId).CateName;
            }

            return model;
        }
    }
}
