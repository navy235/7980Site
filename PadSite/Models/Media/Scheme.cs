namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Scheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MemberID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime LastTime { get; set; }

        public int Status { get; set; }

        public virtual ICollection<SchemeItem> SchemeItem { get; set; }

        public virtual Member Member { get; set; }
    }
}