using nForum.BusinessLogic.Data;
using umbraco.NodeFactory;
using umbraco.interfaces;

namespace nForum.BusinessLogic.Models
{
    public class Forum : ForumBase
    {
        private ForumFactory _factory = new ForumFactory();

        #region Constructor
        
        public Forum()
        {
            var mainForumNode = new Node(_factory.ReturnRootForumId());
            MapForumNodeToModel(mainForumNode);
        }

        #endregion

        #region Methods

        private void MapForumNodeToModel(INode forumRoot)
        {
            Id = forumRoot.Id;
            CreatedOn = forumRoot.CreateDate;
            Url = Helpers.ReturnSiteDomainName();
            Name = forumRoot.GetProperty("forumName").Value;
            PageTitle = string.IsNullOrEmpty(forumRoot.GetProperty("pageTitle").Value) ? Name : forumRoot.GetProperty("pageTitle").Value;
            MetaDescription = forumRoot.GetProperty("metaDescription").Value;
            EmailAdmin = forumRoot.GetProperty("forumAdminEmail").Value;
            EmailNotification = forumRoot.GetProperty("forumNotificationReplyEmail").Value;
            IsClosed = forumRoot.GetProperty("forumClosed").Value == "1";
            EnableKarma = forumRoot.GetProperty("forumEnableKarma").Value == "1";
            EnableAjaxPostSnippets = forumRoot.GetProperty("forumEnableAjaxPostSnippets").Value == "1";
            EnableMemberReporting = forumRoot.GetProperty("forumEnableMemberReporting").Value == "1";
            EnablePrivateMessaging = forumRoot.GetProperty("forumEnablePrivateMessaging").Value == "1";
            PrivateMessagingFloodControlTimeSpan = forumRoot.GetProperty("forumPrivateMessageFloodControl").Value.ToInt32();
            EnableRssFeeds = forumRoot.GetProperty("forumEnableRSSFeeds").Value == "1";
            EnableMarkAsSolution = forumRoot.GetProperty("forumEnableMarkAsSolution").Value == "1";
            EnableSpamReporting = forumRoot.GetProperty("forumEnableSpamReporting").Value == "1";
            EnableTopicEmailSubscriptions = forumRoot.GetProperty("forumEnableTopicEmailSubscriptions").Value == "1";
            ManuallyAuthoriseNewMembers = forumRoot.GetProperty("forumManuallyAuthoriseNewMembers").Value == "1";
            EmailAdminOnNewMemberSignUp = forumRoot.GetProperty("forumEmailAdminOnNewMemberSignUp").Value == "1";
            TopicsPerPage = forumRoot.GetProperty("forumTopicsPerPage").Value.ToInt32();
            PostsPerPage = forumRoot.GetProperty("forumPostsPerPage").Value.ToInt32();
            KarmaPointsAddedPerPost = forumRoot.GetProperty("karmaPointsAddedPerPost").Value.ToInt32();
            KarmaPointsAddedForThumbUps = forumRoot.GetProperty("karmaPointsAddedForThumbsUp").Value.ToInt32();
            KarmaAllowedToVote = forumRoot.GetProperty("forumKarmaAllowedToVoteAmount").Value.ToInt32();
        }

        #endregion


        #region Properties

        #region Forum Settings

        /// <summary>
        /// The forum admin email address
        /// </summary>
        public string EmailAdmin { get; set; }

        /// <summary>
        /// The notification email, usually a noreply@
        /// </summary>
        public string EmailNotification { get; set; }

        /// <summary>
        /// Returns whether the forum is closed or not
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Enable Karma voting on the forum
        /// </summary>
        public bool EnableKarma { get; set; }

        /// <summary>
        /// Allow Ajax snippets when users hover over topics
        /// </summary>
        public bool EnableAjaxPostSnippets { get; set; }

        /// <summary>
        /// Allow members to report another members profile
        /// </summary>
        public bool EnableMemberReporting { get; set; }

        /// <summary>
        /// Allow private messaging between members
        /// </summary>
        public bool EnablePrivateMessaging { get; set; }

        /// <summary>
        /// The time span limit between how often a user can send a message to another member
        /// </summary>
        public int PrivateMessagingFloodControlTimeSpan { get; set; }

        /// <summary>
        /// Enable RSS feeds for topics on the forum
        /// </summary>
        public bool EnableRssFeeds { get; set; }

        /// <summary>
        /// Allow members to mark posts as the answer to their post/topics
        /// </summary>
        public bool EnableMarkAsSolution { get; set; }

        /// <summary>
        /// Allow members to report posts as potential spam
        /// </summary>
        public bool EnableSpamReporting { get; set; }

        /// <summary>
        /// Allow members to subscribe to topics and get email notifications when new posts
        /// </summary>
        public bool EnableTopicEmailSubscriptions { get; set; }

        /// <summary>
        /// Members can sign up, but won't be allowed to use the forum until authorised by admin
        /// </summary>
        public bool ManuallyAuthoriseNewMembers { get; set; }

        /// <summary>
        /// Email the forum admin everytime a new member signs up
        /// </summary>
        public bool EmailAdminOnNewMemberSignUp { get; set; }

        #endregion


        #region Forum Posts & Topic Settings

        /// <summary>
        /// The amount of topics to show per page
        /// </summary>
        public int TopicsPerPage { get; set; }

        /// <summary>
        /// The amount of posts to show per page
        /// </summary>
        public int PostsPerPage { get; set; }

        #endregion


        #region Karma

        /// <summary>
        /// The amount of karma points a member gets for posting
        /// </summary>
        public int KarmaPointsAddedPerPost { get; set; }

        /// <summary>
        /// The amount of Karma a member gets when another member thumbs up their post
        /// </summary>
        public int KarmaPointsAddedForThumbUps { get; set; }

        /// <summary>
        /// The amount of karma needed before a member can thumb up or thumb down other posts
        /// </summary>
        public int KarmaAllowedToVote { get; set; }

        #endregion

        #endregion

    }
}
