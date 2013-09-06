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
namespace PadSite.Controllers
{
    public class RegController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        public RegController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService
            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
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
                             CredentialsImg=company.CredentialsImg,
                              IdentityCard=company.IdentityCard
               

                        };
                        
                        return View(model);
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult OpenBiz(OpenBizModel model)
        {
            if (ModelState.IsValid)
            {
                #region 企业入驻
                try
                {
                    Member member = memberService.Find(CookieHelper.MemberID);

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
                            var company = companyService.IncludeFind(member.MemberID);

                            if (company == null)
                            {
                                CompanyReg reg = new CompanyReg()
                                {
                                    Address = model.Address,
                                    BussinessCode = model.BussinessCode,
                                    CityCode = model.CityCode,
                                    Description = model.Description,
                                    FundCode = model.FundCode,
                                    LinkMan = model.LinkMan,
                                    Mobile = model.Mobile,
                                    Name = model.Name,
                                    Phone = model.Phone,
                                    Position = model.Position,
                                    ScaleCode = model.ScaleCode,
                                    Sex = model.Sex,
                                    CompanyImg = model.CompanyImg,
                                    LinManImg = model.LinManImg,
                                    Logo = model.Logo
                                };
                                companyService.Create(reg);
                            }
                            else
                            {
                                CompanyReg reg = new CompanyReg()
                                {
                                    Address = model.Address,
                                    BussinessCode = model.BussinessCode,
                                    CityCode = model.CityCode,
                                    Description = model.Description,
                                    FundCode = model.FundCode,
                                    LinkMan = model.LinkMan,
                                    Mobile = model.Mobile,
                                    Name = model.Name,
                                    Phone = model.Phone,
                                    Position = model.Position,
                                    ScaleCode = model.ScaleCode,
                                    Sex = model.Sex,
                                    CompanyImg = model.CompanyImg,
                                    LinManImg = model.LinManImg,
                                    Logo = model.Logo,
                                    Fax = company.Fax,
                                    MSN = company.MSN,
                                    QQ = company.QQ
                                };

                                companyService.Update(reg);
                            }
                        }
                    }

                    //memberService.SetLoginCookie(mb);
                    return Redirect(Url.Action("bizOk"));

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
