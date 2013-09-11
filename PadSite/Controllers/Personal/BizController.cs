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


namespace PadSite.Controllers
{
    [LoginAuthorize]
    public class BizController : Controller
    {
        private IMemberService MemberService;
        private IEmailService EmailService;
        private ICompanyService CompanyService;
        private IMember_ActionService Member_ActionService;
        private ICityCateService CityCateService;
        private ICompanyCredentialsImgService CompanyCredentialsImgService;
        private ICompanyNoticeService CompanyNoticeService;
        private ICompanyMessageService CompanyMessageService;
        public BizController(
            IMemberService MemberService,
            IEmailService EmailService,
            ICompanyService CompanyService,
            IMember_ActionService Member_ActionService,
            ICityCateService CityCateService,
            ICompanyCredentialsImgService CompanyCredentialsImgService,
            ICompanyNoticeService CompanyNoticeService,
            ICompanyMessageService CompanyMessageService

            )
        {
            this.MemberService = MemberService;
            this.EmailService = EmailService;
            this.CompanyService = CompanyService;
            this.Member_ActionService = Member_ActionService;
            this.CityCateService = CityCateService;
            this.CompanyCredentialsImgService = CompanyCredentialsImgService;
            this.CompanyNoticeService = CompanyNoticeService;
            this.CompanyMessageService = CompanyMessageService;
        }

        public ActionResult Index()
        {
            ViewBag.MenuItem = "company-baseinfo";
            var member = MemberService.Find(CookieHelper.MemberID);
            var company = CompanyService.Find(CookieHelper.MemberID);
            if (company == null)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            else
            {
                var model = new CompanyBaseInfoViewModel()
                {
                    CityCode = company.CityCodeValue,
                    Description = company.Description,
                    Name = company.Name,
                    IdentityCard = company.IdentityCard,
                    CredentialsImg = company.CredentialsImg,
                    LinkManImg = company.LinkManImg,
                    LogoImg = company.LogoImg
                };
                var cityIds = Utilities.GetIdList(company.CityCodeValue);
                var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                ViewBag.Data_CityCode = cityValues;
                if (member.Status < (int)MemberStatus.CompanyAuth)
                {
                    ViewBag.Authed = false;
                }
                else
                {
                    ViewBag.Authed = true;
                }
                return View(model);
            }
        }

