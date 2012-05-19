using System;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum
{
    public partial class ForumCreateNewTopic : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Before anything see if the category has been marked as private, if so make sure user is loggged in
            var currentCategory = Mapper.MapForumCategory(CurrentNode);

            if (MembershipHelper.IsAuthenticated())
            {
                // If forum is closed then don't show the form or if user is banned
                if (Settings.IsClosed | IsBanned | (CurrentMember.MemberKarmaAmount < currentCategory.KarmaPostAmount))
                {
                    lvNewTopic.Visible = false;
                }
                else
                {
                    if(CurrentMember.MemberIsAdmin)
                    {
                        var pnlMakeSticky = (Panel)lvNewTopic.FindControl("pnlMakeSticky");
                        pnlMakeSticky.Visible = true;
                    }
                }
            }

            SetAjaxAttributes();
        }

        private void SetAjaxAttributes()
        {
            var btnSubmitPost = (Button)lvNewTopic.FindControl("btnSubmitPost");
            btnSubmitPost.Attributes.Add("rel", CurrentNode.Id.ToString());
        }

    }
}