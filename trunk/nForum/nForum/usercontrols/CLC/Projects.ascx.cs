using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using nForum.helpers;

namespace nForum.usercontrols.CLC
{
    public partial class Projects : BaseForumUsercontrol
	{
		

		protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
				InitializeProjects();
            }
        }

		private void InitializeProjects()
        {
			//Get the projects rootfolder.
			var projectsRoot = DocumentHelper.GetOrCreateProjectgroupCategory();
			
        }
    }
}