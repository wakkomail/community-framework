using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.web;
using nForum.BusinessLogic;
using nForum.global;
using nForum.helpers;
using umbraco.cms.businesslogic.media;
using umbraco.cms.businesslogic.template;

namespace nForum.usercontrols.CLC
{
    public partial class ControlPanel : System.Web.UI.UserControl
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
        }

        protected void createMembergroup_Click(object sender, EventArgs e)
        {
            ClearAllTextboxes(pnlMembergroup);
            this.SetOption(enuOption.Membergroup);
        }

        protected void createProject_Click(object sender, EventArgs e)
        {
            ClearAllTextboxes(pnlProject);
            this.SetOption(enuOption.Project);
        }

        protected void createMember_Click(object sender, EventArgs e)
        {
            ClearAllTextboxes(pnlMember);
            this.SetOption(enuOption.Member);
        }

        protected void btnInsertMembergroup_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtMembergroupName.Text))
            {
                // ensure membergroups folder in content tree
                Document contentCategory = DocumentHelper.GetOrCreateMembergroupCategory();                             

                // create membergroup
                Document newMemberGroup = Document.MakeNew(this.txtMembergroupName.Text, DocumentType.GetByAlias(GlobalConstants.MembergroupAlias), User.GetUser(0), contentCategory.Id);
                newMemberGroup.Template = Template.GetByAlias(GlobalConstants.MembergroupTemplateName).Id;
                // set default properties
                newMemberGroup.getProperty(GlobalConstants.PermissionKarmaAmount).Value = GlobalConstants.PermissionKarmaAmountDefaultValue;
                newMemberGroup.getProperty(GlobalConstants.PermissionPostKarmaAmount).Value = GlobalConstants.PermissionPostKarmaAmountDefaultValue;
                newMemberGroup.getProperty(GlobalConstants.IsMainCategory).Value = true;
                newMemberGroup.Publish(User.GetUser(0));

                // clear document cache
                umbraco.library.UpdateDocumentCache(newMemberGroup.Id);

                // ensure membergroups folder in media tree
                Media mediaCategory = MediaHelper.GetOrCreateMembergroupCategory();

                // create media folder
                Media.MakeNew(this.txtMembergroupName.Text, MediaType.GetByAlias("folder"), User.GetUser(0), mediaCategory.Id);

                // create noticeboard
                Document noticeBoard = Document.MakeNew(GlobalConstants.NoticeBoardFolder, DocumentType.GetByAlias(GlobalConstants.NoticeBoardAlias), User.GetUser(0), newMemberGroup.Id);
                noticeBoard.Publish(User.GetUser(0));

                SetOption(enuOption.None);
                lblResultInfo.Text = "Kennisgroep '" + this.txtMembergroupName.Text + "' aangemaakt!";
            }
            else
            {
                lblResultInfo.Text = "Kennisgroepnaam is een verplicht veld";
            }
        }

        protected void btnInsertProject_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtProjectName.Text))
            {
                // ensure membergroups folder in content tree
                Document contentCategory = DocumentHelper.GetOrCreateProjectgroupCategory();

                // create project
                Document newProject = Document.MakeNew(this.txtProjectName.Text, DocumentType.GetByAlias(GlobalConstants.ProjectAlias), User.GetUser(0), contentCategory.Id);
                newProject.Template = Template.GetByAlias(GlobalConstants.ProjectTemplateName).Id;
                // set default properties
                newProject.getProperty(GlobalConstants.PermissionKarmaAmount).Value = GlobalConstants.PermissionKarmaAmountDefaultValue;
                newProject.getProperty(GlobalConstants.PermissionPostKarmaAmount).Value = GlobalConstants.PermissionPostKarmaAmountDefaultValue;
                newProject.getProperty(GlobalConstants.IsMainCategory).Value = true;
                newProject.Publish(User.GetUser(0));

                // clear document cache
                umbraco.library.UpdateDocumentCache(newProject.Id);

                // ensure membergroups folder in media tree
                Media mediaCategory = MediaHelper.GetOrCreateProjectCategory();

                // create media folder
                Media.MakeNew(this.txtProjectName.Text, MediaType.GetByAlias("folder"), User.GetUser(0), mediaCategory.Id);

                // create agenda
                Document agenda = Document.MakeNew(GlobalConstants.AgendaFolder, DocumentType.GetByAlias(GlobalConstants.AgendaAlias), User.GetUser(0), newProject.Id);
                agenda.Template = Template.GetByAlias(GlobalConstants.AgendaTemplateAlias).Id;
                agenda.Publish(User.GetUser(0));
                umbraco.library.UpdateDocumentCache(agenda.Id);

                SetOption(enuOption.None);
                lblResultInfo.Text = "Project '" + this.txtProjectName.Text + "' aangemaakt!";
            }
            else
            {
                lblResultInfo.Text = "Projectnaam is een verplicht veld";
            }
        }

        protected void btnInsertMember_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMemberName.Text) || string.IsNullOrEmpty(this.txtMemberPassword.Text) || string.IsNullOrEmpty(this.txtMemberLoginName.Text) || string.IsNullOrEmpty(this.txtMemberEmail.Text))
            {
                this.lblResultInfo.Text = "Alle invoervelden zijn verplicht";
            }
            else
            {
                if (Membership.GetUserNameByEmail(txtMemberEmail.Text) == null)
                {
                    Member newMember = Member.MakeNew(txtMemberLoginName.Text, MemberType.GetByAlias(GlobalConstants.MemberTypeAlias), new umbraco.BusinessLogic.User(0));
                    newMember.Email = txtMemberEmail.Text;                   
                    newMember.Password = txtMemberPassword.Text;
                    newMember.LoginName = txtMemberLoginName.Text;
                    newMember.getProperty("forumUserLastPrivateMessage").Value = System.DateTime.Now;
                    newMember.getProperty("forumUserAllowPrivateMessages").Value = true;
                    newMember.Save();

                    SetOption(enuOption.None);
                    this.lblResultInfo.Text = "Lid aangemaakt";
                }
                else
                {
                    //member exists                 
                    this.lblResultInfo.Text = "Lid bestaat reeds";
                }
            }
        }

        protected void publishAll_Click(object sender, EventArgs e)
        {            
            // publish root and children
            DocumentHelper.GetRootDocument().PublishWithChildrenWithResult(User.GetUser(0));
            // publish membergroups
            DocumentHelper.GetRootFolderByName(GlobalConstants.MembergroupFolderName).PublishWithSubs(User.GetUser(0));
            // publish projects // TODO
            
        }

        #endregion

        #region Properties / Variables

        private enum enuOption
        {
            None = 0,
            Membergroup = 1,
            Project = 2,
            Member = 3
        }

        private enuOption CurrentOption { get; set; }

        #endregion

        #region Methods

        private void SetOption(enuOption option)
        {
            this.CurrentOption = option;

            this.pnlMembergroup.Visible = (option == enuOption.Membergroup);
            this.pnlProject.Visible = (option == enuOption.Project);
            this.pnlMember.Visible = (option == enuOption.Member);
        }

        private void ClearAllTextboxes(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                switch (control.GetType().Name.ToLower())
                {
                    case "textbox": ((TextBox)control).Text = string.Empty; break;
                    case "checkbox": ((CheckBox)control).Checked = false; break;
                }
            }
        }

        #endregion

    }
}