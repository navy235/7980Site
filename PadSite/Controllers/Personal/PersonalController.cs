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

namespace PadSite.Controllers
{
    [LoginAuthorize]
    public class PersonalController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        public PersonalController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaseInfo()
        {
            ViewBag.MenuItem = "baseinfo";

            Member member = MemberService.GetALL().Single(x => x.MemberID == CookieHelper.MemberID);

            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }
            ProfileViewModel pm = new ProfileViewModel()
            {
                MemberID = member.MemberID,
                Borthday = member.Member_Profile.Borthday,
                Description = member.Member_Profile.Description,
                NickName = member.NickName,
                RealName = member.Member_Profile.RealName,
                CityCode = member.Member_Profile.CityCodeValue,
                Sex = member.Member_Profile.Sex
            };
            return View(pm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BaseInfo(ProfileViewModel model)
        {
            ViewBag.MenuItem = "baseinfo";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.SaveMemberProfile(CookieHelper.MemberID, model);
                    result.Message = "基本信息保存成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "基本信息保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + model.MemberID + "基本信息保存失败!", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            return View(model);
        }



        public ActionResult Avtar()
        {
            ViewBag.MenuItem = "avtar";

            Member member = MemberService.Find(CookieHelper.MemberID);
            AvtarViewModel pm = new AvtarViewModel()
            {
                MemberID = member.MemberID,
                AvtarUrl = member.AvtarUrl ?? Url.Content(ConfigSetting.Default_AvtarUrl)
            };
            return View(pm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Avtar(AvtarViewModel model)
        {
            ViewBag.MenuItem = "avtar";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.SaveMemberAvtar(CookieHelper.MemberID, model);
                    result.Message = "头像保存成功！";

                }
                catch (Exception ex)
                {
                    result.Message = "头像保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + model.MemberID + "头像保存失败!", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            return View(model);
        }


        public ActionResult Contact()
        {
            ViewBag.MenuItem = "contact";

            Member member = MemberService.GetALL().Single(x => x.MemberID == CookieHelper.MemberID);

            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }

            ContactViewModel cm = new ContactViewModel()
            {
                MemberID = member.MemberID,
                Address = member.Member_Profile.Address,
                Email = member.Email,
                Mobile = member.Member_Profile.Mobile,
                Phone = member.Member_Profile.Phone,
                MSN = member.Member_Profile.MSN,
                Position = member.Member_Profile.Lat + "|" + member.Member_Profile.Lng,
                QQ = member.Member_Profile.QQ

            };

            return View(cm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel model)
        {
            ViewBag.MenuItem = "contact";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.SaveMemberContact(CookieHelper.MemberID, model);
                    result.Message = "联系信息保存成功！";

                }
                catch (Exception ex)
                {
                    result.Message = "联系信息保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + model.MemberID + "联系信息保存失败!", ex);
                    return View(model);
                }
            }
            else
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            return View(model);
        }


        public ActionResult BindEmail()
        {
            ViewBag.MenuItem = "email";

            Member member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status <= (int)MemberStatus.Registered)
            {
                int actionEmailActive = (int)MemberActionType.EmailActvie;
                int limitMins = ConfigSetting.GetBindEmailTimeDiffMin;
                if (Member_ActionService.HasActionByActionTypeInLimiteTime(CookieHelper.MemberID, actionEmailActive, limitMins))
                {
                    ViewBag.HasSendEmail = true;
                }
                else
                {
                    ViewBag.HasSendEmail = false;
                }
                ViewBag.Actived = false;
            }
            else
            {
                ViewBag.Actived = true;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BindEmail(string email = null)
        {
            ViewBag.MenuItem = "email";
            Member member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status <= (int)MemberStatus.Registered)
            {
                int actionEmailActive = (int)MemberActionType.EmailActvie;
                int limitMins = ConfigSetting.GetBindEmailTimeDiffMin;
                ViewBag.Actived = false;
                if (Member_ActionService.HasActionByActionTypeInLimiteTime(CookieHelper.MemberID, actionEmailActive, limitMins))
                {
                    ViewBag.HasSendEmail = true;
                }
                else
                {
                    string emailKey = Guid.NewGuid().ToString();
                    string emailTitle = member.NickName + string.Format(" 您好！绑定{0}登录邮箱!", ConfigSetting.SiteName);
                    EmailModel em = EmailService.GetMail(Server.MapPath("~/EmailTemplate/active.htm"), emailTitle, member.MemberID, member.Email,
                        member.NickName,
                        emailKey);
                    EmailService.SendMail(em);
                    Member_ActionService.Create(member, actionEmailActive, emailKey);
                    ViewBag.BeforeSend = true;
                    ViewBag.HasSendEmail = true;
                }
            }
            else
            {
                ViewBag.Actived = true;
            }
            return View();
        }


        public ActionResult ChangePwd()
        {
            ViewBag.MenuItem = "changepwd";
            Member member = MemberService.Find(CookieHelper.MemberID);
            return View(new ChangePasswordViewModel()
            {
                MemberID = member.MemberID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePwd(ChangePasswordViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            ViewBag.MenuItem = "changepwd";
            if (ModelState.IsValid)
            {
                try
                {
                    var memberID = Convert.ToInt32(CookieHelper.UID);
                    if (!MemberService.ChangePassword(memberID, model.OldPassword, model.NewPassword))
                    {
                        result.Message = "旧密码错误!";
                        result.AddServiceError("旧密码错误");
                        return View(model);
                    }
                    result.Message = "密码修改成功!";
                    return RedirectToAction("changepwd");
                }
                catch (Exception ex)
                {
                    result.Message = "密码修改失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + model.MemberID + "密码修改失败!", ex);
                }
            }
            return View(model);
        }



    }
}
