namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Company
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public int MemberID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string LinkMan { get; set; }

        public bool Sex { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        public string QQ { get; set; }

        [MaxLength(50)]
        public string MSN { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public int CityCode { get; set; }
         
        public string CityCodeValue { get; set; }

        public System.DateTime LastTime { get; set; }

        [MaxLength(50)]
        public string LastIP { get; set; }

        public System.DateTime AddTime { get; set; }

        [MaxLength(50)]
        public string AddIP { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Unapprovedlog { get; set; }

        [MaxLength(2000)]
        public string LinkManImg { get; set; }

        [MaxLength(2000)]
        public string CredentialsImg { get; set; }

        [MaxLength(200)]
        public string LogoImg { get; set; }

        [MaxLength(200)]
        public string BannerImg { get; set; }

        [MaxLength(50)]
        public string IdentityCard { get; set; }

        public int Status { get; set; }

        public virtual Member Member { get; set; }
    }
}