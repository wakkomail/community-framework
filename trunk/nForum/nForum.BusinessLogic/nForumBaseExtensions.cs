using System;
using System.Linq;
using System.Text;
using System.Web;
using nForum.BusinessLogic.Data;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.umbracobase;

namespace nForum.BusinessLogic
{
    [RestExtension("Solution")]
    public class nForumBaseExtensions
    {
        private static NodeMapper _mapper = new NodeMapper();
        private static ForumFactory _factory = new ForumFactory();

        #region Mark Post As Solution
        /// <summary>
        /// Mark the post as the solution and the topic as solved
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string MarkAsSolution(string pageId)
        {
            if (MembershipHelper.IsAuthenticated())
            {
                var m = Member.GetCurrentMember();
                var forumPost = _mapper.MapForumPost(new Node(Convert.ToInt32(pageId)));
                if (forumPost != null)
                {
                    var forumTopic = _mapper.MapForumTopic(new Node(forumPost.ParentId.ToInt32()));
                    // If this current member id doesn't own the topic then ignore, also 
                    // if the topic is already solved then ignore.
                    if (m.Id == forumTopic.Owner.MemberId && !forumTopic.IsSolved)
                    {
                        // Get a user to save both documents with
                        var usr = new User(0);

                        // First mark the post as the solution
                        var p = new Document(forumPost.Id);
                        p.getProperty("forumPostIsSolution").Value = 1;
                        p.Publish(usr);
                        library.UpdateDocumentCache(p.Id);

                        // Now update the topic
                        var t = new Document(forumTopic.Id);
                        t.getProperty("forumTopicSolved").Value = 1;
                        t.Publish(usr);
                        library.UpdateDocumentCache(t.Id);

                        return library.GetDictionaryItem("Updated");
                    } 
                }
            }
            return library.GetDictionaryItem("Error");
        } 
        #endregion

        #region Thumbs Up A Post
        /// <summary>
        /// Adds karma to the post if the user adds thumbs up
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string ThumbsUpPost(string postId)
        {
            if (MembershipHelper.IsAuthenticated())
            {
                var m = Member.GetCurrentMember();
                var forumPost = _mapper.MapForumPost(new Node(Convert.ToInt32(postId)));

                // If this current member id owns the post then ignore
                if (forumPost != null)
                {
                    if (m.Id != forumPost.Owner.MemberId)
                    {
                        // Get the member who wrote the post
                        var postMember = new Member(Convert.ToInt32(forumPost.Owner.MemberId));

                        // Get a user to save both documents with
                        var usr = new User(0);

                        // First update the karma on the post and add this logged in user to
                        // list if people who have voted on post
                        var p = new Document(forumPost.Id);
                        var votedUsers = p.getProperty("forumPostUsersVoted").Value.ToString();
                        var formattedMemberId = string.Format("{0}|", m.Id);

                        // Check to make sure they are not fiddling the system
                        if (forumPost.VotedMembersIds == null || !forumPost.VotedMembersIds.Contains(m.Id))
                        {
                            p.getProperty("forumPostKarma").Value = (forumPost.Karma + 1);
                            p.getProperty("forumPostUsersVoted").Value = formattedMemberId + votedUsers;
                            p.Publish(usr);
                            umbraco.library.UpdateDocumentCache(p.Id);
                            var newPostKarma = (forumPost.Karma + 1);

                            // Now update the members karma based on the forum settings
                            forumPost.Karma = Convert.ToInt32(postMember.getProperty("forumUserKarma").Value.ToString());
                            postMember.getProperty("forumUserKarma").Value = (forumPost.Karma + Helpers.MainForumSettings().KarmaPointsAddedForThumbUps);

                            // Save Member details
                            postMember.Save();

                            //Generate member Xml Cache
                            postMember.XmlGenerate(new System.Xml.XmlDocument());

                            return newPostKarma.ToString();
                        }
                    }
                }
            }

            return "0";
        } 
        #endregion

