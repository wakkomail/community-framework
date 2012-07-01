<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Discussion.ascx.cs"
	Inherits="nForum.usercontrols.CLC.Discussion" %>
<%@ Register Src="~/usercontrols/nForum/includes/Emoticons.ascx" TagName="Emoticons"
	TagPrefix="nforum" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<h2 class="c1 kaffeeSatz">
	<asp:Literal ID="litHeading" runat="server" /></h2>
<asp:Repeater ID="rprDiscussionPosts" runat="server" OnItemDataBound="InitializePost">
	<ItemTemplate>
		<div class="post forumpost<%#((ForumPost)Container.DataItem).Id%> thinBorder">
			<div class="postDetails">
				<a name="comment<%#((ForumPost)Container.DataItem).Id%>"></a><b>Geplaatst:
					<%# Helpers.GetPrettyDate(((ForumPost)Container.DataItem).CreatedOn.ToString())%></b>
				<b>Door:
					<asp:Literal ID="litMemberName" runat="server" /></b>
			</div>
			<div id="postcontent<%#((ForumPost)Container.DataItem).Id%>" class="postContent">
				<%# ((ForumPost)Container.DataItem).Content.ConvertBbCode() %>
				<asp:Literal ID="litLastEdited" runat="server" />
			</div>
			<div class="postActions">
				<asp:HyperLink ID="quotebutton" CssClass="c1 quotebutton" runat="server" Visible="false">Quote</asp:HyperLink>
				<asp:Panel ID="pnlUserAdmin" CssClass="userpostadmin" runat="server" Visible="false">
					<asp:HyperLink ID="lUserEdit" runat="server" CssClass="c1">Bewerk</asp:HyperLink>
					<a href="/deletepost.aspx?p=<%# ((ForumPost)Container.DataItem).Id%>" class="c1">Verwijder</a>
				</asp:Panel>
			</div>
		</div>
	</ItemTemplate>
</asp:Repeater>
<asp:Panel ID="pnlPager" runat="server" Visible="false">
	<span class="c1 pagerLabel">Pagina's: </span>
	<asp:Literal ID="litPager" runat="server" />
</asp:Panel>
<div id="newPost" class="fatBorder">
	<asp:LoginView ID="lvNewPost" runat="server">
		<LoggedInTemplate>
			<a name="maineditor"></a>
			<h3 class="c1 kaffeeSatz">
				Reageer</h3>
			<nforum:Emoticons ID="emoticonInclude" runat="server" />
			<asp:TextBox ID="txtPost" runat="server" CssClass="grid_9 alpha omega" TextMode="MultiLine" Rows="8" ClientIDMode="Static"></asp:TextBox>
			<asp:Button ID="btnSubmitPost" CssClass="btnsubmitpost button" runat="server" Text="Plaats reactie" />
		</LoggedInTemplate>
	</asp:LoginView>
	<div class="postsuccess" style="display: none;">
		<h4 style="text-align: center;">
			Posting your reply...</h4>
	</div>
</div>
