using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.Service.Interface;
using System.Data.Entity;
using Maitonn.Core;
namespace PadSite.Service
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork db;

        public MemberService(IUnitOfWork db
            )
        {
            this.db = db;
        }

        public IQueryable<Member> GetALL()
        {
            return db.Set<Member>();
        }

        public IQueryable<Member> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Member>();
        }

        public void Create(Member model)
        {
            db.Add<Member>(model);
            db.Commit();
        }

        public Member Create(ViewModels.RegViewModel model)
        {
            Member entity = new Member();
            entity.Email = model.Email;
            entity.NickName = model.NickName;
            entity.OpenID = model.OpenID;
            entity.OpenType = model.OpenType;
            entity.Status = (int)MemberStatus.Registered;//注册未激活，0为禁用
            entity.Password = CheckHelper.StrToMd5(model.Password);
            entity.GroupID = 1;
            entity.AddTime = DateTime.Now;
            entity.LastTime = DateTime.Now;
            entity.AddIP = HttpHelper.IP;
            entity.LastIP = HttpHelper.IP;
            entity.LoginCount = 1;
            db.Add<Member>(entity);
            db.Commit();
            return entity;
        }

        public void Update(Member model)
        {

            db.Commit();
        }

        public void Delete(Member model)
        {
            var target = Find(model.MemberID);
            db.Remove<Member>(target);
            db.Commit();
        }

        public Member Find(int ID)
        {
            return db.Set<Member>().Single(x => x.MemberID == ID);
        }

        public void SetLoginCookie(Member member)
        {
            CookieHelper.LoginCookieSave(member.MemberID.ToString(),
              member.Email,
              member.NickName,
              member.AvtarUrl,
              member.GroupID.ToString(),
              member.Status.ToString(),
              member.Password, "1");
        }

        public bool Login(string Email, string Md5Password)
        {
            var LoginUser = db.Set<Member>()
               .SingleOrDefault(x => x.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)
                   && x.Password.Equals(Md5Password, StringComparison.CurrentCultureIgnoreCase));
            if (LoginUser != null)
            {
                db.Attach<Member>(LoginUser);
                LoginUser.LastIP = HttpHelper.IP;
                LoginUser.LastTime = DateTime.Now;
                LoginUser.LoginCount = LoginUser.LoginCount + 1;
                int memberAction = (int)MemberActionType.Login;
                Member_Action ma = new Member_Action();
                ma.ActionType = memberAction;
                ma.AddTime = DateTime.Now;
                ma.IP = HttpHelper.IP;
                ma.Description = "登录";
                LoginUser.Member_Action.Add(ma);
                db.Commit();
                SetLoginCookie(LoginUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OpenUserLogin(ViewModels.OpenLoginViewModel OpenUser, OpenLoginType openType)
        {
            int typeValue = (int)openType;
            var LoginUser = db.Set<Member>()
                .SingleOrDefault(x =>
                    x.OpenID.Equals(OpenUser.OpenId)
                    && x.OpenType == typeValue);
            if (LoginUser != null)
            {
                db.Attach<Member>(LoginUser);
                LoginUser.LastIP = HttpHelper.IP;
                LoginUser.LastTime = DateTime.Now;
                LoginUser.LoginCount = LoginUser.LoginCount + 1;
                Member_Action ma = new Member_Action();
                ma.ActionType = (int)MemberActionType.Login;
                ma.AddTime = DateTime.Now;
                ma.Description = "登录";
                LoginUser.Member_Action.Add(ma);
                db.Commit();
                SetLoginCookie(LoginUser);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExistsEmail(string Email)
        {
            return db.Set<Member>().Count(x => x.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }

        public bool ExistsNickName(string NickName)
        {
            return db.Set<Member>().Count(x => x.NickName.Equals(NickName, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }


        public bool ExistsEmailNotMe(int MemberID, string Email)
        {
            return db.Set<Member>().Where(x => x.MemberID != MemberID).Count(x => x.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }

        public bool ExistsNickNameNotMe(int MemberID, string NickName)
        {
            return db.Set<Member>().Where(x => x.MemberID != MemberID).Count(x => x.NickName.Equals(NickName, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }


        public bool ValidatePassword(int MemberID, string Password)
        {
            string Md5Password = CheckHelper.StrToMd5(Password);
            return db.Set<Member>().Count(x => x.Password.Equals(Md5Password, StringComparison.CurrentCultureIgnoreCase) && x.MemberID == MemberID) > 0;
        }


        public void changeStatus(Member member, int Status)
        {
            db.Attach<Member>(member);
            member.Status = Status;
            db.Commit();
            SetLoginCookie(member);
        }

        public void ResetPassword(Member member, string newpassword)
        {
            db.Attach<Member>(member);
            member.Password = CheckHelper.StrToMd5(newpassword);
            db.Commit();
        }

        public bool ChangePassword(int MemberID, string oldpassword, string newpassword)
        {
            bool success = false;
            Member member = Find(MemberID);
            string Md5Password = CheckHelper.StrToMd5(oldpassword);
            if (Md5Password == member.Password)
            {
                success = true;
                db.Attach<Member>(member);
                member.Password = CheckHelper.StrToMd5(newpassword);
                db.Commit();
                SetLoginCookie(member);
            }
            return success;
        }
    }
}