using System;
using System.Text;
using nForum.BusinessLogic;
using umbraco;
using umbraco.NodeFactory;

namespace nForum.usercontrols.nForum
{
    public partial class ForumSpamReport : BaseForumUsercontrol
    {
        public int? PostId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Make sure we have a post id available to us
            PostId = null;
            if (Request.QueryString["p"] != null)
                PostId = Convert.ToInt32(Request.QueryString["p"]);

            // If spam reporting is disabled or no post if is found, hide the form
            if (!Settings.EnableSpamReporting | PostId == null | IsBanned)
            {
                lvSubmitSpam.Visible = false;
            }
        }

        /// <summary>
        /// Send the spam report on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSubmitPostClick(object sender, EventArgs e)
        {
            // get the post so we can create a direct link to it
            var post = Mapper.MapForumPost(new Node(PostId.ToInt32()));
            var commentId = "#comment" + post.Id;
            var directlink = umbraco.library.NiceUrl(post.ParentId.ToInt32()) + commentId;

            // Get ready to store email
            var sb = new StringBuilder();

            // Create message body
            sb.AppendFormat("<p><b>{0}</b></p>", library.GetDictionaryItem("SpamEmailText"));
            sb.AppendFormat(library.GetDictionaryItem("SpamEmailLinkText"), string.Concat(Url(), directlink));
            sb.AppendFormat(library.GetDictionaryItem("SpamEmailReportedText"), CurrentMember.MemberLoginName);
            sb.AppendFormat("<p>{0}</p>", post.Content.ConvertBbCode());

            // Send spam report
            Helpers.SendMail(Settings.EmailAdmin, string.Concat(Settings.Name, library.GetDictionaryItem("SpamReportEmailSubject")), sb.ToString());

            // Show friendly message
            Response.Redirect(string.Concat(directlink.Replace(commentId, ""), "?m=", library.GetDictionaryItem("ThankYouText")));
        }
    }
}