        public ActionResult Contact()
        {
            ViewBag.MenuItem = "company-contact";
            var company = CompanyService.Find(CookieHelper.MemberID);
            var model = new CompanyContactInfoViewModel()
            {
                Fax = company.Fax,
                LinkMan = company.LinkMan,
                Mobile = company.Mobile,
                MSN = company.MSN,
                Phone = company.Phone,
                QQ = company.QQ,
                Sex = company.Sex
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(CompanyContactInfoViewModel model)
        {
            ViewBag.MenuItem = "company-contact";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyService.UpdateContactInfo(CookieHelper.MemberID, model);
                    result.Message = "联系信息保存成功！";
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    result.Message = "联系信息保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "联系信息保存失败!", ex);
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



        public ActionResult Logo()
        {
            ViewBag.MenuItem = "shop-logo";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            else
            {
                var logImg = CompanyService.Find(CookieHelper.MemberID).LogoImg;
                return View(new CompanyLogoViewModel()
                {
                    LogoImg = logImg,
                    MemberID = CookieHelper.MemberID
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logo(CompanyLogoViewModel model)
        {
            ViewBag.MenuItem = "shop-logo";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyService.SaveLogo(CookieHelper.MemberID, model);
                    result.Message = "企业标志保存成功！";
                    return RedirectToAction("Logo");
                }
                catch (Exception ex)
                {
                    result.Message = "企业标志保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "企业标志保存失败!", ex);
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

        public ActionResult Banner()
        {
            ViewBag.MenuItem = "shop-banner";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            else
            {
                var bannerImg = CompanyService.Find(CookieHelper.MemberID).BannerImg;
                return View(new CompanyBannerViewModel()
                {
                    BannerImg = bannerImg,
                    MemberID = CookieHelper.MemberID
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Banner(CompanyBannerViewModel model)
        {
            ViewBag.MenuItem = "shop-banner";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    CompanyService.SaveBanner(CookieHelper.MemberID, model);
                    result.Message = "企业横幅保存成功！";
                    return RedirectToAction("Banner");
                }
                catch (Exception ex)
                {
                    result.Message = "企业横幅保存失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "企业横幅保存失败!", ex);
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



        #region 企业证书
        public ActionResult Credentials()
        {
            ViewBag.MenuItem = "company-credentials";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            else
            {
                var model = CompanyCredentialsImgService.GetALL().Select(x => new CompanyCredentialsViewModel()
                {
                    ID = x.ID,
                    Name = x.Title,
                    ImgUrl = x.ImgUrl
                });
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult DeleteCredentials(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var model = CompanyCredentialsImgService.Find(ID);
                CompanyCredentialsImgService.Delete(model);
                result.Message = "删除证书成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除证书失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除证书失败!", ex);
            }
            return Json(result);
        }

        public ActionResult AddCredentials()
        {
            ViewBag.MenuItem = "company-credentials";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View(new CompanyCredentialsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCredentials(CompanyCredentialsViewModel model)
        {
            ViewBag.MenuItem = "company-credentials";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new CompanyCredentialsImg()
                    {
                        ImgUrl = model.ImgUrl,
                        Title = model.Name,
                        MemberID = CookieHelper.MemberID
                    };

                    CompanyCredentialsImgService.Create(entity);
                    result.Message = "添加证书成功！";

                    return RedirectToAction("Credentials");
                }
                catch (Exception ex)
                {
                    result.Message = "添加证书失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加证书失败!", ex);
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

        public ActionResult EditCredentials(int ID)
        {
            ViewBag.MenuItem = "company-credentials";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            var credentials = CompanyCredentialsImgService.Find(ID);
            if (credentials == null)
            {
                return Content("<script>alert('非法操作！');window.history.go(-1);</script>");
            }
            var model = new CompanyCredentialsViewModel()
            {
                ID = credentials.ID,
                Name = credentials.Title,
                ImgUrl = credentials.ImgUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCredentials(CompanyCredentialsViewModel model)
        {
            ViewBag.MenuItem = "company-credentials";
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new CompanyCredentialsImg()
                    {
                        ImgUrl = model.ImgUrl,
                        Title = model.Name,
                        ID = model.ID
                    };
                    CompanyCredentialsImgService.Update(entity);
                    result.Message = "编辑证书成功！";

                    return RedirectToAction("Credentials");
                }
                catch (Exception ex)
                {
                    result.Message = "编辑证书失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑证书失败!", ex);
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

        #endregion


        #region 企业公告

        public ActionResult Notice()
        {
            ViewBag.MenuItem = "shop-notice";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            ViewBag.CompanyNoticeStatus = UIHelper.CompanyNoticeStatusList;
            return View();
        }

        public ActionResult AddNotice()
        {
            ViewBag.MenuItem = "shop-notice";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View(new CompanyNoticeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotice(CompanyNoticeViewModel model)
        {
            ViewBag.MenuItem = "shop-notice";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new CompanyNotice()
                    {
                        MemberID = CookieHelper.MemberID,
                        Title = model.Name,
                        Content = model.Content,
                        AddTime = DateTime.Now,
                        Status = (int)CompanyNoticeStatus.ShowOnLine
                    };
                    CompanyNoticeService.Create(entity);
                    result.Message = "添加企业公告成功！";

                    return RedirectToAction("Notice");

                }
                catch (Exception ex)
                {
                    result.Message = "添加企业公告失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加企业公告失败!", ex);
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

        public ActionResult EditNotice(int id)
        {
            ViewBag.MenuItem = "shop-notice";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            var notice = CompanyNoticeService.Find(id);
            if (notice == null)
            {
                return Content("<script>alert('非法操作！');window.history.go(-1);</script>");
            }
            var model = new CompanyNoticeViewModel()
            {
                ID = notice.ID,
                Name = notice.Title,
                Content = notice.Content
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNotice(CompanyNoticeViewModel model)
        {
            ViewBag.MenuItem = "shop-notice";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new CompanyNotice()
                    {
                        MemberID = CookieHelper.MemberID,
                        Title = model.Name,
                        Content = model.Content,
                        ID = model.ID
                    };
                    CompanyNoticeService.Update(entity);
                    result.Message = "编辑企业公告成功！";
                    return RedirectToAction("Notice");
                }
                catch (Exception ex)
                {
                    result.Message = "编辑企业公告成功!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑企业公告成功!", ex);
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

        public ActionResult Notice_Read([DataSourceRequest] DataSourceRequest request)
        {
            var statusValue = (int)CompanyNoticeStatus.Delete;
            var model = CompanyNoticeService.GetKendoALL().Where(x => x.MemberID == CookieHelper.MemberID && x.Status >= statusValue).Select(x => new CompanyNoticeListViewModel()
            {
                AddTime = x.AddTime,
                Content = x.Content,
                Name = x.Title,
                ID = x.ID,
                Status = x.Status
            });
            return Json(model.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult NoticeNotShow(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var status = (int)CompanyNoticeStatus.NotShow;
                CompanyNoticeService.ChangeStatus(ids, status);
                result.Message = "设置未显示成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置未显示失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置未显示失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult NoticeShow(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var status = (int)CompanyNoticeStatus.ShowOnLine;
                CompanyNoticeService.ChangeStatus(ids, status);
                result.Message = "设置显示成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置显示失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置显示失败!", ex);
            }
            return Json(result);

        }

        [HttpPost]
        public ActionResult NoticeDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListIds = Utilities.GetIdList(ids);
                foreach (var id in ListIds)
                {
                    var model = CompanyNoticeService.Find(id);
                    CompanyNoticeService.Delete(model);
                }
                result.Message = "删除公告成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除公告失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除公告失败!", ex);
            }
            return Json(result);
        }

        #endregion


        #region 企业留言

        public ActionResult Message()
        {
            ViewBag.MenuItem = "shop-message";
            var member = MemberService.Find(CookieHelper.MemberID);
            if (member.Status < (int)MemberStatus.CompanyAuth)
            {
                return Redirect(Url.Action("openbiz", "reg"));
            }
            return View();
        }

        public ActionResult Message_Read([DataSourceRequest] DataSourceRequest request)
        {
            var statusValue = (int)CompanyMessageStatus.NotShow;
            var model = CompanyMessageService.GetKendoALL()
                .Where(x => x.MemberID == CookieHelper.MemberID
                    && x.Status >= statusValue)
                    .Select(x => new CompanyMessageViewModel()
                    {
                        AddTime = x.AddTime,
                        Content = x.Content,
                        ID = x.ID,
                        Name = x.Title,
                        Status = x.Status
                    });
            return Json(model.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult MessageDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListIds = Utilities.GetIdList(ids);
                foreach (var id in ListIds)
                {
                    var model = CompanyMessageService.Find(id);
                    CompanyMessageService.Delete(model);
                }
                result.Message = "删除留言成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除留言失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除留言失败!", ex);
            }
            return Json(result);
        }

        public ActionResult MessageDetails(int ID)
        {
            var Details = CompanyMessageService.Find(ID);
            if (Details == null)
            {
                return HttpNotFound();
            }
            var member = MemberService.GetALL().Single(x => x.MemberID == Details.SenderID);
            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }
            return View(new CompanyMessageDetailsViewModel()
            {
                ID = Details.ID,
                AddTime = Details.AddTime,
                Content = Details.Content,
                MSN = member.Member_Profile.MSN,
                Name = Details.Title,
                NickName = member.NickName,
                Phone = member.Member_Profile.Phone,
                QQ = member.Member_Profile.QQ
            });
        }

        #endregion

    }
}
