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
    public class MessageController : Controller
    {
        private IMemberService MemberService;
        private IMember_ActionService Member_ActionService;
        private IMessageService MessageService;

        public MessageController(
            IMemberService MemberService
            , IMember_ActionService Member_ActionService
            , IMessageService MessageService

            )
        {
            this.MemberService = MemberService;
            this.Member_ActionService = Member_ActionService;
            this.MessageService = MessageService;
        }

        public ActionResult Index()
        {
            ViewBag.MenuItem = "message-list";
            ViewBag.MessageType = UIHelper.MessageTypeList;
            CookieHelper.ClearCookieMessage();
            return View();
        }

        public ActionResult RecipienterMessage_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = MessageService.GetALL()
                .Where(x => x.RecipientID == CookieHelper.MemberID
                    && x.RecipienterStatus >= (int)MessageStatus.Show);
            return Json(model.ToDataSourceResult(request));
        }


        public ActionResult Send()
        {
            ViewBag.MenuItem = "message-send";
            ViewBag.MessageType = UIHelper.MessageTypeList;
            return View();
        }

        public ActionResult SenderMessage_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = MessageService.GetALL().Where(x => x.SenderID == CookieHelper.MemberID && x.RecipienterStatus >= (int)MessageStatus.Show);
            return Json(model.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult RecipienterMessageDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListIds = Utilities.GetIdList(ids);
                foreach (var id in ListIds)
                {
                    MessageService.DeleteRecipienterMessage(id);
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

        [HttpPost]
        public ActionResult SenderMessageDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListIds = Utilities.GetIdList(ids);
                foreach (var id in ListIds)
                {
                    MessageService.DeleteSenderMessage(id);
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

        public ActionResult SysDetails(int ID)
        {
            var Details = MessageService.Find(ID);

            if (Details == null)
            {
                return HttpNotFound();
            }
            MessageService.ReadMessage(ID);
            return View(new SystemMessageViewModel()
            {
                ID = Details.ID,
                AddTime = Details.AddTime,
                Content = Details.Content,
                Name = Details.Title
            });
        }

        public ActionResult Details(int ID)
        {
            var Details = MessageService.Find(ID);
            if (Details == null)
            {
                return HttpNotFound();
            }
            MessageService.ReadMessage(ID);
            var member = MemberService.GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == Details.SenderID);

            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }
            return View(new MessageViewModel()
            {
                ID = Details.ID,
                AddTime = Details.AddTime,
                Content = Details.Content,
                MSN = member.Member_Profile.MSN,
                Name = Details.Title,
                NickName = member.NickName,
                Phone = member.Member_Profile.Phone,
                QQ = member.Member_Profile.QQ,
                RealName = member.Member_Profile.RealName,
                Mobile = member.Member_Profile.Mobile,
                SenderID = Details.SenderID
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Details(MessageViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new Message()
                    {
                        SenderID = CookieHelper.MemberID,
                        RecipientID = model.SenderID,
                        Title = model.Name,
                        Content = model.Replay,
                        AddTime = DateTime.Now,
                        MessageType = (int)MessageType.Reply,
                        RecipienterStatus = (int)MessageStatus.Show,
                        SenderStatus = (int)MessageStatus.Show
                    };
                    MessageService.Create(entity);
                    result.Message = "回复留言成功！";
                    return Content("<script>window.top.location.reload();</script>");
                }
                catch (Exception ex)
                {
                    result.Message = "回复留言失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "回复留言失败!", ex);
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

        public ActionResult SendMessageDetails(int ID)
        {
            var Details = MessageService.Find(ID);

            if (Details == null)
            {
                return HttpNotFound();
            }

            var member = MemberService.GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == Details.RecipientID);

            if (member.Member_Profile == null)
            {
                member.Member_Profile = new Member_Profile();
            }

            return View(new MessageViewModel()
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

    }
}
