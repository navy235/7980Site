using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ICompanyMessageService
    {
        IQueryable<CompanyMessage> GetALL();

        IQueryable<CompanyMessage> GetKendoALL();

        void Create(CompanyMessage model);

        void Update(CompanyMessage model);

        void Delete(CompanyMessage model);

        CompanyMessage Find(int ID);
    }
}