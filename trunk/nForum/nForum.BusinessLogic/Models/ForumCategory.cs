using System.Collections.Generic;
using nForum.BusinessLogic.Data;
using umbraco.NodeFactory;

namespace nForum.BusinessLogic.Models
{
    public class ForumCategory : ForumBase
    {
        private ForumFactory _factory = new ForumFactory();
        private NodeMapper _mapper = new NodeMapper();

        #region Properties

        /// <summary>
        /// Forum category description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns whether this category is a main category or not
        /// </summary>
        public bool IsMainCategory { get; set; }

        /// <summary>
        /// Returns whether this category is private or not (Members not logged in won't be able to see it)
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// The amount of karma a member needs before they can access this category
        /// </summary>
        public int KarmaAccessAmount { get; set; }

        /// <summary>
        /// The amount of karma needed before they are allowed to post in this category
        /// </summary>
        public int KarmaPostAmount { get; set; }

        /// <summary>
        /// Parent category id, used because examine is having issues with parentid
        /// </summary>
        public int ParentCategoryId { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Get all the topics in the Category
        /// </summary>
        /// <returns>List of ForumTopics</returns>
        public IEnumerable<ForumTopic> SubTopics()
        {
            return _factory.ReturnAllTopicsInCategory(this.Id);
        }

        /// <summary>
        /// Returns the count of topics in this category
        /// </summary>
        /// <returns></returns>
        public int SubTopicsCount()
        {
            return _factory.ReturnAllTopicsInCategoryCount(this.Id);
        }

        /// <summary>
        /// Returns the most recent active topic (Most recent to be posted in)
        /// </summary>
        /// <returns></returns>
        public ForumTopic LatestActiveTopic()
        {
            return _factory.ReturnMostRecentActiveTopicInCategory(this.Id);
        }

        /// <summary>
        /// Returns the latest post within any topic within this category
        /// </summary>
        /// <returns></returns>
        public ForumPost LatestForumPost()
        {
            var topic = LatestActiveTopic();
            return topic != null ? topic.GetLatestPost() : null;
        }

        /// <summary>
        /// Get all sub categories of this category
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumCategory> SubCategories()
        {
            return _factory.ReturnAllSubCategoriesInCategory(this.Id);
        }

        /// <summary>
        /// Get the parent category of this category
        /// </summary>
        /// <returns></returns>
        public ForumCategory GetParentCategory()
        {
            return ParentId != null ? _mapper.MapForumCategory(new Node((int)ParentId)) : null;
        }

        #endregion

    }
}
