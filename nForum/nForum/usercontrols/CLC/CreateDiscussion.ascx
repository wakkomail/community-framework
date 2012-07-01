<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateDiscussion.ascx.cs" Inherits="nForum.usercontrols.CLC.CreateDiscussion" %>
<%@ Register src="~/usercontrols/nForum/includes/Emoticons.ascx" tagname="Emoticons" tagprefix="nforum" %>
<div id="newDiscussionContainer" class="grid_9 fatBorder">
	<div class="createnewtopicform">
			<asp:LoginView ID="lvNewTopic" runat="server">
				<LoggedInTemplate>
					<h2 class="c1 kaffeeSatz">Nieuwe discussie</h2>
					<p class="newDiscussionSummary">
						<asp:Literal ID="litDescription" runat="server" />
					</p>
					<div class="newDiscussionContent fatBorder">
						<asp:TextBox ID="tbTopicTitle" CssClass="discussionSubject" runat="server" ClientIDMode="Static" ToolTip="Onderwerp" placeholder="Onderwerp"></asp:TextBox>
						<nforum:Emoticons ID="emoticonInclude" runat="server" />
						<asp:TextBox ID="txtPost" runat="server" CssClass="grid_9 alpha omega" TextMode="MultiLine" Rows="8" ClientIDMode="Static"></asp:TextBox>
						<asp:Panel ID="pnlMakeSticky" runat="server" Visible="false">
							<asp:CheckBox ID="cbSticky" ClientIDMode="Static" runat="server" /> Sticky discussie
							<asp:CheckBox ID="cbLockTopic" ClientIDMode="Static" runat="server" /> Sluit discussie
						</asp:Panel>
						<asp:Button ID="btnSubmitPost" CssClass="btnsubmittopic button" runat="server" Text="Aanmaken" />
					</div>
				</LoggedInTemplate>
			</asp:LoginView>
	</div>

	<div class="postsuccess" style="display: none;">
	  <h4 style="text-align: center;">Uw discussie wordt aangemaakt...</h4>
	</div>
</div>