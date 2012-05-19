using System;
using System.Text;
using System.Web.Security;
using nForum.BusinessLogic;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum.membership
{
    public partial class Register : BaseForumUsercontrol
    {

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            var redirecturl = Settings.Url;

            // Check the user isn't already registered
            if (Member.GetMemberFromEmail(Helpers.GetSafeHtml(tbEmail.Text)) == null & Member.GetMemberFromLoginName(Helpers.GetSafeHtml(tbLoginName.Text)) == null)
            {
                // Set the member type and group
                var mt = MemberType.GetByAlias(MembershipHelper.ForumUserRoleName);
                var addToMemberGroup = MemberGroup.GetByName(MembershipHelper.ForumUserRoleName);

                //create a member
                var m = Member.MakeNew(Helpers.GetSafeHtml(tbName.Text), mt, new umbraco.BusinessLogic.User(0));

                //var mstatus = new MembershipCreateStatus();
                //var mp = Membership.CreateUser(tbName.Text, tbPassword.Text, tbEmail.Text, string.Empty, string.Empty, true, out mstatus);

                // Set the other properties
                m.Email = Helpers.GetSafeHtml(tbEmail.Text);
                m.LoginName = Helpers.GetSafeHtml(tbLoginName.Text);
                m.Password = Helpers.GetSafeHtml(tbPassword.Text);
                // Add 0 Karma to user, helps us later in the site
                m.getProperty("forumUserKarma").Value = 0;
                m.getProperty("forumUserTwitterUrl").Value = Helpers.GetSafeHtml(tbTwitter.Text);
                m.getProperty("forumUserAllowPrivateMessages").Value = 1;
                m.getProperty("forumUserLastPrivateMessage").Value = DateTime.Now;
                
                //##### Manual Member Authorisation #####
                // If this is not enabled, mark the member as authorised
                if(!Settings.ManuallyAuthoriseNewMembers)
                {
                    m.getProperty("forumUserIsAuthorised").Value = 1;
                }

                m.AddGroup(addToMemberGroup.Id);               

                //Save member
                m.Save();

                //Generate member Xml Cache
                m.XmlGenerate(new System.Xml.XmlDocument());

                if (!Settings.ManuallyAuthoriseNewMembers)
                {
                    //Login the user so they can be redirected to their profile page
                    FormsAuthentication.SetAuthCookie(tbLoginName.Text, false);
                }
                else
                {
                    redirecturl = string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("NotifiedWhenAccountAuth"));
                }

                // If admins wants email notification, then send it here
                if(Settings.EmailAdminOnNewMemberSignUp)
                {
                    SendAdminNotification(m);
                }
            }
            else
            {
                redirecturl = string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("UserAlreadyExists"));
            }

            // Now redirect to the correct page
            Response.Redirect(redirecturl);

        }

        private void SendAdminNotification(Member newmember)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(library.GetDictionaryItem("MemberSignUpEmailText"), 
                Settings.Name, 
                newmember.LoginName, 
                newmember.Text, 
                newmember.Email);
            Helpers.SendMail(Settings.EmailNotification, Settings.EmailAdmin, library.GetDictionaryItem("NewMemberSignUp"), sb.ToString());
        }
    }
}