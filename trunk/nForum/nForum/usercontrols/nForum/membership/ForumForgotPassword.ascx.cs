using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using nForum.BusinessLogic;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum.membership
{
    public partial class ForumForgotPassword : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                btnSubmit.Text = library.GetDictionaryItem("ForgotPassword");
            }
        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            var cMember = Member.GetMemberFromEmail(Helpers.SafePlainText(tbEmail.Text));

            if(cMember != null)
            {
                // Found the user
                var password = Membership.GeneratePassword(10, 1);
                password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");

                // Change the password
                var member = Membership.GetUser(cMember.LoginName);
                member.ChangePassword(member.ResetPassword(), password);
                //cMember.ChangePassword(Helpers.CalculateSha1(password));

                // Save the password
                cMember.Save();
                cMember.XmlGenerate(new System.Xml.XmlDocument());
                
                // Now email the user their password
                var sb = new StringBuilder();
                sb.AppendFormat(string.Format("<p>{0}</p>", library.GetDictionaryItem("NewPasswordRequestedFor")), Settings.Name);
                sb.AppendFormat("<p><b>{0}</b></p>", password);
                Helpers.SendMail(Settings.EmailNotification, member.Email, string.Concat(Settings.Name, library.GetDictionaryItem("NewPasswordRequest")), sb.ToString());
                
                // Disable the button to stop them pressing it again
                btnSubmit.Enabled = false;

                // Show a message to the user
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("NewPasswordSent")));
            }
            else
            {
                // Can't find a user with that email
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("NoUserWithThatEmail")));
            }
        }
    }
}