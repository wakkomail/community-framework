<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmallDiscussionRepeaterItem.ascx.cs" Inherits="nForum.usercontrols.CLC.templates.SmallDiscussionRepeaterItem" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<div class="topicSummary anchorItem thinBorder clearfix">
	<span class="topicHeader c1"><%# FTopic.Name%></span> 
	<b class="topicDate c2">
		<%# Helpers.GetPrettyDate(FTopic.CreatedOn.ToString())%>
	</b> 
	<span class="topicText c2"><%# GetFirstPost(FTopic)%></span>
	<a class="c1" href="<%# FTopic.Url%>"<%# ShowAjaxPostLink(FTopic.Id)%>>Lees meer</a>
</div>