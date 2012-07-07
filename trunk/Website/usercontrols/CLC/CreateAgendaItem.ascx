<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateAgendaItem.ascx.cs" Inherits="nForum.usercontrols.CLC.CreateAgendaItem" %>

<asp:LoginView ID="lvEditItem" runat="server">
    <LoggedInTemplate>
        <h2>Agenda item toevoegen</h2>
        <p>
            Titel
        </p>
        <p>
            <asp:TextBox ID="txtTitle" Rows="1" runat="server" ClientIDMode="Static" CssClass="maxWidthInput"></asp:TextBox>
        </p>
        <p>
            Omschrijving
        </p>
        <p>
            <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="4" runat="server" ClientIDMode="Static" CssClass="maxWidthInput"></asp:TextBox>
        </p>
        <p>
            Datum
        </p>
        <p>
            <asp:Calendar ID="cldDate" runat="server" SelectedDate="<%# System.DateTime.Now.Date %>"></asp:Calendar>
        </p>
        
        <asp:Button ID="createAgendaItem" runat="server" CssClass="button" 
            Text="Agenda item aanmaken" onclick="createAgendaItem_Click" />
    </LoggedInTemplate>
</asp:LoginView>