<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumMainDashboard.ascx.cs" Inherits="nForum.usercontrols.nForum.dashboard.ForumMainDashboard" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>

<umbraco:Feedback ID="MessageFeedback" runat="server" Visible="false" />
<umbraco:Pane ID="FilterPane" runat="server">        

        <h1>Spam Post Check</h1>
        <p>This dashboard flags up posts which have been voted as negative, this can  either be because they are spam or because they are inappropriate</p>

        <asp:GridView ID="gvBadPosts" runat="server" AutoGenerateColumns="false">
            <Columns>

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>                    
                        <h3 style="padding:6px;"><a href='<%#String.Format("/umbraco/editContent.aspx?id={0}", ((ForumPost)Container.DataItem).Id)%>'>
                            <%# ((ForumPost)Container.DataItem).Name%></a></h3>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Karma">
                    <ItemTemplate>
                        <h3 style="padding:6px;"><%# ((ForumPost)Container.DataItem).Karma%></h3>
                   </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

</umbraco:Pane>