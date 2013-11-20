using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class RealPriceViewModel
    {
        public bool IsLogin
        {
            get;
            set;
        }

        public int ID { get; set; }

        public decimal Price { get; set; }
    }
}