using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;

namespace nForum.usercontrols.CLC
{
    public partial class Membergroups : BaseForumUsercontrol
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                CreateMainForumList();
            }
        }

        private void CreateMainForumList()
        {

            // Get the main categories
            var useNodeFactory = Request.QueryString["nf"] != null;
            var mainforums = from f in Factory.ReturnAllCategories(useNodeFactory)
                              where f.IsMainCategory
                              select f;

            // End cache check

            if (mainforums.Any())
            {
                rptMainForumList.DataSource = mainforums;
                rptMainForumList.DataBind();
            }
            else
            {
                mainforumlist.InnerHtml = library.GetDictionaryItem("NoForumCategoriesToDisplay");
            }
        }

        public string GetLastPostInCategory(ForumCategory cat)
        {            
            var post = cat.LatestForumPost();
            if (post != null)
            {
                return string.Format(library.GetDictionaryItem("LastPostByTextFormat"),
                    Helpers.GetPrettyDate(post.CreatedOn.ToString()),
                    post.Owner.MemberLoginName);
            }

			return "Er zijn nog geen discussies in deze kennisgroep.";
           
			
			//var getNa = library.GetDictionaryItem("GetNAText");
            //return string.Format(library.GetDictionaryItem("LastPostByTextFormat"), getNa, getNa);
        }

        protected void HidePrivateCategories(object sender, RepeaterItemEventArgs e)
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
