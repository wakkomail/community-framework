using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using nForum.helpers;
using umbraco.NodeFactory;
using System.Collections.Generic;

namespace nForum.usercontrols.CLC
{
    public partial class PopularProjects : BaseForumUsercontrol
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
			//Get all projects.
			var projects = DocumentHelper.GetOrCreateProjectgroupCategory().Children;
			var projectCategories = new List<ForumCategory>();

			foreach (var project in projects)
			{
				 projectCategories.Add(Mapper.MapForumCategory(new Node(project.Id)));
			}

			var popularMembergroups = projectCategories.Where(m => m.SubTopics().Count(t => t.Posts().Count(p => p.CreatedOn > DateTime.Now.AddDays(-14)) > 5) >= 1);

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
