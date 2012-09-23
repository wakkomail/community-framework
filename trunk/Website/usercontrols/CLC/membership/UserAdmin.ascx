<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAdmin.ascx.cs" Inherits="nForum.usercontrols.CLC.membership.UserAdmin" %>
	 <asp:LoginStatus ID="ctlLogin"
                     runat="server"
                     LoginText="inloggen"
                     LogoutText="uitloggen" OnLoggedOut="UmbracoLogout" />
					 <asp:Literal ID="litSeparator" runat="server" Visible="False"><span>|</span></asp:Literal>
					 <asp:LoginName ID="ctlLoginName" runat="server" />
					 <asp:HyperLink ID="lnkInschrijven" runat="server" Text="inschrijven" Visible="False" />
	<%--<a href="/inloggen.aspx">inloggen</a> <span>|</span> <a href="#">inschrijven</a>--%>