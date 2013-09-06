using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadSite.Models
{
    public class CityCate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "城市名称")]
        public string CateName { get; set; }

        [Display(Name = "父级城市")]
        public int? PID { get; set; }

        [Display(Name = "城市代码")]
        public int Code { get; set; }

        [Display(Name = "城市级别")]
        public int Level { get; set; }

        [Display(Name = "排序代码")]
        public int OrderIndex { get; set; }

        [ScriptIgnore]
        public virtual CityCate PCate { get; set; }

        [ScriptIgnore]
        public virtual ICollection<CityCate> ChildCates { get; set; }


    }
}