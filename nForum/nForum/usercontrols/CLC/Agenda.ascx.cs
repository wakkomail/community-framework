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

namespace nForum.usercontrols.CLC
{
    public partial class Agenda : System.Web.UI.UserControl
    {
        #region Properties

        public bool ShowAll { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // set showAll link based on macro parameter
            showAll.Visible = ShowAll;

            // get all agenda items of current node

            if (Node.GetCurrent().Children.Count > 0)
            {
                var yearFolders = GetYearFolders(Node.GetCurrent());

                if (yearFolders != null)
                {
                    List<Node> agendaItems = new List<Node>();

                    foreach (Node yearFolder in yearFolders)
                    {
                        foreach (Node agendaItem in yearFolder.ChildrenAsList)
                        {
                            agendaItems.Add(agendaItem);
                        }
                    }

                    if (ShowAll == false)
                    {
                        agendaItems = agendaItems.Take(3).ToList();
                    }

                    this.rptAgenda.DataSource = agendaItems;
                    this.rptAgenda.DataBind();
                }
            }

        }

        private IEnumerable<INode> GetYearFolders(Node node)
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

        private bool IsNodeAgenda(Node node)
        {
            return (node.ChildrenAsList.FirstOrDefault(n => n.NodeTypeAlias == GlobalConstants.DateFolderAlias) != null);
        }


    }
}