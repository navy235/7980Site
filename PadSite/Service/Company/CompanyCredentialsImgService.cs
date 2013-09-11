using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class CompanyCredentialsImgService : ICompanyCredentialsImgService
    {
        private readonly IUnitOfWork db;

        public CompanyCredentialsImgService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CompanyCredentialsImg> GetALL()
        {
            return db.Set<CompanyCredentialsImg>();
        }

        public IQueryable<CompanyCredentialsImg> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CompanyCredentialsImg>();
        }

        public void Create(CompanyCredentialsImg model)
        {
            db.Add<CompanyCredentialsImg>(model);
            db.Commit();
        }

        public void Update(CompanyCredentialsImg model)
        {
            var target = Find(model.ID);
            db.Attach<CompanyCredentialsImg>(target);
            target.ImgUrl = model.ImgUrl;
            target.Title = model.Title;
            db.Commit();
        }

        public void Delete(CompanyCredentialsImg model)
        {
            var target = Find(model.ID);
            db.Remove<CompanyCredentialsImg>(target);
            db.Commit();
        }

        public CompanyCredentialsImg Find(int ID)
        {
            return db.Set<CompanyCredentialsImg>().Single(x => x.ID == ID);
        }
    }
}