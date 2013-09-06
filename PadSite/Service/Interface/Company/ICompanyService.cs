using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
namespace PadSite.Service.Interface
{
    public interface ICompanyService
    {
        IQueryable<Company> GetALL();

        IQueryable<Company> GetKendoALL();

        void Create(Company model);

        Company Create(CompanyRegViewModel model);

        void Delete(Company model);

        Company Find(int MemberID);

        Company SaveBasInfo(int MemberID, CompanyRegViewModel model);

        void UpdateAuthInfo(int MemberID, BizAuthViewModel model);

    }
}