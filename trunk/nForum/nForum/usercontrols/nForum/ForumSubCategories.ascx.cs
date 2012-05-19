using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;

namespace nForum.usercontrols.nForum
{
    public partial class ForumSubCategories : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                GetSubCategories();
            }
        }

        private void GetSubCategories()
        {

            var subcats = from sc in Factory.ReturnAllSubCategoriesInCategory(CurrentNode.Id)
                          select sc;

            if (subcats.Any())
            {
                pnlSubCategories.Visible = true;
                rptSubCategories.DataSource = subcats;
                rptSubCategories.DataBind();
            }
        }

        protected void HidePrivateSubCategories(object sender, RepeaterItemEventArgs e)
        {
            // This is for hiding topics which are private
            // First check if user is logged in, if they are check karma amount
            var showcategory = true;
            if (CurrentMember != null)
            {
                // Member is logged in so it doesn't matter if forum is private or not, 
                // now check they have enough karma to view this category and hide if not
                if (CurrentMember.MemberKarmaAmount < ((ForumCategory)e.Item.DataItem).KarmaAccessAmount)
                {
                    showcategory = false;
                }
            }
            else
            {
                if (((ForumCategory)e.Item.DataItem).IsPrivate)
                {
                    showcategory = false;
                }
            }

            e.Item.Visible = showcategory;
        }


    }
}