        #region Thumbs Down A Post
        /// <summary>
        /// Marks down the post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string ThumbsDownPost(string postId)
        {
            if (MembershipHelper.IsAuthenticated())
            {
                var m = Member.GetCurrentMember();
                var forumPost = _mapper.MapForumPost(new Node(Convert.ToInt32(postId)));

                // If this current member id owns the post then ignore
                if (forumPost != null)
                {
                    if (m.Id != forumPost.Owner.MemberId)
                    {
                        // Get the member who wrote the post
                        //var postMember = new Member(Convert.ToInt32(ipage.ForumPostOwnedBy));

                        // Get a user to save both documents with
                        var usr = new User(0);

                        // First update the karma on the post and add this logged in user to
                        // list if people who have voted on post
                        var p = new Document(forumPost.Id);
                        var formattedMemberId = string.Format("{0}|", m.Id);
                        var votedUsers = p.getProperty("forumPostUsersVoted").Value.ToString();

                        // Check to make sure they are not fiddling the system
                        if (forumPost.VotedMembersIds == null || !forumPost.VotedMembersIds.Contains(m.Id))
                        {
                            p.getProperty("forumPostKarma").Value = (forumPost.Karma - 1);
                            p.getProperty("forumPostUsersVoted").Value = formattedMemberId + votedUsers;
                            p.Publish(usr);
                            library.UpdateDocumentCache(p.Id);
                            var newPostKarma = (forumPost.Karma - 1);

                            return newPostKarma.ToString();
                        }
                    }
                }
            }

            return "0";
        } 
        #endregion

        #region Create a New Forum Post
        /// <summary>
        /// Create a new forum post
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string NewForumPost(string topicId)
        {
            // Store the return url
            string returnurl;

            // Get current page                
            var page = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('?')[0] + "?";

            // Get the url
            var url = Helpers.ReturnSiteDomainName();

            // Remove the url from the redirect page
            page = page.Replace(url, "");

            // Get the message variable ready
            string usermessage;

            if (MembershipHelper.IsAuthenticated())
            {
                //Get the posted data
                var post = HttpContext.Current.Request.Form;

                //Get the postbody
                var tbPost = post["postcontent"];

                // Get the logged in user
                var loggedInMember = Member.GetCurrentMember();

                //###################

                if (!String.IsNullOrEmpty(Helpers.SafePlainText(topicId).Trim()))
                {

                    var parentTopicId = topicId.ToInt32();

                    // If empty tell them to not be stupid
                    if (string.IsNullOrWhiteSpace(tbPost))
                    {
                        usermessage = library.GetDictionaryItem("AddContentToPost");
                        returnurl = string.Concat(page, "m=", usermessage.UrlEncode());
                        return returnurl;
                    }

                    // Create a post title for use in the admin only
                    var title = string.Concat(loggedInMember.Text, " - ", DateTime.Now.ToShortDateString(), " - ", DateTime.Now.ToShortTimeString());

                    // Create the new post
                    var u = new User(0);
                    var d = Document.MakeNew(title, DocumentType.GetByAlias("CLC-Post"), u, parentTopicId);

                    // Add the document properties
                    d.getProperty("forumPostContent").Value = Helpers.HtmlEncode(Helpers.GetSafeHtml(tbPost));
                    d.getProperty("forumPostOwnedBy").Value = loggedInMember.Id;
                    d.getProperty("forumPostKarma").Value = 0;
                    d.getProperty("forumPostParentID").Value = parentTopicId;
                    d.getProperty("forumPostIsTopicStarter").Value = 0;
                    d.sortOrder = 0;

                    // publish application node
                    d.Publish(u);

                    // update the document cache so its available in the XML
                    library.UpdateDocumentCache(d.Id);

                    // Little bit of a hack to update the topic with the new latest post date, helps 
                    // with performance elsewhere
                    _factory.UpdateTopicWithLastPostDate(parentTopicId);

                    // Finally before redirect, add the karma from settings to user who posted and update amount of posts
                    var currentKarma = loggedInMember.getProperty("forumUserKarma").Value.ToString();
                    var forumUserPosts = loggedInMember.getProperty("forumUserPosts").Value.ToString();

                    // Check for null                    
                    currentKarma = string.IsNullOrEmpty(currentKarma) ? "1" : (currentKarma.ToInt32() + Helpers.MainForumSettings().KarmaPointsAddedPerPost).ToString();

                    // Assign new value
                    loggedInMember.getProperty("forumUserKarma").Value = currentKarma;
                    loggedInMember.getProperty("forumUserPosts").Value = (forumUserPosts.ToInt32() + 1);

                    // Save Member details
                    loggedInMember.Save();

                    //Generate member Xml Cache
                    loggedInMember.XmlGenerate(new System.Xml.XmlDocument());

                    //Check if user was on a specific page and add to page variable
                    var pager = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query);
                    if (pager["p"] != null)
                    {
                        page = string.Concat(page, "p=", pager["p"].ToInt32(), "&amp;");
                    }

                    //Send notifications if enabled);
                    if (Helpers.MainForumSettings().EnableTopicEmailSubscriptions)
                    {
                        //Get the full list of subscribers
                        var forumTopic = _mapper.MapForumTopic(new Node(parentTopicId));

                        //get the post to use in the emails and the topic
                        var forumPost = _mapper.MapForumPost(new Node(d.Id));

                        // get the subscriber ids just in case we need to remove the current person from it
                        var subscriberIds = forumTopic.SubscriberIds;

                        // Don't email the member their own posts in the notifications
                        if (loggedInMember.Id == forumPost.Owner.MemberId)
                        {
                            subscriberIds.Remove(loggedInMember.Id);
                        }

                        if (subscriberIds.Count > 0)
                        {
                            //Get email ready
                            var sb = new StringBuilder();
                            sb.AppendFormat(library.GetDictionaryItem("NewPostIn"), forumTopic.Name);
                            sb.Append(forumPost.Content.ConvertBbCode());
                            sb.AppendFormat(library.GetDictionaryItem("DirectLink"), HttpContext.Current.Request.UrlReferrer);

                            // Add emails to array
                            var memberemails = subscriberIds.Select(id => new Member(id)).Select(m => m.Email).ToList();
                            
                            // Send emails
                            Helpers.SendMail(Helpers.MainForumSettings().EmailNotification, 
                                             memberemails,
                                             string.Concat(Helpers.MainForumSettings().Name, library.GetDictionaryItem("TopicNotificationEmailSubject")), 
                                             sb.ToString());                         
                        }

                    }

                    // Finally redirect and add message
                    usermessage = library.GetDictionaryItem("PostAdded");
                    returnurl = string.Concat(page, "nf=true&amp;m=", usermessage.UrlEncode());
                    return returnurl;
                }
            }

