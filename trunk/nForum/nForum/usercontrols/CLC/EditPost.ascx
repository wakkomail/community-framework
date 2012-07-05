<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPost.ascx.cs" Inherits="nForum.usercontrols.CLC.EditPost" %>
<%@ Register Src="~/usercontrols/nForum/includes/Emoticons.ascx" TagName="Emoticons"
	TagPrefix="nforum" %>
<div id="editPostContainer" class="grid_9 fatBorder">
		<asp:LoginView ID="lvEditPost" runat="server">
			<LoggedInTemplate>
				<h2 class="c1 kaffeeSatz">
					<asp:Literal ID="litTitle" runat="server" /></h2>
				<asp:Panel ID="pnlTopicTitle" runat="server" Visible="false">
					<asp:TextBox ID="tbTopicTitle" CssClass="topictitletextbox discussionSubject" runat="server" ClientIDMode="Static"
							ToolTip="Onderwerp"></asp:TextBox>
				</asp:Panel>
				<nforum:Emoticons ID="emoticonInclude" runat="server" />
				<asp:TextBox ID="txtPost" runat="server" CssClass="grid_9 alpha omega" TextMode="MultiLine" Rows="8" ClientIDMode="Static"></asp:TextBox>
				<asp:Panel ID="pnlMakeSticky" runat="server" Visible="false">
					<asp:CheckBox ID="cbSticky" ClientIDMode="Static" runat="server" />
						Sticky discussie
					<asp:CheckBox ID="cbLockTopic" ClientIDMode="Static" runat="server" />
						Sluit discussie
				</asp:Panel>
				<asp:Button ID="btnSubmitPost" runat="server" CssClass="button" Text="Edit Post"
					OnClick="BtnSubmitPostClick" />
			</LoggedInTemplate>
		</asp:LoginView>
	</div>
