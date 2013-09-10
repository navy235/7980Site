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
    public class EditViewModel
    {
        public EditViewModel()
        {
            Borthday = DateTime.Now;
        }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }


        [HiddenInput(DisplayValue = true)]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }


        [HiddenInput(DisplayValue = true)]
        [Display(Name = "电子邮箱")]
        public string NickName { get; set; }


        [Required(ErrorMessage = "请设置用户所属群组")]
        [Display(Name = "用户群组")]
        [UIHint("ForeignKeyForRadio")]
        public int GroupID { get; set; }



        //[Required(ErrorMessage = "请上传头像")]
        [Display(Name = "上传头像")]
        [HintClass("avtar")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }


        //[Required(ErrorMessage = "请选择城市")]
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