using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using Maitonn.Core;
namespace PadSite.Service.Interface
{
    public interface ICompanyService
    {
        IQueryable<Company> GetALL();

        IQueryable<Company> GetKendoALL();

        void Create(Company model);

        Company Create(CompanyRegViewModel model);

        Company Update(CompanyRegViewModel model);

        void Delete(Company model);

        Company Find(int MemberID);

        Company SaveBasInfo(int MemberID, CompanyRegViewModel model);

        Company SaveLogo(int MemberID, CompanyLogoViewModel model);

        Company SaveBanner(int MemberID, CompanyBannerViewModel model);

        void UpdateAuthInfo(int MemberID, BizAuthViewModel model);

        void UpdateContactInfo(int MemberID, CompanyContactInfoViewModel model);

        IQueryable<CompanyVerifyViewModel> GetVerifyList(CompanyStatus CompanyStatus);

        void ChangeStatus(string CompangIds, CompanyStatus CompanyStatus);

    }
}