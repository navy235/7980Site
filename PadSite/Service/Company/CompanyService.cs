using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using PadSite.Service.Interface;
using PadSite.Utils;
using Maitonn.Core;
namespace PadSite.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork db;
        private readonly IMemberService MemberService;
        public CompanyService(IUnitOfWork db,
            IMemberService MemberService)
        {
            this.db = db;
            this.MemberService = MemberService;
        }

        public IQueryable<Company> GetALL()
        {
            return db.Set<Company>();
        }

        public IQueryable<Company> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Company>();
        }

        public void Create(Company model)
        {
            db.Add<Company>(model);
            db.Commit();
        }


        public void Delete(Company model)
        {
            var target = Find(model.ID);
            db.Remove<Company>(target);
            db.Commit();
        }

        public Company Find(int MemberID)
        {
            return db.Set<Company>().SingleOrDefault(x => x.MemberID == MemberID);
        }


        public Company Create(CompanyRegViewModel model)
        {
            var MemberID = CookieHelper.MemberID;
            Company company = new Company();
            company.AddIP = HttpHelper.IP;
            company.Address = model.Address;
            company.AddTime = DateTime.Now;
            company.CityCodeValue = model.CityCode;
            company.CityCode = Utilities.GetCascadingId(model.CityCode);
            company.Description = model.Description;
            company.Fax = model.Fax;
            company.LastIP = HttpHelper.IP;
            company.LastTime = DateTime.Now;
            company.Lat = Convert.ToSingle(model.Position.Split('|')[0]);
            company.Lng = Convert.ToSingle(model.Position.Split('|')[1]);
            company.LinkMan = model.LinkMan;
            company.MemberID = MemberID;
            company.Mobile = model.Mobile;
            company.MSN = model.MSN;
            company.Name = model.Name;
            company.Phone = model.Phone;
            company.QQ = model.QQ;
            company.Sex = model.Sex;
            company.Status = (int)CompanyStatus.CompanyApply;
            company.IdentityCard = model.IdentityCard;
            company.CredentialsImg = model.CredentialsImg;
            company.LinkManImg = model.LinkManImg;
            company.LogoImg = model.LogoImg;
            db.Add<Company>(company);
            db.Commit();
            return company;
        }


        public Company SaveBasInfo(int MemberID, CompanyRegViewModel model)
        {
            Company company = new Company();
            company.AddIP = HttpHelper.IP;
            company.Address = model.Address;
            company.AddTime = DateTime.Now;
            company.Description = model.Description;
            company.Fax = model.Fax;
            company.LastIP = HttpHelper.IP;
            company.LastTime = DateTime.Now;
            company.Lat = Convert.ToSingle(model.Position.Split('|')[0]);
            company.Lng = Convert.ToSingle(model.Position.Split('|')[1]);
            company.LinkMan = model.LinkMan;
            company.MemberID = MemberID;
            company.Mobile = model.Mobile;
            company.MSN = model.MSN;
            company.Name = model.Name;
            company.Phone = model.Phone;
            company.QQ = model.QQ;
            company.Sex = model.Sex;
            company.CityCodeValue = model.CityCode;
            company.CityCode = Utilities.GetCascadingId(model.CityCode);
            db.Add<Company>(company);
            db.Commit();
            return company;
        }


        public void UpdateAuthInfo(int MemberID, BizAuthViewModel model)
        {
            var company = Find(MemberID);
            db.Attach<Company>(company);
            company.LogoImg = model.LogoImg;
            company.LinkManImg = model.LinkManImg;
            company.CredentialsImg = model.CredentialsImg;
            company.IdentityCard = model.IdentityCard;
            company.Status = (int)CompanyStatus.CompanyApply;
            db.Commit();
        }


        public Company Update(CompanyRegViewModel model)
        {
            var MemberID = CookieHelper.MemberID;

            Company company = Find(MemberID);

            db.Attach<Company>(company);
            company.LastIP = HttpHelper.IP;
            company.Address = model.Address;
            company.LastTime = DateTime.Now;
            company.Description = model.Description;
            company.Fax = model.Fax;
            company.Lat = Convert.ToSingle(model.Position.Split('|')[0]);
            company.Lng = Convert.ToSingle(model.Position.Split('|')[1]);
            company.LinkMan = model.LinkMan;
            company.MemberID = MemberID;
            company.Mobile = model.Mobile;
            company.MSN = model.MSN;
            company.Name = model.Name;
            company.Phone = model.Phone;
            company.QQ = model.QQ;
            company.Sex = model.Sex;
            company.IdentityCard = model.IdentityCard;
            company.LinkManImg = model.LinkManImg;
            company.LogoImg = model.LogoImg;
            company.CredentialsImg = model.CredentialsImg;
            company.CityCodeValue = model.CityCode;
            company.CityCode = Utilities.GetCascadingId(model.CityCode);
            company.Status = (int)CompanyStatus.CompanyApply;
            db.Commit();

            return company;

        }


        public IQueryable<CompanyVerifyViewModel> GetVerifyList(CompanyStatus CompanyStatus)
        {
            int CompanyStatusValue = (int)CompanyStatus;
            return db.Set<Company>().Select(x => new CompanyVerifyViewModel()
            {
                MemberID = x.MemberID,
                Name = x.Name,
                Description = x.Description,
                LinkMan = x.LinkMan,
                Contact = (string.IsNullOrEmpty(x.Mobile) ? "" : "手机:" + x.Mobile) + (string.IsNullOrEmpty(x.Phone) ? "" : "电话:" + x.Phone),
                Status = x.Status,
                AddTime = x.AddTime,
                LastTime = x.LastTime

            }).Where(x => x.Status == CompanyStatusValue);
        }


        public void ChangeStatus(string CompangIds, CompanyStatus CompanyStatus)
        {
            var IdsArray = CompangIds.Split(',').Select(x => Convert.ToInt32(x));
            var CompanyStatusValue = (int)CompanyStatus;
            db.Set<Company>().Where(x => IdsArray.Contains(x.MemberID)).ToList().ForEach(x => x.Status = CompanyStatusValue);
            db.Commit();
            if (CompanyStatus == CompanyStatus.CompanyFailed)
            {
                MemberService.ChangeStatus(IdsArray, (int)MemberStatus.CompanyFailed);
            }
            else if (CompanyStatus == CompanyStatus.CompanyAuth)
            {
                MemberService.ChangeStatus(IdsArray, (int)MemberStatus.CompanyAuth);
            }
        }


        public void UpdateContactInfo(int MemberID, CompanyContactInfoViewModel model)
        {
            var company = Find(MemberID);
            db.Attach<Company>(company);
            company.LinkMan = model.LinkMan;
            company.Fax = model.Fax;
            company.Mobile = model.Mobile;
            company.MSN = model.MSN;
            company.Phone = model.Phone;
            company.QQ = model.QQ;
            company.Sex = model.Sex;
            db.Commit();
        }


        public Company SaveLogo(int MemberID, CompanyLogoViewModel model)
        {
            var company = Find(MemberID);
            db.Attach<Company>(company);
            company.LogoImg = model.LogoImg;
            db.Commit();
            return company;
        }

        public Company SaveBanner(int MemberID, CompanyBannerViewModel model)
        {
            var company = Find(MemberID);
            db.Attach<Company>(company);
            company.BannerImg = model.BannerImg;
            db.Commit();
            return company;
        }
    }
}