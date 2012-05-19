using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.NodeFactory;

namespace nForum.usercontrols.nForum
{
    public partial class ForumListPostsInTopic : BaseForumUsercontrol
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
                CheckTopicEmailSubscription();
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
        /// Show/Hides email subscription link and changes depending if user has subscribed
        /// </summary>
        private void CheckTopicEmailSubscription()
        {
            // Firstly check if email subscriptions are enabled
            if (Settings.EnableTopicEmailSubscriptions && MembershipHelper.IsAuthenticated() && !IsBanned)
            {
                // Set the link details
                hlEmailSubscribe.Visible = true;
                hlEmailSubscribe.Attributes.Add("rel", ParentTopic.Id.ToString());
                hlEmailSubscribe.NavigateUrl = library.NiceUrl(ParentTopic.Id);

                if (ParentTopic.SubscriberIds != null && ParentTopic.SubscriberIds.Any() && ParentTopic.SubscriberIds.Contains((int)CurrentMember.MemberId))
                {
                        //User has subscribed to the 
                        hlEmailSubscribe.CssClass = "subscribedtotopic";
                        hlEmailSubscribe.Text = library.GetDictionaryItem("UnsubscribeFromTopic");
                }
            }
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

            // Pass to my pager helper
            var pagedResults = new PaginatedList<ForumPost>(topicposts, p ?? 0, Convert.ToInt32(Settings.PostsPerPage));

            // Decide whether to show pager or not
            if (pagedResults.TotalPages > 1)
            {
                litPager.Text = pagedResults.ReturnPager();
            }

            // Now bind
            rptTopicPostList.DataSource = pagedResults;
            rptTopicPostList.DataBind();

            // Set the meta description
            var starterPost = Factory.ReturnTopicStarterPost(ParentTopic.Id);
            
            if (starterPost == null) return;

            var mDesc = Factory.ReturnTopicStarterPost(ParentTopic.Id).Content;
            var metaDescription = new HtmlMeta
                                      {
                                          Name = "description",
                                          Content = library.TruncateString(library.StripHtml(mDesc), 300, "...")
                                      };
            Page.Header.Controls.Add(metaDescription);
        }

