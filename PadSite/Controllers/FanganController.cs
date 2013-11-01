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
            ViewBag.FormatCate = Utilities.GetSelectListData(FormatCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.IndustryCate = Utilities.GetSelectListData(IndustryCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.CrowdCate = Utilities.GetSelectListData(CrowdCateService.GetALL(), x => x.ID, x => x.CateName, false);
            //ViewBag.PurposeCate = Utilities.GetSelectListData(PurposeCateService.GetALL(), x => x.ID, x => x.CateName, false);
            var periodCate = Utilities.GetSelectListData(PeriodCateService.GetALL(), x => x.OrderIndex, x => x.CateName, false);
            periodCate.RemoveAt(0);
            periodCate.RemoveAt(0);
            periodCate.Single(x => x.Value == "365").Selected = true;
            ViewBag.PeriodCate = periodCate;
            //var PriceCate = new List<SelectListItem>();
            //PriceCate.Add(new SelectListItem() { Text = "10万以下", Value = ((int)PriceListType.Price10Lower).ToString(), Selected = true });
            //PriceCate.Add(new SelectListItem() { Text = "10~50万元", Value = ((int)PriceListType.Price50Lower).ToString() });
            //PriceCate.Add(new SelectListItem() { Text = "50~100万元", Value = ((int)PriceListType.Price100Lower).ToString() });
            //PriceCate.Add(new SelectListItem() { Text = "100~200万元", Value = ((int)PriceListType.Price200Lower).ToString() });
            //PriceCate.Add(new SelectListItem() { Text = "200万元以上", Value = ((int)PriceListType.PriceMax).ToString() });
            //ViewBag.PriceCate = PriceCate;
            return View();
        }

        public List<LinkItemTree> GetCityTree()
        {
            List<LinkItemTree> model = new List<LinkItemTree>();
            model = CityCateService.GetALL().Where(x => x.PID == 1).Select(x => new LinkItemTree()
                {
                    Text = x.CateName,
                    Value = x.ID,
                    Code = x.Code
                }).ToList();
            foreach (var item in model)
            {
                GetCityTree(item, item.Value);
            }
            return model;
        }

        public void GetCityTree(LinkItemTree item, int pid)
        {
            //if (CityCateService.GetALL().Any(x => x.PID == pid))
            //{
            var query = CityCateService.GetALL().Where(x => x.PID == pid);
            item.Children = query.Select(x => new LinkItemTree()
            {
                Text = x.CateName,
                Value = x.ID,
                Code = x.Code
            }).ToList();
            //    foreach (var citem in item.Children)
            //    {
            //        GetCityTree(citem, citem.Value);
            //    }
            //}
        }

        public List<LinkItemTree> GetMediaTree()
        {
            List<LinkItemTree> model = new List<LinkItemTree>();
            model = MediaCateService.GetALL().Where(x => x.PID.Equals(null)).Select(x => new LinkItemTree()
            {
                Text = x.CateName,
                Value = x.ID,
                Code = x.Code
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
                    Value = x.ID,
                    Code = x.Code
                }).ToList();
                foreach (var citem in item.Children)
                {
                    GetMediaTree(citem, citem.Value);
                }
            }
        }

        public ActionResult Download(int periodNumber, int periodCate, string mediaIds)
        {
            var idList = Utilities.GetIdList(mediaIds);
            var list = new List<LinkItem>();
            var currentPrice = 0m;
            if (idList.Any())
            {
                list = OutDoorLuceneService.Search(idList);
                var scheme = new Scheme()
                {
                    AddTime = DateTime.Now,
                    Description = ConfigSetting.SiteName + "方案生成",
                    Name = ConfigSetting.SiteName + "方案生成",
                    ID = 0
                };
                var model = new SchemePrintViewModel()
                  {
                      ID = scheme.ID,
                      Medias = list,
                      AddTime = scheme.AddTime,
                      Description = scheme.Description,
                      Name = scheme.Name
                  };
                var day = periodNumber * periodCate;
                foreach (var item in list)
                {
                    currentPrice += ((item.Price / 365) * day);
                }
                model.TotalPrice = currentPrice.ToString("F2");
                var html = Utilities.RenderPartialToString(this.ControllerContext,
                    "download",
                    new ViewDataDictionary(model),
                    new TempDataDictionary());
                byte[] data = System.Text.Encoding.UTF8.GetBytes(html);
                return File(data, "text/html", Guid.NewGuid() + ".html");
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        public ActionResult Print(int periodNumber, int periodCate, string mediaIds)
        {
            var idList = Utilities.GetIdList(mediaIds);
            var list = new List<LinkItem>();
            var currentPrice = 0m;
            if (idList.Any())
            {
                list = OutDoorLuceneService.Search(idList);
                var scheme = new Scheme()
                {
                    AddTime = DateTime.Now,
                    Description = ConfigSetting.SiteName + "方案生成",
                    Name = ConfigSetting.SiteName + "方案生成",
                    ID = 0
                };
                var model = new SchemePrintViewModel()
                {
                    ID = scheme.ID,
                    Medias = list,
                    AddTime = scheme.AddTime,
                    Description = scheme.Description,
                    Name = scheme.Name
                };
                var day = periodNumber * periodCate;
                foreach (var item in list)
                {
                    currentPrice += ((item.Price / 365) * day);
                }
                ViewBag.currentPrice = currentPrice.ToString("F2");
                return View(model);
            }
            else
            {
                return RedirectToAction("index");
            }
        }
    }
}
