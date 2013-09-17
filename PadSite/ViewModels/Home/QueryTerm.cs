using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class QueryTerm
    {

        public int Province { get; set; }

        public int City { get; set; }

        public int CityCateCode { get; set; }

        public int CityMaxCode { get; set; }

        public int MediaCode { get; set; }

        public int MediaCateCode { get; set; }

        public int MediaMaxCode { get; set; }

        public int FormatCode { get; set; }

        public int OwnerCode { get; set; }

        public int PeriodCode { get; set; }

        public int Price { get; set; }

        public int Order { get; set; }

        public int Page { get; set; }

        public int Descending { get; set; }

        public int AuthStatus { get; set; }

        public int DeadLine { get; set; }

        public string Query { get; set; }

        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }
    }
}