<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.ascx.cs" Inherits="nForum.usercontrols.CLC.SearchResult" %>

<asp:Repeater ID="rptSearchResult" runat="server" 
    onitemdatabound="rptSearchResult_ItemDataBound">
<ItemTemplate>
    <div class="topicSummary anchorItem thinBorder clearfix">
        <span class="topicHeader c1">
            <asp:Label ID="lblSearchResultType" runat="server" Text="Label"></asp:Label>  
            <asp:Label ID="lblNodeText" runat="server" Text="Label"></asp:Label> 
            <asp:HyperLink ID="lnkResult" runat="server">HyperLink</asp:HyperLink>
        </span>
    </div>
</ItemTemplate>
</asp:Repeater>

<asp:Label ID="lblNoResults" runat="server" Text="Geen resultaten gevonden" CssClass="noSearchResults" Visible="false"></asp:Label>