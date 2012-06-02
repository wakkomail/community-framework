<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Membergroups.ascx.cs" Inherits="nForum.usercontrols.CLC.Membergroups" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div id="mainforumlist" class="mainforumlist" runat="server">
    <asp:Repeater ID="rptMainForumList" runat="server" onitemdatabound="HidePrivateCategories" EnableViewState="false">       
        <ItemTemplate>
            <div class="forumcategories">

                <div class="forumcategorieslink">
                        <h3><a href="<%# ((ForumCategory)Container.DataItem).Url%>"><%# ((ForumCategory)Container.DataItem).Name%></a></h3>
                        <p class="forumcategoriesdesc"><%# ((ForumCategory)Container.DataItem).Description%></p>
                </div>
                
                <div class="forumcategoriesdetails">
                    <%# ((ForumCategory)Container.DataItem).SubTopicsCount() %> Topics
                </div>

                <div class="forumcategorieslatestpost">
                    <%# GetLastPostInCategory((ForumCategory)Container.DataItem)%>
                </div>

            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>