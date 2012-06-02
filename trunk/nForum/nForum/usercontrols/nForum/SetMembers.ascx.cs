using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.member;
using umbraco.BusinessLogic;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;


namespace nForum.usercontrols.nForum
{
    public partial class SetMembers : BaseForumUsercontrol
    {
        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            Initialize();

            // set memberlist
            var memberList = Member.GetAll;
            this.rptMembers.DataSource = memberList;
            this.rptMembers.DataBind();

            this.btnSetMembers.Visible = CurrentMemberIsAdmin;
        }

        private void Initialize()
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);
        }

        protected bool IsMember(string loginName)
        {
            bool result = false;

            Member member = Member.GetMemberByName(loginName, false)[0];

            foreach (var group in member.Groups.Values)
            {
                if (((umbraco.cms.businesslogic.CMSNode)group).Text == currentCategory.Name)
                {
                    result = true;
                    break;
                }
            }            

            return result;
        }

        protected bool CurrentMemberIsAdmin
        {
            get
            {
                if (CurrentMember == null)
                {
                    return false;
                }
                else
                {
                    return CurrentMember.MemberIsAdmin;
                }                
            }
        }

        protected void btnSetMembers_Click(object sender, EventArgs e)
        {
            // check if membergroup exists, create it if not
            if (MemberGroup.GetByName(currentCategory.Name) == null)
            {
                MemberGroup.MakeNew(currentCategory.Name, User.GetCurrent());
            }

            MemberGroup group = MemberGroup.GetByName(currentCategory.Name);

            foreach (RepeaterItem item in rptMembers.Items)
            {
                CheckBox isMemberControl = (CheckBox)item.Controls[1];
                Member member = Member.GetMemberByName(isMemberControl.Text, false)[0];
                if (isMemberControl.Checked == true)
                {
                    // ensure that member is attached to the group
                    if (!IsMember(member.LoginName))
                    {
                        member.AddGroup(group.Id);
                    }
                }
                else
                {
                    // ensure that member is not attached to the group
                    if (IsMember(member.LoginName))
                    {
                        member.RemoveGroup(group.Id);
                    }
                }
            }
        }

    }
}