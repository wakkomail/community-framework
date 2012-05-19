using System;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum.includes
{
    public partial class MainMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var m = Request.QueryString["m"];
            if (m == null) return;
            var message = Helpers.SafePlainText(m);
            var commentCount = message.IndexOf("#comment");
            if(commentCount >= 0)
            {
                message = message.Substring(0, commentCount);
            }
            msgBox.ShowInfo(message);
        }
    }
}