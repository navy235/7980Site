namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class CompanyNotice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MemberID { get; set; }

        [MaxLength(2000)]
        public string Content { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public DateTime AddTime { get; set; }

        public int Status { get; set; }

        public virtual Member Member { get; set; }
    }
}
