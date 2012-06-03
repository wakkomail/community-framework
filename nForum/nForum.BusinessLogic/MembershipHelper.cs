using System.Web;
using System.Web.Security;
using nForum.BusinessLogic.Models;
using umbraco.cms.businesslogic.member;

namespace nForum.BusinessLogic
{
    public static class MembershipHelper
    {
        /// <summary>
        /// Returns the MemberGroup/MemberType name
        /// </summary>
        public static string ForumUserRoleName
        {
            get { return "ForumUser"; }
        }

        /// <summary>
        /// Logs in a member by their username
        /// </summary>
        /// <param name="username"></param>
        public static void LogInUser(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }

        /// <summary>
        /// Get a member by the member Id
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public static ForumMember ReturnMember(int? memberid)
        {
            if (memberid != null)
            {
                ForumMember cmem;
                if (!CacheHelper.Get(CacheHelper.CacheNameMember(memberid), out cmem))
                {
                    cmem = new ForumMember(memberid);
                    CacheHelper.Add(cmem, CacheHelper.CacheNameMember(memberid));
                }
                return cmem;
            }
            return null;
        }

        /// <summary>
        /// Returns a formatted HTML link to the members profile, using the SEO Url rewriting
        /// </summary>
        /// <param name="username"></param>
        /// <param name="memberid"></param>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static string ReturnMemberProfileLink(string username, int? memberid, int? postid)
        {
            if (username != "N/A")
            {
                string relatt = null;
                if (postid != null)
                    relatt = string.Concat(" class='postmember", postid, "'");
                return string.Format("<a{2} href='/member/{1}.aspx'>{0}</a>",
                                     username,
                                     memberid,
                                     relatt);
            }
            return username;
        }

		/// <summary>
		/// Returns a formatted HTML link to the members profile, using the SEO Url rewriting
		/// </summary>
		public static string ReturnMemberProfileLink(string username, string cssClass, int? memberid, int? postid)
		{
			if (username != "N/A")
			{
				string relatt = null;
				if (postid != null)
					relatt = string.Concat(" postmember", postid);

				return string.Format("<a class='{3}{2}' href='/member/{1}.aspx'>{0}</a>",
									 username,
									 memberid,
									 relatt, cssClass);
			}
			return username;
		}

        /// <summary>
        /// Log out the member
        /// </summary>
        public static void LogoutMember()
        {
            FormsAuthentication.SignOut();
            Roles.DeleteCookie();
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Checks to see if the member is logged in or not
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static bool IsMember(string groupName)
        {
            if (IsAuthenticated() == false)
            {
                return false;
            }

            return HttpContext.Current.User.IsInRole(groupName);
         }

    }
}
