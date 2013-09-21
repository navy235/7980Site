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
    public class OutDoorViewModel
    {
        public OutDoorViewModel()
        {
            //this.StartTime = DateTime.Now;
            //this.EndTime = DateTime.Now;
            this.Deadline = DateTime.Now;
            this.Price = 0;
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HintSeparateTitle("基本信息")]
        [Required(ErrorMessage = "请输入媒体名称")]
        [Display(Name = "媒体名称")]
        [CheckContact]
        [StringLength(25, ErrorMessage = "请输入{2}-{1}位{0}", MinimumLength = 6)]
        [Hint("6-25个字，不允许填写电话和特殊符号。例：新华书店附近户外大牌")]
        public string Name { get; set; }


        [Required(ErrorMessage = "请选择媒体类别")]
        [Display(Name = "媒体类别")]
        [UIHint("Cascading")]
        public string MediaCode { get; set; }

        [Required(ErrorMessage = "请选择媒体展现形式")]
        [Display(Name = "展现形式")]
        [UIHint("DropDownList")]
        public int FormatCode { get; set; }

        [Display(Name = "价格")]
        [UIHint("Price")]
        [AdditionalMetadata("Price", "0,1000")]
        [AdditionalMetadata("PriceUnit", "万元/年")]
        [HintClass("price")]
        [Hint("不填价格，默认为面议，为了方便搜索请填写参考价格")]
        public decimal Price { get; set; }


        [Display(Name = "价格说明")]
        [DataType(DataType.MultilineText)]
        public string PriceExten { get; set; }

        [Required(ErrorMessage = "请选择最短购买周期")]
        [Display(Name = "最短购买")]
        [UIHint("DropdownList")]
        public int PeriodCode { get; set; }


        [Required(ErrorMessage = "请选择档期开始时间")]
        [Display(Name = "档期开始")]
        [DataType(DataType.DateTime)]
        [UIHint("Date")]
        [Hint("设置媒体档期，档期时间之前为不可售，默认为当天")]
        public DateTime Deadline { get; set; }


        [HintSeparateTitle("位置信息")]
        [Required(ErrorMessage = "请选择媒体城市")]
        [Display(Name = "媒体城市")]
        [UIHint("Cascading")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "请输入具体位置")]
        [Display(Name = "具体位置")]
        [CheckContact]
        public string Location { get; set; }

        [Required(ErrorMessage = "请选择媒体区域属性")]
        [Display(Name = "区域属性")]
        [UIHint("CheckList")]
        [CheckMaxLength(5)]
        [HintClass("meida-list")]
        public string AreaCate { get; set; }

        [Display(Name = "地图坐标")]
        [Required(ErrorMessage = "请标记媒体地图坐标.")]
        [UIHint("MapMarker")]
        public string Position { get; set; }

        [Display(Name = "照明情况")]
        [Required(ErrorMessage = "请输入照明信息")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "无,有")]
        [CascadeRequire("LightTime", "照明时间")]
        public bool HasLight { get; set; }

        [Display(Name = "照明时间")]
        [UIHint("TimeQuantum")]
        [HintClass("cascade")]
        [AdditionalMetadata("CascadeBack", "HasLight")]
        public string LightTime { get; set; }

        [Required(ErrorMessage = "请输入媒体面积相关参数")]
        [Display(Name = "媒体面积")]
        [UIHint("Area")]
        [CheckArea]
        public string MediaArea { get; set; }

        [Required(ErrorMessage = "请输入日交通车流量")]
        [Display(Name = "日车流量")]
        [UIHint("IntegerExtension")]
        [AdditionalMetadata("IntegerExtension", "0,200")]
        [AdditionalMetadata("IntegerExtensionUnit", "万辆/天")]
        public int TrafficAuto { get; set; }

        [Required(ErrorMessage = "请输入日交通人流量")]
        [Display(Name = "日人流量")]
        [UIHint("IntegerExtension")]
        [AdditionalMetadata("IntegerExtension", "0,200")]
        [AdditionalMetadata("IntegerExtensionUnit", "万人/天")]
        public int TrafficPerson { get; set; }

        [Display(Name = "媒体视频")]
        [RegularExpression(@"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$", ErrorMessage = "输入视频地址格式不正确.")]
        [HintLabel("视频地址为优酷的flash播放地址，详细请看帮助")]
        public string VideoUrl { get; set; }



        [HintSeparateTitle("图片信息")]
        [Required(ErrorMessage = "请上传媒体图片.")]
        [Display(Name = "媒体图片")]
        [UIHint("UploadImgList")]
        [HintClass("uploadlist")]
        [AdditionalMetadata("UploadImgList", "")]
        [AdditionalMetadata("UploadImgListMaxLength", "6")]
        [HintLabel("请上传1-6张不小于800X600像素的图片,图片文件大小不超过5M")]
        public string MediaImg { get; set; }

        [HintSeparateTitle("规格所有权信息")]
        [Display(Name = "所有权")]
        [UIHint("DropdownList")]
        public int OwnerCode { get; set; }

        [Display(Name = "相关证书")]
        [UIHint("UploadImgList")]
        [HintClass("uploadlist")]
        [AdditionalMetadata("UploadImgList", "3")]
        [AdditionalMetadata("UploadImgListMaxLength", "6")]
        [HintLabel("请上传1-6张不小于800X600像素的图片,图片文件大小不超过5M")]
        public string CredentialsImg { get; set; }




        [HintSeparateTitle("媒体补充信息")]

        [Display(Name = "受众人群")]
        [UIHint("CheckList")]
        [CheckMaxLength(5)]
        [HintClass("meida-list")]
        [Hint("媒体的受众人群类型")]
        public string CrowdCate { get; set; }

        [Display(Name = "投放行业")]
        [UIHint("CheckList")]
        [CheckMaxLength(5)]
        [HintClass("meida-list")]
        [Hint("媒体适合的投放行业")]
        public string IndustryCate { get; set; }

        [Display(Name = "投放目的")]
        [UIHint("CheckList")]
        [CheckMaxLength(5)]
        [HintClass("meida-list")]
        [Hint("媒体投放的应用目的")]
        public string PurposeCate { get; set; }

        [Required(ErrorMessage = "请输入媒体补充说明.")]
        [Display(Name = "补充说明")]
        [DataType(DataType.MultilineText)]
        [StringCheckLength(4, 1000)]
        [Hint("请输入4-1000位媒体补充说明，英文、数字或中文均可（中文占2个字符）。")]
        public string Description { get; set; }
    }
}