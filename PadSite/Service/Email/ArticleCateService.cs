using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class EmailService : IEmailService
    {

        public bool SendMail(ViewModels.EmailModel model)
        {
            throw new NotImplementedException();
        }

        public ViewModels.EmailModel GetMail(int MemberID, string Email, string NickName, string Key)
        {
            throw new NotImplementedException();
        }
    }
}