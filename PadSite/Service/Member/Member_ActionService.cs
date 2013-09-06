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
    public class Member_ActionService : IMember_ActionService
    {
        private readonly IUnitOfWork db;

        public Member_ActionService(IUnitOfWork db)
        {
            this.db = db;
        }

        public Member_Action Create(Member member, int memberAction, string description)
        {
            Member_Action member_Action = new Member_Action()
            {
                MemberID = member.MemberID,
                ActionType = memberAction,
                AddTime = DateTime.Now,
                Description = description,
                IP = HttpHelper.IP
            };
            db.Add<Member_Action>(member_Action);
            db.Commit();
            return member_Action;
        }

        public bool HasDescriptionActionInLimiteTime(string description, int limitHours)
        {
            DateTime LimitDate = DateTime.Now.AddHours(-limitHours);
            var query = db.Set<Member_Action>()
                   .Where(x => x.Description.Equals(description, StringComparison.OrdinalIgnoreCase)
                   && x.AddTime > LimitDate
                   );
            return query.Count() > 0;
        }


        public bool HasActionByActionTypeInLimiteTime(int MemberID, int memberAction, int limitMins)
        {
            DateTime LimitDate = DateTime.Now.AddMinutes(-limitMins);
            var query = db.Set<Member_Action>()
                  .Where(x => x.MemberID == MemberID
                  && x.ActionType == memberAction
                  && x.AddTime > LimitDate
                  );
            return query.Count() > 0;
        }


        public Member_Action Create(MemberActionType MemberActionType, string description)
        {
            var MemberID = CookieHelper.MemberID;
            Member_Action member_Action = new Member_Action()
            {
                MemberID = MemberID,
                ActionType = (int)MemberActionType,
                AddTime = DateTime.Now,
                Description = description,
                IP = HttpHelper.IP
            };
            db.Add<Member_Action>(member_Action);
            db.Commit();
            return member_Action;
        }

        public Member_Action Create(MemberActionType MemberActionType)
        {
            return Create(MemberActionType, string.Empty);
        }


        public bool HasAction(MemberActionType MemberActionType)
        {
            var MemberID = CookieHelper.MemberID;
            var type = (int)MemberActionType;
            return db.Set<Member_Action>().Any(x => x.MemberID == MemberID && x.ActionType == type);
        }
    }
}