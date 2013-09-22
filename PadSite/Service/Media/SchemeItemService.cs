using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class SchemeItemService : ISchemeItemService
    {
        private readonly IUnitOfWork db;

        public SchemeItemService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<SchemeItem> GetALL()
        {
            return db.Set<SchemeItem>();
        }

        public IQueryable<SchemeItem> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<SchemeItem>();
        }

        public void Create(SchemeItem model)
        {
            db.Add<SchemeItem>(model);
            db.Commit();
        }

        public void Update(SchemeItem model)
        {
            var target = Find(model.ID);
            db.Attach<SchemeItem>(target);
            target.PeriodCode = model.PeriodCode;
            target.StartTime = model.StartTime;
            target.EndTime = model.EndTime;
            target.PeriodCount = model.PeriodCount;
            target.Price = model.Price;
            db.Commit();
        }

        public void Delete(SchemeItem model)
        {
            var target = Find(model.ID);
            db.Remove<SchemeItem>(target);
            db.Commit();
        }

        public SchemeItem Find(int ID)
        {
            return db.Set<SchemeItem>().Single(x => x.ID == ID);
        }
    }
}