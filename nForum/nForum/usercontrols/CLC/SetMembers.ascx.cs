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
    public partial class SetMembers : System.Web.UI.UserControl
    {
        private const int SEARCHITEMSCOUNT = 20;

        #region Methods

        private void Initialize()
        {
            if (Request.QueryString["groupid"] != null)
            {
                this.SelectedNodeID = Convert.ToInt32(Request.QueryString["groupid"].ToString());
            }
            SetMemberList();
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

        protected bool IsMember(string loginName)
        {
            bool result = false;

            Member member = Member.GetMemberByName(loginName, false)[0];

            Node selectedNode = new Node(this.SelectedNodeID);

            //return member.Groups.ContainsValue(selectedNode.Name);

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

            //return member.Groups.ContainsValue(selectedNode.Name + "|" + GlobalConstants.RoleManager);

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

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Node selectedNode = new Node(this.SelectedNodeID);

            // TODO: obsolete 

            // check if membershipgroup exists, create it if not
            if (MemberGroup.GetByName(selectedNode.Name) == null)
            {
                MemberGroup.MakeNew(selectedNode.Name, umbraco.BusinessLogic.User.GetUser(0));
            }

            // also create managers membershipgroup
            if (MemberGroup.GetByName(selectedNode.Name + "|" + GlobalConstants.RoleManager) == null)
            {
                MemberGroup.MakeNew(selectedNode.Name + "|" + GlobalConstants.RoleManager, umbraco.BusinessLogic.User.GetUser(0));
            }

            // END TODO

            MemberGroup group = MemberGroup.GetByName(selectedNode.Name);
            MemberGroup managerGroup = MemberGroup.GetByName(selectedNode.Name + "|" + GlobalConstants.RoleManager);


            foreach (RepeaterItem item in rprMembers.Items)
            {
                CheckBox isMemberControl = (CheckBox)item.Controls[1];
                CheckBox isManagerControl = (CheckBox)item.Controls[3];
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

            this.lblResultInfo.Text = "Leden gekoppeld!";

        }

        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("CLCSetMembers.aspx?groupid=" + this.SelectedNodeID + "&memsearch=" + this.txtSearchMember.Text);
        }

        #endregion

        #region Properties

        public int SelectedNodeID { get; set; }

        private string MemberSearchText { get; set; }

        #endregion
    }
}