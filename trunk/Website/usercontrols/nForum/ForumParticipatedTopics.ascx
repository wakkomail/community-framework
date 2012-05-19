<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumParticipatedTopics.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumParticipatedTopics" %>
<%@ Register src="~/usercontrols/nForum/templates/RepeaterItemTopics.ascx" tagname="RepeaterItemTopics" tagprefix="nforum" %>

<div id="topiclist" class="topiclist" runat="server">
    <asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">       
        <ItemTemplate>
            
             <nforum:RepeaterItemTopics ID="RepeaterItemTopics1" FTopic="<%# Container.DataItem %>" runat="server" />

        </ItemTemplate>
    </asp:Repeater>
</div>