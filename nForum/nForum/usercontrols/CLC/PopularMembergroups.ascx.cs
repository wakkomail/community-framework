using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;

namespace nForum.usercontrols.CLC
{
    public partial class PopularMembergroups : BaseForumUsercontrol
	{
        public int AmountToTake { get; set; }

		protected bool UseGridClass
		{
			get
			{
				return AmountToTake <= 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                CreateMainForumList();
            }
        }

        private void CreateMainForumList()
        {

            // Get the main categories
            var useNodeFactory = Request.QueryString["nf"] != null;
            var mainforums = from f in Factory.ReturnAllCategories(useNodeFactory).OrderByDescending(c => c.CreatedOn)
                              where f.IsMainCategory
                              select f;

            // End cache check

			var popularMembergroups = mainforums.Where(m => m.SubTopics().Count(t => t.Posts().Count(p => p.CreatedOn > DateTime.Now.AddDays(-14)) > 5) >= 1);

            if (AmountToTake > 0)
            {
				popularMembergroups = popularMembergroups.Take(AmountToTake);
            }

			if (popularMembergroups.Any())
            {
				rptMainForumList.DataSource = popularMembergroups;
                rptMainForumList.DataBind();
            }
        }
    }
}
