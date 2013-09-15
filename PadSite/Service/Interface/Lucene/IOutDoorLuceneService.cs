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
        bool CreateIndex(string ids);

        bool ChangeStatus(string ids, OutDoorStatus Status);

        bool RefrshIndex(string ids);


    }
}