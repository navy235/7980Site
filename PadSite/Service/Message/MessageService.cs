using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork db;

        public MessageService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Message> GetALL()
        {
            return db.Set<Message>();
        }

        public IQueryable<Message> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Message>();
        }

        public void Create(Message model)
        {
            db.Add<Message>(model);
            db.Commit();
        }

        public void Update(Message model)
        {
            var target = Find(model.ID);
            db.Attach<Message>(target);
            target.Content = model.Content;
            target.Title = model.Title;
            db.Commit();
        }

        public void Delete(Message model)
        {
            var target = Find(model.ID);
            db.Remove<Message>(target);
            db.Commit();
        }

        public Message Find(int ID)
        {
            return db.Set<Message>().Single(x => x.ID == ID);
        }

        public void ReadMessage(int MessageID)
        {
            var target = Find(MessageID);
            db.Attach<Message>(target);
            target.IsRead = true;
            db.Commit();
        }

        public void DeleteSenderMessage(int MessageID)
        {
            var target = Find(MessageID);
            db.Attach<Message>(target);
            target.SenderStatus = (int)MessageStatus.Delete;
            db.Commit();
        }

        public void DeleteRecipienterMessage(int MessageID)
        {
            var target = Find(MessageID);
            db.Attach<Message>(target);
            target.RecipienterStatus = (int)MessageStatus.Delete;
            db.Commit();
        }

    }
}