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
    public class GetPasswordViewModel
    {
        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱：")]
        [Remote("HasEmailUser", "AjaxService", ErrorMessage = "该电子邮箱未被注册")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        public string Email { get; set; }
    }
}