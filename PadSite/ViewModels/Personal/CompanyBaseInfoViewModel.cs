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
    public class CompanyBaseInfoViewModel
    {

        [HintSeparateTitle("基本信息")]
        [Required(ErrorMessage = "请输入公司名称")]
        [Display(Name = "公司名称")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "公司名称含有非法字符.")]
        [StringCheckLength(7, 50)]
        [Hint("请输入7-50位公司名称，英文、数字或中文均可（中文占2个字符）。")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请选择公司所在城市")]
        [Display(Name = "所在城市")]
        [UIHint("Cascading")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "请输入公司简介.")]
        [Display(Name = "公司简介")]
        [DataType(DataType.MultilineText)]
        [StringCheckLength(10, 2000)]
        [Hint("请输入10-2000位公司简介，（中文占2个字符）。")]
        [HintClass("textarea")]
        public string Description { get; set; }

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