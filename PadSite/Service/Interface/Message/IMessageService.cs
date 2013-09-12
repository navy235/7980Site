using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IMessageService
    {
        IQueryable<Message> GetALL();

        IQueryable<Message> GetKendoALL();

        void Create(Message model);

        void Update(Message model);

        void Delete(Message model);

        Message Find(int ID);

        void ReadMessage(int MessageID);

        void DeleteSenderMessage(int MessageID);

        void DeleteRecipienterMessage(int MessageID);

    }
}