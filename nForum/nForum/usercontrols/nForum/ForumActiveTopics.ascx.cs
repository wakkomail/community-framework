using System;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum
{
    public partial class ForumActiveTopics : BaseForumUsercontrol
    {
        public int AmountToTake { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowActiveTopics();
            }
        }

        /// <summary>
        /// Get the 50 latest active topics and bind them to repeater
        /// </summary>
        private void ShowActiveTopics()
        {
            var useNodeFactory = Request.QueryString["nf"] != null;
            var maintopics = Factory.ReturnActiveTopics(AmountToTake, useNodeFactory);
            rptTopicList.DataSource = maintopics;
            rptTopicList.DataBind();
        }
    }
}