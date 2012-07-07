using System.Collections.Generic;
using System.Linq;
using Examine;
using nForum.BusinessLogic.Models;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;

namespace nForum.BusinessLogic.Data
{
    public class ForumFactory
    {
        /// <summary>
        /// Constants
        /// </summary>
        //public const string ExamineSearcher = "nForumEntrySearcher";
        //public const string NodeTypeTopic = "ForumTopic";
        //public const string NodeTypePost = "ForumPost";
        //public const string NodeTypeCategory = "ForumCategory";
        //public const string NodeTypeForum = "Forum";

        public const string ExamineSearcher = "nForumEntrySearcher";
        public const string NodeTypeTopic = "CLC-Discussion";
        public const string NodeTypePost = "CLC-Post";
        public const string NodeTypeCategory = "CLC-Membergroup";
        public const string NodeTypeForum = "CLC-Homepage";
        public const bool UseNodeFactoryForAllQueries = false;
        
        // Store the root forum node for use in nodefactory lookups
        //public readonly int _rootForumNode = Helpers.MainForumSettings().Id;

        /// <summary>
        /// Global objects
        /// </summary>
        public NodeMapper Mapper = new NodeMapper();        
        //var query = criteria.Id(parentnode).And().Field("bodyText", "is awesome".Escape()).Or().Field("bodyText", "rock".Fuzzy());

        //Constructor
        public readonly int _rootForumId;
        public ForumFactory()
        {
            _rootForumId = ReturnRootForumId();
        }

        #region Forum

        /// <summary>
        /// Returns the root forum Id for use in the settings section
        /// </summary>
        /// <returns></returns>
        public int ReturnRootForumId()
        {
            try
            {
                var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
                var query = criteria.NodeTypeAlias(NodeTypeForum);
                var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
                return results.FirstOrDefault().Id;
            }
            catch
            {
                // This is shite, but its to stop the package installer from dying
                return -1;
            }
        }

        #endregion

        #region Category Methods

        /// <summary>
        /// Returns a list of all sub categories within a parent category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <param name="useNodeFactory">Use nodfactory instead of Examine</param>
        /// <returns></returns>
        public IEnumerable<ForumCategory> ReturnAllSubCategoriesInCategory(int categoryNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllSubCategoriesInCategory_NodeFactory(categoryNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumCategoryParentID", categoryNodeId.ToString())
                        .And().NodeTypeAlias(NodeTypeCategory);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());             
            return results.Select(searchResult => Mapper.MapForumCategory(searchResult)).OrderBy(x => x.SortOrder);
        }

        /// <summary>
        /// Returns a list of all sub categories within a parent category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <returns></returns>
        public IEnumerable<ForumCategory> ReturnAllSubCategoriesInCategory_NodeFactory(int categoryNodeId)
        {
            var subCategories = new Node(categoryNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypeCategory).OrderBy(x => x.SortOrder);
            return subCategories.Select(searchResult => Mapper.MapForumCategory(searchResult));
        }

        /// <summary>
        /// Returns all the current categories on the site
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumCategory> ReturnAllCategories(bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllCategories_NodeFactory();

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.NodeTypeAlias(NodeTypeCategory);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumCategory(searchResult)).OrderBy(x => x.SortOrder);
        }


        /// <summary>
        /// Returns all the current categories on the site
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumCategory>ReturnAllCategories_NodeFactory()
        {
            var cats = Helpers.FindChildren(new Node(_rootForumId), t => t.NodeTypeAlias.Equals(NodeTypeCategory)).OrderBy(x => x.SortOrder);
            return cats.Select(searchResult => Mapper.MapForumCategory(searchResult));
        }

        #endregion

        #region Topic Methods

