<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Agenda.ascx.cs" Inherits="nForum.usercontrols.CLC.Agenda" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<div id="agendaContainer" class="fatBorder">
<h3 class="kaffeeSatz c1">Agenda</h3>
<asp:Repeater ID="rptAgenda" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="agendaDate">
            <%# Convert.ToDateTime(((Node)Container.DataItem).GetProperty("date").Value).ToString("m")  %>         
        </div>        
        <p>
            <b>Afspraak: </b><%# ((Node)Container.DataItem).GetProperty("title").Value %>
        </p>
        <p>
            <b>Omschrijving: </b><%# ((Node)Container.DataItem).GetProperty("description").Value %>
        </p>
    </ItemTemplate>
</asp:Repeater>
<p>
	<asp:HyperLink ID="btnShowAll" runat="server" CssClass="showAll c2" Visible="false" Text="Bekijk de volledige agenda" />
</p>
</div>