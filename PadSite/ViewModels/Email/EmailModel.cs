using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PadSite.ViewModels
{
    public class EmailModel
    {

        public string Title { get; set; }

        public string Content { get; set; }
        public string Email { get; set; }
    }
}