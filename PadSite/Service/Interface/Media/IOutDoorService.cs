using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using Maitonn.Core;
namespace PadSite.Service.Interface
{
    public interface IOutDoorService
    {
        IQueryable<OutDoor> GetALL();

        IQueryable<OutDoor> GetKendoALL();

        void Create(OutDoor model);

        OutDoor Create(OutDoorViewModel model);

        OutDoor Update(OutDoorViewModel model);

        void Update(OutDoor model);

        void Delete(OutDoor model);

        OutDoor Find(int ID);

        void ChangeStatus(string ids, OutDoorStatus Status, string reason = null);

        void ChangeSuggestStatus(string ids, int SuggestStatus);
    }
}