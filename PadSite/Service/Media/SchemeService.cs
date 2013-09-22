using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class SchemeService : ISchemeService
    {
        private readonly IUnitOfWork db;

        public SchemeService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Scheme> GetALL()
        {
            return db.Set<Scheme>();
        }

        public IQueryable<Scheme> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Scheme>();
        }

        public void Create(Scheme model)
        {
            db.Add<Scheme>(model);
            db.Commit();
        }

        public void Update(Scheme model)
        {
            var target = Find(model.ID);
            db.Attach<Scheme>(target);
            target.Name = model.Name;
            target.LastTime = DateTime.Now;
            target.Description = model.Description;
            db.Commit();
        }

        public void Delete(Scheme model)
        {
            var target = Find(model.ID);
            db.Remove<Scheme>(target);
            db.Commit();
        }

        public Scheme Find(int ID)
        {
            return db.Set<Scheme>().Single(x => x.ID == ID);
        }
    }
}