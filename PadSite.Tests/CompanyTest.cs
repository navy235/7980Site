using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PadSite.Models;
using PadSite.Service;
using PadSite.ViewModels;
using PadSite.Utils;
using PadSite.Setting;
using Maitonn.Core;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
namespace PadSite.Tests
{
    [TestClass]
    public class ComapnyTest
    {

        private static readonly IUnitOfWork db = new EntitiesContext();

        [TestMethod]
        public void GetCompanyCategories()
        {
            int id = 1;
            List<CompanyCategoryViewModel> result = new List<CompanyCategoryViewModel>();
            result = db.Set<OutDoor>()
                .Where(x => x.MemberID == id
                    && x.Status >= (int)OutDoorStatus.ShowOnline)
                    .GroupBy(x => x.MediaCodeValue)
                    .Select(x => new CompanyCategoryViewModel()
                    {
                        Count = x.Count(),
                        Code = x.Key
                    }).ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item.Count);
                Console.WriteLine(item.Code);
            }
        }
        [TestMethod]
        public void GetMaxCode()
        {
            var codes = new int[] { 10030000, 10030100, 10030101, 10030104, 13000000 };

            foreach (var c in codes)
            {
                var code = Utilities.GetMaxCode(c);
                Console.WriteLine(code);
            }
        }


    }
}
