using System;
using System.Linq;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco.cms.businesslogic.member;
using umbraco.NodeFactory;

namespace nForum.usercontrols.CLC
{
	public partial class MembersList : BaseForumUsercontrol
	{
		#region Properties

		public bool ShowNewest { get; set; }

		#endregion

		#region Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			GetMembers();
		}

		private void GetMembers()
		{
			if (ShowNewest)
			{
				this.header.InnerHtml = "Nieuwe leden";
				//var group = MemberGroup.GetByName(MembershipHelper.ForumUserRoleName);
				var members = Member.GetAll.OrderByDescending(x => x.CreateDateTime).Take(5);//group.GetMembers().OrderByDescending(x => x.CreateDateTime).Take(5);

				if (members.Any())
				{
					rprMembers.DataSource = members;
					rprMembers.DataBind();
				}
			}
			else
			{
				this.header.InnerHtml = "Groepsleden";
				ForumCategory currentCategory = null;

				switch (CurrentNode.NodeTypeAlias)
				{
					case global.GlobalConstants.MembergroupAlias:
					case global.GlobalConstants.ProjectAlias:
						currentCategory = Mapper.MapForumCategory(CurrentNode);
					break;
					case global.GlobalConstants.DiscussionAlias:
						ForumTopic currentTopic = Mapper.MapForumTopic(CurrentNode);

						Node categoryNode = new Node(currentTopic.CategoryId);
						currentCategory = Mapper.MapForumCategory(categoryNode);
					break;
					default:
						break;
				}


				if(currentCategory != null)
				{
					MemberGroup group = MemberGroup.GetByName(currentCategory.Name);
					
					if(group != null)
					{
						rprMembers.DataSource = group.GetMembers();
						rprMembers.DataBind();
					}
				}

			}
		}

		#endregion
	}
}