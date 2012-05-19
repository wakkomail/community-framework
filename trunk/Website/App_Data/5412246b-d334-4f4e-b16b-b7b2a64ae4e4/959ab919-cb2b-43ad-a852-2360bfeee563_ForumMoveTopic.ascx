<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumMoveTopic.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumMoveTopic" %>

<asp:Panel ID="forumtopicmove" CssClass="forumtopicmove" runat="server" Visible="false">    
    <h3>Current Category: <asp:Literal ID="litCategory" runat="server"></asp:Literal></h3>
    <p>Move To: <asp:DropDownList ID="ddlCategories" runat="server"></asp:DropDownList></p>
    <p><asp:Button ID="btnMove" runat="server" Text="Move Topic" onclick="BtnMoveClick" /></p>
</asp:Panel>
