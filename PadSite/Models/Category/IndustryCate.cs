using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadSite.Models
{
    public class IndustryCate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "分类名称")]
        public string CateName { get; set; }

        public virtual ICollection<OutDoor> OutDoor { get; set; }
    }
}