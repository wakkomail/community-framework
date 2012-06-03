<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveDiscussions.ascx.cs" Inherits="nForum.usercontrols.CLC.ActiveDiscussions" %>
<%@ Register src="~/usercontrols/CLC/templates/SmallDiscussionRepeaterItem.ascx" tagname="SmallDiscussionRepeaterItem" tagprefix="CLC" %>

<asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">       
    <ItemTemplate>  
        <CLC:SmallDiscussionRepeaterItem ID="DiscussionRepeaterItem" FTopic="<%# Container.DataItem %>" runat="server" />
    </ItemTemplate>
</asp:Repeater>