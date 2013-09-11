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
    public class CompanyContactInfoViewModel
    {
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

        [Display(Name = "手机号码")]
        [RegularExpression(@"^(13[0-9]|15[0-9]|18[0-9])\d{8}$", ErrorMessage = "您输入的手机号码格式不正确.")]
        [Hint("请输入手机号码.")]
        public string Mobile { get; set; }

        [Display(Name = "电话号码")]
        [Hint("请输入电话号码,电话号码格式010-2013042-1323，区号加电话号码加分机号码，无分机可以不填")]
        [RegularExpression(@"^0\d{2,3}(\-)?\d{7,8}$", ErrorMessage = "您输入的电话号码格式不正确.")]
        [RequireWith("Mobile", "手机号码")]
        public string Phone { get; set; }

        [Display(Name = "传真")]
        [RegularExpression(@"^^\d{3}-\d{8}(-\d{3,4})?|\d{4}-\d{7}(-\d{3,4})??$", ErrorMessage = "您输入的传真号码格式不正确.")]
        [Hint("请输传真号码,传真格式010-2013042-1323，区号加传真号码加分机号码，无分机可以不填")]
        public string Fax { get; set; }

        [Display(Name = "QQ号码")]
        [RegularExpression(@"^[1-9][0-9]{4,10}$", ErrorMessage = "您输入的QQ号码格式不正确.")]
        public string QQ { get; set; }

        [Display(Name = "MSN")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的MSN格式不正确.")]
        public string MSN { get; set; }
    }
}