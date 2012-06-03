<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Membergroup.ascx.cs"
	Inherits="nForum.usercontrols.CLC.Membergroup" %>
<%@ Register src="~/usercontrols/CLC/templates/DiscussionRepeaterItem.ascx" tagname="DiscussionRepeaterItem" tagprefix="CLC" %>
<%@ Register Src="~/usercontrols/nForum/ForumTopicsSticky.ascx" TagName="stickytopics"
	TagPrefix="nforum" %>
<h2 class="c1 kaffeeSatz">
	<asp:Literal ID="litHeading" runat="server" />
</h2>
<div id="description">
	<asp:Literal ID="litDescription" runat="server" />
</div>
<div id="topiclisthead">
	<div id="topiclistheadbuttons">
		<asp:HyperLink ID="lRss" runat="server" Visible="false" Text="Rss Feed For This Topic" />
		<p>
			<asp:HyperLink ID="hlCreateTopic" runat="server" CssClass="forumbutton" Visible="false"
				Text="Create a new topic" /></p>
	</div>
</div>
<div id="discussiesContainer" class="fatBorder">
	<h3 class="kaffeeSatz c1">
		Discussies</h3>
	<asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">
		<ItemTemplate>
			<CLC:DiscussionRepeaterItem ID="RepeaterItem" FTopic="<%# Container.DataItem %>" runat="server" />
		</ItemTemplate>
	</asp:Repeater>
	<a class="showAll c2" href="#">Bekijk alle discussies</a>
</div>

<asp:Literal ID="litPager" runat="server" />