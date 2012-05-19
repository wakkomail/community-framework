using System;
using nForum.BusinessLogic;
using umbraco;

namespace nForum.usercontrols.nForum
{
    public partial class ForumGetLatestPostInTopic : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FindLatestPost();
        }

        private void FindLatestPost()
        {
            // Get the post and topic id
            int? postId = null;
            int? topicId = null;

            if (Request.QueryString["p"] != null)
                postId = Convert.ToInt32(Request.QueryString["p"]);

            if (Request.QueryString["t"] != null)
                topicId = Convert.ToInt32(Request.QueryString["t"]);

            if(postId != null && topicId != null)
            {
                // Get the topic and the post
                //var lTopic = u.ForumTopics.SingleOrDefault(x => x.Id == topicId);
                //start to build the Url
                var urlQs = string.Concat("#comment", postId);
                // Get the amout of posts in this topic, to work out if we need to add a paging variable
                var lTopicPostCount = Factory.ReturnPostCountInTopic(topicId.ToInt32());
                // See if we need to add a pager on
                var pageNumber = (lTopicPostCount / Convert.ToInt32(Settings.PostsPerPage));
                if(pageNumber > 1)
                {
                    urlQs = string.Concat("?p=", pageNumber.ToString("0")) + urlQs;
                }

                urlQs = umbraco.library.NiceUrl((int)topicId) + urlQs;

                Response.Redirect(urlQs);
            }
            else
            {
                // There was a problem getting the post
                Response.Redirect(string.Concat(Settings.Url, "?m=", library.GetDictionaryItem("ErrorGettingLatestPost")));
            }

        }
    }
}