<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumMemberProfile.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.ForumMemberProfile" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div id="memberprofile" class="memberprofile" runat="server">
		<div id="memberprofileleft">
        		<h1><asp:Literal ID="litMemberName" runat="server" EnableViewState="false" /></h1>
                
                <p><asp:Literal ID="litGravatar" runat="server" EnableViewState="false" /></p>

                <p>Joined: <asp:Literal ID="litJoined" runat="server" EnableViewState="false" /></p>

                <p>Twitter: <asp:Literal ID="litTwitter" runat="server" EnableViewState="false" /></p>

                <p>Posts: <asp:Literal ID="litPosts" runat="server" EnableViewState="false" /></p>

                <p>Karma: <asp:Literal ID="litKarma" runat="server" EnableViewState="false" /></p>

                <asp:HyperLink ID="linkMessageUser" runat="server" CssClass="forumbutton" Visible="false" />               
                <asp:HyperLink ID="linkReportMember" runat="server" CssClass="forumbutton" Visible="false" />
                <asp:LinkButton ID="linkBanMember" runat="server" CssClass="forumbutton" Visible="false" onclick="LinkBanMemberClick" />

        </div>
      	<div id="memberprofileright">
        	<h4>Recent Activity</h4>

                    <asp:Repeater ID="rptTopicList" runat="server" EnableViewState="false">       
                    <ItemTemplate>
                        <div class="memberprofiletopics">

                            <div class='memberprofilelatestpost'>
                                    <h6><a href="<%# ((ForumTopic)Container.DataItem).Url%>"><%# ((ForumTopic)Container.DataItem).Name%></a></h6>
                                    <p class="topicsubtext">Started By <%# ((ForumTopic)Container.DataItem).Owner.MemberLoginName%> - <%# Helpers.GetPrettyDate(((ForumTopic)Container.DataItem).CreatedOn.ToString())%></p>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>
        </div>
  </div>