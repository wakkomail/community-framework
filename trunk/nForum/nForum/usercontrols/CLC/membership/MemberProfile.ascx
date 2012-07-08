<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberProfile.ascx.cs"
	Inherits="nForum.usercontrols.CLC.membership.MemberProfile" %>
<%@ Register Src="~/usercontrols/CLC/templates/SmallDiscussionRepeaterItem.ascx"
	TagName="SmallDiscussionRepeaterItem" TagPrefix="CLC" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<div class="grid_12" id="kruimelpad">
	<a href="/" class="c2">Creative Learning Community</a> &gt; <a href="/leden.aspx"
		class="c2">Leden</a> &gt; 
		<asp:Literal ID="litBreadCrumbsName" runat="server" /> 
</div>
<div id="memberprofile" class="memberprofile" runat="server">
	<div class="fatBorder grid_9">
		<h2 class="c1 kaffeeSatz">
			<asp:Literal ID="litMemberName" runat="server" EnableViewState="false" />
		</h2>
		<div id="memberprofileleft" class="grid_9 alpha omega fatBorder">
			<h3 class="kaffeeSatz c1">
				Persoonlijke gegevens</h3>
			<div class="profileItem clearfix">
				<span class="label">Naam</span> <span class="separator">:</span> <span class="value">
					<asp:Literal ID="litFullName" runat="server" /></span>
			</div>
			<div class="profileItem clearfix">
				<span class="label">Lid sinds</span> <span class="separator">:</span> <span class="value">
					<asp:Literal ID="litJoined" runat="server" /></span>
			</div>
			<div class="profileItem clearfix">
				<span class="label">Aantal reacties</span> <span class="separator">:</span> <span
					class="value">
					<asp:Literal ID="litPosts" runat="server" /></span>
			</div>
			<h3 class="kaffeeSatz c1">
				Contactgegevens</h3>
			<div class="profileItem clearfix">
				<span class="label">E-mailadres</span> <span class="separator">:</span> <span class="value">
					<asp:Literal ID="litEmail" runat="server" /></span>
			</div>
			<asp:LinkButton ID="linkBanMember" runat="server" CssClass="forumbutton" Visible="false"
				OnClick="LinkBanMemberClick" />
		</div>
	</div>
	<div id="memberprofileright" class="grid_3 fatBorder">
		<h3 class="kaffeeSatz c2">
			Recente activiteit</h3>
		<asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">
			<ItemTemplate>
				<CLC:SmallDiscussionRepeaterItem ID="DiscussionRepeaterItem" FTopic="<%# Container.DataItem %>"
					runat="server" />
			</ItemTemplate>
		</asp:Repeater>
	</div>
</div>
