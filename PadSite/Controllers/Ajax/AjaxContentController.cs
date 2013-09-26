using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.Utils;
using PadSite.Filters;
using PadSite.ViewModels;
using PadSite.Service;


namespace PadSite.Controllers
{
    public class AjaxContentController : Controller
    {
        //
        // GET: /AjaxContent/

        // GET: /Area/
        private ICityCateService CityCateService;
        private IArticleCateService ArticleCateService;
        private IMemberService MemberService;
        private IMediaCateService MediaCateService;
        private IFormatCateService FormatCateService;
        private IPeriodCateService PeriodCateService;
        private IOwnerCateService OwnerCateService;
        private IFavoriteService FavoriteService;
        private ISchemeItemService SchemeItemService;
        private ISchemeService SchemeService;
        private IOutDoorService OutDoorService;
        private IOutDoorLuceneService OutDoorLuceneService;
        public AjaxContentController(
             ICityCateService CityCateService,
             IArticleCateService ArticleCateService,
             IMemberService MemberService,
             IMediaCateService MediaCateService,
             IFormatCateService FormatCateService,
             IPeriodCateService PeriodCateService,
             IOwnerCateService OwnerCateService,
             IFavoriteService FavoriteService,
             ISchemeItemService SchemeItemService,
             ISchemeService SchemeService,
            IOutDoorService OutDoorService,
            IOutDoorLuceneService OutDoorLuceneService
          )
        {
            this.CityCateService = CityCateService;
            this.ArticleCateService = ArticleCateService;
            this.MemberService = MemberService;
            this.MediaCateService = MediaCateService;
            this.FormatCateService = FormatCateService;
            this.PeriodCateService = PeriodCateService;
            this.OwnerCateService = OwnerCateService;
            this.FavoriteService = FavoriteService;
            this.SchemeItemService = SchemeItemService;
            this.SchemeService = SchemeService;
            this.OutDoorService = OutDoorService;
            this.OutDoorLuceneService = OutDoorLuceneService;
        }

        public ActionResult GetSearchArea(float minX, float minY, float maxX, float maxY, int page = 1, int category = 0, int price = 0)
        {

            var model = new QuerySource();

            var result = new List<LinkItem>();

            QueryTerm query = new QueryTerm();

            query.MinX = minX;
            query.MinY = minY;
            query.MaxX = maxX;
            query.MaxY = maxY;
            if (category != 0)
            {
                query.MediaCode = category;
                query.MediaMaxCode = Utilities.GetMaxCode(category);
            }
            if (price != 0)
            {
                query.Price = price;
            }


            var pageSize = 10;

            int totalHits = 0;

            SearchFilter sf = new SearchFilter();

            sf.PageSize = pageSize;

            sf.Skip = (page - 1) * pageSize;

            sf.Take = pageSize;

            sf.SortProperty = SortProperty.Published;

            sf.SortDirection = SortDirection.Descending;

            result = OutDoorLuceneService.Search(query, sf, out totalHits);

            model.Items = result;

            model.TotalCount = totalHits;

            model.CurrentPage = page;

            model.PageSize = pageSize;

            return Json(model, JsonRequestBehavior.AllowGet);
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

    }
}
