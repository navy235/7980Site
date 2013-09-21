namespace PadSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OutDoor
    {

        public OutDoor()
        {
            this.AreaCate = new HashSet<AreaCate>();
            this.CrowdCate = new HashSet<CrowdCate>();
            this.IndustryCate = new HashSet<IndustryCate>();
            this.PurposeCate = new HashSet<PurposeCate>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public int Integrity { get; set; }

        public int Hit { get; set; }

        public System.DateTime AddTime { get; set; }

        public System.DateTime LastTime { get; set; }

        public int Favorite { get; set; }

        public int Message { get; set; }

        public int MemberID { get; set; }

        [MaxLength(50)]
        public string AddIP { get; set; }

        public int AdminUser { get; set; }

        [MaxLength(50)]
        public string LastIP { get; set; }

        [MaxLength(500)]
        public string Unapprovedlog { get; set; }

        [MaxLength(100)]
        public string SeoTitle { get; set; }

        [MaxLength(250)]
        public string SeoDes { get; set; }

        [MaxLength(100)]
        public string Seokeywords { get; set; }

        public decimal Price { get; set; }

        [MaxLength(100)]
        public string PriceExten { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public bool HasLight { get; set; }

        public int LightStart { get; set; }

        public int LightEnd { get; set; }

        [MaxLength(250)]
        public string VideoUrl { get; set; }


        public bool IsRegular { get; set; }

        public decimal Wdith { get; set; }

        public decimal Height { get; set; }

        public int TotalFaces { get; set; }

        public decimal TotalArea { get; set; }

        public string IrRegularArea { get; set; }


        public int TrafficAuto { get; set; }

        public int TrafficPerson { get; set; }

        public int CityCode { get; set; }

        public string CityCodeValue { get; set; }

        public int FormatCode { get; set; }

        public int MediaCode { get; set; }

        public string MediaCodeValue { get; set; }

        public int PeriodCode { get; set; }

        public int OwnerCode { get; set; }

        public System.DateTime Deadline { get; set; }

        public int AuthStatus { get; set; }

        public int Status { get; set; }

        public string MediaImg { get; set; }

        public string MediaFoucsImg { get; set; }

        public string CredentialsImg { get; set; }

        public virtual ICollection<AreaCate> AreaCate { get; set; }

        public virtual ICollection<CrowdCate> CrowdCate { get; set; }

        public virtual ICollection<IndustryCate> IndustryCate { get; set; }

        public virtual ICollection<PurposeCate> PurposeCate { get; set; }

        public virtual CityCate CityCate { get; set; }

        public virtual MediaCate MediaCate { get; set; }

        public virtual PeriodCate PeriodCate { get; set; }

        public virtual FormatCate FormatCate { get; set; }

        public virtual OwnerCate OwnerCate { get; set; }

        public virtual Member Member { get; set; }
    }
}