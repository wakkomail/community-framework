<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.Login" %>

<asp:Login RenderOuterTable="false" ID="ctlLogin" runat="server" OnLoginError="OnLoginError" onloggedin="OnLoggedIn" RememberMeSet="True" VisibleWhenLoggedIn="False">
    <LayoutTemplate>
        <div id="login">
            <dl class="form">
                <dt>
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label></dt>
                <dd>
                    <asp:TextBox ID="UserName" CssClass="required" ToolTip="Enter username" runat="server"></asp:TextBox>
                </dd>
                <dt>
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="Password" CssClass="required" ToolTip="Enter password" runat="server" TextMode="Password"></asp:TextBox>
                </dd>
                <dt>
                </dt>
                <dd>
                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                </dd>
                <dt></dt>
                <dd>
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" />
                </dd>
            </dl>
        </div>
        <p class="forgotpasswordlink"><a href="/forgotpassword.aspx">Forgot your password?</a></p>
    </LayoutTemplate>
</asp:Login>