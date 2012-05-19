using System;
using Examine;
using umbraco;
using umbraco.interfaces;

namespace nForum.BusinessLogic.Models
{
    public class NodeMapper
    {
        #region Categories

        /// <summary>
        /// Maps an Umbraco 'Category' Node to the ForumCategory type
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumCategory MapForumCategory(INode nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumCategory
            {
                Id = nodetomap.Id,
                CreatedOn = nodetomap.CreateDate,
                Name = nodetomap.Name,
                Url = library.NiceUrl(nodetomap.Id),
                Description = nodetomap.GetProperty("forumCategoryDescription").Value,
                IsMainCategory = nodetomap.GetProperty("forumCategoryIsMainCategory").Value == "1",
                IsPrivate = nodetomap.GetProperty("forumCategoryIsPrivate").Value == "1",
                KarmaAccessAmount = nodetomap.GetProperty("forumCategoryPermissionKarmaAmount").Value.ToInt32(),
                KarmaPostAmount = nodetomap.GetProperty("forumCategoryPostPermissionKarmaAmount").Value.ToInt32(),
                ParentId = nodetomap.Parent.Id,
                ParentCategoryId = nodetomap.GetProperty("forumCategoryParentID").Value.ToInt32(),
                SortOrder = nodetomap.SortOrder
            };
            return fCat;
        }

        /// <summary>
        /// Maps an Umbraco 'Examine' Search Result to the ForumCategory type
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumCategory MapForumCategory(SearchResult nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumCategory
            {
                Id = nodetomap.Id,
                Name = CheckFieldExists(nodetomap, "nodeName"),
                Url = library.NiceUrl(nodetomap.Id),                
                CreatedOn = Convert.ToDateTime(CheckFieldExists(nodetomap, "__Sort_createDate")),
                ParentId = CheckFieldExists(nodetomap, "parentID").ToInt32(),
                Description = CheckFieldExists(nodetomap, "forumCategoryDescription"),
                IsMainCategory = CheckFieldExists(nodetomap, "forumCategoryIsMainCategory") == "1",
                IsPrivate = CheckFieldExists(nodetomap, "forumCategoryIsPrivate") == "1",
                KarmaAccessAmount = CheckFieldExists(nodetomap, "forumCategoryPermissionKarmaAmount").ToInt32(),
                KarmaPostAmount = CheckFieldExists(nodetomap, "forumCategoryPostPermissionKarmaAmount").ToInt32(),
                ParentCategoryId = CheckFieldExists(nodetomap, "forumCategoryParentID").ToInt32(),
                SortOrder = CheckFieldExists(nodetomap, "sortOrder").ToInt32()
            };
            return fCat;
        } 

        #endregion

        #region Posts

        /// <summary>
        /// Maps an Umbraco 'Post' Node to the ForumPost type
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumPost MapForumPost(INode nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumPost
            {
                Id = nodetomap.Id,
                CreatedOn = nodetomap.CreateDate,
                ParentId = nodetomap.Parent.Id,
                Name = nodetomap.Name,
                Url = library.NiceUrl(nodetomap.Id),
                Content = Helpers.HtmlDecode(nodetomap.GetProperty("forumPostContent").Value),
                Owner = MembershipHelper.ReturnMember(nodetomap.GetProperty("forumPostOwnedBy").Value.ToInt32()),
                LastEdited = Helpers.InternalDateFixer(nodetomap.GetProperty("forumPostLastEdited").Value),
                IsSolution = nodetomap.GetProperty("forumPostIsSolution").Value == "1",
                IsTopicStarter = nodetomap.GetProperty("forumPostIsTopicStarter").Value == "1",
                Karma = nodetomap.GetProperty("forumPostKarma").Value.ToInt32(),
                VotedMembersIds = Helpers.StringArrayToIntList(nodetomap.GetProperty("forumPostUsersVoted").Value),
                ParentTopicId = nodetomap.GetProperty("forumPostParentID").Value.ToInt32(),
                SortOrder = nodetomap.SortOrder
            };

            return fCat;
        }

