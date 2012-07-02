﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateNotice.ascx.cs" Inherits="nForum.usercontrols.CLC.CreateNotice" %>

<asp:LoginView ID="lvEditPost" runat="server">
    <LoggedInTemplate>
        <h3 class="kaffeeSatz">Nieuwe notitie</h3>

        <asp:TextBox ID="txtNotice" TextMode="MultiLine" Rows="4" runat="server" ClientIDMode="Static"></asp:TextBox>
        <asp:Button ID="createNotice" ClientIDMode="Static" runat="server" 
            CssClass="button" Text="Notitie aanmaken" onclick="createNotice_Click" />
    </LoggedInTemplate>
</asp:LoginView>