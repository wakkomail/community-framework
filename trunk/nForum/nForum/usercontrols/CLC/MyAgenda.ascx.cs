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
    public partial class MyAgenda : BaseForumUsercontrol
    {
        List<Node> agendaItems = new List<Node>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // get all agenda items of current node

            this.GetAllNodesByType(-1, GlobalConstants.AgendaItemAlias);

            this.rptAgenda.DataSource = agendaItems;
            this.rptAgenda.DataBind();
        }

        private void GetAllNodesByType(int nodeId, string typeName)
        {
            var node = new Node(nodeId);
            foreach (Node childNode in node.Children)
            {
                var child = childNode;
                if (child.NodeTypeAlias == typeName)
                {
                    agendaItems.Add(child);
                }

                if (child.Children.Count > 0)
                {
                    GetAllNodesByType(child.Id, typeName);
                }                    
            }
        }

    }
}