using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class AreaCateService : IAreaCateService
    {
        private readonly IUnitOfWork db;

        public AreaCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<AreaCate> GetALL()
        {
            return db.Set<AreaCate>();
        }

        public IQueryable<AreaCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<AreaCate>();
        }

        public void Create(AreaCate model)
        {
            db.Add<AreaCate>(model);
            db.Commit();
        }

        public void Update(AreaCate model)
        {
            var target = Find(model.ID);
            db.Attach<AreaCate>(target);
            target.CateName = model.CateName;
            db.Commit();
        }

        public void Delete(AreaCate model)
        {
            var target = Find(model.ID);
            db.Remove<AreaCate>(target);
            db.Commit();
        }

        public AreaCate Find(int ID)
        {
            return db.Set<AreaCate>().Single(x => x.ID == ID);
        }
    }
}