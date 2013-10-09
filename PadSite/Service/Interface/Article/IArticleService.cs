using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IArticleService
    {
        IQueryable<Article> GetALL();

        IQueryable<Article> GetKendoALL();

        void Create(Article model);

        void Update(Article model);

        void Delete(Article model);

        Article Find(int ID);
    }
}