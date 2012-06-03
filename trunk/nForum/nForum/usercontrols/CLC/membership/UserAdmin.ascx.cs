using System;
using System.Web.UI;
using nForum.BusinessLogic;

namespace nForum.usercontrols.CLC.membership
{
    public partial class UserAdmin : UserControl
    {
		protected void Page_Load(object sender, EventArgs e)
		{
			if(MembershipHelper.IsAuthenticated())
			{
				this.lnkInschrijven.Visible = false;
			}
		}

        protected void UmbracoLogout(object sender, EventArgs e)
        {
            MembershipHelper.LogoutMember();
        }
    }
}