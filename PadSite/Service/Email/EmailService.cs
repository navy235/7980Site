using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using PadSite.Service.Interface;
using Maitonn.Core;
using PadSite.Setting;
namespace PadSite.Service
{
    public class EmailService : IEmailService
    {

        public bool SendMail(ViewModels.EmailModel model)
        {
            return MailHelper.SendMail(model.Email, model.Title, model.Content, ConfigSetting.SiteName);
        }

        public ViewModels.EmailModel GetMail(string TempleteUrl, string EmailTitle, int MemberID, string Email, string NickName, string Key)
        {
            EmailModel em = new EmailModel();
            em.Email = Email;
            em.Title = EmailTitle;
            em.Content = System.IO.File.ReadAllText(TempleteUrl, System.Text.Encoding.Default);
            em.Content = em.Content.Replace("{key}", Key)
                .Replace("{nid}", NickName)
                .Replace("{uid}", MemberID.ToString())
                .Replace("{time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{email}", Email)
                .Replace("{domain}", ConfigSetting.DomainUrl)
                .Replace("{sitename}", ConfigSetting.SiteName);
            return em;
        }
    }
}