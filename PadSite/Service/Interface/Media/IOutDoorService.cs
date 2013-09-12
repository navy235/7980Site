using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;

namespace PadSite.Service.Interface
{
    public interface IOutDoorService
    {
        IQueryable<OutDoor> GetALL();

        IQueryable<OutDoor> GetKendoALL();

        void Create(OutDoor model);

        void Update(OutDoor model);

        void Delete(OutDoor model);

        OutDoor Find(int ID);
    }
}