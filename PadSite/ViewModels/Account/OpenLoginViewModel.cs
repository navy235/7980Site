using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class OpenLoginViewModel
    {
        public string Uid { get; set; }
        public int OpenType { get; set; }
        public bool Success { get; set; }
        public string OpenId { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string NickName { get; set; }
    }
}