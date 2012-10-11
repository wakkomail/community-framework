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
    public partial class EditMembers : System.Web.UI.UserControl
    {
        private const int SEARCHITEMSCOUNT = 30;
        private Member SelectedMember = null;

        #region Methods

        private void Initialize()
        {
            SetMemberList();
            SetEditForm();
        }

        private void SetMemberList()
        {
            IEnumerable<Member> members;
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

        private void SetEditForm()
        {
            if (Request.QueryString["memid"] != null)
            {
                SelectedMember = new Member(Convert.ToInt32(Request.QueryString["memid"]));
                this.pnlMember.Visible = true;   
            }
            if (!IsPostBack)
            {
                
                if (SelectedMember != null)
                {
                    txtMemberName.Text = SelectedMember.Text;
                    txtLoginName.Text = SelectedMember.LoginName;
                    txtMemberEmail.Text = SelectedMember.Email;                    
                }
            }
        }

        protected bool IsSelected(int memberId)
        {
            bool result = false;

            if (Request.QueryString["memid"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["memid"]) == memberId)
                {
                    result = true;
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
            if (SelectedMember != null)
            {
                SelectedMember.Email = txtMemberEmail.Text;
                if (txtMemberPassword.Text != string.Empty)
                {
                    SelectedMember.Password = txtMemberPassword.Text;
                }
                SelectedMember.Text = txtMemberName.Text;
                SelectedMember.LoginName = txtLoginName.Text;
                
                SelectedMember.getProperty("forumUserLastPrivateMessage").Value = System.DateTime.Now;
                SelectedMember.getProperty("forumUserAllowPrivateMessages").Value = true;
                SelectedMember.Save();

                this.lblResultInfo.Text = "Lid gewijzigd!";
            }
            else
            {
                this.lblResultInfo.Text = "Selecteer lid!";
            }

        }

        protected void search_Click(object sender, EventArgs e)
        {
            Response.Redirect("CLCEditMembers.aspx?memsearch=" + this.txtSearchMember.Text);
        }

        protected void rptMember_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "delete")
                {
                    Member memberToDelete = new Member(id);

                    foreach (MemberGroup memberGroup in memberToDelete.Groups)
                    {
                        memberToDelete.RemoveGroup(memberGroup.Id);
                    }

                    memberToDelete.delete();

                    this.lblResultInfo.Text = "Lid verwijderd uit alle groepen!";
                }
            }
        }        

        #endregion

        #region Properties

        private string MemberSearchText { get; set; }

        #endregion
    }
}