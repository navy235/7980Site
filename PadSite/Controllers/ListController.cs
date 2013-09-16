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

        public ListController(
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


        public ActionResult Index(int province = 1, int city = 0,
            int mediacode = 0,
            int formatcode = 0,
            int ownercode = 0,
            int periodcode = 0,
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
                Price = price,
                Order = order,
                Descending = descending,
                Query = query
            };

            //ViewBag.Bread = GetBread(LeftMenu, queryTerm);

            ViewBag.Search = GetSearch(queryTerm);

            ViewBag.PriceListFilter = GetPriceListFilter(queryTerm);

            ViewBag.DefaultOrderUrl = Url.Action("index", new
            {
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                price = queryTerm.Price,
                order = 0,
                descending = 0,
                page = queryTerm.Page,
            });

            ViewBag.PriceOrderAscUrl = Url.Action("index", new
            {
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                price = queryTerm.Price,
                order = (int)SortProperty.Price,
                descending = (int)SortDirection.Ascending,
                page = queryTerm.Page
            });

            ViewBag.PriceOrderDescUrl = Url.Action("index", new
            {
                city = queryTerm.City,
                mediacode = queryTerm.MediaCode,
                formatcode = queryTerm.FormatCode,
                ownercode = queryTerm.OwnerCode,
                periodcode = queryTerm.PeriodCode,
                price = queryTerm.Price,
                order = (int)SortProperty.Price,
                descending = (int)SortDirection.Descending,
                page = queryTerm.Page
            });

            ViewBag.Result = GetResult(queryTerm);

            ViewBag.Sort = GetSort(queryTerm);

            ViewBag.Query = queryTerm;

            return View();
        }


        private List<LinkGroup> GetSearch(QueryTerm queryTerm)
        {
            List<LinkGroup> result = new List<LinkGroup>();

            #region CityGroup

            LinkGroup cityGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = "城市",
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = 0,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };

            var cityRootList = CityCateService.GetALL()
               .Where(x => x.PID == queryTerm.Province).ToList();

            var cityRootSelectList = cityRootList.Select(x => new LinkItem()
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
                       price = queryTerm.Price,
                       order = queryTerm.Order,
                       descending = queryTerm.Descending,
                       page = 1
                   })
               }).ToList();

            cityGroup.Items = cityRootSelectList;
            if (queryTerm.City != 0)
            {
                var city = CityCateService.Find(queryTerm.City);
                var level = city.Level;
                if (level == 1)
                {
                    cityRootSelectList.Single(x => x.ID == queryTerm.City).Selected = true;
                }
                else
                {
                    for (var i = 2; i <= level; i++)
                    {
                        var pcityCode = GetLevelCode(level, city.Code);
                        var pcity = CityCateService.GetALL().Single(x => x.Code == pcityCode);
                        var childcityGroup = new LinkGroup()
                        {
                            Group = new LinkItem()
                            {
                                Name = pcity.CateName,
                                Url = Url.Action("index", new
                                {
                                    province = queryTerm.Province,
                                    city = pcity.ID,
                                    mediacode = queryTerm.MediaCode,
                                    formatcode = queryTerm.FormatCode,
                                    ownercode = queryTerm.OwnerCode,
                                    periodcode = queryTerm.PeriodCode,
                                    price = queryTerm.Price,
                                    order = queryTerm.Order,
                                    descending = queryTerm.Descending,
                                    page = 1
                                })
                            }
                        };

                        var childCityList = CityCateService.GetALL()
                            .Where(x => x.PID == pcity.ID).ToList();

                        var childCitySelectList = childCityList.Select(x => new LinkItem()
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
                                price = queryTerm.Price,
                                order = queryTerm.Order,
                                descending = queryTerm.Descending,
                                page = 1
                            })
                        }).ToList();

                        childcityGroup.Items = cityRootSelectList;
                    }
                }

            }

            #endregion

            #region MediaCode

            LinkGroup categoryGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = "媒体类别",
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = 0,
                        childmediacode = 0,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode
                    })
                }
            };
            categoryGroup.Items = outDoorMediaCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = x.ID,
                    childmediacode = 0,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode

                }),
                Selected = queryTerm.MediaCode == x.ID

            }).ToList();

            result.Add(categoryGroup);

            #endregion

            #region ChildMediaCode
            if (queryTerm.MediaCode != 0)
            {
                LinkGroup childCategoryGroup = new LinkGroup()
                {
                    Group = new LinkItem()
                    {
                        Name = "媒体子类别",
                        Url = Url.Action("index", new
                        {
                            province = queryTerm.Province,
                            city = queryTerm.City,
                            mediacode = queryTerm.MediaCode,
                            childmediacode = 0,
                            formatcode = queryTerm.FormatCode,
                            ownercode = queryTerm.OwnerCode,
                            periodcode = queryTerm.PeriodCode
                        })
                    }
                };

                childCategoryGroup.Items = outDoorMediaCateService.GetALL().Where(x => x.PID == queryTerm.MediaCode).ToList().Select(x => new LinkItem()
                {
                    ID = x.ID,
                    Name = x.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = queryTerm.City,
                        mediacode = queryTerm.MediaCode,
                        childmediacode = x.ID,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode

                    }),
                    Selected = queryTerm.ChildMediaCode == x.ID

                }).ToList();

                result.Add(childCategoryGroup);
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
                        childmediacode = queryTerm.ChildMediaCode,
                        formatcode = 0,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode
                    })
                }
            };
            formatGroup.Items = formatCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    childmediacode = queryTerm.ChildMediaCode,
                    formatcode = x.ID,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = queryTerm.PeriodCode

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
                        childmediacode = queryTerm.ChildMediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = 0,
                        periodcode = queryTerm.PeriodCode
                    })
                }
            };
            ownerGroup.Items = ownerCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    childmediacode = queryTerm.ChildMediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = x.ID,
                    periodcode = queryTerm.PeriodCode

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
                        childmediacode = queryTerm.ChildMediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode
                    })
                }
            };
            periodGroup.Items = periodCateService.GetALL().Where(x => x.PID.Equals(null)).ToList().Select(x => new LinkItem()
            {
                ID = x.ID,
                Name = x.CateName,
                Url = Url.Action("index", new
                {
                    province = queryTerm.Province,
                    city = queryTerm.City,
                    mediacode = queryTerm.MediaCode,
                    childmediacode = queryTerm.ChildMediaCode,
                    formatcode = queryTerm.FormatCode,
                    ownercode = queryTerm.OwnerCode,
                    periodcode = x.ID

                }),
                Selected = queryTerm.PeriodCode == x.ID

            }).ToList();

            result.Add(periodGroup);

            #endregion
        }



        private void GetCityGroup(List<LinkGroup> result, QueryTerm queryTerm, CityCate city)
        {
            var pcityCode = GetLevelCode(city.Level, city.Code);
            var pcity = CityCateService.GetALL().Single(x => x.Code == pcityCode);
            var childcityGroup = new LinkGroup()
            {
                Group = new LinkItem()
                {
                    Name = pcity.CateName,
                    Url = Url.Action("index", new
                    {
                        province = queryTerm.Province,
                        city = pcity.ID,
                        mediacode = queryTerm.MediaCode,
                        formatcode = queryTerm.FormatCode,
                        ownercode = queryTerm.OwnerCode,
                        periodcode = queryTerm.PeriodCode,
                        price = queryTerm.Price,
                        order = queryTerm.Order,
                        descending = queryTerm.Descending,
                        page = 1
                    })
                }
            };

            var childCityList = CityCateService.GetALL()
                .Where(x => x.PID == pcity.ID).ToList();

            var childCitySelectList = childCityList.Select(x => new LinkItem()
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
                    price = queryTerm.Price,
                    order = queryTerm.Order,
                    descending = queryTerm.Descending,
                    page = 1
                })
            }).ToList();
            childcityGroup.Items = childCitySelectList;
            result.Add(childcityGroup);
            if (childcityGroup.Items.Any(x => x.ID == queryTerm.City))
            {
                childcityGroup.Items.Single(x => x.ID == queryTerm.City).Selected = true;
            }
            else { 
                
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
    }


}
