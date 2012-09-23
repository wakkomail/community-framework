using System;
using System.Linq;
using nForum.BusinessLogic;

namespace nForum.usercontrols.CLC
{
    public partial class PopularDiscussions : BaseForumUsercontrol
    {
        public int AmountToTake { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
				ShowPopularTopics();
            }
        }

        private void ShowPopularTopics()
        {
            var useNodeFactory = Request.QueryString["nf"] != null;
            var latestTopics = Factory.ReturnActiveTopics(50, useNodeFactory);

			var popularTopics = latestTopics.Where(t => t.Posts().Count(p => p.CreatedOn > DateTime.Now.AddDays(-14)) > 5).Take(AmountToTake);

			rptTopicList.DataSource = popularTopics;
            rptTopicList.DataBind();
        }
    }
}