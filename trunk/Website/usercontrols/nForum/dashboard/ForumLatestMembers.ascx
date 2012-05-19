<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumLatestMembers.ascx.cs" Inherits="nForum.usercontrols.nForum.dashboard.ForumLatestMembers" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>

<umbraco:Feedback ID="MessageFeedback" runat="server" Visible="false" />
<umbraco:Pane ID="FilterPane" runat="server">        

        <h1>Latest Registered Members</h1>
        <p>This dashboard shows the latest members to sign up to your forum, if you are pre-authorising members then you can use this to quickly authorise new members.</p>

        <asp:GridView ID="gvMembers" runat="server" AutoGenerateColumns="false">
            <Columns>

                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <h3 style="padding:6px;">
                            <a href='/umbraco/members/editMember.aspx?id=<%#((Member)Container.DataItem).Id%>'>
                                <%# ((Member)Container.DataItem).LoginName%>
                            </a>
                        </h3>
                    </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <h5 style="padding:6px;"><%# ((Member)Container.DataItem).Text%></h5>
                   </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Email Address">
                    <ItemTemplate>
                        <h5 style="padding:6px;"><%# ((Member)Container.DataItem).Email%></h5>
                   </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Registered">
                    <ItemTemplate>
                        <h5 style="padding:6px;"><%# ((Member)Container.DataItem).CreateDateTime.ToShortDateString()%></h5>
                   </ItemTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Authorised">
                    <ItemTemplate>
                        <h3 style="padding:6px;"><%# IsAuthorsied(((Member)Container.DataItem).getProperty("forumUserIsAuthorised").Value.ToString())%></h3>
                   </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

</umbraco:Pane>