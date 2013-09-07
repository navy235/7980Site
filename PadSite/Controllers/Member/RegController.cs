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
    public class RegController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        public RegController(
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


        #region 普通注册
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
                try
                {
                    Member mb = MemberService.Create(model);
                    MemberService.SetLoginCookie(mb);
                    return Redirect(Url.Action("regok"));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + model.Email + "注册失败!", ex);
                    TempData["FormError"] = true;
                    return View(model);
                }
            }
            else
            {
                TempData["FormError"] = true;
                return View(model);
            }
        }



        public ActionResult RegAuto()
        {
            if (null == Session["registerAuto"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                OpenLoginViewModel OpenUser = (OpenLoginViewModel)Session["registerAuto"];
                Session["registerAuto"] = null;
                return View(new RegViewModel()
                {
                    OpenType = OpenUser.OpenType,
                    OpenID = OpenUser.OpenId,
                    NickName = OpenUser.NickName
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegAuto(RegViewModel model)
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
                    LogHelper.WriteLog("用户:" + model.Email + "注册失败!", ex);
                    TempData["FormError"] = true;
                    return View(model);
                }
                #endregion
            }
            else
            {
                TempData["FormError"] = true;
                return View(model);
            }
        }


        [LoginAuthorize]
        public ActionResult RegOk()
        {
            var memberID = CookieHelper.MemberID;

            Member member = MemberService.GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == memberID);

            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }
            else
            {
                return Redirect(Url.Action("baseinfo", "personal"));
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
        public ActionResult RegOk(ProfileViewModel model)
        {
            var memberID = CookieHelper.MemberID;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.SaveMemberProfile(memberID, model);
                    return Redirect(Url.Action("activeemail"));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + memberID + "填写详细信息失败!", ex);
                    TempData["FormError"] = true;
                    return View(model);
                }
            }
            else
            {
                TempData["FormError"] = true;
                return View(model);
            }
        }

        #endregion


        #region 企业用户注册

        public ActionResult RegBiz()
        {
            return View(new RegBizViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegBiz(RegBizViewModel model)
        {
            if (ModelState.IsValid)
            {
                #region 注册用户并登录
                try
                {
                    RegViewModel rm = new RegViewModel()
                    {
                        Email = model.Email,
                        NickName = model.NickName,
                        Password = model.Password
                    };
                    Member mb = MemberService.Create(rm);
                    MemberService.SetLoginCookie(mb);
                    ProfileViewModel pm = new ProfileViewModel()
                    {
                        CityCode = model.CityCode,
                        Borthday = DateTime.Now,
                        NickName = mb.NickName,
                        RealName = model.LinkMan,
                        Sex = model.Sex
                    };
                    MemberService.SaveMemberProfile(mb.MemberID, pm);
                    ContactViewModel cm = new ContactViewModel()
                    {
                        Address = model.Address,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Phone = model.Phone,
                        Position = model.Position
                    };
                    MemberService.SaveMemberContact(mb.MemberID, cm);
                    CompanyRegViewModel cr = new CompanyRegViewModel()
                    {
                        Address = model.Address,
                        CityCode = model.CityCode,
                        Description = model.Description,
                        LinkMan = model.LinkMan,
                        Mobile = model.Mobile,
                        Name = model.Name,
                        Phone = model.Phone,
                        Position = model.Position,
                        Sex = model.Sex
                    };
                    CompanyService.SaveBasInfo(mb.MemberID, cr);
                    return Redirect(Url.Action("regauth"));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + model.Email + "企业注册失败!", ex);
                    TempData["FormError"] = true;
                    return View(model);
                }
                #endregion
            }
            else
            {
                TempData["FormError"] = true;
                return View(model);
            }
        }


        public ActionResult RegAuth()
        {
            Member member = MemberService.Find(CookieHelper.MemberID);

            if (member.Status >= (int)MemberStatus.CompanyAuth)
            {
                return Content("<script>alert('您的企业已经认证通过了!');window.top.location='" + Url.Action("index", "personal") + "';</script>");
            }
            else
            {
                if (member.Status >= (int)MemberStatus.EmailActived)
                {
                    return Redirect(Url.Action("OpenBiz"));
                }

                Company cpy = CompanyService.Find(CookieHelper.MemberID);

                if (cpy == null)
                {
                    return Content("<script>alert('请先填写企业基本信息然后再填写认证信息!');window.top.location='" + Url.Action("openbiz") + "';</script>");
                }
                else
                {
                    if (cpy.Status > (int)CompanyStatus.Default)
                    {
                        return View(new BizAuthViewModel()
                        {
                            MemberID = CookieHelper.MemberID,
                            CredentialsImg = cpy.CredentialsImg,
                            LinkManImg = cpy.LinkManImg,
                            LogoImg = cpy.LogoImg

                        });
                    }
                    else
                    {
                        return View(new BizAuthViewModel());
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegAuth(BizAuthViewModel model)
        {
            Member member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status >= (int)MemberStatus.CompanyAuth)
            {
                return Content("<script>alert('您的企业已经认证通过了!');window.top.location='" + Url.Action("index", "personal") + "';</script>");
            }
            else
            {
                if (member.Status >= (int)MemberStatus.EmailActived)
                {
                    return Redirect(Url.Action("OpenBiz"));
                }
                Company cpy = CompanyService.Find(CookieHelper.MemberID);
                if (cpy == null)
                {
                    return Content("<script>alert('请先填写企业基本信息然后再填写认证信息!');window.top.location='" + Url.Action("openbiz") + "';</script>");
                }
                else
                {
                    ServiceResult result = new ServiceResult();

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            CompanyService.UpdateAuthInfo(CookieHelper.MemberID, model);
                            return Redirect(Url.Action("BizActiveEmail"));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog("用户:" + member.MemberID + "填写认证信息失败!", ex);
                            TempData["FormError"] = true;
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["FormError"] = true;
                        return View(model);
                    }
                }
            }

        }


        #endregion


        #region 开通企业商铺

        [LoginAuthorize]
        public ActionResult OpenBiz()
        {
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.EmailActived)
            {
                return Content("<script>alert('您的邮箱还未绑定，请先绑定邮箱再进行企业认证!');window.top.location='" + Url.Action("activeemail") + "';</script>");
            }
            else
            {
                if (member.Status >= (int)MemberStatus.CompanyAuth)
                {
                    return Content("<script>alert('您的企业已经认证通过了!');window.top.location='" + Url.Action("index", "personal") + "';</script>");
                }
                else
                {
                    var company = CompanyService.Find(member.MemberID);

                    if (company == null)
                    {
                        return View(new OpenBizViewModel());
                    }
                    else
                    {

                        var model = new OpenBizViewModel()
                        {
                            Address = company.Address,
                            CityCode = company.CityCodeValue,
                            Description = company.Description,
                            LinkMan = company.LinkMan,
                            Mobile = company.Mobile,
                            Name = company.Name,
                            Phone = company.Phone,
                            Position = company.Lat + "|" + company.Lng,
                            Sex = company.Sex,
                            CredentialsImg = company.CredentialsImg,
                            IdentityCard = company.IdentityCard


                        };

                        return View(model);
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult OpenBiz(OpenBizViewModel model)
        {

            Member member = MemberService.Find(CookieHelper.MemberID);
            if (ModelState.IsValid)
            {

                try
                {
                    if (member.Status < (int)MemberStatus.EmailActived)
                    {
                        return Content("<script>alert('您的邮箱还未绑定，请先绑定邮箱再进行企业认证!');window.top.location='" + Url.Action("activeemail") + "';</script>");
                    }
                    else
                    {
                        if (member.Status >= (int)MemberStatus.CompanyAuth)
                        {
                            return Content("<script>alert('您的企业已经认证通过了!');window.top.location='" + Url.Action("index", "personal") + "';</script>");
                        }
                        else
                        {
                            var company = CompanyService.Find(member.MemberID);

                            if (company == null)
                            {
                                CompanyRegViewModel reg = new CompanyRegViewModel()
                                {
                                    Address = model.Address,
                                    CityCode = model.CityCode,
                                    Description = model.Description,
                                    LinkMan = model.LinkMan,
                                    Mobile = model.Mobile,
                                    Name = model.Name,
                                    Phone = model.Phone,
                                    Position = model.Position,
                                    Sex = model.Sex,
                                    LinkManImg = model.LinkManImg,
                                    CredentialsImg = model.CredentialsImg,
                                    LogoImg = model.LogoImg,
                                    IdentityCard = model.IdentityCard
                                };
                                CompanyService.Create(reg);
                            }
                            else
                            {
                                CompanyRegViewModel reg = new CompanyRegViewModel()
                                {
                                    Address = model.Address,
                                    CityCode = model.CityCode,
                                    Description = model.Description,
                                    LinkMan = model.LinkMan,
                                    Mobile = model.Mobile,
                                    Name = model.Name,
                                    Phone = model.Phone,
                                    Position = model.Position,
                                    Sex = model.Sex,
                                    LinkManImg = model.LinkManImg,
                                    CredentialsImg = model.CredentialsImg,
                                    LogoImg = model.LogoImg,
                                    IdentityCard = model.IdentityCard,
                                    Fax = company.Fax,
                                    MSN = company.MSN,
                                    QQ = company.QQ
                                };
                                CompanyService.Update(reg);
                            }
                        }
                    }
                    return Redirect(Url.Action("bizOk"));

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + member.MemberID + "开通企业注册失败!", ex);
                    TempData["FormError"] = true;
                    return View(model);
                }
            }
            else
            {
                TempData["FormError"] = true;
                return View(model);
            }
        }


        [LoginAuthorize]
        public ActionResult BizOk()
        {
            Member member = MemberService.Find(CookieHelper.MemberID);

            if (member.Status < (int)MemberStatus.EmailActived)
            {
                return Content("<script>alert('您的邮箱还未绑定，请先绑定邮箱再进行企业认证!');window.top.location='" + Url.Action("activeemail") + "';</script>");
            }
            else
            {
                if (member.Status >= (int)MemberStatus.CompanyAuth)
                {
                    return Content("<script>alert('您的企业已经认证通过了!');window.top.location='" + Url.Action("index", "personal") + "';</script>");
                }
                else
                {
                    var company = CompanyService.Find(member.MemberID);
                    if (company == null)
                    {
                        return Redirect(Url.Action("OpenBiz"));
                    }
                    else
                    {
                        return View();
                    }
                }

            }
        }
        #endregion


        #region 邮件激活


        [LoginAuthorize]
        public ActionResult ActiveOk()
        {
            Member member = MemberService.Find(CookieHelper.MemberID);

            if (member.Status < (int)MemberStatus.EmailActived)
            {
                return Content("<script>alert('您的邮箱还未绑定，请先绑定邮箱再进行企业认证!');window.top.location='" + Url.Action("activeemail") + "';</script>");
            }

            return View();
        }


        [LoginAuthorize]
        public ActionResult BizActiveEmail()
        {

            Member member = MemberService.Find(CookieHelper.MemberID);

            ViewBag.Email = member.Email;

            if (member.Status <= (int)MemberStatus.Registered)
            {
                int actionEmailActive = (int)MemberActionType.EmailActvie;

                int limitMins = ConfigSetting.GetBindEmailTimeDiffMin;

                if (!Member_ActionService.HasActionByActionTypeInLimiteTime(member.MemberID, actionEmailActive, limitMins))
                {
                    string emailKey = Guid.NewGuid().ToString();
                    string emailTitle = member.NickName + string.Format(" 您好！绑定{0}登录邮箱!", ConfigSetting.SiteName);
                    EmailModel em = EmailService.GetMail(Server.MapPath("~/EmailTemplate/bizactive.htm"), emailTitle, member.MemberID, member.Email,
                        member.NickName,
                        emailKey);
                    EmailService.SendMail(em);
                    Member_ActionService.Create(member, actionEmailActive, emailKey);
                }
            }
            else
            {
                return Content("<script>alert('您的邮箱已经绑定，请勿重复绑定!');window.top.location='" + Url.Action("activeok") + "';</script>");
            }

            return View();
        }





        [LoginAuthorize]
        public ActionResult ActiveEmail()
        {

            Member member = MemberService.Find(CookieHelper.MemberID);
            ViewBag.Email = member.Email;
            if (member.Status <= (int)MemberStatus.Registered)
            {
                int actionEmailActive = (int)MemberActionType.EmailActvie;
                int limitMins = ConfigSetting.GetBindEmailTimeDiffMin;
                if (!Member_ActionService.HasActionByActionTypeInLimiteTime(CookieHelper.MemberID, actionEmailActive, limitMins))
                {
                    string emailKey = Guid.NewGuid().ToString();
                    string emailTitle = member.NickName + string.Format(" 您好！绑定{0}登录邮箱!", ConfigSetting.SiteName);
                    EmailModel em = EmailService.GetMail(Server.MapPath("~/EmailTemplate/active.htm"), emailTitle, member.MemberID, member.Email,
                        member.NickName,
                        emailKey);
                    EmailService.SendMail(em);

                    Member_ActionService.Create(member, actionEmailActive, emailKey);
                }
            }
            else
            {
                return Content("<script>alert('您的邮箱已经绑定，请勿重复绑定!');window.top.location='" + Url.Action("index", "home") + "';</script>");
            }
            return View();
        }


        public ActionResult EmailActive(string email, string emailKey)
        {
            if (string.IsNullOrEmpty(emailKey) || !MemberService.ExistsEmail(email))
            {
                return Content("<script>alert('非法提交!');window.top.location='/" + Url.Action("index", "home") + "';</script>");
            }
            else
            {
                int limitHours = ConfigSetting.ActiveEmailTimeDiffHour;
                int emailActived = (int)MemberStatus.EmailActived;
                Member member = MemberService.FindDescriptionMemberInLimitTime(emailKey, limitHours);
                if (member != null)
                {
                    if (member.Status >= emailActived)
                    {
                        return Content("<script>alert('您的邮箱已经绑定，请勿重复绑定!');window.top.location='" + Url.Action("index", "home") + "';</script>");
                    }
                    else
                    {
                        if (member.Status < (int)MemberStatus.Registered)
                        {
                            return Content("<script>alert('您的帐号由于非法操作已经被锁定!');window.top.location='" + Url.Action("index", "home") + "';</script>");
                        }
                        else
                        {
                            var company = CompanyService.Find(member.MemberID);

                            if (company != null && company.Status > (int)CompanyStatus.Default)
                            {
                                emailActived = (int)MemberStatus.CompanyApply;
                            }

                            MemberService.changeStatus(member, emailActived);

                            return Redirect(Url.Action("ActiveOk"));
                        }
                    }
                }
                else
                {
                    return Content("<script>alert('您的验证已过期或非法提交，请重新获取绑定邮件!');window.location='" + Url.Action("activeemail") + "';</script>");
                }

            }
        }

        [LoginAuthorize]
        public ActionResult GetActiveEmail()
        {

            Member member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status <= (int)MemberStatus.Registered)
            {
                int actionEmailActive = (int)MemberActionType.EmailActvie;
                int limitMins = ConfigSetting.GetBindEmailTimeDiffMin;
                if (Member_ActionService.HasActionByActionTypeInLimiteTime(CookieHelper.MemberID, actionEmailActive, limitMins))
                {
                    return Content("<script>alert('您所使用的邮箱刚获取过绑定邮件，请到您的邮箱收取邮件!');window.top.location='" + Url.Action("activeemail") + "';</script>");
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
                    return Content("<script>alert('绑定邮件已经发送到您的邮箱，请在" + ConfigSetting.ActiveEmailTimeDiffHour + "小时内进行绑定');window.top.location='" + Url.Action("bindemail", "personal") + "';</script>");
                }
            }
            else
            {
                return Content("<script>alert('您的邮箱已经绑定，请勿重复绑定!');window.top.location='" + Url.Action("bindemail", "personal") + "';</script>");
            }
        }

        #endregion


        #region 重置密码

        public ActionResult GetPassword()
        {
            ViewBag.SendMail = false;
            return View(new GetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPassword(GetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                int memberAction = (int)MemberActionType.GetPassword;
                int limitMin = ConfigSetting.GetPasswordEmailTimeDiffMin;
                if (MemberService.HasGetPasswordActionInLimitTime(model.Email, limitMin, memberAction))
                {
                    ViewBag.SendMail = true;
                    ViewBag.HasSendMail = true;
                    ViewBag.Message = limitMin;
                }
                else
                {
                    Member member = MemberService.GetALL().Single(x => x.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase));
                    string userKey = Guid.NewGuid().ToString();
                    string emailTitle = member.NickName + string.Format(" 您好！找回{0}密码!", ConfigSetting.SiteName);
                    EmailModel em = EmailService.GetMail(Server.MapPath("~/EmailTemplate/getpwd.htm"), emailTitle, member.MemberID, member.Email, member.NickName, userKey);
                    EmailService.SendMail(em);
                    Member_ActionService.Create(member, memberAction, userKey);
                    ViewBag.HasSendMail = false;
                    ViewBag.SendMail = true;
                    ViewBag.Title = "";
                }
                return View(model);

            }
            return View(model);
        }


        public ActionResult ResetPassword(string userKey)
        {
            if (string.IsNullOrEmpty(userKey))
            {
                return Content("<script>alert('非法提交!');window.top.location='/" + Url.Action("GetPassword") + "';</script>");
            }
            else
            {
                int limitHours = ConfigSetting.ResetPasswordTimeDiffHour;
                if (Member_ActionService.HasDescriptionActionInLimiteTime(userKey, limitHours))
                {
                    ViewBag.haveChangePwd = false;
                    return View(new ResetPasswordViewModel());
                }
                else
                {
                    return Content("<script>alert('您的验证已过期或非法提交，请重新获取密码!');window.location='/" + Url.Action("GetPassword") + "';</script>");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string userKey, ResetPasswordViewModel model)
        {

            if (string.IsNullOrEmpty(userKey))
            {
                return Content("<script>alert('非法提交!');window.top.location='/" + Url.Action("GetPassword") + "';</script>");
            }
            else
            {
                Member member = MemberService.FindDescriptionMemberInLimitTime(userKey, 12);
                if (member != null)
                {
                    if (model.NewPassword.Equals(model.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                    {
                        MemberService.ResetPassword(member, model.NewPassword);
                    }
                    ViewBag.haveChangePwd = true;
                    ViewBag.Email = member.Email;
                    return View(model);
                }
                else
                {
                    return Content("<script>alert('您的验证已过期或非法提交，请重新获取密码!');window.location='/" + Url.Action("GetPassword") + "';</script>");
                }
            }
        }

        #endregion
    }
}
