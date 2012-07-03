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

namespace nForum.usercontrols.CLC
{
	public static class AgendaHelper
	{
		public static List<Node> GetAgendaItems(int nodeId)
		{
			Node node = new Node(nodeId);
			List<Node> agendaItems = new List<Node>();

			if (node != null && node.Children.Count > 0)
			{
				var yearFolders = GetYearFolders(node);

				if (yearFolders != null)
				{
					foreach (Node yearFolder in yearFolders)
					{
						foreach (Node agendaItem in yearFolder.ChildrenAsList)
						{
							agendaItems.Add(agendaItem);
						}
					}
				}
			}

			return agendaItems;
		}

		private static IEnumerable<INode> GetYearFolders(Node node)
		{
			if (node.ChildrenAsList.Count == 0)
			{
				return null;
			}
			else if (!IsNodeAgenda(node))
			{
				// no agenda folder, go level deeper
				foreach (Node childNode in node.ChildrenAsList)
				{
					if (IsNodeAgenda(childNode))
					{
						// agenda found
						return childNode.ChildrenAsList.Where(n => n.NodeTypeAlias == GlobalConstants.DateFolderAlias);
					}
				}
				return null;
			}
			else
			{
				// agenda found
				return node.ChildrenAsList.Where(n => n.NodeTypeAlias == GlobalConstants.DateFolderAlias);
			}
		}

		private static bool IsNodeAgenda(Node node)
		{
			return (node.ChildrenAsList.FirstOrDefault(n => n.NodeTypeAlias == GlobalConstants.DateFolderAlias) != null);
		}
	}

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

			List<Node> agendaItems = AgendaHelper.GetAgendaItems(Node.GetCurrent().Id);
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