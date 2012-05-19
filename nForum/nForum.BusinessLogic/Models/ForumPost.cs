using System;
using System.Collections.Generic;
using System.Linq;
using umbraco.NodeFactory;

namespace nForum.BusinessLogic.Models
{
    public class ForumPost : ForumBase
    {
        private NodeMapper _mapper = new NodeMapper();

        #region Properties

        /// <summary>
        /// The post content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The member who made the post
        /// </summary>
        public ForumMember Owner { get; set; }

        /// <summary>
        /// The date and time the post was last edited
        /// </summary>
        public DateTime? LastEdited { get; set; }

        /// <summary>
        /// If this post has been marked as the solution for the topic
        /// </summary>
        public bool IsSolution { get; set; }

        /// <summary>
        /// If this is the first post in a topic - The starting post that created the topic
        /// </summary>
        public bool IsTopicStarter { get; set; }

        /// <summary>
        /// The karma score for this post
        /// </summary>
        public int Karma { get; set; }

        /// <summary>
        /// Parent Topic id, used because examine doesn't seem to work on parent ID
        /// </summary>
        public int ParentTopicId { get; set; }

        /// <summary>
        /// List of member ids who have applied karma to the post
        /// </summary>
        public List<int> VotedMembersIds { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// List of members who have applied karma to the post
        /// </summary>
        public IEnumerable<ForumMember> VotedMembers()
        {
            return VotedMembersIds.Select(id => MembershipHelper.ReturnMember(id));
        }

        /// <summary>
        /// Returns the parent Topic for this post
        /// </summary>
        /// <returns></returns>
        public ForumTopic ParentTopic()
        {
            return this.ParentId != null ? _mapper.MapForumTopic(new Node(this.ParentId.ToInt32())) : null;
        }

        #endregion

    }
}
