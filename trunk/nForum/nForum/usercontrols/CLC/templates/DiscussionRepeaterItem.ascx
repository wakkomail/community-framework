<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionRepeaterItem.ascx.cs" Inherits="nForum.usercontrols.CLC.templates.DiscussionRepeaterItem" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<div class="topicSummary thinBorder clearfix">
	<span class="topicHeader c1"><%# FTopic.Name%></span> 
	<span class="topicText c2"><%# GetFirstPost(FTopic)%></span>
	<div class="topicInfo">
		<b><%# FTopic.CreatedOn.ToString("dd-MM-yyyy")%></b> <b class="separator">|</b> <b><%# FTopic.CreatedOn.ToString("HH:mm")%></b> <b class="separator">|</b>
		<b>Door: <%# MembershipHelper.ReturnMemberProfileLink(FTopic.Owner.MemberLoginName, "c2", FTopic.Owner.MemberId, null)%></b> <b class="separator">|</b>
		<b>Reacties: <%# (FTopic.PostCount() - 1) %></b> <b class="separator">|</b> <a href="<%# FTopic.Url%>"<%# ShowAjaxPostLink(FTopic.Id)%>>Lees meer</a>
	</div>
</div>