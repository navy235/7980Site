using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Maitonn.Core;
using PadSite.Models;

namespace PadSite.Filters
{
    public class PermissionAuthorizeAttribute : LoginAuthorizeAttribute
    {
        private IUnitOfWork db;

        public PermissionAuthorizeAttribute()
            : base()
        {
            db = new EntitiesContext();

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool hasPermission = false;
            base.AuthorizeCore(httpContext);
            if (CookieHelper.IsLogin)
            {
                int groupID = Convert.ToInt32(CookieHelper.GroupID);
                string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
                string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
                var query = db.Set<Group>()
                    .Include(x => x.Roles)
                    .Where(g =>
                        (g.Roles.Any(r =>
                            r.Permissions.Count(p =>
                                p.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                                &&
                                (p.Action.Equals(action, StringComparison.OrdinalIgnoreCase) || p.Action.Equals("controller", StringComparison.OrdinalIgnoreCase))) > 0))
                        && g.ID == groupID);

                hasPermission = query.Any();
            }
            return hasPermission;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(context.RequestContext);
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NoPermission",
                        LogOnUrl = urlHelper.Action("index", "login")
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                context.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary 
                                   {
                                       { "action", "index" },

                                       { "controller", "error" },

                                       { "id", (int)ErrorType.NoPermission},

                                       {"returnurl",context.RequestContext.HttpContext.Request.Url}
                                   });
            }

        }
    }
}
