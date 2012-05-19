using System;
using System.Collections.Generic;
using System.Linq;
using nForum.BusinessLogic.Data;
using umbraco.NodeFactory;

namespace nForum.BusinessLogic.Models
{
    public class ForumTopic : ForumBase
    {
        private ForumFactory _factory = new ForumFactory();
        private NodeMapper _mapper = new NodeMapper();

        #region Properties
        
        /// <summary>
        /// The category ID, the category id is not the same as parent id due to the date folders
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Member who started this topic
        /// </summary>
        public ForumMember Owner { get; set; }

        /// <summary>
        /// Whether this topic is closed or not
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Whether this topic is marked as solved
        /// </summary>
        public bool IsSolved { get; set; }

        /// <summary>
        /// Whether this is a sticky topic or not
        /// </summary>
        public bool IsSticky { get; set; }

        /// <summary>
        /// List of member ids who have subscribed to email notifications
        /// </summary>
        public List<int> SubscriberIds { get; set; }

        /// <summary>
        /// Gets the date of the last post in this topic
        /// </summary>
        public DateTime? LastPostDate { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Returns list of members who have subscribed to email notifications
        /// </summary>
        public IEnumerable<ForumMember> Subscribers()
        {
            return SubscriberIds.Select(id => MembershipHelper.ReturnMember(id));
        }

        /// <summary>
        /// Returns a list of all posts in this topic
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumPost> Posts()
        {
            return _factory.ReturnAllPostsInTopic(this.Id);
        }

        /// <summary>
        /// Returns the amount of posts in the topic
        /// </summary>
        /// <returns></returns>
        public int PostCount()
        {
            return _factory.ReturnPostCountInTopic(this.Id);
        }

        /// <summary>
        /// Returns the total amount of all post karma added up in this topic
        /// </summary>
        /// <returns></returns>
        public int PostVotesTotal()
        {
            return _factory.ReturnSumOfVotesInTopic(this.Id);
        }

        /// <summary>
        /// Get the latest post in this topic
        /// </summary>
        /// <returns></returns>
        public ForumPost GetLatestPost()
        {
            return _factory.ReturnLatestPostInTopic(this.Id);
        }

        /// <summary>
        /// Returns the parent category of this topic
        /// </summary>
        /// <returns></returns>
        public ForumCategory ParentCategory()
        {
            return _mapper.MapForumCategory(new Node(CategoryId));
        }
        #endregion

    }
}
