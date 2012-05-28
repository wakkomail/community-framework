using System;
using umbraco.cms.businesslogic.member;

namespace nForum.BusinessLogic.Models
{
    public class ForumMember
    {
        /// <summary>
        /// Constructor creates 
        /// </summary>
        /// <param name="memberId"></param>
        public ForumMember(int? memberId)
        {
            if (memberId == 0)
            {
                memberId = null;
            }

            var m = memberId == null ? Member.GetCurrentMember() : new Member((Int32)memberId);

            if (m != null)
            {
                MemberId = m.Id;
                MemberName = m.Text;
                MemberLoginName = m.LoginName;
                MemberEmail = m.Email;
                MemberDateRegistered = m.CreateDateTime;

                MemberTwitterUrl = m.getProperty("forumUserTwitterUrl").Value != null ? m.getProperty("forumUserTwitterUrl").Value.ToString() : null;
                MemberPostAmount = m.getProperty("forumUserPosts").Value.ToString().ToInt32();
                MemberKarmaAmount = m.getProperty("forumUserKarma").Value.ToString().ToInt32();
                MemberAllowPrivateMessages = m.getProperty("forumUserAllowPrivateMessages").Value.ToString() == "1";
                MemberLastPrivateMessageTime = Convert.ToDateTime(m.getProperty("forumUserLastPrivateMessage").Value.ToString());
                MemberIsAdmin = m.getProperty("forumUserIsAdmin").Value.ToString() == "1";
                MemberIsBanned = m.getProperty("forumUserIsBanned").Value.ToString() == "1";
                MemberIsAuthorised = m.getProperty("forumUserIsAuthorised").Value.ToString() == "1";
            }
        }

        public int? MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberLoginName { get; set; }
        public string MemberEmail { get; set; }
        public DateTime MemberDateRegistered { get; set; }
        public string MemberTwitterUrl { get; set; }
        public int? MemberPostAmount { get; set; }
        public int? MemberKarmaAmount { get; set; }
        public bool MemberAllowPrivateMessages { get; set; }
        public DateTime MemberLastPrivateMessageTime { get; set; }
        public bool MemberIsAdmin { get; set; }
        public bool MemberIsBanned { get; set; }
        public bool MemberIsAuthorised { get; set; }

    }
}
