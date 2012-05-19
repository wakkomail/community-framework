<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumTopics.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumTopics" %>
<%@ Register src="~/usercontrols/nForum/templates/RepeaterItemTopics.ascx" tagname="RepeaterItemTopics" tagprefix="nforum" %>
<%@ Register src="~/usercontrols/nForum/ForumTopicsSticky.ascx" tagname="stickytopics" tagprefix="nforum" %>

    <div id="topiclisthead">
        <div id="topiclistheadtitle">
            <h2><asp:Literal ID="litHeading" runat="server" /></h2>
            <p><asp:Literal ID="litDescription" runat="server" /></p>
        </div>
        <div id="topiclistheadbuttons">
            <asp:HyperLink ID="lRss" runat="server" Visible="false" Text="Rss Feed For This Topic" />
            <p><asp:HyperLink ID="hlCreateTopic" runat="server" CssClass="forumbutton" Visible="false" Text="Create a new topic" /></p>
        </div>
    </div>

<nforum:stickytopics ID="rptSticky" runat="server" />

<div id="topiclist" class="topiclist" runat="server">
    <asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">       
        <ItemTemplate>
            <nforum:RepeaterItemTopics ID="RepeaterItemTopics1" FTopic="<%# Container.DataItem %>" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</div>

<asp:Literal ID="litPager" runat="server" />

