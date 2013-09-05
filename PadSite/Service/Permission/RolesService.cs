using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using System.Data.Entity;
using Maitonn.Core;
namespace PadSite.Service
{
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork db;
        private readonly IPermissionsService PermissionsService;
        public RolesService(IUnitOfWork db
            , IPermissionsService PermissionsService)
        {
            this.db = db;
            this.PermissionsService = PermissionsService;
        }

        public IQueryable<Roles> GetALL()
        {
            return db.Set<Roles>();
        }

        public IQueryable<Roles> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Roles>();
        }

        public void Create(Roles model)
        {
            db.Add<Roles>(model);
            db.Commit();
        }

        public void Update(Roles model)
        {
            var target = GetALL().Include(x => x.Permissions).Single(x => x.ID == model.ID);
            db.Attach<Roles>(target);
            target.Description = model.Description;
            target.Name = model.Name;
            var PermissionList = model.Permissions;
            var currentPermissionArray = target.Permissions.Select(x => x.ID).ToList();
            foreach (Permissions ps in PermissionsService.GetALL())
            {
                if (PermissionList.Count(x => x.ID == ps.ID) > 0)
                {
                    if (!currentPermissionArray.Contains(ps.ID))
                    {
                        target.Permissions.Add(ps);
                    }
                }
                else
                {
                    if (currentPermissionArray.Contains(ps.ID))
                    {
                        target.Permissions.Remove(ps);
                    }
                }
            }
            db.Commit();
        }

        public void Delete(Roles model)
        {
            var target = Find(model.ID);
            db.Remove<Roles>(target);
            db.Commit();
        }

        public Roles Find(int ID)
        {
            return db.Set<Roles>().Single(x => x.ID == ID);
        }


    }
}