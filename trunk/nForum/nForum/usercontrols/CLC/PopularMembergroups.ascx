<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularMembergroups.ascx.cs" Inherits="nForum.usercontrols.CLC.PopularMembergroups" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div class="fatBorder" id="relevanteKennisgroepenContainer">
    <h3 class="kaffeeSatz c2">Populaire kennisgroepen</h3>
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
