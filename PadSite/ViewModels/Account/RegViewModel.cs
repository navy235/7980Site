﻿using System;
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
    public class RegViewModel
    {


        [Required(ErrorMessage = "请选择用户类型")]
        [Display(Name = "用户类型")]
        [UIHint("RadioCheckList")]
        [HintClass("regchecklist")]
        [Hint("请认真选择用户类型，用户类型所具备的功能不一致，详细请看帮助。")]
        public int MemberType { get; set; }

        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱")]
        [Remote("EmailExists", "AjaxService", ErrorMessage = "该电子邮箱已经注册")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        [Hint("此电子邮箱将作为登陆帐号,并作为密码找回邮箱，请认真填写。")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入昵称")]
        [Display(Name = "昵称")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "昵称含有非法字符.")]
        [Remote("NickNameExists", "AjaxService", ErrorMessage = "该昵称含有非法字符或已经注册")]
        [StringCheckLength(4, 14)]
        [Hint("请输入4-14位昵称，英文、数字或中文均可（中文占2个字符）。")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(15, ErrorMessage = "请输入{2}-{1}位密码", MinimumLength = 6)]
        [Display(Name = "设定密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string OpenID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int OpenType { get; set; }

        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }


        //[Required(ErrorMessage = "请输入验证码")]
        //[Display(Name = "验证码")]
        //[StringLength(4, ErrorMessage = "长度为4位", MinimumLength = 4)]
        //[Remote("ValidateVCode", "AjaxService", ErrorMessage = "验证码错误或过期")]
        //[UIHint("ValidateVCode")]
        //[HintClass("validatecode")]
        //public string Vcode { get; set; }




        [Required(ErrorMessage = "请输入手机号码")]
        [Display(Name = "手机号码")]
        [RegularExpression(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$", ErrorMessage = "您输入的手机号码格式不正确.")]
        [Hint("请输入手机号码,并验证")]
        [HintClass("contact")]
        public string Mobile { get; set; }


        [Required(ErrorMessage = "请输入短信验证码")]
        [Display(Name = "短信验证码")]
        [Hint("请输入手机验证码，没有收到验证码请稍候点击获取（测试阶段输入1241）")]
        [StringLength(4, ErrorMessage = "长度为4位", MinimumLength = 4)]
        [Remote("ValidateSmsVCode", "AjaxService", ErrorMessage = "验证码错误或过期")]
        [UIHint("SmsVCode")]
        [AdditionalMetadata("mobile", "Mobile")]
        [HintClass("validatecode")]
        public string SmsVcode { get; set; }
    }
}