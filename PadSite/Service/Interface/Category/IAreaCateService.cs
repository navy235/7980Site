using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IAreaCateService
    {
        IQueryable<AreaCate> GetALL();

        IQueryable<AreaCate> GetKendoALL();

        void Create(AreaCate model);

        void Update(AreaCate model);

        void Delete(AreaCate model);

        AreaCate Find(int ID);
    }
}