using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ISchemeItemService
    {
        IQueryable<SchemeItem> GetALL();

        IQueryable<SchemeItem> GetKendoALL();

        void Create(SchemeItem model);

        void Update(SchemeItem model);

        void Delete(SchemeItem model);

        SchemeItem Find(int ID);
    }
}