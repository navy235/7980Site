using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class MediaCateService : IMediaCateService
    {
        private readonly IUnitOfWork db;

        public MediaCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<MediaCate> GetALL()
        {
            return db.Set<MediaCate>();
        }

        public IQueryable<MediaCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<MediaCate>();
        }

        public void Create(MediaCate model)
        {
            db.Add<MediaCate>(model);
            db.Commit();
        }

        public void Update(MediaCate model)
        {
            var target = Find(model.ID);
            db.Attach<MediaCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(MediaCate model)
        {
            var target = Find(model.ID);
            db.Remove<MediaCate>(target);
            db.Commit();
        }

        public MediaCate Find(int ID)
        {
            return db.Set<MediaCate>().Single(x => x.ID == ID);
        }
    }
}