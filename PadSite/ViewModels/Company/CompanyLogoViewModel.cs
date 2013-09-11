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
    public class CompanyLogoViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }


        [Required(ErrorMessage = "请上传企业标志")]
        [Display(Name = "企业标志")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string LogoImg { get; set; }
    }
}