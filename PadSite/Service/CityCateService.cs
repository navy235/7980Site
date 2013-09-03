using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class CityCateService : ICityCateService
    {
        private readonly IUnitOfWork db;

        public CityCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CityCate> GetALL()
        {
            return db.Set<CityCate>();
        }

        public IQueryable<CityCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CityCate>();
        }

        public void Create(CityCate model)
        {
            db.Add<CityCate>(model);
            db.Commit();
        }

        public void Update(CityCate model)
        {
            var target = Find(model.ID);
            db.Attach<CityCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(CityCate model)
        {
            var target = Find(model.ID);
            db.Remove<CityCate>(target);
            db.Commit();
        }

        public CityCate Find(int ID)
        {
            return db.Set<CityCate>().Single(x => x.ID == ID);
        }
    }
}