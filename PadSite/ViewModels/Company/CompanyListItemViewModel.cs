using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadSite.ViewModels
{
    public class CompanyListItemViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Display(Name = "企业名称")]
        public string Name { get; set; }
    }
}