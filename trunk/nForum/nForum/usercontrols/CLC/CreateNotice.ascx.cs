using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;
using umbraco.BusinessLogic;
using nForum.global;
using nForum.BusinessLogic.Models;

namespace nForum.usercontrols.CLC
{
    public partial class CreateNotice : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void createNotice_Click(object sender, EventArgs e)
        {
            string name = User.GetUser(0).Name;
            string dateTime = DateTime.Now.ToString("MM-dd-yy H:mm:ss");

            // get proper noticeboard folder
            Node noticeBoard = (Node)Node.GetCurrent().ChildrenAsList.First(n => n.NodeTypeAlias == GlobalConstants.NoticeBoardAlias);
            
            Document newDocument = Document.MakeNew(name + "|" + dateTime, DocumentType.GetByAlias(GlobalConstants.NoticeAlias), User.GetUser(0), noticeBoard.Id);
            newDocument.getProperty("content").Value = ((TextBox)this.lvEditPost.FindControl("txtNotice")).Text;
            newDocument.Publish(User.GetUser(0));
            // clear document cache
            umbraco.library.UpdateDocumentCache(newDocument.Id);

            //Response.Redirect(Request.RawUrl);
        }
    }
}