<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopularDiscussions.ascx.cs" Inherits="nForum.usercontrols.CLC.PopularDiscussions" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<%@ Register Src="~/usercontrols/CLC/templates/SmallDiscussionRepeaterItem.ascx" TagName="SmallDiscussionRepeaterItem" TagPrefix="CLC" %>

<div class="fatBorder" id="relevanteDiscussiesContainer">
    <h3 class="kaffeeSatz c2">Populaire discussies</h3>
    <ul>
        <asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">
            <ItemTemplate>
                <li class="relevantTopic">
                    <a href="<%# ((ForumTopic)Container.DataItem).Url%>" class="c2">
                        <%# Helpers.GetPrettyDate(((ForumTopic)Container.DataItem).LastPostDate.ToString()) %>
                        <br />
                        <%# ((ForumTopic)Container.DataItem).Name %>
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
