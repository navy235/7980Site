using System;

namespace PadSite.Service
{
    public class OutDoorIndexEntity
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int CityCode { get; set; }

        public int CityCateCode { get; set; }

        public string CityCateValue { get; set; }

        public string CityCateName { get; set; }

        public int MediaCode { get; set; }

        public int MediaCateCode { get; set; }

        public string MediaCateValue { get; set; }

        public string MediaCateName { get; set; }

        public int FormatCode { get; set; }

        public string FormatName { get; set; }

        public int PeriodCode { get; set; }

        public string PeriodName { get; set; }

        public int OwnerCode { get; set; }

        public string OwnerName { get; set; }

        public int HasLight { get; set; }

        public int LightStart { get; set; }

        public int LightEnd { get; set; }

        public int Status { get; set; }

        public int AuthStatus { get; set; }

        public int TrafficAuto { get; set; }

        public int TrafficPerson { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string AreaCate { get; set; }
        public string CrowdCate { get; set; }
        public string IndustryCate { get; set; }
        public string PurposeCate { get; set; }

        public string ImgUrl { get; set; }

        public string FocusImgUrl { get; set; }

        public string CredentialsImg { get; set; }

        public string VideoUrl { get; set; }



        public int IsRegular { get; set; }

        public int Hit { get; set; }

        public decimal Price { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public int TotalFaces { get; set; }

        public decimal TotalArea { get; set; }

        public string IrRegularArea { get; set; }


        public int MemberStatus { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public string CompanyName { get; set; }

        public DateTime Published { get; set; }

        public DateTime DeadLine { get; set; }
    }
}