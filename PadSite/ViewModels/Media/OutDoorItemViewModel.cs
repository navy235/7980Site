using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace PadSite.ViewModels
{
    public class OutDoorItemViewModel
    {
        [Display(Name = "媒体ID")]
        public int ID { get; set; }

        [Display(Name = "媒体名称")]
        public string Name { get; set; }

        [Display(Name = "媒体图片")]
        public string MediaFocusImg { get; set; }

        [Display(Name = "审核状态")]
        public int Status { get; set; }

        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }

    }
}