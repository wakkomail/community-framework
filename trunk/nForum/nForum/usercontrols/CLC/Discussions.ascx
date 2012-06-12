<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Discussions.ascx.cs"
	Inherits="nForum.usercontrols.CLC.Discussions" %>
<%@ Register src="~/usercontrols/CLC/templates/DiscussionRepeaterItem.ascx" tagname="DiscussionRepeaterItem" tagprefix="CLC" %>
<div id="discussiesContainer" class="fatBorder">
	<h3 class="kaffeeSatz c1">
		Discussies</h3>
	<asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">
		<ItemTemplate>
			<CLC:DiscussionRepeaterItem ID="RepeaterItem" FTopic="<%# Container.DataItem %>" runat="server" />
		</ItemTemplate>
	</asp:Repeater>
	<a id="showAll" runat="server" class="showAll c2" href="/Discussions.aspx" visible="false">Bekijk alle discussies</a>
</div>

<asp:Literal ID="litPager" runat="server" />