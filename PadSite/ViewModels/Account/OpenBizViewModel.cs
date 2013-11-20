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
    public class OpenBizViewModel
    {
        [HintSeparateTitle("企业基本信息")]
        [Required(ErrorMessage = "请输入公司名称")]
        [Display(Name = "公司名称")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "公司名称含有非法字符.")]
        //[Remote("NickNameExists", "AjaxService", ErrorMessage = "该公司名称含有非法字符或已经注册")]
        [StringCheckLength(7, 50)]
        [Hint("请输入7-50位公司名称，英文、数字或中文均可（中文占2个字符）。")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请选择公司所在城市")]
        [Display(Name = "所在城市")]
        [HintClass("city")]
        [UIHint("Cascading")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "请输入公司简介.")]
        [Display(Name = "公司简介")]
        [DataType(DataType.MultilineText)]
        [StringCheckLength(10, 2000)]
        [Hint("请输入10-2000位公司简介，（中文占2个字符）。")]
        [HintClass("textarea")]
        public string Description { get; set; }

        [HintSeparateTitle("联系方式")]
        [Required(ErrorMessage = "请输入公司联系地址")]
        [Display(Name = "联系地址")]
        [StringCheckLength(7, 50)]
        [Hint("公司联系地址7-50个字，英文、数字或中文均可（中文占2个字符），不允许填写电话和特殊符号.")]
        public string Address { get; set; }

        [Display(Name = "地图位置")]
        [Required(ErrorMessage = "请标记公司所在地图坐标.")]
        [UIHint("MapMarker")]
        public string Position { get; set; }

        [Required(ErrorMessage = "请输入联系人")]
        [Display(Name = "联系人")]
        [StringCheckLength(4, 10)]
        [Hint("请输入4-10位联系人姓名，（中文占2个字符）。")]
        public string LinkMan { get; set; }

        [Required(ErrorMessage = "请选择联系人性别")]
        [Display(Name = "性别")]
        [AdditionalMetadata("RadioList", "男,女")]
        [UIHint("RadioList")]
        public bool Sex { get; set; }

        //[Display(Name = "手机号码")]
        //[RegularExpression(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$", ErrorMessage = "您输入的手机号码格式不正确.")]
        //[Hint("请输入手机号码,手机号码和电话号码只需填一项.")]
        //[HintClass("contact")]
        //public string Mobile { get; set; }

        //[Display(Name = "电话号码")]
        //[Hint("请输入电话号码,电话号码格式010-2013042-1323，区号加电话号码加分机号码，无分机可以不填")]
        //[RegularExpression(@"^0\d{2,3}(\-)?\d{7,8}$", ErrorMessage = "您输入的电话号码格式不正确.")]
        //[RequireWith("Mobile", "手机号码")]
        //[HintClass("contact")]
        //public string Phone { get; set; }

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


        [Required(ErrorMessage = "请输入验证码")]
        [Display(Name = "验证码")]
        [StringLength(4, ErrorMessage = "长度为4位", MinimumLength = 4)]
        [Remote("ValidateVCode", "AjaxService", ErrorMessage = "验证码错误或过期")]
        [UIHint("ValidateVCode")]
        [HintClass("validatecode")]
        public string Vcode { get; set; }
    }
}