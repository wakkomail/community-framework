using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nForum.helpers;
using nForum.global;
using umbraco.NodeFactory;
using umbraco.interfaces;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using nForum.helpers.businessobjects;

namespace nForum.usercontrols.CLC
{
    public partial class MyAgenda : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get all future agendaitems
            if (!IsPostBack)
            {
                IEnumerable<Node> nodes = AgendaHelper.GetUpcomingEvents(-1);

                this.rptAgenda.DataSource = nodes;
                this.rptAgenda.DataBind();
            }
        }

        protected int GetProjectID(int agendaItemId)
        {
            return NodeHelper.GetParentNodeByType(agendaItemId, GlobalConstants.ProjectAlias).Id;
        }

    }
}