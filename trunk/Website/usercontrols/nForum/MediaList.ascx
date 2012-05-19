<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaList.ascx.cs" Inherits="nForum.usercontrols.nForum.MediaList" %>
<%@ Import Namespace="umbraco.cms.businesslogic.media" %>

<asp:Repeater ID="rptMedia" runat="server">
    <ItemTemplate>
        <p class="documentlink">
            <asp:HyperLink ID="lnkDocument" runat="server" NavigateUrl='<%# ((Media)Container.DataItem).getProperty("umbracoFile").Value.ToString() %>' Text='<%# ((Media)Container.DataItem).Text %>' >
                    
            </asp:HyperLink>
        </p>
        <p>
            <asp:Image ID="imgDocument" runat="server" Visible='<%# IsImage(((Media)Container.DataItem).Text) %>' ImageUrl='<%# GetThumbnail(((Media)Container.DataItem).getProperty("umbracoFile").Value.ToString())%>' />                                
        </p>
    </ItemTemplate>    
</asp:Repeater>
