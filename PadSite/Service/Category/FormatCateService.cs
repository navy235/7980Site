using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class FormatCateService : IFormatCateService
    {
        private readonly IUnitOfWork db;

        public FormatCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<FormatCate> GetALL()
        {
            return db.Set<FormatCate>();
        }

        public IQueryable<FormatCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<FormatCate>();
        }

        public void Create(FormatCate model)
        {
            db.Add<FormatCate>(model);
            db.Commit();
        }

        public void Update(FormatCate model)
        {
            var target = Find(model.ID);
            db.Attach<FormatCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(FormatCate model)
        {
            var target = Find(model.ID);
            db.Remove<FormatCate>(target);
            db.Commit();
        }

        public FormatCate Find(int ID)
        {
            return db.Set<FormatCate>().Single(x => x.ID == ID);
        }
    }
}