using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IFavoriteService
    {
        IQueryable<Favorite> GetALL();

        IQueryable<Favorite> GetKendoALL();

        void Create(Favorite model);

        void Update(Favorite model);

        void Delete(Favorite model);

        Favorite Find(int ID);
    }
}