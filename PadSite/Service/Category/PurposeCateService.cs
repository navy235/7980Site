using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class PurposeCateService : IPurposeCateService
    {
        private readonly IUnitOfWork db;

        public PurposeCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<PurposeCate> GetALL()
        {
            return db.Set<PurposeCate>();
        }

        public IQueryable<PurposeCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<PurposeCate>();
        }

        public void Create(PurposeCate model)
        {
            db.Add<PurposeCate>(model);
            db.Commit();
        }

        public void Update(PurposeCate model)
        {
            var target = Find(model.ID);
            db.Attach<PurposeCate>(target);
            target.CateName = model.CateName;
            db.Commit();
        }

        public void Delete(PurposeCate model)
        {
            var target = Find(model.ID);
            db.Remove<PurposeCate>(target);
            db.Commit();
        }

        public PurposeCate Find(int ID)
        {
            return db.Set<PurposeCate>().Single(x => x.ID == ID);
        }
    }
}