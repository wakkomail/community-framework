using System;
using System.Linq;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco.cms.businesslogic.member;

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
				
				ForumCategory currentCategory = Mapper.MapForumCategory(CurrentNode);
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