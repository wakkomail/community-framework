using System;
using System.Linq;
using nForum.BusinessLogic;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum.dashboard
{
    public partial class ForumLatestMembers : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var Members = Member.GetAll.
            GetLatestMembers();
        }

        private void GetLatestMembers()
        {
            if (Page.IsPostBack) return;

            var group = MemberGroup.GetByName(MembershipHelper.ForumUserRoleName);
            var members = group.GetMembers().OrderByDescending(x => x.CreateDateTime).Take(40);

            if(members.Any())
            {
                gvMembers.DataSource = members;
                gvMembers.DataBind();
            }
        }

        public string IsAuthorsied(string boolvalue)
        {
            if(boolvalue == "1")
            {
                return library.GetDictionaryItem("Authorised");
            }
            return library.GetDictionaryItem("UnAuthorised");
        }

    }
}