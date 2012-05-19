using System;
using nForum.BusinessLogic;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum.membership
{
    public partial class ForumMemberProfileEdit : BaseForumUsercontrol
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            if (CurrentMember == null | IsBanned)
            {
                membereditprofile.Visible = false;
            }
            else
            {
                //We have a Member logged in fill the data
                tbLoginName.Text = CurrentMember.MemberLoginName;
                tbEmail.Text = CurrentMember.MemberEmail;
                tbName.Text = CurrentMember.MemberName;
                tbTwitter.Text = CurrentMember.MemberTwitterUrl;
                if (CurrentMember.MemberAllowPrivateMessages)
                    cbAllowPrivateMessages.Checked = true;
            }
        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
            var cMem = Member.GetCurrentMember();
            cMem.Text = tbName.Text;
            cMem.LoginName = tbLoginName.Text;
            cMem.Email = tbEmail.Text;
            cMem.getProperty("forumUserTwitterUrl").Value = tbTwitter.Text;
            cMem.getProperty("forumUserAllowPrivateMessages").Value = cbAllowPrivateMessages.Checked ? "1" : "0";

            //Save member
            cMem.Save();

            //Generate member Xml Cache
            cMem.XmlGenerate(new System.Xml.XmlDocument());

            // Clear member cache (This is done via an event)
            //CacheHelper.Clear(CacheHelper.CacheNameMember(cMem.Id));

            // Show message
            Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("ProfileUpdated")));

        }
    }
}