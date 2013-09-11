using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class CompanyMessageService : ICompanyMessageService
    {
        private readonly IUnitOfWork db;

        public CompanyMessageService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CompanyMessage> GetALL()
        {
            return db.Set<CompanyMessage>();
        }

        public IQueryable<CompanyMessage> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CompanyMessage>();
        }

        public void Create(CompanyMessage model)
        {
            db.Add<CompanyMessage>(model);
            db.Commit();
        }

        public void Update(CompanyMessage model)
        {
            var target = Find(model.ID);
            db.Attach<CompanyMessage>(target);
            target.Content = model.Content;
            target.Title = model.Title;
            target.Status = model.Status;
            db.Commit();
        }

        public void Delete(CompanyMessage model)
        {
            var target = Find(model.ID);
            db.Remove<CompanyMessage>(target);
            db.Commit();
        }

        public CompanyMessage Find(int ID)
        {
            return db.Set<CompanyMessage>().Single(x => x.ID == ID);
        }
    }
}