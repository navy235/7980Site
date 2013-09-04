using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class ArticleCateService : IArticleCateService
    {
        private readonly IUnitOfWork db;

        public ArticleCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<ArticleCate> GetALL()
        {
            return db.Set<ArticleCate>();
        }

        public IQueryable<ArticleCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<ArticleCate>();
        }

        public void Create(ArticleCate model)
        {
            db.Add<ArticleCate>(model);
            db.Commit();
        }

        public void Update(ArticleCate model)
        {
            var target = Find(model.ID);
            db.Attach<ArticleCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(ArticleCate model)
        {
            var target = Find(model.ID);
            db.Remove<ArticleCate>(target);
            db.Commit();
        }

        public ArticleCate Find(int ID)
        {
            return db.Set<ArticleCate>().Single(x => x.ID == ID);
        }
    }
}