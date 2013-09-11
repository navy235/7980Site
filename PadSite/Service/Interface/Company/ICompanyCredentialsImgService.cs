using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ICompanyCredentialsImgService
    {
        IQueryable<CompanyCredentialsImg> GetALL();

        IQueryable<CompanyCredentialsImg> GetKendoALL();

        void Create(CompanyCredentialsImg model);

        void Update(CompanyCredentialsImg model);

        void Delete(CompanyCredentialsImg model);

        CompanyCredentialsImg Find(int ID);
    }
}