            usermessage = library.GetDictionaryItem("InvalidTopicId");
            returnurl = string.Concat(page, "m=", usermessage.UrlEncode());
            return returnurl;

            //###################
        } 
        #endregion

        #region Create A New Forum Topic
        /// <summary>
        /// Create new forum topic
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string NewForumTopic(string catId)
        {

            // Store the return url
            string returnurl;

            // Get current page                
            var page = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('?')[0] + "?";

            // Get the url
            var url = Helpers.ReturnSiteDomainName();

            // Remove the url from the redirect page
            page = page.Replace(url, "");

            // Get the message variable ready
            string usermessage;

            if (MembershipHelper.IsAuthenticated())
            {
                //Get the posted data
                var post = HttpContext.Current.Request.Form;

                //Get the postbody
                var tbPost = post["postcontent"];
                var tbTitle = post["posttitle"];
                var cbSticky = post["poststicky"];
                var cbLocked = post["postlocked"];

                // Get the logged in user
                var loggedInMember = Member.GetCurrentMember();

                // ==================

                if (!String.IsNullOrEmpty(Helpers.SafePlainText(catId).Trim()))
                {
                    if (!string.IsNullOrWhiteSpace(tbPost) && !string.IsNullOrWhiteSpace(tbTitle))
                    {
                        // Create a user we can use for both
                        var u = new User(0);

                        // Store topic title as we'll use it for this first posts title, reason we do this is so that when
                        // We are searching with Examine we can search the nodeName and it will pick it up
                        var topicTitle = Helpers.SafePlainText(tbTitle);

                        // Create the topic
                        var t = Document.MakeNew(topicTitle, DocumentType.GetByAlias("CLC-Discussion"), u, catId.ToInt32());

                        // Format the member id ready to add in the subscribers list
                        var formattedMemberid = string.Format("{0}|", loggedInMember.Id);

                        // Add the document properties
                        t.getProperty("forumTopicOwnedBy").Value = loggedInMember.Id;
                        t.getProperty("forumTopicLastPostDate").Value = DateTime.Now;
                        t.getProperty("forumTopicParentCategoryID").Value = catId.ToInt32();
                        t.getProperty("forumTopicSubscribedList").Value = formattedMemberid;

                        if(cbSticky == "true")
                        {
                            t.getProperty("forumTopicIsSticky").Value = 1;
                        }

                        if(cbLocked == "true")
                        {
                            t.getProperty("forumTopicClosed").Value = 1;
                        }

                        t.sortOrder = 0;

                        // publish application node
                        t.Publish(u);
                        // update the document cache so its available in the XML
                        library.UpdateDocumentCache(t.Id);

                        /*### Now create the first post in the topic ###*/

                        // Create the post
                        var p = Document.MakeNew(topicTitle, DocumentType.GetByAlias("CLC-Post"), u, t.Id);

                        // Add the document properties
                        p.getProperty("forumPostContent").Value = Helpers.HtmlEncode(Helpers.GetSafeHtml(tbPost));
                        p.getProperty("forumPostOwnedBy").Value = loggedInMember.Id;
                        p.getProperty("forumPostKarma").Value = 0;
                        p.getProperty("forumPostIsTopicStarter").Value = 1;
                        p.getProperty("forumPostParentID").Value = t.Id;
                        p.sortOrder = 0;

                        // Now the post is created, use the id for the latest post Id
                        //t.getProperty("forumTopicLastPostID").Value = p.Id;

                        // publish application node
                        p.Publish(u);
                        // update the document cache so its available in the XML
                        library.UpdateDocumentCache(p.Id);

                        // Finally sort karma out
                        // Finally before redirect, add the karma from settings to user who posted and update amount of posts
                        var currentKarma = loggedInMember.getProperty("forumUserKarma").Value.ToString();
                        var forumUserPosts = loggedInMember.getProperty("forumUserPosts").Value.ToString();

                        // Check for null
                        currentKarma = string.IsNullOrEmpty(currentKarma) ? "1" : (currentKarma.ToInt32() + Helpers.MainForumSettings().KarmaPointsAddedPerPost).ToString();

                        // Assign new value
                        loggedInMember.getProperty("forumUserKarma").Value = currentKarma;
                        loggedInMember.getProperty("forumUserPosts").Value = (forumUserPosts.ToInt32() + 1);
                        loggedInMember.sortOrder = 0;

                        // Save Member details
                        loggedInMember.Save();

                        //Generate member Xml Cache
                        loggedInMember.XmlGenerate(new System.Xml.XmlDocument());

                        // Finally redirect and add message
                        page = string.Concat(library.NiceUrl(t.Id), "?");
                        usermessage = library.GetDictionaryItem("TopicCreated");
                        returnurl = string.Concat(page, "nf=true&amp;m=", usermessage.UrlEncode());
                        return returnurl;
                    }

                    // Bah.. Show an error as no data in the fields
                    usermessage = library.GetDictionaryItem("CompleteBothFields");
                    returnurl = string.Concat(page, "m=", usermessage.UrlEncode());
                    return returnurl;
                }
            }

            // ==================

            usermessage = library.GetDictionaryItem("InvalidCategoryId");
            returnurl = string.Concat(page, "m=", usermessage.UrlEncode());
            return returnurl;
        } 
        #endregion

