using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PadSite.Models;
using PadSite.Service;
using PadSite.ViewModels;
using PadSite.Utils;
using PadSite.Setting;
using Maitonn.Core;
namespace PadSite.Tests
{
    [TestClass]
    public class MessageTest
    {

        private static readonly IUnitOfWork db = new EntitiesContext();

        private static readonly MessageService MessageService = new MessageService(db);

        [TestMethod]
        public void AddSysMessage()
        {
            var message = new Message()
            {
                AddTime = DateTime.Now,
                SenderID = 0,
                RecipientID = 1,
                MessageType = (int)MessageType.System,
                RecipienterStatus = (int)MessageStatus.Show,
                Title = "系统留言添加",
                Content = "系统留言添加"
            };
            MessageService.Create(message);
        }

        [TestMethod]
        public void AddMemberMessage()
        {
            var message = new Message()
            {
                AddTime = DateTime.Now,
                SenderID = 2,
                RecipientID = 1,
                MessageType = (int)MessageType.Member,
                RecipienterStatus = (int)MessageStatus.Show,
                Title = "用户发送留言",
                Content = "用户发送留言"
            };
            MessageService.Create(message);
        }
    }
}
