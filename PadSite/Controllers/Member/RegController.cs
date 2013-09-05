using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PadSite.ViewModels;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Service.Interface;
namespace PadSite.Controllers
{
    public class RegController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        public RegController(
            IMemberService MemberService,
            IEmailService EmailService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
        }

        public ActionResult Index()
        {
            var model = new RegViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegViewModel model)
        {
            if (ModelState.IsValid)
            {
                #region 注册用户并登录
                try
                {
                    Member mb = MemberService.Create(model);
                    MemberService.SetLoginCookie(mb);
                    return Redirect(Url.Action("regok"));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
            }
            else
            {
                return View(model);
            }
        }
    }
}
