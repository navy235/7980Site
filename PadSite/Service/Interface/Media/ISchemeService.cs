using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface ISchemeService
    {
        IQueryable<Scheme> GetALL();

        IQueryable<Scheme> GetKendoALL();

        void Create(Scheme model);

        void Update(Scheme model);

        void Delete(Scheme model);

        Scheme Find(int ID);
    }
}