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
    public class GroupViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入群组名称")]
        [Display(Name = "群组名称")]
        [StringCheckLength(4, 25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入群组描述")]
        [Display(Name = "群组描述")]
        [StringCheckLength(4, 75)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "群组角色")]
        [Required(ErrorMessage = "请选择群组角色")]
        [UIHint("ForeignKeyForCheck")]
        public string Roles { get; set; }
    }
}