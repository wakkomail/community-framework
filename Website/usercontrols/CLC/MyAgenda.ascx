﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyAgenda.ascx.cs" Inherits="nForum.usercontrols.CLC.MyAgenda" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<div id="agendaContainer" class="fatBorder">
<asp:Repeater ID="rptAgenda" runat="server" EnableViewState="false">
    <ItemTemplate>
            <a href="<%# umbraco.library.NiceUrl(GetProjectID(((Node)Container.DataItem).Id)) %>" class="c2">
                <div class="agendaDate">
                    <span>
                        <%# Convert.ToDateTime(((Node)Container.DataItem).GetProperty("date").Value).ToString("dd MMM")  %>        
                    </span> 
                </div>        
                <p>
                    <b>Afspraak: </b><%# ((Node)Container.DataItem).GetProperty("title").Value %>
                </p>
                <p>
                    <b>Omschrijving: </b><%# ((Node)Container.DataItem).GetProperty("description").Value %>
                </p>       
            </a>        
    </ItemTemplate>
</asp:Repeater>
</div>