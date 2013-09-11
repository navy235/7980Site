using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
using PadSite.Utils;
namespace PadSite.Service
{
    public class CompanyNoticeService : ICompanyNoticeService
    {
        private readonly IUnitOfWork db;

        public CompanyNoticeService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CompanyNotice> GetALL()
        {
            return db.Set<CompanyNotice>();
        }

        public IQueryable<CompanyNotice> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CompanyNotice>();
        }

        public void Create(CompanyNotice model)
        {
            db.Add<CompanyNotice>(model);
            db.Commit();
        }

        public void Update(CompanyNotice model)
        {
            var target = Find(model.ID);
            db.Attach<CompanyNotice>(target);
            target.Content = model.Content;
            target.Title = model.Title;
            db.Commit();
        }

        public void Delete(CompanyNotice model)
        {
            var target = Find(model.ID);
            db.Remove<CompanyNotice>(target);
            db.Commit();
        }

        public CompanyNotice Find(int ID)
        {
            return db.Set<CompanyNotice>().Single(x => x.ID == ID);
        }


        public void ChangeStatus(string Ids, int Status)
        {
            var listIds = Utilities.GetIdList(Ids);
            db.Set<CompanyNotice>().Where(x => listIds.Contains(x.ID)).ToList().ForEach(x => x.Status = Status);
            db.Commit();
        }
    }
}