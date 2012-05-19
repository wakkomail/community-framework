using System;
using System.Linq;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum
{
    public partial class ForumTopicsSticky : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetStickyTopicsFromCategory();
            }
        }


        /// <summary>
        /// Gets all the topics from the parent category
        /// </summary>
        private void GetStickyTopicsFromCategory()
        {
            // Get the sticky topics
            var stickytopics = from t in Factory.ReturnAllTopicsInCategory(CurrentNode.Id)
                                where t.IsSticky
                                select t;

            if (stickytopics.Any())
            {
                rptTopicList.DataSource = stickytopics;
                rptTopicList.DataBind();
            }
            else
            {
                stickytopiclist.Visible = false;
            }
        }
    }
}