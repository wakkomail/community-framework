<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadMedia.ascx.cs" Inherits="nForum.usercontrols.nForum.UploadMedia" %>
<asp:Panel ID="pnlUpload" runat="server">
    <asp:FileUpload ID="uploadMedia" runat="server" />
    <asp:Button ID="btnUpload" runat="server" Text="Uploaden" 
        onclick="btnUpload_Click" />
</asp:Panel>
