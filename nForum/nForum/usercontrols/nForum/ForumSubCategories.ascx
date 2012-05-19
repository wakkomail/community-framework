<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumSubCategories.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumSubCategories" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<asp:Panel ID="pnlSubCategories" runat="server" CssClass="subcategories" Visible="false">
    <div id="subcategoriesinner">

        <h6>Sub Categories</h6>
        <ul class="subcategorylist">
        <asp:Repeater ID="rptSubCategories" runat="server" onitemdatabound="HidePrivateSubCategories">
            <ItemTemplate>
                <li><a href="<%# ((ForumCategory)Container.DataItem).Url%>"><%# ((ForumCategory)Container.DataItem).Name%></a></li>
            </ItemTemplate>
        </asp:Repeater>
        </ul>

    </div>
</asp:Panel>