        #region Subscribe/Unsubscribe to Topic
        /// <summary>
        /// Subscribe to a topic
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string SubScribeToTopic(string topicId)
        {
            if (!String.IsNullOrEmpty(Helpers.SafePlainText(topicId).Trim()) && MembershipHelper.IsAuthenticated())
            {
                // Create the token user
                var u = new User(0);

                // Get the current logged in member
                var m = Member.GetCurrentMember();

                // Get the formated member id we will use to remove from list
                var formattedMemberid = string.Format("{0}|", m.Id);

                // Get the topic
                var t = new Document(topicId.ToInt32());

                //Get the full list of subscribers
                var subscriberlist = t.getProperty("forumTopicSubscribedList").Value.ToString();

                // Add the document properties
                t.getProperty("forumTopicSubscribedList").Value = subscriberlist + formattedMemberid;

                // publish application node
                t.Publish(u);

                // update the document cache so its available in the XML
                library.UpdateDocumentCache(t.Id);

                return library.GetDictionaryItem("Success");
            }
            return library.GetDictionaryItem("Error");
        }

        /// <summary>
        /// Unsubscribe to a topic
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [RestExtensionMethod]
        public static string UnSubScribeToTopic(string topicId)
        {
            if (!String.IsNullOrEmpty(Helpers.SafePlainText(topicId).Trim()) && MembershipHelper.IsAuthenticated())
            {
                var tID = topicId.ToInt32();

                // Get the topic from factory, as we'll use this to remove the member from the topics
                // to make sure there are no mistakes
                var forumTopic = _mapper.MapForumTopic(new Node(tID));

                // Create the token user
                var u = new User(0);

                // Get the current logged in member
                var m = Member.GetCurrentMember();

                // Remove this member ID from the list
                var remainingMembers = forumTopic.SubscriberIds.Where(x => x != m.Id).ToList();
                var remainingMembersFormatted = string.Empty;
                foreach (var id in remainingMembers)
                {
                    remainingMembersFormatted += (id + "|");
                }

                // Get the topic
                var t = new Document(tID);

                // Add the document properties
                t.getProperty("forumTopicSubscribedList").Value = remainingMembersFormatted;

                // publish application node
                t.Publish(u);

                // update the document cache so its available in the XML
                library.UpdateDocumentCache(tID);

                return library.GetDictionaryItem("Success");
            }
            return library.GetDictionaryItem("Error");
        } 
        #endregion

    }
}
