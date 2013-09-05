using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PadSite.ViewModels;
using Maitonn.Core;
namespace PadSite.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        //public LoginController(IUnitOfWork _DB_Service
        //   , IMemberService _memberService)
        //{
        //    DB_Service = _DB_Service;
        //    memberService = _memberService;
        //}


        public ActionResult Index(string username = null)
        {
            var model = new LoginViewModel();
            if (!string.IsNullOrEmpty(username))
            {
                model.Email = username;
            }

            return View(model);
        }

    }
}
