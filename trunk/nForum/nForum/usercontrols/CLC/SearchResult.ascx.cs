using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.NodeFactory;
using nForum.helpers;
using nForum.global;

namespace nForum.usercontrols.CLC
{
    public partial class SearchResult : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["search"] != null)
            {
                string searchText = Request.QueryString["search"].ToString();

                // get all nodes with the searchtext in the text field
                IEnumerable<Node> searchresult = NodeHelper.GetAllNodesByTypes(-1, new string[] {  GlobalConstants.MembergroupAlias, 
                                                                                            GlobalConstants.ProjectAlias,
                                                                                            GlobalConstants.AgendaItemAlias,                                                                                            
                                                                                            GlobalConstants.PostAlias}
                                                                         ).Where(n => n.Name.Contains(searchText) ||
                                                                                 n.GetProperty("description").Value.Contains(searchText) ||
                                                                                 n.GetProperty("forumPostContent").Value.Contains(searchText));

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
            // set type label
            Label typeLabel = (Label)e.Item.FindControl("lblSearchResultType");

            // set node text label
            Label textLabel = (Label)e.Item.FindControl("lblNodeText");

            // set link
            HyperLink link = (HyperLink)e.Item.FindControl("lnkResult");
        }
    }
}