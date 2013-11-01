using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PadSite.ViewModels;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Filters;
using PadSite.Utils;
using PadSite.Service.Interface;
using PadSite.Setting;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PadSite.Service;
namespace PadSite.Controllers
{
    public class HomeController : Controller
    {

        private IOutDoorLuceneService OutDoorLuceneService;
        public HomeController(IOutDoorLuceneService OutDoorLuceneService)
        {
            this.OutDoorLuceneService = OutDoorLuceneService;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            #region sql


            var roots = new List<CateItem>();

            var outdoor = new CateItem()
            {
                CateName = "户外大牌"
            };




            //string city = "招商 渠道扩张 品牌建设 促销活动";
            //string province = "湖南";
            //string city = "长沙,株洲,湘潭,衡阳,邵阳,岳阳,常德,张家界,益阳,郴州,永州,怀化,娄底,湘西";
            //var xian = new string[]{
            //    "岳麓区、芙蓉区、天心区、开福区、雨花区、浏阳市、长沙县、望城县、宁乡县",
            //    "天元区、荷塘区、芦淞区、石峰区、醴陵市、株洲县、炎陵县、茶陵县、攸县",
            //    "岳塘区、雨湖区、湘乡市、韶山市、湘潭县",
            //    "雁峰区、珠晖区、石鼓区、蒸湘区、南岳区、耒阳市、常宁市、衡阳县、衡东县、衡山县、衡南县、祁东县",
            //    "双清区、大祥区、北塔区、武冈市、邵东县、洞口县、新邵县、绥宁县、新宁县、邵阳县、隆回县、城步苗族自治县",
            //    "岳阳楼区、云溪区、君山区、临湘市、汨罗市、岳阳县、湘阴县、平江县、华容县",
            //    "武陵区、鼎城区、津市市、澧县、临澧县、桃源县、汉寿县、安乡县、石门县",
            //    "永定区、武陵源区、慈利县、桑植县",
            //    "赫山区、资阳区、沅江市、桃江县、南县、安化县",
            //    "北湖区、苏仙区、资兴市、宜章县、汝城县、安仁县、嘉禾县、临武县、桂东县、永兴县、桂阳县",
            //    "冷水滩区、零陵区、祁阳县、蓝山县、宁远县、新田县、东安县、江永县、道县、双牌县、江华瑶族自治县",
            //    "鹤城区、洪江市、会同县、沅陵县、辰溪县、溆浦县、中方县、新晃侗族自治县、芷江侗族自治县、通道侗族自治县、靖州苗族侗族自治县、麻阳苗族自治县",
            //    "娄星区、冷水江市、涟源市、新化县、双峰县",
            //    "吉首市、古丈县、龙山县、永顺县、凤凰县、泸溪县、保靖县、花垣县"
            //};

            //StringBuilder printer = new StringBuilder();
            //printer.AppendLine("USE [pad_db]");
            //printer.AppendLine("SET IDENTITY_INSERT [dbo].[PurposeCate] ON ");
            //printer.AppendLine("GO");



            //var cityArr = city.Split(' ').ToList();

            //var id = 0;

            //for (var i = 0; i < cityArr.Count; i++)
            //{
            //    id++;
            //    //printer.AppendLine("INSERT [dbo].[OwnerCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (" + id + ", N'" + cityArr[i] + "', 1, 10" + GetString(i + 1) + ", 0, " + (id - 1) + ")");

            //    printer.AppendLine("INSERT [dbo].[PurposeCate] ([ID], [CateName]) VALUES (" + id + ", N'" + cityArr[i] + "'" + ")");
            //    printer.AppendLine("GO");
            //}

            ////var cityCount = cityArr.Count;

            ////for (var i = 0; i < cityArr.Count; i++)
            ////{
            ////    var xianArr = xian[i].Split('、').ToList();
            ////    for (var j = 0; j < xianArr.Count; j++)
            ////    {
            ////        id++;
            ////        printer.AppendLine("INSERT [dbo].[CityCate] ([ID], [CateName], [PID], [Code], [Level], [OrderIndex]) VALUES (" + id + ", N'" + xianArr[j] + "', " + (i + 2) + ", 10" + GetString(i + 1) + GetString(j + 1) + ", 2, " + (id - 1) + ")");
            ////        printer.AppendLine("GO");
            ////    }

            ////}

            //ViewBag.Str = printer.ToString();
            #endregion



            return View();
        }

        public ActionResult Source()
        {
            return View();
        }


        public ActionResult Test()
        {
            int totalHits = 0;
            var query = OutDoorLuceneService.Search(out totalHits);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public string GetString(int number)
        {
            if (number > 9)
            {
                return number.ToString();
            }
            else
            {
                return "0" + number.ToString();
            }
        }


        public class CateItem
        {
            public string CateName { get; set; }


            public List<CateItem> ChildItems { get; set; }

            public CateItem()
            {
                this.ChildItems = new List<CateItem>();
            }
        }
    }
}
