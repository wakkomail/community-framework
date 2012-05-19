using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.cms.businesslogic.member;
using umbraco.DataLayer;
using umbraco.cms.presentation.Trees;
using umbraco.DataLayer;
using umbraco.BusinessLogic;

namespace nForum.DataAccess
{
    class ForumPost
    {
            //IRecordsReader reader = Application.SqlHelper.ExecuteReader("select * from somewhere");
            //while(reader.Read())
            //{
            //    reader.GetString("wrongWord");
            //    reader.GetInt("id").ToString();
            //}

        #region Properties

        public int ForumPostId { get; set; }
        public int ForumPostParentNodeId { get; set; }
        public int ForumPostAuthorId { get; set; }
        public Member ForumPostAuthor { get; set; }
        public string ForumPostUrl { get; set; }
        public DateTime ForumPostCreated { get; set; }
        public string ForumPostContent { get; set; }
        public DateTime ForumPostLastEdited { get; set; }
        public int ForumPostInReplyTo { get; set; }
        public bool ForumPostIsSolution { get; set; }
        public bool ForumPostIsTopicStarter { get; set; }
        public int ForumPostKarma { get; set; }
        public string ForumPostUsersVoted { get; set; }
        #endregion
    }
}
