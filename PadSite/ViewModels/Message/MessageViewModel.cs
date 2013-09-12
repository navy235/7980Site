using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadSite.ViewModels
{
    public class MessageViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "消息标题")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "消息内容")]
        public string Content { get; set; }

        [Display(Name = "状态")]
        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }


        [Display(Name = "发送人ID")]
        [HiddenInput(DisplayValue = false)]
        public int SenderID { get; set; }


        [HintSeparateTitle("联系人信息")]

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }


        [HiddenInput(DisplayValue = true)]
        [Display(Name = "电话")]
        public string Phone { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "MSN")]
        public string MSN { get; set; }

        [HiddenInput(DisplayValue = true)]
        [Display(Name = "留言时间")]
        public DateTime AddTime { get; set; }

        [Required(ErrorMessage = "请输入回复信息")]
        [Display(Name = "回复信息")]
        [StringCheckLength(4, 2000)]
        //[Remote("ValidateDescription", "AjaxService", ErrorMessage = "简介含有非法字符")]
        [DataType(DataType.MultilineText)]
        [Hint("请输入至少4-2000个字，支持中文、英文。（中文占2个字符）。")]
        public string Replay { get; set; }
    }
}