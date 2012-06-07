using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nForum.BusinessLogic.Models;
using umbraco.NodeFactory;
using nForum.BusinessLogic;
using umbraco.cms.businesslogic.web;
using nForum.helpers;
using nForum.global;

namespace nForum.usercontrols.CLC
{
    public partial class Noticeboard : BaseForumUsercontrol
    {
        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);

            // get proper noticeboard folder
            Node noticeBoard = (Node)Node.GetCurrent().ChildrenAsList.First(n => n.NodeTypeAlias == GlobalConstants.NoticeBoardAlias);
           
            // get list of notice items and bind it to the repeater
            Node currentNode = Node.GetCurrent();
            this.rptNoticeBoard.DataSource = noticeBoard.Children;
            this.rptNoticeBoard.DataBind();
        }
    }
}