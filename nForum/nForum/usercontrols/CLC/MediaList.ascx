<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaList.ascx.cs" Inherits="nForum.usercontrols.CLC.MediaList" %>
<%@ Import Namespace="umbraco.cms.businesslogic.media" %>
<div id="documentenContainer" class="fatBorder">
	<h3 class="kaffeeSatz c1">
		Documenten</h3>
    <asp:Panel ID="pnlDocumentlist" runat="server">
	    <ul id="documentenList" class="anchorList clearfix">
		    <asp:Repeater ID="rptMedia" runat="server" OnItemCommand="rptMedia_ItemCommand">
			    <ItemTemplate>
				    <li><a href="<%# ((Media)Container.DataItem).getProperty("umbracoFile").Value.ToString() %>"
					    class="c2">
					    <%# ((Media)Container.DataItem).Text %></a>
					    <asp:HyperLink ID="lnkDeleteMedia" runat="server" CssClass="deleteMedia deletebutton"
						    Visible="<%# ShowDelete %>" NavigateUrl="">
						    <div class="confirmDeleteMedia" style="display: none;">
							    <p>
								    Weet u zeker dat u het document "<%# ((Media)Container.DataItem).Text %>" wilt verwijderen?</p>
							    <asp:Button ID="deletebutton" runat="server" Text="Verwijderen" CommandName="delete"
								    CommandArgument="<%# ((Media)Container.DataItem).Id %>" CssClass="button" />
							    <asp:Button ID="cancelbutton" CssClass="cancel button" Text="Annuleren" runat="server" />
						    </div>
					    </asp:HyperLink>
				    </li>
			    </ItemTemplate>
		    </asp:Repeater>
	    </ul>
	    <asp:HyperLink ID="btnShowAll" runat="server" CssClass="showAll c2" Visible="false"
		    Text="Bekijk alle documenten" />
    </asp:Panel>
    <asp:Label ID="lblNoAccess" runat="server" Text="U dient lid te zijn van deze groep om documenten te kunnen zien" Visible="false"></asp:Label>
</div>
