using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork db;

        public FavoriteService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Favorite> GetALL()
        {
            return db.Set<Favorite>();
        }

        public IQueryable<Favorite> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Favorite>();
        }

        public void Create(Favorite model)
        {
            db.Add<Favorite>(model);
            db.Commit();
        }

        public void Update(Favorite model)
        {
            var target = Find(model.ID);
            db.Attach<Favorite>(target);
            target.Status = model.Status;
            db.Commit();
        }

        public void Delete(Favorite model)
        {
            var target = Find(model.ID);
            db.Remove<Favorite>(target);
            db.Commit();
        }

        public Favorite Find(int ID)
        {
            return db.Set<Favorite>().Single(x => x.ID == ID);
        }
    }
}