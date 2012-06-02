<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveDiscussions.ascx.cs" Inherits="nForum.usercontrols.CLC.ActiveDiscussions" %>
<%@ Register src="~/usercontrols/CLC/templates/DiscussionRepeaterItem.ascx" tagname="DiscussionRepeaterItem" tagprefix="CLC" %>

<asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">       
    <ItemTemplate>  
        <CLC:DiscussionRepeaterItem ID="DiscussionRepeaterItem" FTopic="<%# Container.DataItem %>" runat="server" />
    </ItemTemplate>
</asp:Repeater>