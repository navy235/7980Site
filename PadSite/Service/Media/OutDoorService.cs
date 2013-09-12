using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class OutDoorService : IOutDoorService
    {
        private readonly IUnitOfWork db;

        public OutDoorService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<OutDoor> GetALL()
        {
            return db.Set<OutDoor>();
        }

        public IQueryable<OutDoor> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<OutDoor>();
        }

        public void Create(OutDoor model)
        {
            db.Add<OutDoor>(model);
            db.Commit();
        }

        public void Update(OutDoor model)
        {
            var target = Find(model.ID);
            db.Attach<OutDoor>(target);
            target.Name = model.Name;
            db.Commit();
        }

        public void Delete(OutDoor model)
        {
            var target = Find(model.ID);
            db.Remove<OutDoor>(target);
            db.Commit();
        }

        public OutDoor Find(int ID)
        {
            return db.Set<OutDoor>().Single(x => x.ID == ID);
        }
    }
}