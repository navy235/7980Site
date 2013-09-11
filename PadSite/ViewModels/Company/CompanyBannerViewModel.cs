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
    public class CompanyBannerViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }


        [Required(ErrorMessage = "请上传企业横幅")]
        [Display(Name = "企业横幅")]
        [UIHint("UploadImg")]
        public string BannerImg { get; set; }
    }
}