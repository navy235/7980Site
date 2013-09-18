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
    public class CompanyController : Controller
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
        public CompanyController(
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
        public ActionResult Index(int ID)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Categories = GetCompanyCategorise(ID);
            SetCompanyIndexCategories(model.Categories, ID);
            return View(model);
        }


        public ActionResult Intro(int ID)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Categories = GetCompanyCategorise(ID);
            return View(model);
        }

        public ActionResult source(int ID, int c = 0, int page = 1)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Categories = GetCompanyCategorise(ID);
            model.Sources = GetCompanySources(ID, c, page);
            return View(model);
        }


        public ActionResult show(int ID)
        {
            var outdoor = OutDoorLuceneService.Search(ID);
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
            model.Categories = GetCompanyCategorise(outdoor.MemberID);
            model.Links = outdoor;
            return View(model);

        }

        public ActionResult notice(int ID, int page = 1)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Categories = GetCompanyCategorise(ID);
            model.Sources = GetCompanyNotices(ID, page);
            return View(model);
        }


        public ActionResult viewnotice(int ID, int noticeId)
        {
            var company = CompanyService.Find(ID);
            var notice = CompanyNoticeService.GetALL().SingleOrDefault(x => x.ID == noticeId && x.Status >= (int)CompanyNoticeStatus.ShowOnLine);
            if (company == null || notice == null)
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
            model.Categories = GetCompanyCategorise(ID);
            model.Notice = notice;

            return View(model);
        }


        public ActionResult contact(int ID)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Address = company.Address;
            model.QQ = company.QQ;
            model.Categories = GetCompanyCategorise(ID);
            return View(model);
        }

        public ActionResult Credentials(int ID, int page = 1)
        {
            var company = CompanyService.Find(ID);
            if (company == null)
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
            model.Address = company.Address;
            model.QQ = company.QQ;
            model.Categories = GetCompanyCategorise(ID);
            model.Sources = GetCompanyCredentials(ID, page);
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

        private void SetCompanyIndexCategories(List<CompanyCategoryViewModel> categories, int ID)
        {
            int PageSize = 4;
            foreach (var category in categories)
            {
                var codeId = Convert.ToInt32(category.Code);
                var cate = MediaCateService.Find(codeId);
                var maxCode = Utilities.GetMaxCode(cate.Code, cate.Level);
                QueryTerm queryTerm = new QueryTerm()
                {
                    MemberID = ID,
                    MediaCode = codeId,
                    MediaCateCode = cate.Code,
                    MediaMaxCode = maxCode,
                    Page = 1
                };
                var searchFilter = GetSearchFilter(queryTerm.Query, queryTerm.Order, queryTerm.Descending, queryTerm.Page, PageSize);
                searchFilter.Take = 4;
                int totalHits = 0;
                var query = OutDoorLuceneService.Search(queryTerm, searchFilter, out totalHits);
                category.Products = query.Select(x => new CompanyProductViewMidel()
                {
                    AddTime = x.AddTime,
                    MediaCateName = x.MediaCateName,
                    CityCateName = x.CityCateName,
                    DeadLine = x.DeadLine,
                    Description = x.Description,
                    FocusImgUrl = x.FocusImgUrl,
                    ID = x.ID,
                    Name = x.Name,
                    Price = x.Price,
                    FormatName = x.FormatName,
                    PeriodName = x.PeriodName
                }).ToList();
            }
        }

        private SearchFilter GetSearchFilter(string q, int sortOrder, int descending, int page, int pageSize)
        {
            var searchFilter = new SearchFilter
            {
                PageSize = pageSize,
                SearchTerm = q,
                Skip = (page - 1) * pageSize, // pages are 1-based. 
                Take = pageSize
            };
            searchFilter.SortProperty = (SortProperty)sortOrder;

            searchFilter.SortDirection = (SortDirection)descending;

            return searchFilter;
        }

        private QuerySource GetCompanySources(int ID, int c, int page)
        {
            const int PageSize = 15;
            var model = new QuerySource();
            var query = new List<LinkItem>();
            int totalHits;
            QueryTerm queryTerm = new QueryTerm()
            {
                MemberID = ID,
                Page = page
            };
            if (c != 0)
            {
                var cate = MediaCateService.Find(c);
                var maxCode = Utilities.GetMaxCode(cate.Code, cate.Level);
                queryTerm.MediaCode = c;
                queryTerm.MediaCateCode = cate.Code;
                queryTerm.MediaMaxCode = Utilities.GetMaxCode(cate.Code, cate.Level);
            }
            var searchFilter = GetSearchFilter(queryTerm.Query, queryTerm.Order, queryTerm.Descending, queryTerm.Page, PageSize);
            query = OutDoorLuceneService.Search(queryTerm, searchFilter, out totalHits);

            model.Items = query;

            model.TotalCount = totalHits;

            model.CurrentPage = queryTerm.Page;

            model.PageSize = PageSize;

            model.Querywords = string.IsNullOrEmpty(queryTerm.Query) ? "" : queryTerm.Query;

            return model;
        }

        private QuerySource GetCompanyNotices(int ID, int page)
        {
            const int PageSize = 15;
            QuerySource model = new QuerySource();
            var query = CompanyNoticeService.GetALL()
                .Where(x => x.MemberID == ID
                    && x.Status >= (int)CompanyNoticeStatus.ShowOnLine)
                 .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList().Select(x => new LinkItem()
                {
                    Name = x.Title,
                    Description = x.Content,
                    ID = x.ID,
                    MemberID = x.MemberID,
                    AddTime = x.AddTime
                }).ToList();
            int totalCount = CompanyNoticeService.GetALL()
                .Count(x => x.MemberID == ID);
            model.Items = query;
            model.CurrentPage = page;
            model.PageSize = PageSize;
            model.TotalCount = totalCount;
            return model;
        }

        public QuerySource GetCompanyCredentials(int ID, int page)
        {
            const int PageSize = 15;
            QuerySource model = new QuerySource();
            var query = CompanyCredentialsImgService.GetALL()
                .Where(x => x.MemberID == ID)
                 .OrderByDescending(x => x.ID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList().Select(x => new LinkItem()
                {
                    Name = x.Title,
                    ID = x.ID,
                    MemberID = x.MemberID,
                    FocusImgUrl = x.ImgUrl
                }).ToList();
            int totalCount = CompanyNoticeService.GetALL()
                .Count(x => x.MemberID == ID);
            model.Items = query;
            model.CurrentPage = page;
            model.PageSize = PageSize;
            model.TotalCount = totalCount;
            return model;
        }
    }
}
