using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class PeriodCateService : IPeriodCateService
    {
        private readonly IUnitOfWork db;

        public PeriodCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<PeriodCate> GetALL()
        {
            return db.Set<PeriodCate>();
        }

        public IQueryable<PeriodCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<PeriodCate>();
        }

        public void Create(PeriodCate model)
        {
            db.Add<PeriodCate>(model);
            db.Commit();
        }

        public void Update(PeriodCate model)
        {
            var target = Find(model.ID);
            db.Attach<PeriodCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(PeriodCate model)
        {
            var target = Find(model.ID);
            db.Remove<PeriodCate>(target);
            db.Commit();
        }

        public PeriodCate Find(int ID)
        {
            return db.Set<PeriodCate>().Single(x => x.ID == ID);
        }
    }
}