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
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Borthday = DateTime.Now;
        }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱")]
        [Remote("EmailExists", "AjaxService", ErrorMessage = "该电子邮箱已经注册")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        [Hint("此电子邮箱将作为广知网登陆帐号,并作为密码找回邮箱，请认真填写。")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入昵称")]
        [Display(Name = "昵称")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "昵称含有非法字符.")]
        [Remote("NickNameExists", "AjaxService", ErrorMessage = "该昵称含有非法字符或已经注册")]
        [StringCheckLength(4, 14)]
        [Hint("请输入4-14位昵称，英文、数字或中文均可（中文占2个字符）。")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "请设置用户所属群组")]
        [Display(Name = "用户群组")]
        [UIHint("ForeignKeyForRadio")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(15, ErrorMessage = "请输入{2}-{1}位密码", MinimumLength = 6)]
        [Display(Name = "设定密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "上传头像")]
        [HintClass("avtar")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }

        [HintClass("city")]
        [Display(Name = "所在城市")]
        [UIHint("Cascading")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "请选择性别")]
        [Display(Name = "性别")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "男,女")]
        public bool Sex { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "出生日期")]
        [UIHint("Date")]
        public DateTime Borthday { get; set; }


        [Display(Name = "个人简介")]
        [StringCheckLength(4, 200)]
        //[Remote("ValidateDescription", "AjaxService", ErrorMessage = "简介含有非法字符")]
        [DataType(DataType.MultilineText)]
        [Hint("请输入至少4-200个字，支持中文、英文。（中文占2个字符）。")]
        public string Description { get; set; }
    }
}