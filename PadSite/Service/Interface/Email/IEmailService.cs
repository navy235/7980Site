using System;
using PadSite.ViewModels;

namespace PadSite.Service.Interface
{
    public interface IEmailService
    {
        bool SendMail(EmailModel model);

        EmailModel GetMail(string TempleteUrl, string EmailTitle, int MemberID, string Email, string NickName, string Key);
    }
}