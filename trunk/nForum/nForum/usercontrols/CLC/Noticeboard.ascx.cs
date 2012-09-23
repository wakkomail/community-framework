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
using umbraco.cms.businesslogic.template;
using umbraco.interfaces;
using nForum.helpers;
using nForum.global;

namespace nForum.usercontrols.CLC
{
    public partial class Noticeboard : BaseForumUsercontrol
    {
        #region Properties

        public bool ShowAll { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();            
        }

        private void Initialize()
        {
            Node noticeBoard = null;

            // get proper noticeboard folder
            if (Node.GetCurrent().NodeTypeAlias == GlobalConstants.NoticeBoardAlias)
            {
                noticeBoard = Node.GetCurrent();
            }
            else
            {
                noticeBoard = (Node)Node.GetCurrent().ChildrenAsList.FirstOrDefault(n => n.NodeTypeAlias == GlobalConstants.NoticeBoardAlias);
            }
             
           
			if(noticeBoard != null)
			{
               List<INode> notices = noticeBoard.ChildrenAsList.OrderByDescending(n => n.CreateDate).ToList();

                if (!ShowAll)
                {
                    notices = notices.Take(3).ToList();
                    lnkNoticeboard.Visible = true;
                }
				// get list of notice items and bind it to the repeater
                this.rptNoticeBoard.DataSource = notices;
				this.rptNoticeBoard.DataBind();
			}

            // set link to the noticeboard
            var noticeboard = CurrentNode.ChildrenAsList.FirstOrDefault(n => n.NodeTypeAlias == GlobalConstants.NoticeBoardAlias);
            if (noticeboard != null)
            {
                this.lnkNoticeboard.NavigateUrl = noticeboard.Url;
            }
        }

		protected string GetCreatedBy(Node notice)
		{
			return notice.GetProperty("createdBy") != null ? notice.GetProperty("createdBy").Value : "-"; 
		}
    }
}