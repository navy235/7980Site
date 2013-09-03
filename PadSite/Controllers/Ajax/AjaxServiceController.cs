﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.Utils;

namespace PadSite.Controllers
{
    public class AjaxServiceController : Controller
    {

        // GET: /Area/
        private ICityCateService cityService;
        public AjaxServiceController(
             ICityCateService _cityService
          )
        {
            cityService = _cityService;
        }

        public ActionResult CityCode(int pid = 0)
        {
            var query = cityService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_CityCode(int value)
        {
            var selectValues = new List<int>();
            selectValues.Add(value);
            selectValues.Add(4);
            selectValues.Reverse();
            return Json(selectValues, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CategoryCode(int pid = 0)
        {
            var selects = GetSelectList();

            var list = selects.Where(x => x.PID == pid).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public List<ListCategory> GetSelectList()
        {
            List<ListCategory> selects = new List<ListCategory>();

            for (var i = 1; i <= 10; i++)
            {
                ListCategory list = new ListCategory();

                list.ID = i;
                list.Level = 1;
                list.Name = "Category" + i;
                list.PID = 0;
                for (var j = 1; j <= 10; j++)
                {
                    ListCategory clist = new ListCategory();
                    clist.ID = 10 * i + j;
                    clist.Name = "Category" + (10 * i + j);
                    clist.Level = 2;
                    clist.PID = list.ID;
                    for (var k = 1; k <= 10; k++)
                    {
                        ListCategory cclist = new ListCategory();
                        cclist.ID = 100 * i + 10 * j + k;
                        cclist.Name = "Category" + (100 * i + 10 * j + k);
                        cclist.Level = 3;
                        cclist.PID = clist.ID;

                        for (var m = 1; m < 10; m++)
                        {
                            ListCategory ccclist = new ListCategory();
                            ccclist.ID = 1000 * i + 100 * j + 10 * k + m;
                            ccclist.Name = "Category" + (1000 * i + 100 * j + 10 * k + m);
                            ccclist.Level = 4;
                            ccclist.PID = cclist.ID;
                            selects.Add(ccclist);
                        }

                        selects.Add(cclist);
                    }

                    selects.Add(clist);
                }
                selects.Add(list);
            }

            return selects;
        }

        public class ListCategory
        {
            public ListCategory()
            {
                this.ChildCategoies = new List<ListCategory>();
            }

            public int ID { get; set; }

            public string Name { get; set; }

            public int Level { get; set; }

            public int PID { get; set; }

            public ListCategory PCategory { get; set; }

            public List<ListCategory> ChildCategoies { get; set; }
        }

    }
}
