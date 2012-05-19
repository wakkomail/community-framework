<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.ascx.cs" Inherits="nForum.MessageBox" %>

    
    <asp:Panel ID="pnlMessageBox" runat="server" CssClass="container" EnableViewState="false">
        <asp:HyperLink runat="server" id="CloseButton" >
            <asp:Image ID="Image1" runat="server" ImageUrl="/nforum/img/msg/close.png" AlternateText="Remove message" />
        </asp:HyperLink>
        <p><asp:Literal ID="litMessage" runat="server" /></p>
    </asp:Panel>