using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IFormatCateService
    {
        IQueryable<FormatCate> GetALL();

        IQueryable<FormatCate> GetKendoALL();

        void Create(FormatCate model);

        void Update(FormatCate model);

        void Delete(FormatCate model);

        FormatCate Find(int ID);
    }
}