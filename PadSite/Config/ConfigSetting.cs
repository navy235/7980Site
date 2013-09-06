using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Maitonn.Core;

namespace PadSite.Setting
{
    public static class ConfigSetting
    {
        public static string Default_AvtarUrl { get; set; }

        public static string Default_LogoUrl { get; set; }

        public static int GetPasswordEmailTimeDiffMin { get; set; }

        public static int ResetPasswordTimeDiffHour { get; set; }

        public static int GetBindEmailTimeDiffMin { get; set; }

        public static int ActiveEmailTimeDiffHour { get; set; }

        public static string DomainUrl { get; set; }

        public static string SiteName { get; set; }

        static ConfigSetting()
        {
            Default_AvtarUrl = ConfigurationManager.AppSettings["Default_AvtarUrl"];
            Default_LogoUrl = ConfigurationManager.AppSettings["Default_LogoUrl"];
            GetPasswordEmailTimeDiffMin = Convert.ToInt32(ConfigurationManager.AppSettings["GetPasswordEmailTimeDiffMin"]);
            ResetPasswordTimeDiffHour = Convert.ToInt32(ConfigurationManager.AppSettings["ResetPasswordTimeDiffHour"]);
            GetBindEmailTimeDiffMin = Convert.ToInt32(ConfigurationManager.AppSettings["GetBindEmailTimeDiffMin"]);
            ActiveEmailTimeDiffHour = Convert.ToInt32(ConfigurationManager.AppSettings["ActiveEmailTimeDiffHour"]);
            DomainUrl = ConfigurationManager.AppSettings["LocalDomain"];
            SiteName = ConfigurationManager.AppSettings["SiteName"];
        }
    }
}