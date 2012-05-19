using System.Web;
using System.Web.UI;
using nForum.BusinessLogic.Data;
using nForum.BusinessLogic.Models;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.member;

namespace nForum.BusinessLogic
{
    public class BaseForumUsercontrol : UserControl
    {
        /// <summary>
        /// Gets access to the current node the user is on
        /// </summary>
        public Node CurrentNode
        {
            get
            {
                return Node.GetCurrent();
            }
        }

        /// <summary>
        /// Gets a Factory instance
        /// </summary>
        public ForumFactory Factory = new ForumFactory();
        
        /// <summary>
        /// Gets a Mapper Instance
        /// </summary>
        public NodeMapper Mapper = new NodeMapper();

        /// <summary>
        /// Gets a cached version of the main forum settings
        /// </summary>
        public Forum Settings {
            get
            {
                return Helpers.MainForumSettings();
            }
        }

        /// <summary>
        /// Gets a cached version of the currently logged in member
        /// </summary>
        public ForumMember CurrentMember
        {
            get
            {
                var cMember = Member.GetCurrentMember();
                return cMember != null ? MembershipHelper.ReturnMember(cMember.Id) : null;
            }
        }

        /// <summary>
        /// Returns a bool as to whether the currently logged in member is banned or not
        /// </summary>
        public bool IsBanned
        {
            get {
                return CurrentMember != null && CurrentMember.MemberIsBanned;
            }
        }

        /// <summary>
        ///  Gets the current page absolute Url
        /// </summary>
        public string CurrentPageAbsoluteUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsolutePath;
            }
        }

        /// <summary>
        /// Gets the current Url via the request
        /// </summary>
        public string Url()
        {
            return Helpers.ReturnSiteDomainName();
        }
    }
}
