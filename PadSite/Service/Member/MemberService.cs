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

        #region base

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
        #endregion

        #region Login

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

        #endregion

        #region validate

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

        #endregion

        #region change

        public void ChangeStatus(Member member, int Status)
        {
            db.Attach<Member>(member);
            member.Status = Status;
            db.Commit();
            SetLoginCookie(member);
        }

        public void ChangeStatus(IEnumerable<int> Ids, int Status)
        {
            db.Set<Member>().Where(x => Ids.Contains(x.MemberID)).ToList().ForEach(x => x.Status = Status);
            db.Commit();
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

        #endregion

        #region profile

        public void SaveMemberProfile(int MemberID, ViewModels.ProfileViewModel model)
        {
            Member member = GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == MemberID);
            db.Attach<Member>(member);
            Member_Profile profile = new Member_Profile();
            if (member.Member_Profile != null)
            {
                profile = member.Member_Profile;
            }
            profile.MemberID = model.MemberID;
            profile.Borthday = model.Borthday;
            profile.CityCodeValue = model.CityCode;
            var cityCode = 0;
            if (!string.IsNullOrEmpty(model.CityCode))
            {
                cityCode = Convert.ToInt32(model.CityCode.Split(',').Last());
            }
            profile.CityCode = cityCode;
            profile.Description = model.Description;
            member.NickName = model.NickName;
            profile.RealName = model.RealName;
            profile.Sex = model.Sex;
            member.Member_Profile = profile;
            db.Commit();
            SetLoginCookie(member);
        }

        #endregion


        public void SaveMemberContact(int MemberID, ViewModels.ContactViewModel model)
        {
            Member member = GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == MemberID);
            db.Attach<Member>(member);
            Member_Profile mp = new Member_Profile();
            if (member.Member_Profile != null)
            {
                mp = member.Member_Profile;
            }
            mp.MemberID = member.MemberID;
            mp.Address = model.Address;
            mp.Phone = model.Phone;
            mp.Mobile = model.Mobile;
            mp.MSN = model.MSN;
            mp.QQ = model.QQ;
            if (model.Position.IndexOf("|") != -1)
            {
                mp.Lat = Convert.ToDouble(model.Position.Split('|')[0]);
                mp.Lng = Convert.ToDouble(model.Position.Split('|')[1]);
            }
            member.Member_Profile = mp;
            db.Commit();
        }


        public Member FindDescriptionMemberInLimitTime(string description, int limitHours)
        {

            DateTime LimitDate = DateTime.Now.AddHours(-limitHours);
            var query = db.Set<Member>()
                .Include(x => x.Member_Action)
                .Where(x =>
                    (x.Member_Action
                        .Any(ma => ma.Description.Equals(description, StringComparison.OrdinalIgnoreCase)
                            && ma.AddTime > LimitDate
                            )
                     ));
            if (query.Count() > 0)
            {
                return query.Single();
            }
            else
            {
                return null;
            }
        }



        public bool HasGetPasswordActionInLimitTime(string email, int limitMin, int memberAction)
        {
            DateTime LimitDate = DateTime.Now.AddMinutes(-(limitMin));
            string IP = HttpHelper.IP;
            var query = db.Set<Member>()
                .Include(x => x.Member_Action)
                .Where(x =>
                    (x.Member_Action.Any(ma =>
                        ma.ActionType == memberAction
                        && ma.AddTime > LimitDate
                        && ma.IP == IP
                        )) && x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return query.Count() > 0;
        }


        public void SaveMemberAvtar(int MemberID, ViewModels.AvtarViewModel model)
        {
            Member member = Find(MemberID);
            db.Attach<Member>(member);
            member.AvtarUrl = model.AvtarUrl;
            db.Commit();
            SetLoginCookie(member);
        }


        public Member Create(ViewModels.DetailsViewModel model)
        {
            Member member = new Member();
            member.Email = model.Email;
            member.NickName = model.NickName;
            member.AvtarUrl = model.AvtarUrl;
            member.GroupID = model.GroupID;
            member.Password = CheckHelper.StrToMd5(model.Password);
            member.Status = 1;//注册未激活，0为禁用
            member.AddTime = DateTime.Now;
            member.LastTime = DateTime.Now;
            member.AddIP = HttpHelper.IP;
            member.LastIP = HttpHelper.IP;
            member.Member_Profile = new Member_Profile();
            member.Member_Profile.Borthday = model.Borthday;
            var cityCode = 0;
            if (!string.IsNullOrEmpty(model.CityCode))
            {
                cityCode = Convert.ToInt32(model.CityCode.Split(',').Last());
            }
            member.Member_Profile.CityCode = cityCode;
            member.Member_Profile.CityCodeValue = model.CityCode;
            member.Member_Profile.Description = model.Description;
            member.Member_Profile.Sex = model.Sex;
            db.Add<Member>(member);
            db.Commit();
            return member;
        }


        public Member Update(ViewModels.EditViewModel model)
        {
            Member member = GetALL().Include(x => x.Member_Profile).Single(x => x.MemberID == model.MemberID);
            db.Attach<Member>(member);
            member.GroupID = model.GroupID;
            member.AvtarUrl = model.AvtarUrl;
            var cityCode = 0;
            if (!string.IsNullOrEmpty(model.CityCode))
            {
                cityCode = Convert.ToInt32(model.CityCode.Split(',').Last());
            }
            member.Member_Profile.CityCode = cityCode;
            member.Member_Profile.CityCodeValue = model.CityCode;
            member.Member_Profile.Sex = model.Sex;
            member.Member_Profile.Borthday = model.Borthday;
            member.Member_Profile.Description = model.Description;
            db.Commit();
            return member;
        }



    }
}