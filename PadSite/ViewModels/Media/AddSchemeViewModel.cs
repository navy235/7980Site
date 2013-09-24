using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class AddSchemeViewModel
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string endTime { get; set; }

        public string startTime { get; set; }

        public int schemeId { get; set; }

        public string price { get; set; }

        public int periodCode { get; set; }

        public int periodCount { get; set; }
    }

    public class EditSchemeViewModel
    {
        public int id { get; set; }

        public string startTime { get; set; }

        public string price { get; set; }

        public int periodCode { get; set; }

        public int periodCount { get; set; }
    }

    public class GenerateSchemeViewModel
    {
        public string mediaCode { get; set; }

        public string industryCate { get; set; }

        public string cityCode { get; set; }

        public string purposeCate { get; set; }

        public string crowdCate { get; set; }

        public int priceCate { get; set; }

        public int generateType { get; set; }

        public int day { get; set; }
    }
}