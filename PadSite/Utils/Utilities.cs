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
                eList.Insert(0, new SelectListItem { Selected = true, Text = "请选择", Value = "" });

            return eList;
        }

        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, List<int> SeletdValues, bool addDefaultSelectItem = true)
        {
            var list = GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem);

            foreach (var item in list)
            {
                if (SeletdValues.Contains(Convert.ToInt32(item.Value)))
                {
                    item.Selected = true;
                }
            }
            return list;
        }

        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, int value, bool addDefaultSelectItem = true)
        {
            var list = GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem);

            foreach (var item in list)
            {
                if (item.Value != "")
                {

                    if (value == Convert.ToInt32(item.Value))
                    {
                        item.Selected = true;
                    }
                }
            }
            return list;
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

        public static int GetMaxCode(int code, int level)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            codeStr = codeStr.Substring(0, (level + 1) * 2);
            var needLength = maxLength - codeStr.Length;
            if (needLength > 0)
            {
                for (var i = 0; i < needLength; i++)
                {
                    codeStr += "9";
                }
            }
            return Convert.ToInt32(codeStr);
        }

        public static int GetMaxCode(int code)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            var lastNotEquelZeroIndex = 0;
            for (var i = maxLength - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(codeStr.Substring(i, 1)) != 0)
                {
                    lastNotEquelZeroIndex = i;
                    break;
                }
            }
            if (lastNotEquelZeroIndex % 2 == 0)
            {
                lastNotEquelZeroIndex += 1;
            }
            if (lastNotEquelZeroIndex < maxLength - 1)
            {
                codeStr = codeStr.Substring(0, lastNotEquelZeroIndex + 1);
                var needLength = maxLength - lastNotEquelZeroIndex - 1;
                for (var i = 0; i < needLength; i++)
                {
                    codeStr += "9";
                }
            }
            return Convert.ToInt32(codeStr);
        }
    }
}