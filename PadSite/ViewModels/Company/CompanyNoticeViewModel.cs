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
    public class CompanyNoticeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入公告标题")]
        [Display(Name = "公告标题")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "公告标题含有非法字符.")]
        [StringCheckLength(7, 50)]
        [Hint("请输入7-50位公告标题，英文、数字或中文均可（中文占2个字符）。")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入公告内容")]
        [Display(Name = "公告内容")]
        [DataType(DataType.MultilineText)]
        [StringCheckLength(10, 2000)]
        [Hint("请输入10-2000位公告内容，（中文占2个字符）。")]
        [HintClass("textarea")]
        public string Content { get; set; }
    }

    public class CompanyNoticeListViewModel
    {
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