<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularProjects.ascx.cs" Inherits="nForum.usercontrols.CLC.PopularProjects" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div class="fatBorder" id="relevanteProjectenContainer">
    <h3 class="kaffeeSatz c2">Populaire projecten</h3>
    <ul>
            <asp:Repeater ID="rptMainForumList" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <li>
                        <a href="<%# ((ForumCategory)Container.DataItem).Url%>" class="c2">
                            <%# ((ForumCategory)Container.DataItem).Name%>
                        </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
    </ul>
</div>
