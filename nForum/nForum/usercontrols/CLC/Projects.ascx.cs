﻿using System;
using System.Linq;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using nForum.helpers;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.web;
using umbraco.NodeFactory;
using System.Collections.Generic;
using nForum.helpers.businessobjects;

namespace nForum.usercontrols.CLC
{
    public partial class Projects : BaseForumUsercontrol
	{
        public int AmountToTake { get; set; }
		protected bool UseGridClass
		{
			get 
			{
				return AmountToTake <= 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
				InitializeProjects();
            }
        }

		private void InitializeProjects()
        {
			//Get the projects rootfolder.
			var projectsRoot = DocumentHelper.GetOrCreateProjectgroupCategory();
            var projects = from p in projectsRoot.Children.OrderByDescending(p => p.CreateDateTime)
                           select p;

            if (AmountToTake > 0)
            {
                projects = projects.Take(AmountToTake);
            }
            this.rprProjects.DataSource = projects;
			this.rprProjects.DataBind();
        }

		protected int GetMemberCount(string projectName)
		{
			int count = 0;

			MemberGroup group = MemberGroup.GetByName(projectName);

			if(group != null)
			{
				count = group.GetMembers().Length;
			}

			return count;
		}

		protected string GetNextEvent(int nodeId)
		{
			string result = String.Empty;
            IEnumerable<Node> agendaItems = AgendaHelper.GetUpcomingEvents(nodeId);

			if (agendaItems.Any())
			{
				Node nextEvent = agendaItems.FirstOrDefault(n => Convert.ToDateTime(n.GetProperty("date").Value).Date >= DateTime.Now.Date);
				result = String.Format("Volgende event op: {0}", nextEvent != null ? Convert.ToDateTime(nextEvent.GetProperty("date").Value).ToString("dd-MM-yyyy") : "Geen komende events");
			}
			else
			{
				result = "Volgende event op: -";
			}

			return result;
		}
    }
}