using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUnitOfWork db;

        public PermissionsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Permissions> GetALL()
        {
            return db.Set<Permissions>();
        }

        public IQueryable<Permissions> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Permissions>();
        }

        public void Create(Permissions model)
        {
            db.Add<Permissions>(model);
            db.Commit();
        }

        public void Update(Permissions model)
        {
            var target = Find(model.ID);
            db.Attach<Permissions>(target);
            target.Name = model.Name;
            target.Action = model.Action;
            target.Namespace = model.Namespace;
            target.Controller = model.Controller;
            target.Description = model.Description;
            target.DepartmentID = model.DepartmentID;
            db.Commit();
        }

        public void Delete(Permissions model)
        {
            var target = Find(model.ID);
            db.Remove<Permissions>(target);
            db.Commit();
        }

        public Permissions Find(int ID)
        {
            return db.Set<Permissions>().Single(x => x.ID == ID);
        }
    }
}