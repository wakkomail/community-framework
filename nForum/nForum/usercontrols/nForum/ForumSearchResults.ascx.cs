using System;
using System.Linq;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;

namespace nForum.usercontrols.nForum
{
    public partial class ForumSearchResults : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get Search Results
            GetSearchResults();
        }

        private void GetSearchResults()
        {
            // Get the paging variable
            int? p = null;
            if (Request.QueryString["p"] != null)
                p = Convert.ToInt32(Request.QueryString["p"]);

            // Get the search variable
            string s = null;
            if (Request.QueryString["s"] != null)
                s = Request.QueryString["s"];

            if(s != null)
            {
                // Strip out any nasties from the term
                var searchterm = Helpers.SafePlainText(s).ToLower();

                // Get the posts that contain the search
                var posts = Factory.SearchPosts(searchterm).Select(x => x.ParentId);

                // Now get topics the posts are in
                var maintopics = from t in Factory.ReturnAllTopics(true)
                                 where posts.Contains(t.Id)
                                 select t;

                // Pass to my pager helper
                var pagedResults = new PaginatedList<ForumTopic>(maintopics, p ?? 0, Helpers.MainForumSettings().TopicsPerPage);

                // Decide whether to show pager or not
                if (pagedResults.TotalPages > 1)
                {
                    litPager.Text = pagedResults.ReturnPager();
                }

                // Now bind
                rptTopicList.DataSource = pagedResults;
                rptTopicList.DataBind();
            }
            else
            {
                topiclist.Visible = false;
            }
        }
    }
}