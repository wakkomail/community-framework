using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.NodeFactory;

namespace nForum.usercontrols.CLC
{
    public partial class Discussion : BaseForumUsercontrol
    {       
        public ForumTopic ParentTopic { get; set; }
        public int? p { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetPublicProperties();

            if(!Page.IsPostBack)
            {
                ShowCanonicalTag();
                GetTopicPosts();
                IsForumClosed();
                SortHeadings();
            }

        }

        private void ShowCanonicalTag()
        {
            if (HttpContext.Current.Request.QueryString["p"] != null)
            {
                // this means there is a pageing variable, so return the canonical tag
                var cLink = new HtmlLink();
                cLink.Attributes.Add("rel", "canonical");
                cLink.Href = CurrentPageAbsoluteUrl;
                Page.Header.Controls.Add(cLink);
            }
        }

        /// <summary>
        /// Populates properties for use throughout this page
        /// </summary>
        private void GetPublicProperties()
        {            
            //First get parent topic and add to local variable
            ParentTopic = Mapper.MapForumTopic(Node.GetCurrent());      
        }

        /// <summary>
        /// If the forum is set to closed, then hide the post button
        /// </summary>
        private void IsForumClosed()
        {
            if(Settings.IsClosed | ParentTopic.IsClosed | IsBanned)
            {
                lvNewPost.Visible = false;
            }
        }

        /// <summary>
        /// Adds the Topic name as a H1 at the top of the page
        /// </summary>
        private void SortHeadings()
        {
            litHeading.Text = ParentTopic.Name;
        }

        /// <summary>
        /// Gets the topics and returns them in a pager
        /// </summary>
        private void GetTopicPosts()
        {
            // Get the paging variable
            if (Request.QueryString["p"] != null)
                p = Convert.ToInt32(Request.QueryString["p"]);  

            // Before anything see if the category has been marked as private, if so make sure user is loggged in
            var parentCategory = Mapper.MapForumCategory(new Node(ParentTopic.CategoryId));
            var userHasAccess = true;
            if (MembershipHelper.IsAuthenticated())
            {
                // Member is logged in so it doesn't matter if forum is private or not, 
                // now check they have enough karma to view this category and hide if not
                if (CurrentMember.MemberKarmaAmount < parentCategory.KarmaAccessAmount)
                {
                    userHasAccess = false;
                }

                // check user has enough karma to be able to post in this category
                if (CurrentMember.MemberKarmaAmount < parentCategory.KarmaPostAmount)
                {
                    lvNewPost.Visible = false;
                }
                
            }
            else
            {
                if (parentCategory.IsPrivate)
                {
                    userHasAccess = false;
                }
            }
            // Check to see if user has access
            if (!userHasAccess)
            {
                Response.Redirect(string.Concat(Settings.Url, "?m=", library.GetDictionaryItem("NoPermissionToViewPage")));
            }

            //Get all the posts
            var useNodeFactory = Request.QueryString["nf"] != null;
            var topicposts = Factory.ReturnAllPostsInTopic(ParentTopic.Id, useNodeFactory);

			var starterPost = topicposts.SingleOrDefault(tp => tp.IsTopicStarter);
			if (starterPost == null) return;

            // Pass to my pager helper
            var pagedResults = new PaginatedList<ForumPost>(topicposts, p ?? 0, Convert.ToInt32(Settings.PostsPerPage));

            // Decide whether to show pager or not
            if (pagedResults.TotalPages > 1)
            {
				pnlPager.Visible = true;
                litPager.Text = pagedResults.ReturnPager();
            }

			rprDiscussionPosts.DataSource = pagedResults;
			rprDiscussionPosts.DataBind();

            var mDesc = Factory.ReturnTopicStarterPost(ParentTopic.Id).Content;
            var metaDescription = new HtmlMeta
                                      {
                                          Name = "description",
                                          Content = library.TruncateString(library.StripHtml(mDesc), 300, "...")
                                      };
            Page.Header.Controls.Add(metaDescription);
        }

		protected void InitializePost(object sender, RepeaterItemEventArgs e)
		{
			// Get a reference to the forum post in this row
			var post = (ForumPost)e.Item.DataItem;

			var litMemberName = (Literal)e.Item.FindControl("litMemberName");
			var litLastEdited = (Literal)e.Item.FindControl("litLastEdited");

			// Check the member exists who created this post
			// Set all the members details who created the post
			if (post.Owner != null)
			{
				// Create the link to the members profile
				litMemberName.Text = MembershipHelper.ReturnMemberProfileLink(post.Owner.MemberLoginName, "c2", post.Owner.MemberId, post.Id);
			}
			else
			{
				// If the member no longer exists fill with dummy data
				litMemberName.Text = "N/A";
			}

			// If post has been edited show it at bottom of post
			if (!string.IsNullOrEmpty(post.LastEdited.ToString()))
			{
				litLastEdited.Text = string.Format(library.GetDictionaryItem("PostEditedTextFormat"), Helpers.GetPrettyDate(post.LastEdited.ToString()));
			}

			// Do actions that only matter if the user is logged in
			// post has a member attached
			// User is not banned
			// And forum is not closed
			if (CurrentMember != null && litMemberName.Text != "N/A" && IsBanned == false && !Settings.IsClosed)
			{
				//Get all the controls we need to use here
				var btnSubmitPost = (Button)lvNewPost.FindControl("btnSubmitPost");
				var pnlUserAdmin = (Panel)e.Item.FindControl("pnlUserAdmin");
				var lUserEdit = (HyperLink)pnlUserAdmin.FindControl("lUserEdit");
				var quotebutton = (HyperLink)e.Item.FindControl("quotebutton");

				//Add the topic id to the rel of the postbutton so we can use it for ajax post
				btnSubmitPost.Attributes.Add("rel", ParentTopic.Id.ToString());

				// See if user owns post
				// If the user is on a different page, store the variable
				var addPage = Request.QueryString["p"] != null ? string.Concat("?p=", Request.QueryString["p"].ToInt32()) : null;
				if (post.Owner.MemberId == CurrentMember.MemberId)
				{
					pnlUserAdmin.Visible = true;
					lUserEdit.NavigateUrl = string.Concat(post.Url, addPage);
				}
				else if (CurrentMember.MemberIsAdmin)
				{
					// show anyway as user is marked as forum admin
					pnlUserAdmin.Visible = true;
					lUserEdit.NavigateUrl = string.Concat(post.Url, addPage);
				}

				//Show quote button
				string tpager = null;
				if (p != null)
					tpager = string.Concat("?p=", p);
				quotebutton.Visible = true;
				quotebutton.NavigateUrl = string.Concat(CurrentPageAbsoluteUrl, tpager, "#maineditor");
				quotebutton.Attributes.Add("rel", post.Id.ToString());
			}
		}
    }
}