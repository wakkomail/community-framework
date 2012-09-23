using System;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Data;
using umbraco;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.CLC.membership
{
    public partial class Login : BaseForumUsercontrol
    {
		protected void Page_Load(object sender, EventArgs e)
		{
			var mainForumNode = new Node(new ForumFactory().ReturnRootForumId());
			litLoginDescription.Text = mainForumNode.GetProperty("loginDescription").Value;
		}

        protected void OnLoginError(object sender, EventArgs e)
        {
            Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", ctlLogin.FailureText));
        }

        protected void OnLoggedIn(object sender, EventArgs e)
        {
            // Get the user
            var m = Member.GetMemberFromLoginName(ctlLogin.UserName);
            // Check if user is banned or not
            if (m.getProperty("forumUserIsBanned").Value.ToString() == "1")
            {
                MembershipHelper.LogoutMember();
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("AccountBanned")));
            }

            // Check if user is authorised
            if(Settings.ManuallyAuthoriseNewMembers)
            {
                // if user is not authorised, logout and return a message
                if (m.getProperty("forumUserIsAuthorised").Value.ToString() != "1")
                {
                    MembershipHelper.LogoutMember();
                    Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("AccountNotAuth")));
                }
            }
       
            // Successful login, so redirect to forum root
            Response.Redirect(Settings.Url);

        }
    }
}