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

    public class AvtarViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }


        [Required(ErrorMessage = "请上传头像")]
        [Display(Name = "上传头像")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }
    }
}