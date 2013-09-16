using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using Maitonn.Core;
namespace PadSite.Service.Interface
{
    public interface IOutDoorLuceneService
    {
        void CreateIndex(string ids);

        void ChangeStatus(string ids, OutDoorStatus Status);

    }
}