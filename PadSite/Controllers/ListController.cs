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
    public class ListController : Controller
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
        public ListController(
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


        public ActionResult Index(int province = 1, int city = 0,
            int mediacode = 0,
            int formatcode = 0,
            int ownercode = 0,
            int periodcode = 0,
            int authstatus = 0,
            int deadline = 0,
            int price = 0,
            int order = 0,
            int descending = 0,
            int page = 1,
            string query = null)
        {

            //搜索条件
            QueryTerm queryTerm = new QueryTerm()
            {
                Province = province,
                City = city,
                MediaCode = mediacode,
                FormatCode = formatcode,
                OwnerCode = ownercode,
                PeriodCode = periodcode,
                Page = page,
                AuthStatus = authstatus,
                DeadLine = deadline,
                Price = price,
                Order = order,
                Descending = descending,
                Query = query
            };

            //CacheService.Clear();

            if (queryTerm.City != 0)
            {
                var cityCate = CityCateService.Find(queryTerm.City);
                queryTerm.CityCateCode = cityCate.Code;
                queryTerm.CityMaxCode = Utilities.GetMaxCode(cityCate.Code, cityCate.Level);
            }

            if (queryTerm.MediaCode != 0)
            {
                var mediaCate = MediaCateService.Find(queryTerm.City);
                queryTerm.MediaCateCode = mediaCate.Code;
                queryTerm.MediaMaxCode = Utilities.GetMaxCode(mediaCate.Code, mediaCate.Level);
            }


            ViewBag.Search = GetSearch(queryTerm);

            ViewBag.PriceListFilter = GetPriceListFilter(queryTerm);

            ViewBag.DeadLineMonthFilter = GetDeadLineMonthFilter(queryTerm);

            ViewBag.DefaultOrderUrl = Url.Action("index", new
            {
                province = queryTerm.Province,
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                authstatus = queryTerm.AuthStatus,
                deadline = queryTerm.DeadLine,
                price = queryTerm.Price,
                order = 0,
                descending = 0,
                page = 1,
            });

            ViewBag.PriceOrderAscUrl = Url.Action("index", new
            {
                province = queryTerm.Province,
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                authstatus = queryTerm.AuthStatus,
                deadline = queryTerm.DeadLine,
                price = queryTerm.Price,
                order = (int)SortProperty.Price,
                descending = (int)SortDirection.Ascending,
                page = 1
            });

            ViewBag.PriceOrderDescUrl = Url.Action("index", new
            {
                province = queryTerm.Province,
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                authstatus = queryTerm.AuthStatus,
                deadline = queryTerm.DeadLine,
                price = queryTerm.Price,
                order = (int)SortProperty.Price,
                descending = (int)SortDirection.Descending,
                page = 1
            });

            ViewBag.NoAuthedUrl = Url.Action("index", new
            {
                province = queryTerm.Province,
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                authstatus = 0,
                deadline = queryTerm.DeadLine,
                price = queryTerm.Price,
                order = queryTerm.Order,
                descending = queryTerm.Descending,
                page = 1
            });

            ViewBag.AuthedUrl = Url.Action("index", new
            {
                province = queryTerm.Province,
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                authstatus = 1,
                deadline = queryTerm.DeadLine,
                price = queryTerm.Price,
                order = queryTerm.Order,
                descending = queryTerm.Descending,
                page = 1
            });

            ViewBag.Authed = queryTerm.AuthStatus == 1;


            ViewBag.Result = GetResult(queryTerm);

            ViewBag.Sort = GetSort(queryTerm);

            ViewBag.Query = queryTerm;

            return View();
        }
        private Dictionary<string, string> CreateSearchDic(string MethodName, QueryTerm queryTerm)
        {
            Dictionary<string, string> cacheDic = new Dictionary<string, string>();
            cacheDic.Add(CacheService.ServiceName, "ListController");
            cacheDic.Add(CacheService.ServiceMethod, MethodName);
            cacheDic.Add("Province", queryTerm.Province.ToString());
            cacheDic.Add("City", queryTerm.City.ToString());
            cacheDic.Add("MediaCode", queryTerm.MediaCode.ToString());
            cacheDic.Add("FormatCode", queryTerm.FormatCode.ToString());
            cacheDic.Add("OwnerCode", queryTerm.OwnerCode.ToString());
            cacheDic.Add("PeriodCode", queryTerm.PeriodCode.ToString());
            cacheDic.Add("AuthStatus", queryTerm.AuthStatus.ToString());
            cacheDic.Add("DeadLine", queryTerm.DeadLine.ToString());
            cacheDic.Add("Order", queryTerm.Order.ToString());
            cacheDic.Add("Descending", queryTerm.Descending.ToString());
            cacheDic.Add("Price", queryTerm.Price.ToString());
            cacheDic.Add("Page", queryTerm.Page.ToString());
            return cacheDic;
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

        private QuerySource GetResult(QueryTerm queryTerm)
        {
            const int PageSize = 15;
            var model = new QuerySource();
            var query = new List<LinkItem>();
            int totalHits;
            Dictionary<string, string> cacheDic = CreateSearchDic("ResultList", queryTerm);
            Dictionary<string, string> countDic = CreateSearchDic("ResultCount", queryTerm);
            if (string.IsNullOrWhiteSpace(queryTerm.Query)
                && CacheService.Exists(cacheDic)
                && CacheService.Exists(countDic))
            {
                query = CacheService.Get<List<LinkItem>>(cacheDic);
                totalHits = CacheService.GetInt32Value(countDic);
            }
            else
            {
                var searchFilter = GetSearchFilter(queryTerm.Query, queryTerm.Order, queryTerm.Descending, queryTerm.Page, PageSize);
                query = OutDoorLuceneService.Search(queryTerm, searchFilter, out totalHits);
                if (string.IsNullOrWhiteSpace(queryTerm.Query))
                {
                    CacheService.Add<List<LinkItem>>(query, cacheDic, 10);
                    CacheService.AddInt32Value(totalHits, countDic, 10);
                }
            }
            model.Items = query;
            model.TotalCount = totalHits;
            model.CurrentPage = queryTerm.Page;
            model.PageSize = PageSize;
            model.Querywords = string.IsNullOrEmpty(queryTerm.Query) ? "" : queryTerm.Query;
            return model;
        }


        private List<LinkGroup> GetSearch(QueryTerm queryTerm)
        {
            List<LinkGroup> result = new List<LinkGroup>();

            Dictionary<string, string> cacheDic = new Dictionary<string, string>();

            cacheDic.Add(CacheService.ServiceName, "ListController");
            cacheDic.Add(CacheService.ServiceMethod, "GetSearch");
            cacheDic.Add("City", queryTerm.City.ToString());
            cacheDic.Add("MediaCode", queryTerm.MediaCode.ToString());
            cacheDic.Add("FormatCode", queryTerm.FormatCode.ToString());
            cacheDic.Add("OwnerCode", queryTerm.OwnerCode.ToString());
            cacheDic.Add("PeriodCode", queryTerm.PeriodCode.ToString());
            if (CacheService.Exists(cacheDic))
            {
                result = CacheService.Get<List<LinkGroup>>(cacheDic);
                return result;
            }


            #region CityGroup
            if (queryTerm.City != 0)
            {
                var city = CityCateService.Find(queryTerm.City);
                var prevCityGroup = new List<LinkGroup>();
                GetPrevCityGroup(prevCityGroup, city, queryTerm);
                prevCityGroup.Reverse();
                result.AddRange(prevCityGroup);
                GetNextCityGroup(result, city, queryTerm);
            }
            else
            {
                var city = CityCateService.Find(queryTerm.Province);
                GetNextCityGroup(result, city, queryTerm);
            }
            #endregion

            #region MediaCode
            if (queryTerm.MediaCode != 0)
            {
                var media = MediaCateService.Find(queryTerm.MediaCode);
                var prevMediaGroup = new List<LinkGroup>();
                GetPrevMediaGroup(prevMediaGroup, media, queryTerm);
                prevMediaGroup.Reverse();
                result.AddRange(prevMediaGroup);
                GetNextMediaGroup(result, media, queryTerm, false);
            }
            else
            {
                GetNextMediaGroup(result, null, queryTerm, true);
            }
            #endregion

            #region FormatCode
            LinkGroup formatGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = "表现形式",
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = queryTerm.MediaCode,
                        formatcode = 0,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };
            formatGroup.Items = FormatCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    formatcode = x.ID,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode,
                    authstatus = queryTerm.AuthStatus,
                    deadline = queryTerm.DeadLine,
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1

                }),
                Selected = queryTerm.FormatCode == x.ID

            }).ToList();

            result.Add(formatGroup);

            #endregion

            #region OwnerCode
            LinkGroup ownerGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = "代理类型",
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = 0,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };
            ownerGroup.Items = OwnerCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = x.ID,
                    periodcode = queryTerm.PeriodCode,
                    authstatus = queryTerm.AuthStatus,
                    deadline = queryTerm.DeadLine,
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1

                }),
                Selected = queryTerm.OwnerCode == x.ID

            }).ToList();

            result.Add(ownerGroup);

            #endregion

            #region PeriodCode
            LinkGroup periodGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = "投放周期",
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };
            periodGroup.Items = PeriodCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = x.ID,
                    authstatus = queryTerm.AuthStatus,
                    deadline = queryTerm.DeadLine,
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1

                }),
                Selected = queryTerm.PeriodCode == x.ID

            }).ToList();

            result.Add(periodGroup);

            #endregion

            CacheService.Add<List<LinkGroup>>(result, cacheDic, 180);
            return result;
        }


        private QuerySort GetSort(QueryTerm queryTerm)
        {
            QuerySort result = new QuerySort();
            if (queryTerm.Order == 0)
            {
                result.SortDefault = true;
            }
            else if (queryTerm.Order == (int)SortProperty.Price)
            {
                if (queryTerm.Descending == (int)SortDirection.Descending)
                {
                    result.SortPriceDesc = true;
                }
                else
                {
                    result.SortPriceAsc = true;
                }
            }
            return result;
        }

        private void GetPrevCityGroup(List<LinkGroup> result, CityCate city, QueryTerm queryTerm)
        {
            var pCity = CityCateService.GetALL().Single(x => x.ID == city.PID);

            LinkGroup cityGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = !pCity.PID.HasValue ? "城市" : pCity.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = !pCity.PID.HasValue ? 0 : pCity.ID,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };

            var cityList = CityCateService.GetALL()
               .Where(x => x.PID == pCity.ID).ToList();

            var citySelectList = cityList.Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = x.ID,
                    mediacode = queryTerm.MediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode,
                    authstatus = queryTerm.AuthStatus,
                    deadline = queryTerm.DeadLine,
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1
                })
            }).ToList();

            if (citySelectList.Any(x => x.ID == city.ID))
            {
                citySelectList.Single(x => x.ID == city.ID).Selected = true;
            }

            cityGroup.Items = citySelectList;

            result.Add(cityGroup);

            if (pCity.PID.HasValue)
            {
                GetPrevCityGroup(result, pCity, queryTerm);
            }

        }

        private void GetNextCityGroup(List<LinkGroup> result, CityCate city, QueryTerm queryTerm)
        {
            if (CityCateService.GetALL().Any(x => x.PID == city.ID))
            {
                LinkGroup cityGroup = new LinkGroup()
                {
                    Group = new LinkItem()
                    {
                        Name = city.CateName,
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = city.ID,
                            mediacode = queryTerm.MediaCode,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode,
                            authstatus = queryTerm.AuthStatus,
                            deadline = queryTerm.DeadLine,
                            price = queryTerm.Price,
                            order = queryTerm.Order,
                            descending = queryTerm.Descending,
                            page = 1
                        })
                    }
                };

                var cityList = CityCateService.GetALL()
                   .Where(x => x.PID == city.ID).ToList();

                var citySelectList = cityList.Select(x => new LinkItem()
                {
                    ID = x.ID,
                    Name = x.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = x.ID,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }).ToList();


                cityGroup.Items = citySelectList;

                result.Add(cityGroup);
            }
        }

        private void GetPrevMediaGroup(List<LinkGroup> result, MediaCate media, QueryTerm queryTerm)
        {
            if (media.PID.HasValue)
            {
                var pMedia = MediaCateService.GetALL().Single(x => x.ID == media.PID);

                LinkGroup mediaGroup = new LinkGroup()
                {
                    Group = new LinkItem()
                    {
                        Name = pMedia.CateName,
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = queryTerm.City,
                            mediacode = pMedia.ID,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode,
                            authstatus = queryTerm.AuthStatus,
                            deadline = queryTerm.DeadLine,
                            price = queryTerm.Price,
                            order = queryTerm.Order,
                            descending = queryTerm.Descending,
                            page = 1
                        })
                    }
                };

                var mediaList = MediaCateService.GetALL()
                   .Where(x => x.PID == pMedia.ID).ToList();

                var mediaSelectList = mediaList.Select(x => new LinkItem()
                {
                    ID = x.ID,
                    Name = x.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = x.ID,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }).ToList();

                if (mediaSelectList.Any(x => x.ID == media.ID))
                {
                    mediaSelectList.Single(x => x.ID == media.ID).Selected = true;
                }

                mediaGroup.Items = mediaSelectList;

                result.Add(mediaGroup);

                GetPrevMediaGroup(result, pMedia, queryTerm);
            }
            else
            {
                LinkGroup mediaGroup = new LinkGroup()
                {
                    Group = new LinkItem()
                    {
                        Name = "媒体分类",
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = queryTerm.City,
                            mediacode = 0,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode,
                            authstatus = queryTerm.AuthStatus,
                            deadline = queryTerm.DeadLine,
                            price = queryTerm.Price,
                            order = queryTerm.Order,
                            descending = queryTerm.Descending,
                            page = 1
                        })
                    }
                };

                var mediaList = MediaCateService.GetALL()
                   .Where(x => x.PID.Equals(null)).ToList();

                var mediaSelectList = mediaList.Select(x => new LinkItem()
                {
                    ID = x.ID,
                    Name = x.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = x.ID,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }).ToList();

                if (mediaSelectList.Any(x => x.ID == media.ID))
                {
                    mediaSelectList.Single(x => x.ID == media.ID).Selected = true;
                }

                mediaGroup.Items = mediaSelectList;

                result.Add(mediaGroup);

            }
        }

        private void GetNextMediaGroup(List<LinkGroup> result, MediaCate media, QueryTerm queryTerm, bool root)
        {
            if (root)
            {
                LinkGroup mediaGroup = new LinkGroup()
                {
                    Group = new LinkItem()
                    {
                        Name = "媒体分类",
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = queryTerm.City,
                            mediacode = 0,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode,
                            authstatus = queryTerm.AuthStatus,
                            deadline = queryTerm.DeadLine,
                            price = queryTerm.Price,
                            order = queryTerm.Order,
                            descending = queryTerm.Descending,
                            page = 1
                        })
                    }
                };

                var mediaList = MediaCateService.GetALL()
                   .Where(x => x.PID.Equals(null)).ToList();

                var mediaSelectList = mediaList.Select(x => new LinkItem()
                {
                    ID = x.ID,
                    Name = x.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = x.ID,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        authstatus = queryTerm.AuthStatus,
                        deadline = queryTerm.DeadLine,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }).ToList();

                mediaGroup.Items = mediaSelectList;

                result.Add(mediaGroup);
            }
            else
            {
                if (MediaCateService.GetALL().Any(x => x.PID == media.ID))
                {
                    LinkGroup mediaGroup = new LinkGroup()
                    {
                        Group = new LinkItem()
                        {
                            Name = media.CateName,
                            Url = Url.Action("index", new
                            {
                                province = queryTerm.Province,
                                city = queryTerm.City,
                                mediacode = media.ID,
                                formatcode = queryTerm.FormatCode,
                                ownercode = queryTerm.OwnerCode,
                                periodcode = queryTerm.PeriodCode,
                                authstatus = queryTerm.AuthStatus,
                                deadline = queryTerm.DeadLine,
                                price = queryTerm.Price,
                                order = queryTerm.Order,
                                descending = queryTerm.Descending,
                                page = 1
                            })
                        }
                    };

                    var mediaList = MediaCateService.GetALL()
                       .Where(x => x.PID == media.ID).ToList();

                    var mediaSelectList = mediaList.Select(x => new LinkItem()
                    {
                        ID = x.ID,
                        Name = x.CateName,
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = queryTerm.City,
                            mediacode = x.ID,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode,
                            authstatus = queryTerm.AuthStatus,
                            deadline = queryTerm.DeadLine,
                            price = queryTerm.Price,
                            order = queryTerm.Order,
                            descending = queryTerm.Descending,
                            page = 1
                        })
                    }).ToList();

                    mediaGroup.Items = mediaSelectList;

                    result.Add(mediaGroup);
                }
            }
        }

        private int GetLevelCode(int level, int code)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            codeStr = codeStr.Substring(0, 2 * level);
            var needLength = maxLength - codeStr.Length;
            for (var i = 0; i < needLength; i++)
            {
                codeStr += "0";
            }
            return Convert.ToInt32(codeStr);
        }

        private List<LinkItem> GetPriceListFilter(QueryTerm queryTerm)
        {
            List<LinkItem> result = new List<LinkItem>();
            result = UIHelper.PriceList.Select(x => new LinkItem()
            {
                Name = x.Text,
                Selected = x.Value == queryTerm.Price.ToString(),
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode,
                    authstatus = queryTerm.AuthStatus,
                    deadline = queryTerm.DeadLine,
                    price = Convert.ToInt32(x.Value),
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1,
                })

            }).ToList();
            return result;
        }
        private List<LinkItem> GetDeadLineMonthFilter(QueryTerm queryTerm)
        {
            List<LinkItem> result = new List<LinkItem>();

            result = UIHelper.DeadLineMonthList.Select(x => new LinkItem()
            {
                Name = x.Text,
                Selected = x.Value == queryTerm.DeadLine.ToString(),
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode,
                    authstatus = queryTerm.AuthStatus,
                    deadline = Convert.ToInt32(x.Value),
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1,
                })

            }).ToList();
            return result;
        }


    }


}
