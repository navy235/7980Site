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
    public class PermissionsViewModel
    {
        public PermissionsViewModel()
        {
            this.Action = "controller";
            this.Namespace = "default";
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入权限名称")]
        [Display(Name = "权限名称")]
        [StringCheckLength(4, 25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入权限描述")]
        [Display(Name = "权限描述")]
        [StringCheckLength(4, 75)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "请输入控制器名称")]
        [Display(Name = "控制器名称")]
        [StringCheckLength(4, 25)]
        public string Controller { get; set; }

        [Required(ErrorMessage = "请输入操作名称")]
        [Display(Name = "操作名称")]
        [StringCheckLength(4, 25)]
        public string Action { get; set; }

        [Required(ErrorMessage = "请输入命名空间")]
        [Display(Name = "命名空间")]
        [StringCheckLength(4, 25)]
        public string Namespace { get; set; }

        [Display(Name = "所属部门")]
        [Required(ErrorMessage = "请选择所属部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }



    }
}