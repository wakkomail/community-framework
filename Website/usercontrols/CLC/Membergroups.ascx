<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Membergroups.ascx.cs" Inherits="nForum.usercontrols.CLC.Membergroups" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div id="mainforumlist" class="mainforumlist" runat="server">
    <asp:Repeater ID="rptMainForumList" runat="server" onitemdatabound="HidePrivateCategories" EnableViewState="false">       
        <ItemTemplate>
			<div class="topicSummary anchorItem thinBorder clearfix">
				<span class="topicHeader c1"><%# ((ForumCategory)Container.DataItem).Name%></span> 
				<b class="topicCount c2"><%# ((ForumCategory)Container.DataItem).SubTopicsCount() %> topics</b>
				<span class="topicText c2"><%# GetLastPostInCategory((ForumCategory)Container.DataItem)%></span>
				<a class="c1" href="<%# ((ForumCategory)Container.DataItem).Url%>">Bekijk kennisgroep</a>
			</div>
        </ItemTemplate>
    </asp:Repeater>
</div>