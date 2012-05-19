using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;

namespace nForum.usercontrols.nForum
{
    public partial class ForumTopics : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                ShowCanonicalTag();
                ShowCreateTopicButton();
                GetTopicsFromCategory();
                ShowRssLink();
            }
        }

        private void ShowCanonicalTag()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["p"])) return;

            // this means there is a pageing variable, so return the canonical tag
            var cLink = new HtmlLink();
            cLink.Attributes.Add("rel", "canonical");
            cLink.Href = CurrentPageAbsoluteUrl;
            Page.Header.Controls.Add(cLink);
        }

        /// <summary>
        /// Choose whether to show the RSS feed
        /// </summary>
        private void ShowRssLink()
        {
            if (!Helpers.MainForumSettings().EnableRssFeeds) return;

            var rssurl = library.NiceUrl(CurrentNode.Id);                
            lRss.NavigateUrl = Helpers.AlternateTemplateUrlFix("/topicrss", rssurl);
            lRss.Visible = true;
            lRss.ImageUrl = "/nforum/img/rss.png";
            lRss.CssClass = "topicrssicon";
        }

        /// <summary>
        /// Displays the create new topic button if user is logged in
        /// </summary>
        private void ShowCreateTopicButton()
        {
            if (MembershipHelper.IsAuthenticated() && !IsBanned)
            {
                var url = library.NiceUrl(CurrentNode.Id);
                hlCreateTopic.Visible = true;
                hlCreateTopic.NavigateUrl = Helpers.AlternateTemplateUrlFix("/createtopic", url);
            }
        }

        /// <summary>
        /// Gets all the topics from the parent category
        /// </summary>
        private void GetTopicsFromCategory()
        {
            // Before anything see if the category has been marked as private, if so make sure user is loggged in
            var currentCategory = Mapper.MapForumCategory(CurrentNode);
            var userHasAccess = true;
            if (MembershipHelper.IsAuthenticated())
            {
                // Member is logged in so it doesn't matter if forum is private or not, 
                // now check they have enough karma to view this category and hide if not
                if (CurrentMember.MemberKarmaAmount < currentCategory.KarmaAccessAmount)
                {
                    userHasAccess = false;
                }

                // Double check they have enough karma to post a topic
                if (CurrentMember.MemberKarmaAmount < currentCategory.KarmaPostAmount)
                {
                    hlCreateTopic.Visible = false;
                }
            }
            else
            {
                if (currentCategory.IsPrivate)
                {
                    userHasAccess = false;
                }
            }

            // Check to see if user has access
            if(!userHasAccess)
            {
                Response.Redirect(string.Concat(Settings.Url, "?m=", library.GetDictionaryItem("NoPermissionToViewPage")));
            }


            // First get title and description
            litHeading.Text = currentCategory.Name;
            var fDescription = currentCategory.Description;
            litDescription.Text = fDescription;
            // Set the meta description
            var metaDescription = new HtmlMeta
            {
                Name = "description",
                Content = library.StripHtml(fDescription)
            };
            Page.Header.Controls.Add(metaDescription);

            // Get the paging variable
            int? p = null;
            if (Request.QueryString["p"] != null)
                p = Convert.ToInt32(Request.QueryString["p"]);

            // Set cache variables
            var useNodeFactory = Request.QueryString["nf"] != null;
            var maintopics = from t in Factory.ReturnAllTopicsInCategory(CurrentNode.Id, true, useNodeFactory)
                             where !t.IsSticky
                             select t;

            // Pass to my pager helper
            var pagedResults = new PaginatedList<ForumTopic>(maintopics, p ?? 0, Convert.ToInt32(Settings.TopicsPerPage));

            // Decide whether to show pager or not
            if (pagedResults.TotalPages > 1)
            {
                litPager.Text = pagedResults.ReturnPager();
            }

            // Now bind
            rptTopicList.DataSource = pagedResults;
            rptTopicList.DataBind();
        }

    }
}