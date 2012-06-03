<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembersList.ascx.cs" Inherits="nForum.usercontrols.CLC.MembersList" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<div id="ledenContainer" class="fatBorder">
	<h3 id="header" class="kaffeeSatz c2" runat="server"></h3>
	<ul>
		<asp:Repeater ID="rprMembers" runat="server">
			<ItemTemplate>
				<li>
					<%# MembershipHelper.ReturnMemberProfileLink(((Member)Container.DataItem).LoginName, "c2", ((Member)Container.DataItem).Id, null)%>
				</li>
			</ItemTemplate>
		</asp:Repeater> 
	</ul>
</div>