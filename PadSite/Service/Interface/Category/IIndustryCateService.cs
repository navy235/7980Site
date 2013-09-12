using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IIndustryCateService
    {
        IQueryable<IndustryCate> GetALL();

        IQueryable<IndustryCate> GetKendoALL();

        void Create(IndustryCate model);

        void Update(IndustryCate model);

        void Delete(IndustryCate model);

        IndustryCate Find(int ID);
    }
}