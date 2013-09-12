using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IOwnerCateService
    {
        IQueryable<OwnerCate> GetALL();

        IQueryable<OwnerCate> GetKendoALL();

        void Create(OwnerCate model);

        void Update(OwnerCate model);

        void Delete(OwnerCate model);

        OwnerCate Find(int ID);
    }
}