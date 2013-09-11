using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PadSite.Utils
{
    public class Utilities
    {
        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, bool addDefaultSelectItem = true)
        {
            var eList = entities
                   .Select(x => new SelectListItem
                   {
                       Value = funcToGetValue(x).ToString(),
                       Text = funcToGetText(x).ToString()
                   }).ToList();

            if (addDefaultSelectItem)
                eList.Insert(0, new SelectListItem { Selected = true, Text = "请选择", Value = "0" });

            return eList;
        }

        public static SelectList CreateSelectList<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, bool addDefaultSelectItem = true)
        {
            return new SelectList(GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem), "Value", "Text");

        }

        public static string GetInnerMostException(Exception ex)
        {
            return ex.GetBaseException().Message;
        }

        public static List<int> GetIdList(string Ids)
        {
            var list = new List<int>();
            if (!string.IsNullOrEmpty(Ids))
            {
                list = Ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            }
            return list;
        }

        public static int GetCascadingId(string Ids)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(Ids))
            {
                id = Convert.ToInt32(Ids.Split(',').Last());
            }
            return id;
        }
    }
}