using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadSite.Models;
using PadSite.ViewModels;
using Maitonn.Core;

namespace PadSite.Service.Interface
{
    public interface IMemberService
    {
        IQueryable<Member> GetALL();

        IQueryable<Member> GetKendoALL();

        void Create(Member model);

        Member Create(RegViewModel model);

        Member Create(DetailsViewModel model);

        Member Update(EditViewModel model);

        void Delete(Member model);

        Member Find(int ID);

        bool Login(string Email, string Md5Password);

        bool OpenUserLogin(OpenLoginViewModel OpenUser, OpenLoginType openType);

        void SetLoginCookie(Member member);

        bool ExistsEmail(string Email);

        bool ExistsNickName(string NickName);

        bool ExistsEmailNotMe(int MemberID, string Email);

        bool ExistsNickNameNotMe(int MemberID, string NickName);

        bool ValidatePassword(int MemberID, string Password);

        void ResetPassword(Member member, string newpassword);

        bool ChangePassword(int MemberID, string oldpassword, string newpassword);

        void ChangeStatus(Member member, int Status);

        void ChangeStatus(IEnumerable<int> Ids, int Status);

        void SaveMemberProfile(int MemberID, ProfileViewModel model);

        void SaveMemberContact(int MemberID, ContactViewModel model);

        void SaveMemberAvtar(int MemberID, AvtarViewModel model);

        Member FindDescriptionMemberInLimitTime(string description, int limitHours);

        bool HasGetPasswordActionInLimitTime(string email, int limitMin, int memberAction);
    }
}