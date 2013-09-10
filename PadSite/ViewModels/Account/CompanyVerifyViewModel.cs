using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PadSite.ViewModels
{
    public class CompanyVerifyViewModel
    {
        [Display(Name = "公司ID")]
        public int MemberID { get; set; }

        [Display(Name = "公司名称")]
        public string Name { get; set; }

        [Display(Name = "公司描述")]
        public string Description { get; set; }

        [Display(Name = "公司联系人")]
        public string LinkMan { get; set; }

        [Display(Name = "联系方式")]
        public string Contact { get; set; }

        [Display(Name = "审核状态")]
        public int Status { get; set; }

        [Display(Name = "添加时间")]
        public DateTime AddTime { get; set; }

        [Display(Name = "最后时间")]
        public DateTime LastTime { get; set; }
    }
}