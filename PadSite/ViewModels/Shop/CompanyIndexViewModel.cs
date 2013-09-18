using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;

namespace PadSite.ViewModels
{
    public class CompanyIndexViewModel
    {
        public CompanyIndexViewModel()
        {
            this.Categories = new List<CompanyCategoryViewModel>();
           
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BannerImg { get; set; }

        public string LogoImg { get; set; }

        public string LinkMan { get; set; }

        public string Address { get; set; }

        public string CityName { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public bool Sex { get; set; }

        public string Mobile { get; set; }

        public string QQ { get; set; }

        public string Phone { get; set; }

        public List<CompanyCategoryViewModel> Categories { get; set; }

        public QuerySource Sources { get; set; }

        public CompanyNotice Notice { get; set; }

        public LinkItem Links { get; set; }

    }

    public class CompanyCategoryViewModel
    {

        public CompanyCategoryViewModel()
        {
            this.Products = new List<CompanyProductViewMidel>();
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Count { get; set; }

        public List<CompanyProductViewMidel> Products { get; set; }
    }

    public class CompanyProductViewMidel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string CityCateName { get; set; }

        public string PeriodName { get; set; }

        public string FormatName { get; set; }

        public string MediaCateName { get; set; }

        public string FocusImgUrl { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime DeadLine { get; set; }
    }
}