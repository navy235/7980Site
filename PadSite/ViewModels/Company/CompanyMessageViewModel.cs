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
    public class CompanyMessageViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }


        [Display(Name = "标题")]
        public string Name { get; set; }


        [Display(Name = "内容")]
        public string Content { get; set; }


        [Display(Name = "状态")]
        public int Status { get; set; }


        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}