<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="nForum.usercontrols.CLC.membership.Login" %>
<div id="contentContainer" class="grid_12 full">
	<div id="inlogFormContainer" class="grid_6">
		<h3 class="kaffeeSatz c1">
			Inloggen</h3>
		<asp:Login RenderOuterTable="false" ID="ctlLogin" runat="server" OnLoginError="OnLoginError"
			OnLoggedIn="OnLoggedIn" RememberMeSet="True" VisibleWhenLoggedIn="false">
			<LayoutTemplate>
				<asp:Panel ID="pnlInloggen" runat="server" DefaultButton="LoginButton">
					<asp:TextBox ID="UserName" CssClass="required gebruikersnaam" ToolTip="Vul je gebruikersnaam in"
						runat="server"></asp:TextBox>
					<asp:TextBox ID="Password" CssClass="required wachtwoord" ToolTip="Vul je wachtwoord in"
						runat="server" TextMode="Password"></asp:TextBox>
					<div class="clearfix">
						<asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="login" />
						<asp:CheckBox ID="RememberMe" runat="server" Text="Aangemeld blijven" />
					</div>
					<a class="c2" href="/WachtwoordVergeten.aspx">Wachtwoord vergeten</a>
				</asp:Panel>
			</LayoutTemplate>
		</asp:Login>
	</div>
	<div id="inlogInfoContainer" class="grid_6 alpha">
		<h3 class="kaffeeSatz c1">
			Waarom inloggen</h3>
        <asp:Literal ID="litLoginDescription" runat="server" />
	</div>
</div>
