<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumListPostsInTopic.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumListPostsInTopic" %>
<%@ Register src="~/usercontrols/nForum/includes/Emoticons.ascx" tagname="Emoticons" tagprefix="nforum" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div id="topiclisthead">
        <div id="topiclistheadtitle">
            <h1 id="topicheading"><asp:Literal ID="litHeading" runat="server" /></h1>
        </div>
        <div id="topiclistheadbuttons">
            
            <p><asp:HyperLink ID="hlEmailSubscribe" runat="server" CssClass="notsubscribedtotopic" Visible="false" Text="Subscribe To Topic" /></p>
        </div>
    </div>

<div id="topicpostlist" class="topicpostlist" runat="server">
    <asp:Repeater ID="rptTopicPostList" runat="server" onitemdatabound="SortIndividualPostActions">       
        <ItemTemplate>
            <div class="forumpost<%#IsSolution(((ForumPost)Container.DataItem).IsSolution)%> forumpost<%#((ForumPost)Container.DataItem).Id%>">
                
                <div class="forumpostmemberdetails">                
                    <div class="forumpostmemberdetailsinner">
                    <a name="comment<%#((ForumPost)Container.DataItem).Id%>"></a>
                        <span>
                            <asp:Literal ID="litMemberGravatar" runat="server" />
                        </span>
                        <span><asp:Literal ID="litMemberPosts" runat="server" /></span>
                            Posts
                        <span><asp:Literal ID="litMemberKarma" runat="server" /></span>
                            Karma
                    </div>
                    <asp:Literal ID="litUserAccreds" runat="server" />
                </div>
                
                <div class="wmd-output forumpostbody">
                    
                    <div class="forumpostbodytop">
                        <asp:Literal ID="litMemberName" runat="server" /> posted this <%# Helpers.GetPrettyDate(((ForumPost)Container.DataItem).CreatedOn.ToString())%> 
                        <asp:HyperLink ID="lSpamReport" CssClass="spamreportlink" Visible="false" runat="server">Spam?</asp:HyperLink>
                    </div>

                    <asp:LoginView ID="lvSolution" runat="server">
                        <LoggedInTemplate>
                                    <asp:Panel ID="forumpostisolution" Visible="false" CssClass="forumpostisolution" runat="server">
                                        <asp:HyperLink ID="lsolution" runat="server" CssClass="marksolution" NavigateUrl="#">Is Solution</asp:HyperLink>
                                    </asp:Panel>
                        </LoggedInTemplate>
                    </asp:LoginView>

                    <div id="postcontent<%#((ForumPost)Container.DataItem).Id%>">
                    <%# ((ForumPost)Container.DataItem).Content.ConvertBbCode() %>
                    </div>

                    <asp:Literal ID="litLastEdited" runat="server" />
                    


                </div>

                <div class="forumpostkarmaouter">
                
                    <asp:Panel CssClass="forumpostkarma" runat="server" ID="forumpostkarma">
                        <span class="karmascore karma<%#((ForumPost)Container.DataItem).Id%>"><%# GetPostKarma(((ForumPost)Container.DataItem).Karma)%></span>
                        <asp:LoginView ID="lvKarma" runat="server">
                            <LoggedInTemplate>
                                        <asp:Panel ID="forumpostkarmathumbs" runat="server" Visible="false">
                                            <div class="forumpostkarmathumbup">
                                                <asp:HyperLink ID="lthumbuplink" runat="server" ImageUrl="/nforum/img/thumbup.jpg" NavigateUrl="#" Text="Vote Post Up" ToolTip="Vote Post Up" CssClass="thumbuplink" />
                                            </div>
                                            <div class="forumpostkarmathumbdown">
                                                <asp:HyperLink ID="lthumbdownlink" runat="server" ImageUrl="/nforum/img/thumbdown.jpg" NavigateUrl="#" Text="Vote Post Down" ToolTip="Vote Post Down" CssClass="thumbdownlink" />
                                            </div>
                                        </asp:Panel>
                            </LoggedInTemplate>
                        </asp:LoginView>
                    </asp:Panel>

                    <div class="postadminui">
                        <asp:HyperLink ID="quotebutton" CssClass="quotebutton" runat="server" Visible="false">Quote</asp:HyperLink>
                        <asp:Panel ID="pnlUserAdmin" CssClass="userpostadmin" runat="server" Visible="false">
                            <asp:HyperLink ID="lUserEdit" runat="server">Edit</asp:HyperLink>
                            <a href="/deletepost.aspx?p=<%# ((ForumPost)Container.DataItem).Id%>">Delete</a>
                        </asp:Panel>
                        <asp:HyperLink ID="lMoveTopic" runat="server" Visible="false">Move</asp:HyperLink>
                    </div>

                </div>                

            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<asp:Literal ID="litPager" runat="server" />

<div id="topicpostlistnewpost" class="topicpostlistnewpost">
        <asp:LoginView ID="lvNewPost" runat="server">
            <LoggedInTemplate>
                <a name="maineditor"></a>
                <h6>Create New Post</h6>
                <nforum:Emoticons ID="emoticonInclude" runat="server" />
                <asp:TextBox ID="txtPost" runat="server" TextMode="MultiLine" Rows="14" ClientIDMode="Static"></asp:TextBox>
                <p class="submitbutton"><asp:Button ID="btnSubmitPost" CssClass="btnsubmitpost" runat="server" Text="Submit Post" /></p>
            </LoggedInTemplate>
        </asp:LoginView>
</div>

<div class="postsuccess" style="display: none;">
  <h4 style="text-align: center;">Posting your reply...</h4>
</div>

