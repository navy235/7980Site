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
    public class CompanyCredentialsViewModel
    {
        [Required(ErrorMessage = "请输入证书名称")]
        [Display(Name = "证书名称")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "证书名称含有非法字符.")]
        [StringCheckLength(7, 20)]
        [Hint("请输入7-20位证书名称，英文、数字或中文均可（中文占2个字符）。")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请上传企业证书")]
        [Display(Name = "企业证书")]
        [UIHint("UploadImg")]
        public string ImgUrl { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
    }
}