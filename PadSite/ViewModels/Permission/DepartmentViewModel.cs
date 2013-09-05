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
    public class DepartmentViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入部门名称")]
        [Display(Name = "部门名称")]
        [StringCheckLength(4, 25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入部门描述")]
        [Display(Name = "部门描述")]
        [StringCheckLength(4, 75)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "请输入部门领导")]
        [Display(Name = "部门领导")]
        [StringCheckLength(4, 25)]
        public string Leader { get; set; }


    }
}