using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ICrowdCateService
    {
        IQueryable<CrowdCate> GetALL();

        IQueryable<CrowdCate> GetKendoALL();

        void Create(CrowdCate model);

        void Update(CrowdCate model);

        void Delete(CrowdCate model);

        CrowdCate Find(int ID);
    }
}