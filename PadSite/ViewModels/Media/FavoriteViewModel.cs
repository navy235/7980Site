using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class FavoriteViewModel
    {
        public int ID { get; set; }

        public int MediaID { get; set; }

        public int MemberID { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public DateTime AddTime { get; set; }
    }
}