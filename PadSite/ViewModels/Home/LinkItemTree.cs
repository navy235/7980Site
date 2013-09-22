using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class LinkItemTree
    {
        public LinkItemTree()
        {
            this.Children = new List<LinkItemTree>();
        }

        public int Value { get; set; }

        public int PID { get; set; }

        public string Text { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public List<LinkItemTree> Children { get; set; }
    }


}