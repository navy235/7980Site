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

        public static string MapKey { get; set; }

        public static string QQAppID { get; set; }

        public static string QQKey { get; set; }

        public static string TaboBaoAppID { get; set; }

        public static string TaboBaoKey { get; set; }

        public static string SinaAppID { get; set; }

        public static string SinaKey { get; set; }

        public static string RenRenAppID { get; set; }

        public static string RenRenAPPKey { get; set; }

        public static string RenRenSecretKey { get; set; }

        public static string DouBanKey { get; set; }

        public static string DouBanSecret { get; set; }

        public static string SmsAppID { get; set; }

        public static string SmsAppSecret { get; set; }

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
            MapKey = ConfigurationManager.AppSettings["MapKey"];

            QQAppID = ConfigurationManager.AppSettings["QQAppID"];
            QQKey = ConfigurationManager.AppSettings["QQKey"];

            TaboBaoAppID = ConfigurationManager.AppSettings["TaboBaoAppID"];
            TaboBaoKey = ConfigurationManager.AppSettings["TaboBaoKey"];
            SinaAppID = ConfigurationManager.AppSettings["SinaAppID"];
            SinaKey = ConfigurationManager.AppSettings["SinaKey"];
            RenRenAppID = ConfigurationManager.AppSettings["RenRenAppID"];
            RenRenAPPKey = ConfigurationManager.AppSettings["RenRenAPPKey"];
            RenRenSecretKey = ConfigurationManager.AppSettings["RenRenSecretKey"];
            DouBanKey = ConfigurationManager.AppSettings["DouBanKey"];
            DouBanSecret = ConfigurationManager.AppSettings["DouBanSecret"];
            SmsAppID = ConfigurationManager.AppSettings["SmsAppID"];
            SmsAppSecret = ConfigurationManager.AppSettings["SmsAppSecret"];
        }
    }
}