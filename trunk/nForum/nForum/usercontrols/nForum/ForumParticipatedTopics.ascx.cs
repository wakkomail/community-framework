using System;
using System.Linq;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum
{
    public partial class ForumParticipatedTopics : BaseForumUsercontrol
    {
        public int AmountToShow { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowActiveTopics();
            }
        }

        /// <summary>
        /// Get the 30 latest active topics user posted in and bind them to repeater
        /// </summary>
        private void ShowActiveTopics()
        {
            if (MembershipHelper.IsAuthenticated())
            {
                // Get the lastest 30 posts from this user
                var posts = (from p in Factory.ReturnAllPostsByMemberId(CurrentMember.MemberId.ToInt32())
                             select p.ParentId).Distinct().Take(AmountToShow);

                // Now get topics where the user has posted
                var maintopics = from t in Factory.ReturnAllTopics(true)
                                 where posts.Contains(t.Id)
                                 select t;

                // Now bind
                rptTopicList.DataSource = maintopics;
                rptTopicList.DataBind();
            }
            else
            {
                // Whoever this is isn't logged in, so hide or we'll get an error
                topiclist.Visible = false;
            }
        }
    }
}