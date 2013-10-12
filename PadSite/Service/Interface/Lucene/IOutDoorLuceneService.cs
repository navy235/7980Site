using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using Maitonn.Core;
namespace PadSite.Service.Interface
{
    public interface IOutDoorLuceneService
    {
        void CreateIndex(string ids);

        void UpdateIndex(string ids);

        List<LinkItem> Search(QueryTerm queryTerm, SearchFilter searchFilter, out int totalHits);

        List<LinkItem> Search(GenerateSchemeViewModel model, out int totalHits);

        List<LinkItem> Search(out int totalHits);

        List<LinkItem> Search(IEnumerable<int> idArr);

        LinkItem Search(int MediaID);


    }
}