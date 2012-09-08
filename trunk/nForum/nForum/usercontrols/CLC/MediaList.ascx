<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaList.ascx.cs" Inherits="nForum.usercontrols.CLC.MediaList" %>
<%@ Import Namespace="umbraco.cms.businesslogic.media" %>

<div id="documentenContainer" class="fatBorder">
	<h3 class="kaffeeSatz c1">Documenten</h3>
	<ul id="documentenList" class="anchorList clearfix">
		<asp:Repeater ID="rptMedia" runat="server" onitemcommand="rptMedia_ItemCommand">
			<ItemTemplate>
				<li>
					<a href="<%# ((Media)Container.DataItem).getProperty("umbracoFile").Value.ToString() %>" class="c2"><%# ((Media)Container.DataItem).Text %></a>

                    <a href="#" class="deleteMedia deletebutton">
                         <div class="confirmDeleteMedia" style="display: none;">
                         <p>Weet u zeker dat u het document "<%# ((Media)Container.DataItem).Text %>" wilt verwijderen?</p>
                            <asp:Button ID="deletebutton" runat="server" Text="Verwijderen" CommandName="delete" CommandArgument="<%# ((Media)Container.DataItem).Id %>"  CssClass="button" />
                            <asp:Button ID="cancelbutton" CssClass="cancel button" Text="Annuleren" runat="server"/>
                        </div>
                    </a>
				</li>
			</ItemTemplate>    
		</asp:Repeater>
	</ul>
	<a class="showAll c2" href="#">Bekijk alle documenten</a>
</div>