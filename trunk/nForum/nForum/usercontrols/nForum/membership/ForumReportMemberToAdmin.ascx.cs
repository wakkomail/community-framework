using System;
using System.Text;
using nForum.BusinessLogic;
using umbraco;

namespace nForum.usercontrols.nForum.membership
{
    public partial class ForumReportMemberToAdmin : BaseForumUsercontrol
    {
        public int? MemberId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MembershipHelper.IsAuthenticated() && !IsBanned)
            {
                // Make sure we have a member id available to us
                MemberId = null;
                if (Request.QueryString["mem"] != null)
                    MemberId = Convert.ToInt32(Request.QueryString["mem"]);

                // If member reporting is disabled or no member is found, hide the form
                if (!Settings.EnableMemberReporting | MemberId == null | IsBanned)
                {
                    HideForm();
                } 
            }
            else
            {
                HideForm();
            }
        }

        private void HideForm()
        {
            lvReportMember.Visible = false;
        }

        /// <summary>
        /// Send the member report to the admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSubmitPostClick(object sender, EventArgs e)
        {

            // get the post so we can create a direct link to it
            var directlink = string.Concat("/member/", MemberId, ".aspx");

            // Get ready to store email
            var sb = new StringBuilder();

            // Create message body
            sb.AppendFormat(library.GetDictionaryItem("ReportMemberEmailText"), 
                            string.Concat(Url(), directlink),
                            CurrentMember.MemberLoginName);

            // Send spam report
            Helpers.SendMail(Settings.EmailAdmin, string.Concat(Settings.Name, library.GetDictionaryItem("MemberReportSubject")), sb.ToString());

            // Show friendly message
            Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("MemberReportThankYou")));
        }
    }
}