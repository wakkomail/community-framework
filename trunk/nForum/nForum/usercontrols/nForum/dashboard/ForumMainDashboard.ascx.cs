using System;
using System.Linq;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum.dashboard
{
    public partial class ForumMainDashboard : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetBadPosts();
        }

        private void GetBadPosts()
        {
            // Get the lastest 30 posts that have negative karma
            var posts = from p in Factory.ReturnAllBadPosts(30)
                         where p.Karma < 0
                         select p;

            // Now bind
            gvBadPosts.DataSource = posts;
            gvBadPosts.DataBind();
        }
    }
}