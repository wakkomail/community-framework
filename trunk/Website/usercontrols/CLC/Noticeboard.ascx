<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Noticeboard.ascx.cs" Inherits="nForum.usercontrols.CLC.Noticeboard" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

<h3 class="kaffeeSatz">Notities</h3>
<asp:Repeater ID="rptNoticeBoard" runat="server" EnableViewState="false">
    <ItemTemplate>
        <p>
            <%# ((Node)Container.DataItem).GetProperty("content").Value %>
        </p>
	    <b class="topicDate c2">
		    <%# Helpers.GetPrettyDate(((Node)Container.DataItem).CreateDate.ToString())%> |
            <%# ((Node)Container.DataItem).CreatorName  %> |            
	    </b> 
    </ItemTemplate>
</asp:Repeater>

<asp:HyperLink ID="lnkNoticeboard" runat="server" Visible="false">Bekijk alle notities</asp:HyperLink>