using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadSite.ViewModels
{
    public class SystemMessageViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Display(Name = "信息标题")]
        public string Name { get; set; }


        [Display(Name = "信息内容")]
        [HintClass("textarea")]
        public string Content { get; set; }

        [Display(Name = "状态")]
        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }

        [Display(Name = "发送时间")]
        public DateTime AddTime { get; set; }
    }
}