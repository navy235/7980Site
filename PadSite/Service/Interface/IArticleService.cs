using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IArticleCateService
    {
        IQueryable<ArticleCate> GetALL();

        IQueryable<ArticleCate> GetKendoALL();

        void Create(ArticleCate model);

        void Update(ArticleCate model);

        void Delete(ArticleCate model);

        ArticleCate Find(int ID);
    }
}