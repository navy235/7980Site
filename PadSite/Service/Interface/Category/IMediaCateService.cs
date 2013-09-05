using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
namespace PadSite.Service.Interface
{
    public interface IMediaCateService
    {
        IQueryable<MediaCate> GetALL();

        IQueryable<MediaCate> GetKendoALL();

        void Create(MediaCate model);

        void Update(MediaCate model);

        void Delete(MediaCate model);

        MediaCate Find(int ID);
    }
}