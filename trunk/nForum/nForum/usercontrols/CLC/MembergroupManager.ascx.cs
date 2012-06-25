using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.media;
using nForum.BusinessLogic.Models;
using nForum.BusinessLogic;
using nForum.helpers;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.member;
using umbraco.interfaces;
using umbraco.BusinessLogic;

namespace nForum.usercontrols.CLC
{
    public partial class MembergroupManager : BaseForumUsercontrol
    {

        private int SelectedNodeID { get; set; }
        private string MembergroupSearchText { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.selectMembers.Visible = false;
            if (Request.QueryString["memid"] != null)
            {
                this.SelectedNodeID = Convert.ToInt32(Request.QueryString["memid"]);
                this.selectMembers.Visible = true;
            }

            SetMembergroupList();
            SetMemberList();
        }

        public bool IsMembergroupSelected(int memberGroupID)
        {
            bool result = false;

            if (Request.QueryString["memid"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["memid"]) == memberGroupID)
                {
                    result = true;
                }
            }

            return result;
        }

        private void SetMemberList()
        {
            IEnumerable<Member> members;
            if (Request.QueryString["memid"] != null) 
            {
                if (Request.QueryString["memsearch"] != null)
                {
                    this.txtSearchMember.Text = Request.QueryString["memsearch"].ToString();
                    members = Member.GetAllAsList().Where(m => m.LoginName.Contains(this.txtSearchMember.Text) || m.Email.Contains(Request.QueryString["memsearch"].ToString().ToLower()));
                }
                else
                {
                    members = Member.GetAllAsList().OrderByDescending(m => m.CreateDateTime).Take(20);
                    lastMembers.Visible = true;
                }

                this.rprMembers.DataSource = members;
                this.rprMembers.DataBind();
            }
        }

        private void SetMembergroupList()
        {
            IEnumerable<ForumCategory> memberGroups = null;

            if (Request.QueryString["memgsearch"] != null || !string.IsNullOrEmpty(this.txtSearchMembergroup.Text))
            {
                if (!string.IsNullOrEmpty(this.txtSearchMembergroup.Text))
                {
                    this.MembergroupSearchText = this.txtSearchMembergroup.Text;
                }
                else
                {
                    this.MembergroupSearchText = Request.QueryString["memgsearch"];
                    this.txtSearchMembergroup.Text = this.MembergroupSearchText;
                }

                var useNodeFactory = Request.QueryString["nf"] != null;
                memberGroups = from m in Factory.ReturnAllCategories(useNodeFactory)
                               where m.IsMainCategory && m.Name.ToLower().Contains(this.MembergroupSearchText)
                               select m;
            }
            else
            {
                var useNodeFactory = Request.QueryString["nf"] != null;
                memberGroups = from m in Factory.ReturnAllCategories(useNodeFactory)
                               where m.IsMainCategory orderby m.CreatedOn descending
                               select m;
                memberGroups = memberGroups.Take(20);
                this.lastGroups.Visible = true;
            }

            if (memberGroups.Any())
            {
                this.rprGroups.DataSource = memberGroups;
                this.rprGroups.DataBind();
            }
        }

        protected bool IsMember(string loginName)
        {
            bool result = false;

            Member member = Member.GetMemberByName(loginName, false)[0];

            Node selectedNode = new Node(this.SelectedNodeID);

            foreach (var group in member.Groups.Values)
            {
                if (((umbraco.cms.businesslogic.CMSNode)group).Text == selectedNode.Name)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("membergroupmanagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&memsearch=" + this.txtSearchMember.Text + "&memid=" + this.SelectedNodeID);
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Node selectedNode = new Node(this.SelectedNodeID);

            // check if membergroup exists, create it if not
            if (MemberGroup.GetByName(selectedNode.Name) == null)
            {
                MemberGroup.MakeNew(selectedNode.Name, User.GetCurrent());
            }

            MemberGroup group = MemberGroup.GetByName(selectedNode.Name);

            foreach (RepeaterItem item in rprMembers.Items)
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

            this.lblResultInfo.Text = "Wijzigingen opgeslagen!";

        }

        protected void searchMembergroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("membergroupmanagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text);
        }

    }
}