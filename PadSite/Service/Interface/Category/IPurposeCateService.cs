using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IPurposeCateService
    {
        IQueryable<PurposeCate> GetALL();

        IQueryable<PurposeCate> GetKendoALL();

        void Create(PurposeCate model);

        void Update(PurposeCate model);

        void Delete(PurposeCate model);

        PurposeCate Find(int ID);
    }
}