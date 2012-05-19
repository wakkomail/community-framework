using System;

namespace nForum.BusinessLogic.Models
{
    public class ForumBase
    {
        /// <summary>
        /// Node ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Node Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parent node/category ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// When the category was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Returns the page title if one is added
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Returns the meta description if one is added
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// The standard sort order in the Umbraco admin
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Returns the internal Umbraco Url to the node
        /// </summary>
        public string Url { get; set; }

    }
}