        /// <summary>
        /// Maps an Umbraco Examine 'Post' Search Result to the ForumPost type
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumPost MapForumPost(SearchResult nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumPost
            {
                Id = nodetomap.Id,
                Url = library.NiceUrl(nodetomap.Id),
                Name = CheckFieldExists(nodetomap, "nodeName"),
                CreatedOn = Convert.ToDateTime(CheckFieldExists(nodetomap, "__Sort_createDate")),
                ParentId = CheckFieldExists(nodetomap, "parentID").ToInt32(),
                Content = Helpers.HtmlDecode(CheckFieldExists(nodetomap, "forumPostContent")),
                Owner = MembershipHelper.ReturnMember(CheckFieldExists(nodetomap, "forumPostOwnedBy").ToInt32()),
                LastEdited = Helpers.InternalDateFixer(CheckFieldExists(nodetomap, "__Sort_forumPostLastEdited")),
                IsSolution = CheckFieldExists(nodetomap, "forumPostIsSolution") == "1",
                IsTopicStarter = CheckFieldExists(nodetomap, "forumPostIsTopicStarter") == "1",
                Karma = CheckFieldExists(nodetomap, "forumPostKarma").ToInt32(),
                VotedMembersIds = Helpers.StringArrayToIntList(CheckFieldExists(nodetomap, "forumPostUsersVoted")),
                ParentTopicId = CheckFieldExists(nodetomap, "forumPostParentID").ToInt32(),
                SortOrder = CheckFieldExists(nodetomap, "sortOrder").ToInt32()
            };

            return fCat;
        } 

        #endregion

        #region Topics

        /// <summary>
        /// Maps an Umbraco 'Topic' Node to the ForumTopic type
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumTopic MapForumTopic(INode nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumTopic
            {
                Id = nodetomap.Id,
                Url = library.NiceUrl(nodetomap.Id),
                Name = nodetomap.Name,
                CreatedOn = nodetomap.CreateDate,
                ParentId = nodetomap.Parent.Id,
                CategoryId = nodetomap.GetProperty("forumTopicParentCategoryID").Value.ToInt32(),
                Owner = MembershipHelper.ReturnMember(nodetomap.GetProperty("forumTopicOwnedBy").Value.ToInt32()),
                IsClosed = nodetomap.GetProperty("forumTopicClosed").Value == "1",
                IsSolved = nodetomap.GetProperty("forumTopicSolved").Value == "1",
                IsSticky = nodetomap.GetProperty("forumTopicIsSticky").Value == "1",
                SubscriberIds = Helpers.StringArrayToIntList(nodetomap.GetProperty("forumTopicSubscribedList").Value),
                LastPostDate = Helpers.InternalDateFixer(nodetomap.GetProperty("forumTopicLastPostDate").Value),
                SortOrder = nodetomap.SortOrder
            };
            return fCat;
        }

        /// <summary>
        /// Maps an Umbraco Examine 'Topic' Search Result to the ForumTopic type 
        /// </summary>
        /// <param name="nodetomap"></param>
        /// <returns></returns>
        public ForumTopic MapForumTopic(SearchResult nodetomap)
        {
            if (nodetomap == null) return null;
            var fCat = new ForumTopic
            {
                Id = nodetomap.Id,
                Url = library.NiceUrl(nodetomap.Id),
                Name = CheckFieldExists(nodetomap, "nodeName"),
                CreatedOn = Convert.ToDateTime(CheckFieldExists(nodetomap, "__Sort_createDate")),
                ParentId = CheckFieldExists(nodetomap, "parentID").ToInt32(),
                CategoryId = CheckFieldExists(nodetomap, "forumTopicParentCategoryID").ToInt32(),
                Owner = MembershipHelper.ReturnMember(CheckFieldExists(nodetomap, "forumTopicOwnedBy").ToInt32()),
                IsClosed = CheckFieldExists(nodetomap, "forumTopicClosed") == "1",
                IsSolved = CheckFieldExists(nodetomap, "forumTopicSolved") == "1",
                IsSticky = CheckFieldExists(nodetomap, "forumTopicIsSticky") == "1",
                SubscriberIds = Helpers.StringArrayToIntList(CheckFieldExists(nodetomap, "forumTopicSubscribedList")),
                SortOrder = CheckFieldExists(nodetomap, "sortOrder").ToInt32(),
                LastPostDate = Helpers.InternalDateFixer(CheckFieldExists(nodetomap, "__Sort_forumTopicLastPostDate"))
            };
            return fCat;
        } 

        #endregion

        /// <summary>
        /// This just checks the field exists and then returns the value,
        /// lucene is a frickin pain in the arse
        /// </summary>
        /// <returns></returns>
        private string CheckFieldExists(SearchResult result, string keyName)
        {
            return result.Fields.ContainsKey(keyName) ? result.Fields[keyName] : "";
        }
    }
}
