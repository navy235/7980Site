using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork db;

        public DepartmentService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Department> GetALL()
        {
            return db.Set<Department>();
        }

        public IQueryable<Department> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Department>();
        }

        public void Create(Department model)
        {
            db.Add<Department>(model);
            db.Commit();
        }

        public void Update(Department model)
        {
            var target = Find(model.ID);
            db.Attach<Department>(target);
            target.Description = model.Description;
            target.Leader = model.Leader;
            target.Name = model.Name;
            db.Commit();
        }

        public void Delete(Department model)
        {
            var target = Find(model.ID);
            db.Remove<Department>(target);
            db.Commit();
        }

        public Department Find(int ID)
        {
            return db.Set<Department>().Single(x => x.ID == ID);
        }
    }
}