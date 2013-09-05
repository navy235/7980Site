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
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork db;
        private readonly IRolesService RolesService;
        public GroupService(IUnitOfWork db
            , IRolesService RolesService)
        {
            this.db = db;
            this.RolesService = RolesService;
        }

        public IQueryable<Group> GetALL()
        {
            return db.Set<Group>();
        }

        public IQueryable<Group> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Group>();
        }

        public void Create(Group model)
        {
            db.Add<Group>(model);
            db.Commit();
        }

        public void Update(Group model)
        {
            var target = GetALL().Include(x => x.Roles).Single(x => x.ID == model.ID);
            db.Attach<Group>(target);
            target.Description = model.Description;
            target.Name = model.Name;
            var RoleList = model.Roles;
            var currentPermissionArray = target.Roles.Select(x => x.ID).ToList();
            foreach (Roles ps in RolesService.GetALL())
            {
                if (RoleList.Count(x => x.ID == ps.ID) > 0)
                {
                    if (!currentPermissionArray.Contains(ps.ID))
                    {
                        target.Roles.Add(ps);
                    }
                }
                else
                {
                    if (currentPermissionArray.Contains(ps.ID))
                    {
                        target.Roles.Remove(ps);
                    }
                }
            }
            db.Commit();
        }

        public void Delete(Group model)
        {
            var target = Find(model.ID);
            db.Remove<Group>(target);
            db.Commit();
        }

        public Group Find(int ID)
        {
            return db.Set<Group>().Single(x => x.ID == ID);
        }


    }
}