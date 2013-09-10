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
    public class ContactViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "手机号码")]
        [Display(Name = "手机号码")]
        [RegularExpression(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$", ErrorMessage = "请输入正确的手机号码.")]
        public string Mobile { get; set; }


        [Display(Name = "固定电话")]
        [RegularExpression(@"^0\d{2,3}(\-)?\d{7,8}$", ErrorMessage = "请按照格式输入正确的固定电话号码.")]
        [Hint("请按照区号-电话号码（如：021-888234）格式输入")]
        public string Phone { get; set; }

        [Display(Name = "QQ号码")]
        [RegularExpression(@"^[1-9][0-9]{4,10}$", ErrorMessage = "请输入正确的QQ号码.")]
        public string QQ { get; set; }

        [Display(Name = "MSN")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的MSN格式不正确.")]
        public string MSN { get; set; }


        [Display(Name = "详细地址")]
        [StringCheckLength(10, 60)]
        public string Address { get; set; }

        [Display(Name = "标记坐标")]
        [UIHint("MapMarker")]
        [Hint("您还可以在地图上标注您的位置，更方便大家找到您")]
        public string Position { get; set; }
    }
}