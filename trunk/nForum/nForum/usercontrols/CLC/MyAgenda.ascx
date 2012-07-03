<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyAgenda.ascx.cs" Inherits="nForum.usercontrols.CLC.MyAgenda" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<div id="agendaContainer" class="fatBorder">
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
</div>


TEST