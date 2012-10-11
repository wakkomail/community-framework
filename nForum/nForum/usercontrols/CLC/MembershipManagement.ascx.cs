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
using umbraco.BasePages;

namespace nForum.usercontrols.CLC
{
    public partial class MembershipManagement : BaseForumUsercontrol
    {

        private int SelectedNodeID { get; set; }
        private string MembergroupSearchText { get; set; }
        private string ProjectSearchText { get; set; }
        private string MemberSearchText { get; set; }
        Document selectedGroup = null;

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

            if (Request.QueryString["groupid"] != null)
            {
                this.SelectedNodeID = Convert.ToInt32(Request.QueryString["groupid"]);
                this.selectMembers.Visible = true;
                this.pnlEditGroup.Visible = true;
                SetEditForm();
            }

            SetMemberList();
            this.DataBind();
        }

        public bool IsGroupSelected(int memberGroupID)
        {
            bool result = false;

            if (Request.QueryString["groupid"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["groupid"]) == memberGroupID)
                {
                    result = true;
                }
            }

            return result;
        }

        private void SetEditForm()
        {
                selectedGroup =  new Document(Convert.ToInt32(Request.QueryString["groupid"]));
                if (!IsPostBack)
                {
                    if (selectedGroup != null)
                    {
                        this.txtGroupName.Text = selectedGroup.Text;
                        this.txtGroupDescription.Text = selectedGroup.getProperty(GlobalConstants.DescriptionField).Value.ToString();
                    }
                }
        }

        private void SetMemberList()
        {
            IEnumerable<Member> members;
            if (this.SelectedNodeID > 0) 
            {
                if (Request.QueryString["memsearch"] != null || !string.IsNullOrEmpty(this.txtSearchMember.Text))
                {
                    if (!string.IsNullOrEmpty(this.txtSearchMember.Text))
                    {
                        this.MemberSearchText = this.txtSearchMember.Text;
                    }
                    else
                    {
                        this.MemberSearchText = Request.QueryString["memsearch"].ToString();
                        this.txtSearchMember.Text = this.MemberSearchText;
                    }
                    members = Member.GetAllAsList().Where(m => m.LoginName.ToLower().Contains(this.MemberSearchText.ToLower()) || m.Email.Contains(this.MemberSearchText.ToLower()));
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

        protected bool IsManager(string loginName)
        {
            bool result = false;

            Member member = Member.GetMemberByName(loginName, false)[0];

            Node selectedNode = new Node(this.SelectedNodeID);

            foreach (var group in member.Groups.Values)
            {
                if (((umbraco.cms.businesslogic.CMSNode)group).Text == selectedNode.Name + "|" + GlobalConstants.RoleManager)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("MembershipManagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&memsearch=" + this.txtSearchMember.Text + "&groupid=" + this.SelectedNodeID + "&documenttype=" + Request.QueryString["documenttype"].ToString());
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            // delete group
           Document group = new Document(this.SelectedNodeID);
           group.delete(true);
            

            //Response.Redirect("MembershipManagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&memsearch=" + this.txtSearchMember.Text + "&groupid=" + this.SelectedNodeID + "&documenttype=" + Request.QueryString["documenttype"].ToString());
        }

        protected void searchProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("MembershipManagement.aspx?projsearch=" + this.txtSearchProject.Text + "&documenttype=" + GlobalConstants.ProjectAlias);
        }

        protected void searchMembergroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("MembershipManagement.aspx?memgsearch=" + this.txtSearchMembergroup.Text + "&documenttype=" + GlobalConstants.MembergroupAlias);
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Node selectedNode = new Node(this.SelectedNodeID);
            
            // TODO: obsolete 

            // check if membershipgroup exists, create it if not
            if (MemberGroup.GetByName(selectedNode.Name) == null)
            {
                MemberGroup.MakeNew(selectedNode.Name, User.GetUser(0));
            }

            // also create managers membershipgroup
            if (MemberGroup.GetByName(selectedNode.Name + "|" + GlobalConstants.RoleManager) == null)
            {
                MemberGroup.MakeNew(selectedNode.Name + "|" + GlobalConstants.RoleManager, User.GetUser(0));
            }

            // END TODO

            MemberGroup group = MemberGroup.GetByName(selectedNode.Name);
            MemberGroup managerGroup = MemberGroup.GetByName(selectedNode.Name + "|" + GlobalConstants.RoleManager);


            foreach (RepeaterItem item in rprMembers.Items)
            {
				CheckBox isMemberControl = (CheckBox)item.FindControl("chkMember");
				CheckBox isManagerControl = (CheckBox)item.FindControl("chkManager");
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

                if (isManagerControl.Checked == true)
                {
                    // ensure that member is attached to the managergroup
                    if (!IsManager(member.LoginName))
                    {
                        member.AddGroup(managerGroup.Id);
                    }
                }
                else
                {
                    // ensure that member is not attached to the managergroup
                    if (IsManager(member.LoginName))
                    {
                        member.RemoveGroup(managerGroup.Id);
                    }
                }
            }

            // save document
            selectedGroup.Text = this.txtGroupName.Text;
            selectedGroup.getProperty(GlobalConstants.DescriptionField).Value = this.txtGroupDescription.Text;
            selectedGroup.Publish(User.GetUser(0));
            umbraco.library.UpdateDocumentCache(selectedGroup.Id);

            this.lblResultInfo.Text = "Wijzigingen opgeslagen!";

        }        
    }
}