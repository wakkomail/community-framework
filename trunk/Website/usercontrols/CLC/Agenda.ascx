<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Agenda.ascx.cs" Inherits="nForum.usercontrols.CLC.Agenda" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<h3 class="kaffeeSatz c1">Agenda</h3>
<asp:Repeater ID="rptAgenda" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="agendaDate">
            <%# Convert.ToDateTime(((Node)Container.DataItem).GetProperty("date").Value).ToString("m")  %>         
        </div>        
        <b>
            <%# ((Node)Container.DataItem).GetProperty("title").Value %>
        </b>
        <p>
            <%# ((Node)Container.DataItem).GetProperty("description").Value %>
        </p>
    </ItemTemplate>
</asp:Repeater>
<a id="showAll" runat="server" class="showAll c2" href="/Agenda.aspx" visible="false">Bekijk alle discussies</a>