using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class CrowdCateService : ICrowdCateService
    {
        private readonly IUnitOfWork db;

        public CrowdCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CrowdCate> GetALL()
        {
            return db.Set<CrowdCate>();
        }

        public IQueryable<CrowdCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CrowdCate>();
        }

        public void Create(CrowdCate model)
        {
            db.Add<CrowdCate>(model);
            db.Commit();
        }

        public void Update(CrowdCate model)
        {
            var target = Find(model.ID);
            db.Attach<CrowdCate>(target);
            target.CateName = model.CateName;
            db.Commit();
        }

        public void Delete(CrowdCate model)
        {
            var target = Find(model.ID);
            db.Remove<CrowdCate>(target);
            db.Commit();
        }

        public CrowdCate Find(int ID)
        {
            return db.Set<CrowdCate>().Single(x => x.ID == ID);
        }
    }
}