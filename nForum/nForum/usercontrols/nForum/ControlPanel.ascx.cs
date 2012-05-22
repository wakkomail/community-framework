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
                // create membergroup
                Document newMemberGroup = Document.MakeNew(this.txtMembergroupName.Text, DocumentType.GetByAlias("CLCMembergroup"), User.GetUser(0), -1);

                // create documentsharing page
                Document newDocumentSharing = Document.MakeNew("Documenten", DocumentType.GetByAlias("CLCMediaSharing"), User.GetUser(0), newMemberGroup.Id);

                // create discussion page
                Document newDiscussion = Document.MakeNew("Discussies", DocumentType.GetByAlias("CLCDiscussions"), User.GetUser(0), newMemberGroup.Id);
            }
            else
            {
                lblResultInfo.Text = "Groepnaam is een verplicht veld";
            }
        }

        protected void btnInsertProject_Click(object sender, EventArgs e)
        {

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