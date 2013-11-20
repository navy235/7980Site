using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Maitonn.Core;
using PadSite.Models;
using PadSite.Service.Interface;
using PadSite.Utils;
using PadSite.Filters;
using PadSite.ViewModels;

namespace PadSite.Controllers
{
    public class AjaxServiceController : Controller
    {

        // GET: /Area/
        private ICityCateService CityCateService;
        private IArticleCateService ArticleCateService;
        private IMemberService MemberService;
        private IMediaCateService MediaCateService;
        private IFormatCateService FormatCateService;
        private IPeriodCateService PeriodCateService;
        private IOwnerCateService OwnerCateService;
        private IFavoriteService FavoriteService;
        private ISchemeItemService SchemeItemService;
        private ISchemeService SchemeService;
        private IOutDoorService OutDoorService;
        private IOutDoorLuceneService OutDoorLuceneService;
        private IMessageService MessageService;
        public AjaxServiceController(
             ICityCateService CityCateService,
             IArticleCateService ArticleCateService,
             IMemberService MemberService,
             IMediaCateService MediaCateService,
             IFormatCateService FormatCateService,
             IPeriodCateService PeriodCateService,
             IOwnerCateService OwnerCateService,
             IFavoriteService FavoriteService,
             ISchemeItemService SchemeItemService,
             ISchemeService SchemeService,
            IOutDoorService OutDoorService,
            IOutDoorLuceneService OutDoorLuceneService,
            IMessageService MessageService
          )
        {
            this.CityCateService = CityCateService;
            this.ArticleCateService = ArticleCateService;
            this.MemberService = MemberService;
            this.MediaCateService = MediaCateService;
            this.FormatCateService = FormatCateService;
            this.PeriodCateService = PeriodCateService;
            this.OwnerCateService = OwnerCateService;
            this.FavoriteService = FavoriteService;
            this.SchemeItemService = SchemeItemService;
            this.SchemeService = SchemeService;
            this.OutDoorService = OutDoorService;
            this.OutDoorLuceneService = OutDoorLuceneService;
            this.MessageService = MessageService;
        }

        public ActionResult isLogin()
        {
            return Json(
                new
                {
                    Login = CookieHelper.IsLogin,
                    NickName = CookieHelper.NickName
                }, JsonRequestBehavior.AllowGet
              );
        }

        #region control

