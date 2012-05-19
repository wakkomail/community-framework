using System;
using System.Web.UI;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum.membership
{
    public partial class UserAdmin : UserControl
    {
        protected void UmbracoLogout(object sender, EventArgs e)
        {
            MembershipHelper.LogoutMember();
        }
    }
}