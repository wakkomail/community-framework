<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumListSubForums.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumListSubForums" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<ul class="subforumlist" runat="server" id="subforumlist" Visible="false">
        <li>Sub Forum(s):</li>
        <asp:Repeater ID="rptSubCategories" runat="server" onitemdatabound="HidePrivateSubCategories">
            <ItemTemplate>
                <li><a href="<%# ((ForumCategory)Container.DataItem).Url%>"><%# ((ForumCategory)Container.DataItem).Name%></a></li>
            </ItemTemplate>
        </asp:Repeater>
        </ul>
