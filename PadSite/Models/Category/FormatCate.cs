using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadSite.Models
{
    public class FormatCate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        public string CateName { get; set; }

        public int? PID { get; set; }

        public int Code { get; set; }

        public int Level { get; set; }

        public int OrderIndex { get; set; }

        [ScriptIgnore]
        public virtual FormatCate PCate { get; set; }

        [ScriptIgnore]
        public virtual ICollection<FormatCate> ChildCates { get; set; }
    }
}