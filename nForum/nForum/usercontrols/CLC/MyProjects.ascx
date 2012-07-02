<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyProjects.ascx.cs" Inherits="nForum.usercontrols.CLC.MyProjects" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="umbraco.cms.businesslogic.web" %>
<asp:Repeater ID="rprProjects" runat="server" EnableViewState="false">
	<ItemTemplate>
		<div class="topicSummary anchorItem thinBorder clearfix">
			<span class="topicHeader c1"><%# ((Document)Container.DataItem).Text %></span> <span class="topicText c2">
				<%# ((Document)Container.DataItem).getProperty("forumCategoryDescription").Value %></span> <span class="memberCount c2">
					Aantal projectleden: <%# GetMemberCount(((Document)Container.DataItem).Text)%></span>
					<span class="nextEvent c2"><%# GetNextEvent(((Document)Container.DataItem).Id)%></span>
			<a class="c1" href="<%# umbraco.library.NiceUrl(((Document)Container.DataItem).Id) %>">Bekijk project</a>
		</div>
	</ItemTemplate>
</asp:Repeater>