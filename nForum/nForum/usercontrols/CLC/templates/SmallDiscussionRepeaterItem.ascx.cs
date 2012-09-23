using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using nForum.global;
using nForum.helpers;

namespace nForum.usercontrols.CLC.templates
{
	public partial class SmallDiscussionRepeaterItem : BaseForumUsercontrol
    {
        public ForumTopic FTopic { get; set; }

        public string ShowAjaxPostLink(int postid)
        {
            // If Ajax snippets are enabled add them
            return Settings.EnableAjaxPostSnippets ? string.Format(" class=\"c1 postpreview\" rel=\"{0}\"", postid) : null;
        }

		public string GetFirstPost(ForumTopic topic)
		{
            string result = topic.GetLatestPost().Content.StripHTML();

            if (result.Length > GlobalConstants.SummaryMaxLength)
            {
                result = result.Substring(0, GlobalConstants.SummaryMaxLength) + "...";
            }


            return result; 
		}
    }
}