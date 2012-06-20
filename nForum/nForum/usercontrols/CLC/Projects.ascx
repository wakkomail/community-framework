<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Projects.ascx.cs" Inherits="nForum.usercontrols.CLC.Projects" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<asp:Repeater ID="rprProjects" runat="server" EnableViewState="false">
	<ItemTemplate>
		<%--<div class="topicSummary anchorItem thinBorder clearfix">
			<span class="topicHeader c1">
				<%# ((ForumCategory)Container.DataItem).Name%></span> <b class="topicCount c2">
					<%# ((ForumCategory)Container.DataItem).SubTopicsCount() %>
					topics</b> <span class="topicText c2">
						<%# GetLastPostInCategory((ForumCategory)Container.DataItem)%></span>
			<a class="c1" href="<%# ((ForumCategory)Container.DataItem).Url%>">Bekijk kennisgroep</a>
		</div>--%>
		<div class="topicSummary anchorItem thinBorder clearfix">
			<span class="topicHeader c1">Creative Learning Community</span> <span class="topicText c2">
				Het ontwikkelen van een Creative Learning Community.</span> <span class="memberCount c2">
					Aantal projectleden: 2</span> <span class="nextEvent c2">Volgende event: 22-06-2012</span>
			<a class="c1" href="#">Bekijk project</a>
		</div>
	</ItemTemplate>
</asp:Repeater>