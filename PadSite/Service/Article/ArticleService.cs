using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using Maitonn.Core;
namespace PadSite.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork db;

        public ArticleService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Article> GetALL()
        {
            return db.Set<Article>();
        }

        public IQueryable<Article> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Article>();
        }

        public void Create(Article model)
        {
            db.Add<Article>(model);
            db.Commit();
        }

        public void Update(Article model)
        {
            var target = Find(model.ID);
            db.Attach<Article>(target);
            target.Name = model.Name;
            target.Content = model.Content;
            target.ArticleCode = model.ArticleCode;
            target.ArticleCodeValue = model.ArticleCodeValue;
            target.LastTime = model.LastTime;
            db.Commit();
        }

        public void Delete(Article model)
        {
            var target = Find(model.ID);
            db.Remove<Article>(target);
            db.Commit();
        }

        public Article Find(int ID)
        {
            return db.Set<Article>().Single(x => x.ID == ID);
        }
    }
}