        public ActionResult CityCode(int pid = 0)
        {
            var query = CityCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MediaCode(int pid = 0)
        {
            var query = MediaCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticleCode(int pid = 0)
        {
            var query = ArticleCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FormatCodeName(int key)
        {
            return Content(FormatCateService.Find(key).CateName);
        }

        public ActionResult PeriodCodeName(int key)
        {
            return Content(PeriodCateService.Find(key).CateName);
        }

        public ActionResult OwnerCodeName(int key)
        {
            return Content(OwnerCateService.Find(key).CateName);
        }

        #endregion


        #region Business
        [LoginAuthorize]
        public ActionResult CheckFavorite(int id)
        {
            ServiceResult result = new ServiceResult();
            var MemberID = CookieHelper.MemberID;
            try
            {
                if (FavoriteService.GetALL().Any(x => x.MemberID == MemberID && x.MediaID == id))
                {
                    result.AddServiceError("已经收藏了该媒体");
                    result.Message = "已经收藏了该媒体";
                }
            }
            catch (Exception ex)
            {
                result.Message = "媒体收藏失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "验证媒体收藏失败!", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize]
        public ActionResult AddFavorite(int id)
        {
            ServiceResult result = new ServiceResult();
            var MemberID = CookieHelper.MemberID;
            if (!FavoriteService.GetALL().Any(x => x.MemberID == MemberID && x.MediaID == id))
            {
                try
                {
                    Favorite entity = new Favorite()
                    {
                        MediaID = id,
                        MemberID = MemberID,
                        AddTime = DateTime.Now
                    };
                    FavoriteService.Create(entity);
                }
                catch (Exception ex)
                {
                    result.Message = "添加收藏失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加收藏失败!", ex);
                }
            }
            else
            {
                result.Message = "已经收藏了该媒体";
                result.AddServiceError("已经收藏了该媒体");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize]
        public ActionResult GetScheme()
        {
            var MemberID = CookieHelper.MemberID;
            var renderRadioList = SchemeService.GetALL().Where(x => x.MemberID == MemberID);
            return Json(Utilities.GetSelectListData(renderRadioList, x => x.ID, x => x.Name, false),
                JsonRequestBehavior.AllowGet);
        }


        [LoginAuthorize]
        [HttpPost]
        public ActionResult AddScheme(AddSchemeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                SchemeItem entity = new SchemeItem();
                entity.MediaID = model.id;
                entity.StartTime = Convert.ToDateTime(model.startTime);
                entity.EndTime = Convert.ToDateTime(model.endTime);
                entity.Price = Convert.ToDecimal(model.price);
                entity.PeriodCode = model.periodCode;
                entity.PeriodCount = model.periodCount;
                if (string.IsNullOrEmpty(model.Name) && model.schemeId != 0)
                {
                    entity.SchemeID = model.schemeId;
                    if (SchemeItemService.GetALL().Any(x => x.MediaID == model.id && x.SchemeID == model.schemeId))
                    {
                        result.AddServiceError("该方案已经包含了此媒体");
                        result.Message = "该方案已经包含了此媒体！";
                    }
                }
                else
                {
                    Scheme scheme = new Scheme()
                    {
                        AddTime = DateTime.Now,
                        Name = model.Name,
                        Description = model.Description,
                        LastTime = DateTime.Now,
                        MemberID = CookieHelper.MemberID
                    };
                    SchemeService.Create(scheme);
                    entity.SchemeID = scheme.ID;
                }
                if (result.Success)
                {
                    SchemeItemService.Create(entity);
                    result.Message = "加入方案成功！";
                }
            }
            catch (Exception ex)
            {
                result.Message = "加入方案失败！";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "加入方案失败!", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize]
        [HttpPost]
        public ActionResult EditSchemeMedia(EditSchemeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var startTime = Convert.ToDateTime(model.startTime);

                var entity = new SchemeItem()
                {
                    ID = model.id,
                    PeriodCode = model.periodCode,
                    PeriodCount = model.periodCount,
                    StartTime = startTime,
                    Price = Convert.ToDecimal(model.price),
                    EndTime = startTime.AddDays(model.periodCode * model.periodCount)
                };

                SchemeItemService.Update(entity);
            }
            catch (Exception ex)
            {
                result.Message = "加入方案失败！";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog(CookieHelper.MemberID + model.id + "加入方案失败", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMediaPeriodCode(int id)
        {
            var renderRadioList = PeriodCateService.GetALL().Where(x => x.ID >= 3);
            return Json(Utilities.GetSelectListData(renderRadioList,
                x => x.OrderIndex,
                x => x.CateName, false), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GenerateScheme(GenerateSchemeViewModel model)
        {
            var totalCount = 0;
            var list = OutDoorLuceneService.Search(model, out totalCount);
            var maxPrice = model.price;
            var currentPrice = 0m;
            var day = model.day;
            var result = new List<LinkItem>();
            foreach (var item in list)
            {
                currentPrice += ((item.Price / 365) * day);
                int privceValue = Convert.ToInt32(currentPrice);
                if (privceValue > maxPrice)
                {
                    currentPrice -= ((item.Price / 365) * day);
                    break;
                }
                else
                {
                    result.Add(item);
                }
            }
            ViewBag.currentPrice = currentPrice.ToString("F2");
            ViewBag.Dq = Convert.ToDateTime(model.dq).ToString("yyyy-MM-dd")
                + " 至 " + Convert.ToDateTime(model.dq).AddDays(model.day).ToString("yyyy-MM-dd");
            //return Json(result, JsonRequestBehavior.AllowGet);
            return View(result);
        }


        public ActionResult GetRealPrice(int id)
        {
            RealPriceViewModel model = new RealPriceViewModel();
            model.ID = id;
            if (CookieHelper.IsLogin)
            {
                model.IsLogin = true;
                model.Price = OutDoorService.Find(id).RealPrice;
            }
            return View(model);
        }


        [LoginAuthorize]
        [HttpPost]
        public ActionResult AddMessage(int id, string name, string content)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var message = new Message()
                {
                    AddTime = DateTime.Now,
                    SenderID = CookieHelper.MemberID,
                    RecipientID = id,
                    MessageType = (int)MessageType.Member,
                    RecipienterStatus = (int)MessageStatus.Show,
                    Title = name,
                    Content = content
                };
                MessageService.Create(message);
                result.Message = "留言成功！";
            }
            catch (Exception ex)
            {
                result.Message = "留言失败！";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog(CookieHelper.MemberID + "留言失败!", ex);
            }

            return Json(result);
        }


        #endregion

        #region Validate

        public void GetValidateCode()
        {
            ValidateCode VCode = new ValidateCode("VCode", 100, 40);
        }

        public JsonResult ValidateVCode(string vcode)
        {
            bool status = false;
            if (Session["VCode"] != null)
            {
                status = Session["VCode"].ToString().Equals(vcode, StringComparison.OrdinalIgnoreCase);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ValidateSmsVCode(string smsvcode)
        {
            bool status = false;
            if (smsvcode == "1241")
            {
                status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认是否存在该Email用户，注册用户时远程验证，存在返回false，不存在返回true
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult EmailExists(string email)
        {
            if (MemberService.ExistsEmail(email))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 确认是否存在该Email用户，注册找回密码时验证，存在返回true，不存在返回false
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult HasEmailUser(string email)
        {
            if (MemberService.ExistsEmail(email))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NickNameExists(string nickName)
        {
            if (!MemberService.ExistsNickName(nickName) && !BadWordsHelper.HasBadWord(nickName))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EmailExistsNotMe(string email)
        {

            if (!MemberService.ExistsEmailNotMe(CookieHelper.MemberID, email))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NickNameExistsNotMe(string nickName)
        {

            if (!MemberService.ExistsNickNameNotMe(CookieHelper.MemberID, nickName))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidateDescription(string Description)
        {
            if (!BadWordsHelper.HasBadWord(Description))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidatePassword(string OldPassword)
        {
            if (MemberService.ValidatePassword(CookieHelper.MemberID, OldPassword))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
