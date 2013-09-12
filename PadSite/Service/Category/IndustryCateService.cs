using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class IndustryCateService : IIndustryCateService
    {
        private readonly IUnitOfWork db;

        public IndustryCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<IndustryCate> GetALL()
        {
            return db.Set<IndustryCate>();
        }

        public IQueryable<IndustryCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<IndustryCate>();
        }

        public void Create(IndustryCate model)
        {
            db.Add<IndustryCate>(model);
            db.Commit();
        }

        public void Update(IndustryCate model)
        {
            var target = Find(model.ID);
            db.Attach<IndustryCate>(target);
            target.CateName = model.CateName;
            db.Commit();
        }

        public void Delete(IndustryCate model)
        {
            var target = Find(model.ID);
            db.Remove<IndustryCate>(target);
            db.Commit();
        }

        public IndustryCate Find(int ID)
        {
            return db.Set<IndustryCate>().Single(x => x.ID == ID);
        }
    }
}