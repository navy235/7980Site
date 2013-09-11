using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ICompanyNoticeService
    {
        IQueryable<CompanyNotice> GetALL();

        IQueryable<CompanyNotice> GetKendoALL();

        void Create(CompanyNotice model);

        void Update(CompanyNotice model);

        void Delete(CompanyNotice model);

        CompanyNotice Find(int ID);

        void ChangeStatus(string Ids, int Status);
    }
}