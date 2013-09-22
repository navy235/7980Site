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
    public class FanganController : Controller
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
        public FanganController(
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

        public ActionResult Index()
        {
            ViewBag.CityTree = GetCityTree();
            ViewBag.MediaTree = GetMediaTree();
            return View();
        }

        public List<LinkItemTree> GetCityTree()
        {
            List<LinkItemTree> model = new List<LinkItemTree>();
            model = CityCateService.GetALL().Where(x => x.PID == 1).Select(x => new LinkItemTree()
                {
                    Text = x.CateName,
                    Value = x.ID
                }).ToList();
            foreach (var item in model)
            {
                GetCityTree(item, item.Value);
            }
            return model;
        }



        public void GetCityTree(LinkItemTree item, int pid)
        {
            if (CityCateService.GetALL().Any(x => x.PID == pid))
            {
                var query = CityCateService.GetALL().Where(x => x.PID == pid);
                item.Children = query.Select(x => new LinkItemTree()
                {
                    Text = x.CateName,
                    Value = x.ID
                }).ToList();
                foreach (var citem in item.Children)
                {
                    GetCityTree(citem, citem.Value);
                }
            }
        }

        public List<LinkItemTree> GetMediaTree()
        {
            List<LinkItemTree> model = new List<LinkItemTree>();
            model = MediaCateService.GetALL().Where(x => x.PID == 1).Select(x => new LinkItemTree()
            {
                Text = x.CateName,
                Value = x.ID
            }).ToList();
            foreach (var item in model)
            {
                GetMediaTree(item, item.Value);
            }
            return model;
        }

        public void GetMediaTree(LinkItemTree item, int pid)
        {
            if (MediaCateService.GetALL().Any(x => x.PID == pid))
            {
                var query = MediaCateService.GetALL().Where(x => x.PID == pid);
                item.Children = query.Select(x => new LinkItemTree()
                {
                    Text = x.CateName,
                    Value = x.ID
                }).ToList();
                foreach (var citem in item.Children)
                {
                    GetMediaTree(citem, citem.Value);
                }
            }
        }
    }
}
