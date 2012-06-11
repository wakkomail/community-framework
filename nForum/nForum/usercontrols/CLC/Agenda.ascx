<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Agenda.ascx.cs" Inherits="nForum.usercontrols.CLC.Agenda" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<asp:Repeater ID="rptAgenda" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="agendaDate">
            <%# ((Node)Container.DataItem).GetProperty("date").Value %>         
        </div>        
        <b>
            <%# ((Node)Container.DataItem).GetProperty("title").Value %>
        </b>
        <p>
            <%# ((Node)Container.DataItem).GetProperty("description").Value %>
        </p>
    </ItemTemplate>
</asp:Repeater>