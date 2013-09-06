namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CompanyCredentialsImg
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MemberID { get; set; }

        [MaxLength(200)]
        public string ImgUrl { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public virtual Member Member { get; set; }
    }
}
