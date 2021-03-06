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
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "请输入新密码")]
        [StringLength(15, ErrorMessage = "请输入{2}-{1}位密码", MinimumLength = 6)]
        [Display(Name = "输入新密码：")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "再确认一次：")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}