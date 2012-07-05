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
			<asp:HyperLink ID="hlCreateTopic" runat="server" CssClass="forumbutton c1" Visible="false"
				Text="Maak een nieuwe discussie aan" /></p>
	</div>
</div>