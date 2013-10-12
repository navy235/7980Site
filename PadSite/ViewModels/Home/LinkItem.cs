using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class LinkItem
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public string ImgUrl { get; set; }

        public string FocusImgUrl { get; set; }

        public string CredentialsImg { get; set; }


        public string VideoUrl { get; set; }

        public string MidImgUrl { get; set; }

        public string SmallImgUrl { get; set; }

        public string HintClass { get; set; }

        public string AreaCate { get; set; }

        public string CrowdCate { get; set; }

        public string IndustryCate { get; set; }

        public string PurposeCate { get; set; }

        public string Url { get; set; }

        public string Location { get; set; }

        public int TrafficAuto { get; set; }

        public int TrafficPerson { get; set; }

        public decimal Price { get; set; }

        public int AuthStatus { get; set; }

        public int CityCode { get; set; }

        public string CityCateName { get; set; }

        public string CityCateValue { get; set; }

        public int CityCateCode { get; set; }

        public int MediaCode { get; set; }

        public string MediaCateName { get; set; }

        public string MediaCateValue { get; set; }

        public int MediaCateCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Mobile { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public int MemberStatus { get; set; }

        public bool Selected { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public int TotalFaces { get; set; }

        public bool HasLight { get; set; }

        public int LightStart { get; set; }

        public int LightEnd { get; set; }

        public bool IsRegular { get; set; }

        public string IrRegularArea { get; set; }

        public decimal TotalArea { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime AddTime { get; set; }

        public string PeriodName { get; set; }

        public string OwnerName { get; set; }

        public string FormatName { get; set; }

        public int Status { get; set; }

        public int SuggestStatus { get; set; }

        public int Hit { get; set; }

    }

    public class LinkGroup
    {
        public LinkGroup()
        {
            this.Items = new List<LinkItem>();
        }

        public LinkItem Group { get; set; }

        public List<LinkItem> Items { get; set; }
    }
}