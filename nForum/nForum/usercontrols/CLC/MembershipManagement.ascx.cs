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
using nForum.global;
using umbraco.cms.businesslogic.web;

namespace nForum.usercontrols.CLC
{
    public partial class MembershipManagement : BaseForumUsercontrol
    {

        private int SelectedNodeID { get; set; }
        private string MembergroupSearchText { get; set; }
        private string ProjectSearchText { get; set; }

        private const int SEARCHITEMSCOUNT = 20;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.selectMembers.Visible = false;
            this.selectMembergroup.Visible = false;
            this.selectProject.Visible = false;

            if (Request.QueryString["documenttype"] != null)
            {
                switch (Request.QueryString["documenttype"].ToString())
                {
                    case GlobalConstants.MembergroupAlias:
                        {
                            this.selectMembergroup.Visible = true;
                            SetMembergroupList();
                        }break;
                    case GlobalConstants.ProjectAlias:
                        {
                            this.selectProject.Visible = true;
                            SetProjectList();
                        }break;
                }
            }

            if (Request.QueryString["memid"] != null || Request.QueryString["projid"] != null)
            {
                this.SelectedNodeID = (Request.QueryString["memid"] != null) ? Convert.ToInt32(Request.QueryString["memid"]) : Convert.ToInt32(Request.QueryString["projid"]);
                this.selectMembers.Visible = true;
            }

            SetMemberList();
            this.DataBind();
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

        public bool IsProjectSelected(int projectID)
        {
            bool result = false;

            if (Request.QueryString["projid"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["projid"]) == projectID)
                {
                    result = true;
                }
            }

            return result;
        }

        private void SetMemberList()
        {
            IEnumerable<Member> members;
            if (this.SelectedNodeID > 0) 
            {
                if (Request.QueryString["memsearch"] != null)
                {
                    this.txtSearchMember.Text = Request.QueryString["memsearch"].ToString();
                    members = Member.GetAllAsList().Where(m => m.LoginName.Contains(this.txtSearchMember.Text) || m.Email.Contains(Request.QueryString["memsearch"].ToString().ToLower()));
                }
                else
                {
                    members = Member.GetAllAsList().OrderByDescending(m => m.CreateDateTime).Take(SEARCHITEMSCOUNT);
                    lastMembers.Visible = true;
                }

                this.rprMembers.DataSource = members;
                this.rprMembers.DataBind();
            }
        }

        private void SetProjectList()
        {
            IEnumerable<Document> projects = null;

            if (Request.QueryString["projsearch"] != null || !string.IsNullOrEmpty(this.txtSearchProject.Text))
            {
                if (!string.IsNullOrEmpty(this.txtSearchProject.Text))
                {
                    this.ProjectSearchText = this.txtSearchMembergroup.Text;
                }
                else
                {
                    this.ProjectSearchText = Request.QueryString["projsearch"];
                    this.txtSearchProject.Text = this.ProjectSearchText;
                }

                projects = DocumentHelper.GetOrCreateProjectgroupCategory().Children.Where(p => p.Text.Contains(this.ProjectSearchText));
            }
            else
            {
                projects = DocumentHelper.GetOrCreateProjectgroupCategory().Children.Take(SEARCHITEMSCOUNT);                
            }

            if (projects.Any())
            {
                rprProjects.DataSource = projects;
                rprProjects.DataBind();
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
                memberGroups = memberGroups.Take(SEARCHITEMSCOUNT);
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
            Response.Redirect("membergroupmanagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&memsearch=" + this.txtSearchMember.Text + "&memid=" + this.SelectedNodeID + "&documenttype=" + GlobalConstants.MembergroupAlias);
        }

        protected void searchProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("membergroupmanagement.aspx?projsearch=" + this.txtSearchMembergroup.Text + "&documenttype=" + GlobalConstants.ProjectAlias);
        }

        protected void searchMembergroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("membergroupmanagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&documenttype=" + GlobalConstants.MembergroupAlias);
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


    }
}