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
		<p>
			Via de ledenadministratie kan je als lid van Balans een inlogaccount voor het Creative
			Learning Community aanvragen als je nog geen inloggegevens heeft, of als het inloggen
			met je bestaande inloggegevens niet (meer) lukt.
		</p>
		<p>
			Heb je als lid van je organisatie nog geen inloggegevens voor CLC, of lukt het niet
			om met je bestaande inloggegevens op Mijn Balans in te loggen? Klik dan op wachtwoord
			vergeten en vul de gegevens in die je wel weet.
		</p>
		<p>
			Aan de hand van de ingevulde gegevens zoekt de ledenadministratie jou op in het
			systeem en kijkt of jouw e-mailadres al in het systeem staat, of dat jouw geregistreerde
			e-mailadres nog klopt met het huidge.
		</p>
		<p>
			Na invoering of wijziging van je e-mailadres ontvang je automatisch een e-mail met
			een link om een wachtwoord in te stellen. Hierna kun je direct op het Creative Learning
			Community inloggen.
		</p>
		<p>
			Nog geen inloggegevens? <a href="#" class="c1">Schrijf je dan nu in</a>
		</p>
	</div>
</div>
