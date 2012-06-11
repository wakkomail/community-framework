using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nForum.BusinessLogic.Models;
using nForum.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;
using umbraco.BusinessLogic;
using nForum.global;
using nForum.BusinessLogic.Models;

namespace nForum.usercontrols.CLC
{
    public partial class CreateAgendaItem : BaseForumUsercontrol
    {
        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);
        }

        protected void createAgendaItem_Click(object sender, EventArgs e)
        {
            // get proper noticeboard folder
            Node agenda = (Node)Node.GetCurrent().ChildrenAsList.First(n => n.NodeTypeAlias == GlobalConstants.AgendaAlias);
            DateTime itemDate = ((Calendar)this.lvEditItem.FindControl("cldDate")).SelectedDate;

            // todo, first find if current year folder is present, if not, create it. Make this the parent node


            // set date
            Document newDocument = Document.MakeNew(itemDate.ToString("MM-dd-yy H:mm:ss"), DocumentType.GetByAlias(GlobalConstants.AgendaItemAlias), User.GetUser(0), agenda.Id);
            newDocument.getProperty("title").Value = ((TextBox)this.lvEditItem.FindControl("txtTitle")).Text;
            newDocument.getProperty("description").Value = ((TextBox)this.lvEditItem.FindControl("txtDescription")).Text;
            newDocument.getProperty("date").Value = itemDate;
            newDocument.Publish(User.GetUser(0));

            // clear document cache
            umbraco.library.UpdateDocumentCache(newDocument.Id);
        }
    }
}