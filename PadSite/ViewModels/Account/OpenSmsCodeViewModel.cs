using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadSite.ViewModels
{
    public class OpenSmsCodeViewModel
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int res_code { get; set; }
        public bool Success { get; set; }
        public string open_id { get; set; }
        public string refresh_token { get; set; }
        public string res_message { get; set; }
    }
}