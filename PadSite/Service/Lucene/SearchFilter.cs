using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.Service
{
    public class SearchFilter
    {
        public string SearchTerm { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public int PageSize { get; set; }

        public SortProperty SortProperty { get; set; }

        public SortDirection SortDirection { get; set; }

        public bool CountOnly { get; set; }
    }

    public enum SortProperty
    {
        Published = 0,
        Hit = 1,
        Price = 2
    }

    public enum SortDirection
    {
        Descending = 0,
        Ascending = 1,
    }
}