        /// <summary>
        /// Defines permissions based on the user, when they are logged in or not - Bacically this is where most of the stuff happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SortIndividualPostActions(object sender, RepeaterItemEventArgs e)
        {
            // Get a reference to the forum post in this row
            var post = (ForumPost)e.Item.DataItem;       

            // Get literals to fill with member details
            var litMemberPosts = (Literal) e.Item.FindControl("litMemberPosts");
            var litMemberKarma = (Literal)e.Item.FindControl("litMemberKarma");
            var litMemberName = (Literal)e.Item.FindControl("litMemberName");
            var litMemberGravatar = (Literal) e.Item.FindControl("litMemberGravatar");
            var litLastEdited = (Literal)e.Item.FindControl("litLastEdited");
            var litUserAccreds = (Literal)e.Item.FindControl("litUserAccreds");
            var forumpostkarma = (Panel)e.Item.FindControl("forumpostkarma");

            // Check the member exists who created this post
            // Set all the members details who created the post
            if (post.Owner != null)
            {
                var forumUserPosts = post.Owner.MemberPostAmount.ToString();
                litMemberPosts.Text = string.IsNullOrEmpty(forumUserPosts) ? "0" : forumUserPosts;

                var forumUserKarma = post.Owner.MemberKarmaAmount.ToString();
                litMemberKarma.Text = string.IsNullOrEmpty(forumUserKarma) ? "0" : forumUserKarma;

                litMemberGravatar.Text = string.Format("<img src='{0}' alt='{1}' />"
                                        , Helpers.GetGravatarImage(post.Owner.MemberEmail, 40)
                                        , post.Owner.MemberLoginName);

                // Create the link to the members profile
                litMemberName.Text = MembershipHelper.ReturnMemberProfileLink(post.Owner.MemberLoginName, post.Owner.MemberId, post.Id);

                if (post.Owner.MemberIsAdmin)
                {
                    litUserAccreds.Text = string.Format("<span class='isadmin'>{0}</span>", library.GetDictionaryItem("AdminText"));
                }
            }
            else
            {
                // If the member no longer exists fill with dummy data
                litMemberPosts.Text = "0";
                litMemberKarma.Text = "0";
                litMemberName.Text = "N/A";              
            }

            // If post has been edited show it at bottom of post
            if(!string.IsNullOrEmpty(post.LastEdited.ToString()))
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
                var lvKarma = (LoginView)e.Item.FindControl("lvKarma");
                var forumpostkarmathumbs = (Panel)lvKarma.FindControl("forumpostkarmathumbs");
                var lvSolution = (LoginView)e.Item.FindControl("lvSolution");
                var forumpostisolution = (Panel)lvSolution.FindControl("forumpostisolution");
                var lsolution = (HyperLink)lvSolution.FindControl("lsolution");
                var lSpamReport = (HyperLink)e.Item.FindControl("lSpamReport");
                var btnSubmitPost = (Button)lvNewPost.FindControl("btnSubmitPost");
                var pnlUserAdmin = (Panel)e.Item.FindControl("pnlUserAdmin");
                var lUserEdit = (HyperLink)pnlUserAdmin.FindControl("lUserEdit");
                var lMoveTopic = (HyperLink)e.Item.FindControl("lMoveTopic");
                var quotebutton = (HyperLink)e.Item.FindControl("quotebutton");

                //Add the topic id to the rel of the postbutton so we can use it for ajax post
                btnSubmitPost.Attributes.Add("rel", ParentTopic.Id.ToString());
                 
                // If spam reporting is enabled then add the link
                if(Settings.EnableSpamReporting)
                {
                    lSpamReport.Visible = true;
                    lSpamReport.NavigateUrl = "/spamreport.aspx?p=" + post.Id;
                }

                forumpostisolution.Visible = true;
                // Choose the variables that decide whether or not to show the mark a solution button
                if (!Settings.EnableMarkAsSolution | ParentTopic.IsSolved | ParentTopic.Owner.MemberId != CurrentMember.MemberId | post.Owner.MemberId == CurrentMember.MemberId)
                {
                    // hide 
                    forumpostisolution.Visible = false;
                }
                else
                {
                    // Set the rel attribute so the Ajax works
                    lsolution.Attributes.Add("rel", post.Id.ToString());
                }


                // Add a unique hook to the panel as we are using it
                forumpostkarmathumbs.Visible = true;
                forumpostkarmathumbs.CssClass = "forumpostkarmathumbs karmathumbs" + post.Id;
                
                // See if user owns post
                // If the user is on a different page, store the variable
                var addPage = Request.QueryString["p"] != null ? string.Concat("?p=", Request.QueryString["p"].ToInt32()) : null;
                if(post.Owner.MemberId == CurrentMember.MemberId)
                {
                    // Hide thumbs up / thumbs down
                    forumpostkarmathumbs.Visible = false;

                    pnlUserAdmin.Visible = true;
                    lUserEdit.NavigateUrl = string.Concat(post.Url, addPage);
                }
                else if (CurrentMember.MemberIsAdmin)
                {
                    // show anyway as user is marked as forum admin
                    pnlUserAdmin.Visible = true;
                    lUserEdit.NavigateUrl = string.Concat(post.Url, addPage);
                }

                // This post isn't owned by the current logged in user, So
                // Add some hooks to the thumb up/thumb down so I can call Ajax base
                if (forumpostkarmathumbs.Visible && Settings.EnableKarma)
                {
                    var lthumbuplink = (HyperLink)forumpostkarmathumbs.FindControl("lthumbuplink");
                    lthumbuplink.Attributes.Add("rel", post.Id.ToString());

                    var lthumbdownlink = (HyperLink)forumpostkarmathumbs.FindControl("lthumbdownlink");
                    lthumbdownlink.Attributes.Add("rel", post.Id.ToString());

                    if (post.VotedMembersIds != null && post.VotedMembersIds.Any())
                    {
                        if (post.VotedMembersIds.Contains((int)CurrentMember.MemberId))
                        {
                            forumpostkarmathumbs.Visible = false;
                        }
                    }
                }

                //Check karma level and see if they are allowed to vote yet
                var karmaallowedtovote = (CurrentMember.MemberKarmaAmount >= Settings.KarmaAllowedToVote);
                if(!karmaallowedtovote)
                {
                    forumpostkarmathumbs.Visible = false;
                }

                // Show move topic button
                // Check if user is admin and this is a topic starter post
                if (CurrentMember.MemberIsAdmin && post.IsTopicStarter)
                {
                    // Now show the link
                    lMoveTopic.Visible = true;
                    lMoveTopic.NavigateUrl = "~/forummovetopic.aspx?n=" + post.Id;
                }


                //Show quote button
                string tpager = null;
                if (p != null)
                    tpager = string.Concat("?p=", p);
                quotebutton.Visible = true;
                quotebutton.NavigateUrl = string.Concat(CurrentPageAbsoluteUrl, tpager, "#maineditor");
                quotebutton.Attributes.Add("rel", post.Id.ToString());

            }

            //Check if karma is enabled
            if (!Settings.EnableKarma)
            {
                forumpostkarma.Visible = false;
            }

        }

        /// <summary>
        /// Adds a style to the post so it can be styled as the solution
        /// </summary>
        /// <param name="solved"></param>
        /// <returns></returns>
        protected string IsSolution(bool solved)
        {
            return " solution" + solved;
        }

        /// <summary>
        /// Gets the post karma and returns 0 if null
        /// </summary>
        /// <param name="forumPostKarma"></param>
        /// <returns></returns>
        protected int GetPostKarma(int? forumPostKarma)
        {
            if(forumPostKarma != null)
            {
                return (Int32)forumPostKarma;
            }
            return 0;
        }

    }
}