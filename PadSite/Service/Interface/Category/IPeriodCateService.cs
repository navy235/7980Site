using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IPeriodCateService
    {
        IQueryable<PeriodCate> GetALL();

        IQueryable<PeriodCate> GetKendoALL();

        void Create(PeriodCate model);

        void Update(PeriodCate model);

        void Delete(PeriodCate model);

        PeriodCate Find(int ID);
    }
}