        /// <summary>
        /// Returns a list of topics within a category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <param name="sortByLastPostDate"> </param>
        /// <param name="useNodeFactory"> </param>
        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnAllTopicsInCategory(int categoryNodeId, bool sortByLastPostDate = false, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllTopicsInCategory_NodeFactory(categoryNodeId, sortByLastPostDate);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumTopicParentCategoryID", categoryNodeId.ToString())
                        .And().NodeTypeAlias(NodeTypeTopic);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumTopic(searchResult)).OrderByDescending(x => sortByLastPostDate ? x.LastPostDate : x.CreatedOn);
        }

        /// <summary>
        /// Returns a list of topics within a category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <param name="sortByLastPostDate"></param>
        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnAllTopicsInCategory_NodeFactory(int categoryNodeId, bool sortByLastPostDate = false)
        {
            var cats = Helpers.FindChildren(new Node(categoryNodeId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypeTopic) &&
                                            t.GetProperty("forumTopicParentCategoryID").Value == categoryNodeId.ToString())
                                            .OrderByDescending(x => x.CreateDate);
            if(sortByLastPostDate)
            {
                cats = cats.OrderByDescending(x => x.GetProperty("forumTopicLastPostDate").Value.ToDateTime());
            }
            return cats.Select(searchResult => Mapper.MapForumTopic(searchResult));
        }

        /// <summary>
        /// Returns the count of all topics within this category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <returns></returns>
        public int ReturnAllTopicsInCategoryCount(int categoryNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllTopicsInCategoryCount_NodeFactory(categoryNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumTopicParentCategoryID", categoryNodeId.ToString())
                        .And().NodeTypeAlias(NodeTypeTopic);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile()).Count();
            return results;
        }

        /// <summary>
        /// Returns the count of all topics within this category
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <returns></returns>
        public int ReturnAllTopicsInCategoryCount_NodeFactory(int categoryNodeId)
        {
            var cats = Helpers.FindChildren(new Node(categoryNodeId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypeTopic) &&
                                            t.GetProperty("forumTopicParentCategoryID").Value == categoryNodeId.ToString()).Count;

            return cats;
        }
        
        /// <summary>
        /// Returns all forum topics
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnAllTopics(bool sortByLastPostDate = false, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllTopics_NodeFactory(sortByLastPostDate);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.NodeTypeAlias(NodeTypeTopic);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumTopic(searchResult)).OrderByDescending(x => sortByLastPostDate ? x.LastPostDate : x.CreatedOn);
        }

        /// <summary>
        /// Returns all forum topics
        /// </summary>
        /// <param name="sortByLastPostDate"></param>
        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnAllTopics_NodeFactory(bool sortByLastPostDate)
        {
            var cats = Helpers.FindChildren(new Node(_rootForumId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypeTopic))
                                            .OrderByDescending(x => x.CreateDate);
            if (sortByLastPostDate)
            {
                cats = cats.OrderByDescending(x => x.GetProperty("forumTopicLastPostDate").Value.ToDateTime());
            }
            return cats.Select(searchResult => Mapper.MapForumTopic(searchResult));
        }

        /// <summary>
        /// Returns a specified amount of active topics
        /// </summary>
        /// <param name="amountToTake"></param>
        /// <param name="sortByLastPostDate"></param>
        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnActiveTopics(int amountToTake, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries)
                return ReturnActiveTopics_NodeFactory(amountToTake);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.NodeTypeAlias(NodeTypeTopic);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumTopic(searchResult)).OrderByDescending(x => x.LastPostDate).Take(amountToTake);
        }

        /// <summary>
        /// Returns a specified amount of active topics
        /// </summary>
        /// <param name="amountToTake"></param>

        /// <returns></returns>
        public IEnumerable<ForumTopic> ReturnActiveTopics_NodeFactory(int amountToTake)
        {
            var cats = Helpers.FindChildren(new Node(_rootForumId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypeTopic))
                                            .OrderByDescending(x => x.GetProperty("forumTopicLastPostDate").Value.ToDateTime());
            return cats.Take(amountToTake).Select(searchResult => Mapper.MapForumTopic(searchResult));
        }

        /// <summary>
        /// Returns the most recent active topic in a category (Most recent to have a post in)
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <returns></returns>
        public ForumTopic ReturnMostRecentActiveTopicInCategory(int categoryNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries)
                return ReturnMostRecentActiveTopicInCategory_NodeFactory(categoryNodeId);

            return ReturnAllTopicsInCategory(categoryNodeId, true).FirstOrDefault();
        }

        /// <summary>
        /// Returns the most recent active topic in a category (Most recent to have a post in)
        /// </summary>
        /// <param name="categoryNodeId"></param>
        /// <returns></returns>
        public ForumTopic ReturnMostRecentActiveTopicInCategory_NodeFactory(int categoryNodeId)
        {
            return ReturnAllTopicsInCategory_NodeFactory(categoryNodeId, true).FirstOrDefault();
        }

        /// <summary>
        /// This updates a topic with the current latest post date
        /// </summary>
        /// <param name="topicId"></param>
        public void UpdateTopicWithLastPostDate(int topicId)
        {
            // This is a bit shite, but its the best way to keep performance on some of the larger select queries.
            // Especially if you have 10000's of nodes
            // Get the topic to update and get generic user
            var u = new User(0);
            var topic = new Document(topicId);

            // Now get the latest post in this topic, we do this with the node API to make 100%
            // sure that its correct as Examine takes a few seconds to update
            var postList = new Node(topicId).ChildrenAsList;
            var lastPost = (from p in postList
                            orderby p.CreateDate descending
                            select p).FirstOrDefault();

            // Update the topic
            if(lastPost != null)
            {
                topic.getProperty("forumTopicLastPostDate").Value = lastPost.CreateDate;
            }
            else
            {
                topic.getProperty("forumTopicLastPostDate").Value = null;
            }

            // Finally save it all
            topic.sortOrder = 0;

            // publish application node
            topic.Publish(u);

            // update the document cache so its available in the XML
            umbraco.library.UpdateDocumentCache(topicId);
        }

        #endregion

        #region Post Methods

        /// <summary>
        /// Returns a list of posts from a parent topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <param name="useNodeFactory"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllPostsInTopic(int topicNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllPostsInTopic_NodeFactory(topicNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumPostParentID", topicNodeId.ToString())
                .And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumPost(searchResult)).OrderBy(x => x.CreatedOn);
        }

        /// <summary>
        /// Returns a list of posts from a parent topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllPostsInTopic_NodeFactory(int topicNodeId)
        {
            var posts = new Node(topicNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypePost).OrderBy(x => x.CreateDate);
            return posts.Select(searchResult => Mapper.MapForumPost(searchResult));
        }

        /// <summary>
        /// Returns the sum of all votes for all posts within a topic 
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public int ReturnSumOfVotesInTopic(int topicNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnSumOfVotesInTopic_NodeFactory(topicNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumPostParentID", topicNodeId.ToString()).And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => searchResult["forumPostKarma"].ToInt32()).Sum();          
        }

        /// <summary>
        /// Returns the sum of all votes for all posts within a topic 
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public int ReturnSumOfVotesInTopic_NodeFactory(int topicNodeId)
        {
            var posts = new Node(topicNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypePost);
            return posts.Select(searchResult => searchResult.GetProperty("forumPostKarma").Value.ToInt32()).Sum();
        }

        /// <summary>
        /// Returns the count of posts within a topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public int ReturnPostCountInTopic(int topicNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnPostCountInTopic_NodeFactory(topicNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumPostParentID", topicNodeId.ToString()).And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile()).Count();
            return results;
        }

        /// <summary>
        /// Returns the count of posts within a topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public int ReturnPostCountInTopic_NodeFactory(int topicNodeId)
        {
            var posts = new Node(topicNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypePost);
            return posts.Count();
        }

        /// <summary>
        /// Return a list of posts by a specific member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllPostsByMemberId(int memberId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllPostsByMemberId_NodeFactory(memberId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumPostOwnedBy", memberId.ToString())
                .And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumPost(searchResult)).OrderBy(x => x.CreatedOn);
        }

        /// <summary>
        /// Return a list of posts by a specific member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllPostsByMemberId_NodeFactory(int memberId)
        {
            var cats = Helpers.FindChildren(new Node(_rootForumId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypePost) &&
                                            t.GetProperty("forumPostOwnedBy").Value == memberId.ToString())
                                            .OrderByDescending(x => x.CreateDate);
            return cats.Select(searchResult => Mapper.MapForumPost(searchResult));
        }

        /// <summary>
        /// Get a specifed amount of posts ordered by negative karma
        /// </summary>
        /// <param name="amounttotake"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllBadPosts(int amounttotake, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnAllBadPosts_NodeFactory(amounttotake);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumPost(searchResult)).OrderBy(x => x.Karma).Take(amounttotake);
        }

        /// <summary>
        /// Get a specifed amount of posts ordered by negative karma
        /// </summary>
        /// <param name="amounttotake"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> ReturnAllBadPosts_NodeFactory(int amounttotake)
        {
            var cats = Helpers.FindChildren(new Node(_rootForumId),
                                            t =>
                                            t.NodeTypeAlias.Equals(NodeTypePost))
                                            .OrderBy(x => x.GetProperty("forumPostKarma").Value.ToInt32()).Take(amounttotake);
            return cats.Select(searchResult => Mapper.MapForumPost(searchResult));
        }

        /// <summary>
        /// Returns a list of posts based upon a search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IEnumerable<ForumPost> SearchPosts(string searchTerm)
        {
            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.GroupedOr(new string[] { "nodeName", "forumPostContent" }, searchTerm).And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumPost(searchResult)).OrderByDescending(x => x.CreatedOn);
        }

        /// <summary>
        /// Returns the latest post in a topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public ForumPost ReturnLatestPostInTopic(int topicNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnLatestPostInTopic_NodeFactory(topicNodeId);

            var criteria = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].CreateSearchCriteria();
            var query = criteria.Field("forumPostParentID", topicNodeId.ToString())
                .And().NodeTypeAlias(NodeTypePost);
            var results = ExamineManager.Instance.SearchProviderCollection[ExamineSearcher].Search(query.Compile());
            return results.Select(searchResult => Mapper.MapForumPost(searchResult)).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
        }

        /// <summary>
        /// Returns the latest post in a topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public ForumPost ReturnLatestPostInTopic_NodeFactory(int topicNodeId)
        {
            var posts = new Node(topicNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypePost).OrderByDescending(x => x.CreateDate);
            return Mapper.MapForumPost(posts.FirstOrDefault());
        }

        /// <summary>
        /// Returns the post that started the topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public ForumPost ReturnTopicStarterPost(int topicNodeId, bool useNodeFactory = false)
        {
            if (useNodeFactory | UseNodeFactoryForAllQueries) return ReturnTopicStarterPost_NodeFactory(topicNodeId);

            return ReturnAllPostsInTopic(topicNodeId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the post that started the topic
        /// </summary>
        /// <param name="topicNodeId"></param>
        /// <returns></returns>
        public ForumPost ReturnTopicStarterPost_NodeFactory(int topicNodeId)
        {
            var posts = new Node(topicNodeId).ChildrenAsList.Where(x => x.NodeTypeAlias == NodeTypePost && x.GetProperty("forumPostIsTopicStarter").Value == "1");
            return Mapper.MapForumPost(posts.FirstOrDefault());
        }

        #endregion

    }
}
