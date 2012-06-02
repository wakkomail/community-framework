using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;

namespace nForum.usercontrols.CLC.templates
{
	public partial class DiscussionRepeaterItem : BaseForumUsercontrol
    {
        public ForumTopic FTopic { get; set; }

        public string ShowAjaxPostLink(int postid)
        {
            // If Ajax snippets are enabled add them
            return Settings.EnableAjaxPostSnippets ? string.Format(" class=\"postpreview\" rel=\"{0}\"", postid) : null;
        }

		public string GetFirstPost(ForumTopic topic)
		{
			return topic.GetFirstPost().Content;
		}
    }
}