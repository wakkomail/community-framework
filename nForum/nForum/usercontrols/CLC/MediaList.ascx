<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaList.ascx.cs" Inherits="nForum.usercontrols.CLC.MediaList" %>
<%@ Import Namespace="umbraco.cms.businesslogic.media" %>

<div id="documentenContainer" class="fatBorder">
	<h3 class="kaffeeSatz c1">Documenten</h3>
	<ul id="documentenList" class="anchorList clearfix">
		<asp:Repeater ID="rptMedia" runat="server">
			<ItemTemplate>
				<li>
					<a href="<%# ((Media)Container.DataItem).getProperty("umbracoFile").Value.ToString() %>" class="c2"><%# ((Media)Container.DataItem).Text %></a>
				</li>
			</ItemTemplate>    
		</asp:Repeater>
	</ul>
	<a class="showAll c2" href="#">Bekijk alle documenten</a>
</div>