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
    public class BizAuthViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

  

        [HintSeparateTitle("认证信息")]
        [Required(ErrorMessage = "请上传企业LOGO.")]
        [Display(Name = "企业LOGO")]
        [UIHint("UploadImgEdit")]
        [HintClass("uploadlogo")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        [HintLabel("请上传不小于200X200像素的图片,图片文件大小不超过5M")]
        public string LogoImg { get; set; }


        [Required(ErrorMessage = "请上传企业营业执照.")]
        [Display(Name = "企业执照")]
        [UIHint("UploadImgList")]
        [HintClass("uploadlist")]
        [AdditionalMetadata("UploadImgList", "")]
        [AdditionalMetadata("UploadImgListMaxLength", "6")]
        [HintLabel("请上传1-6张不小于800X600像素的图片,图片文件大小不超过5M")]
        public string CredentialsImg { get; set; }


        [Required(ErrorMessage = "请输入身份证号码")]
        [Display(Name = "身份证号码")]
        public string IdentityCard { get; set; }

        [Required(ErrorMessage = "请上传联系人身份证照片.")]
        [Display(Name = "身份证照片")]
        [UIHint("UploadImgList")]
        [HintClass("uploadlist")]
        [AdditionalMetadata("UploadImgList", "2")]
        [AdditionalMetadata("UploadImgListMaxLength", "6")]
        [HintLabel("请上传1-6张不小于800X600像素的图片,图片文件大小不超过5M")]
        public string LinkManImg { get; set; }
    }
}