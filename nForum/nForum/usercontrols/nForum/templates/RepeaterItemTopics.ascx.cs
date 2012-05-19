using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;

namespace nForum.usercontrols.nForum.templates
{
    public partial class RepeaterItemTopics : BaseForumUsercontrol
    {
        public ForumTopic FTopic { get; set; }

        public string ShowAjaxPostLink(int postid)
        {
            // If Ajax snippets are enabled add them
            return Settings.EnableAjaxPostSnippets ? string.Format(" class=\"postpreview\" rel=\"{0}\"", postid) : null;
        }

        /// <summary>
        /// Gets some pretty info for the topic, and lets the user browse directly to the last post in the topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public string GetLastPostInfo(ForumTopic topic)
        {
            var latestPost = topic.GetLatestPost();
            if (latestPost != null)
            {
            var createlatestpostlink = string.Concat("/getlatestpost.aspx?t=", topic.Id, "&p=", latestPost.Id);
            return string.Format(library.GetDictionaryItem("LastPostByLinkFormat"),
                        createlatestpostlink,
                        Helpers.GetPrettyDate(latestPost.CreatedOn.ToString()),
                        latestPost.Owner.MemberLoginName);
            }
            return library.GetDictionaryItem("ErrorGettingLatestPost");
        }

    }
}