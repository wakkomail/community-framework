using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace nForum.usercontrols
{
    public partial class ControlPanel : System.Web.UI.UserControl
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void createMembergroup_Click(object sender, EventArgs e)
        {
            this.SetOption(enuOption.Membergroup);
        }

        protected void createProject_Click(object sender, EventArgs e)
        {
            this.SetOption(enuOption.Project);
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

            }
            else
            {
                lblResultInfo.Text = "Groepnaam is een verplicht veld";
            }
        }

        protected void btnInsertProject_Click(object sender, EventArgs e)
        {

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
            Membergroup = 0,
            Project = 1
        }

        private enuOption CurrentOption { get; set; }

        #endregion

        #region Methods

        private void SetOption(enuOption option)
        {
            this.CurrentOption = option;

            this.pnlMembergroup.Visible = option == enuOption.Membergroup;
            this.pnlMembergroup.Visible = option == enuOption.Project;
        }



        #endregion



    }
}