using System;
using PadSite.ViewModels;

namespace PadSite.Service.Interface
{
    public interface IEmailService
    {
        bool SendMail(EmailModel model);

        EmailModel GetMail(int MemberID, string Email, string NickName, string Key);
    }
}