using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadSite.ViewModels
{
    public class PeriodCateViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入分类名称")]
        [Display(Name = "分类名称")]
        [StringCheckLength(4, 25)]
        public string CateName { get; set; }

        [Required(ErrorMessage = "请输入分类代码")]
        [Display(Name = "分类代码")]
        [UIHint("Integer")]
        public int Code { get; set; }

        [Display(Name = "父级分类")]
        [UIHint("DropDownList")]
        public int? PID { get; set; }

        [Display(Name = "排序代码")]
        [Required(ErrorMessage = "请输入分类代码")]
        [UIHint("Integer")]
        public int OrderIndex { get; set; }

        [Display(Name = "分类级别")]
        [Required(ErrorMessage = "请输入分类代码")]
        [UIHint("Integer")]
        public int Level { get; set; }


    }
}