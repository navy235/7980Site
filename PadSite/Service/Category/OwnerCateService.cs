using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class OwnerCateService : IOwnerCateService
    {
        private readonly IUnitOfWork db;

        public OwnerCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<OwnerCate> GetALL()
        {
            return db.Set<OwnerCate>();
        }

        public IQueryable<OwnerCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<OwnerCate>();
        }

        public void Create(OwnerCate model)
        {
            db.Add<OwnerCate>(model);
            db.Commit();
        }

        public void Update(OwnerCate model)
        {
            var target = Find(model.ID);
            db.Attach<OwnerCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(OwnerCate model)
        {
            var target = Find(model.ID);
            db.Remove<OwnerCate>(target);
            db.Commit();
        }

        public OwnerCate Find(int ID)
        {
            return db.Set<OwnerCate>().Single(x => x.ID == ID);
        }
    }
}