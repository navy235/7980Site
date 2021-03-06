﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PadSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "defaultlist",
               url: "list-{city}-{mediacode}-{formatcode}-{ownercode}-{periodcode}-{authstatus}-{deadline}-{price}-{order}-{descending}-p_{page}",
               defaults: new
               {
                   controller = "List",
                   action = "Index",
                   province = 1,
                   city = 0,
                   mediacode = 0,
                   formatcode = 0,
                   ownercode = 0,
                   periodcode = 0,
                   authstatus = 0,
                   deadline = 0,
                   price = 0,
                   order = 0,
                   descending = 0,
                   page = 1
               },
               constraints: new
               {
                   province = @"\d+",
                   city = @"\d+",
                   mediacode = @"\d+",
                   formatcode = @"\d+",
                   ownercode = @"\d+",
                   periodcode = @"\d+",
                   authstatus = @"\d+",
                   deadline = @"\d+",
                   price = @"\d+",
                   order = @"\d+",
                   descending = @"\d+",
                   page = @"\d+"
               }
            );
            routes.MapRoute(
              name: "meida",
              url: "media-{id}",
              defaults: new { controller = "company", action = "show", id = UrlParameter.Optional }
          );

            routes.MapRoute(
           name: "company",
           url: "company-{id}",
           defaults: new { controller = "company", action = "index", id = UrlParameter.Optional }
       );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );


        }
    }
}