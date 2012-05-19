using System;
using System.Text;
using System.Web;
using System.Web.UI;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Data;
using umbraco;

namespace nForum.usercontrols.nForum
{
    public partial class ForumAjaxPostSnippet : UserControl
    {
        private ForumFactory _factory = new ForumFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetPostSnippet();
        }

        private void GetPostSnippet()
        {
            //Get the posted data
            var post = HttpContext.Current.Request;

            //Get the postbody
            var topicId = post["postid"];

            // Must cache this stuff
            if(topicId != null)
            {
                // See if the main cats are in the cached, if not cache them
                var fpost = _factory.ReturnTopicStarterPost(topicId.ToInt32());                
                var sb = new StringBuilder();
                sb.AppendFormat("<div class='{0} {1} {2}'>", "personPopupResult", topicId, "postsnippet");
                sb.Append(library.GetDictionaryItem("PostSnippet"));
                sb.AppendFormat("<p>{0}</p>", library.TruncateString(library.StripHtml(fpost.Content), 300, "..."));
                sb.Append("</div>");
                litPost.Text = sb.ToString();
            }
            else
            {
                litPost.Text = library.GetDictionaryItem("NoPostId");
            }
            
        }
    }
}