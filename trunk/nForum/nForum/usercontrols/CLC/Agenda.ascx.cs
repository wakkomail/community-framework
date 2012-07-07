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


    public partial class Agenda : BaseForumUsercontrol
    {
        #region Properties

        public bool ShowAll { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // set showAll link based on macro parameter
            btnShowAll.Visible = !ShowAll;

            // get all agenda items of current node

			IEnumerable<Node> agendaItems = AgendaHelper.GetUpcomingEvents(Node.GetCurrent().Id);
			if (ShowAll == false)
			{
				agendaItems = agendaItems.Take(3).ToList();
			}

			this.rptAgenda.DataSource = agendaItems;
			this.rptAgenda.DataBind();

            SetAllAgendaItemsButton();
        }

        private void SetAllAgendaItemsButton()
        {
            var url = library.NiceUrl(CurrentNode.Id);
            btnShowAll.NavigateUrl = Helpers.AlternateTemplateUrlFix("/Agenda", url);
        }
    }


}