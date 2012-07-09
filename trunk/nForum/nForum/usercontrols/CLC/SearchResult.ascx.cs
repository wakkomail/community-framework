using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.NodeFactory;
using nForum.helpers;
using nForum.global;
using nForum.BusinessLogic;

namespace nForum.usercontrols.CLC
{
    public partial class SearchResult : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["search"] != null)
            {
                string searchText = Request.QueryString["search"].ToString().ToLower();

                // get all nodes with the searchtext in the text field
                IEnumerable<Node> searchresult = NodeHelper.GetAllNodesByTypes(-1, new string[] {  GlobalConstants.MembergroupAlias, 
                                                                                            GlobalConstants.ProjectAlias,
                                                                                            GlobalConstants.AgendaItemAlias,                                                                                            
                                                                                            GlobalConstants.PostAlias})
                                                                         .Where(n => n.Name != null && (n.Name.ToLower().Contains(searchText) ||
                                                                         (n.GetProperty("description") != null && n.GetProperty("description").Value.ToLower().Contains(searchText)) ||
                                                                         (n.GetProperty("forumPostContent") != null && n.GetProperty("forumPostContent").Value.ToLower().Contains(searchText))));                

                rptSearchResult.DataSource = searchresult;
                rptSearchResult.DataBind();

                lblNoResults.Visible = false;
            }
            else{
                lblNoResults.Visible = true;
            }
        }

        protected void rptSearchResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            Node node = (Node)e.Item.DataItem;

            // set type label
            Label typeLabel = (Label)e.Item.FindControl("lblSearchResultType");            

            // set node text label
            Label textLabel = (Label)e.Item.FindControl("lblNodeText");

            // set link
            HyperLink link = (HyperLink)e.Item.FindControl("lnkResult");

            switch (node.NodeTypeAlias)
            {
                case GlobalConstants.PostAlias:
                    {
                        typeLabel.Text = "Post in discussie: ";
                        textLabel.Text = node.Name;

                        Node topicNode = NodeHelper.GetParentNodeByType(node.Id, GlobalConstants.DiscussionAlias);
                        link.NavigateUrl = umbraco.library.NiceUrl(topicNode.Id);
                        link.Attributes.Add("rel", topicNode.Id.ToString());
                        link.CssClass = "c1 postpreview";
                    }break;
                case GlobalConstants.MembergroupAlias:
                    {
                        typeLabel.Text = "Kennisgroep: ";
                        textLabel.Text = node.Name;
                        link.NavigateUrl = umbraco.library.NiceUrl(node.Id);
                    } break;
                case GlobalConstants.ProjectAlias:
                    {
                        typeLabel.Text = "Project: ";
                        textLabel.Text = node.Name;
                        link.NavigateUrl = umbraco.library.NiceUrl(node.Id);
                    } break;
                case GlobalConstants.AgendaItemAlias:
                    {
                        typeLabel.Text = "Agenda item: ";
                        textLabel.Text = node.GetProperty("description").Value.ToString();
                        Node agendaNode = NodeHelper.GetParentNodeByType(node.Id, GlobalConstants.AgendaAlias);
                        link.NavigateUrl = umbraco.library.NiceUrl(agendaNode.Id);
                    } break;
            }


        }
    }
}