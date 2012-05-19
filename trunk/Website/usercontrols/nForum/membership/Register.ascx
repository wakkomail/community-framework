<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Register.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.Register" %>

    <div id="forumregistration" class="validate">

            <dl class="form">
                
                <dt><label for="<%= tbLoginName.ClientID %>">Login/Username</label></dt>
                <dd><asp:TextBox ToolTip="Enter username" CssClass="required" ID="tbLoginName" runat="server" /></dd>

                <dt><label for="<%= tbName.ClientID %>">Name</label></dt>
                <dd><asp:TextBox ToolTip="Enter name" CssClass="required" ID="tbName" runat="server" /></dd>

                <dt><label for="<%= tbEmail.ClientID %>">Email</label></dt>
                <dd><asp:TextBox ToolTip="Enter email address" CssClass="required email" ID="tbEmail" runat="server" /></dd>

                <dt><label for="<%= tbPassword.ClientID %>">Password</label></dt>
                <dd><asp:TextBox ToolTip="Enter a password" CssClass="required" ID="tbPassword" TextMode="Password" runat="server" /></dd>

                <dt><label for="<%= tbTwitter.ClientID %>">Twitter Username</label></dt>
                <dd><asp:TextBox ID="tbTwitter" runat="server" /></dd>

                <dt> </dt>
                <dd><asp:Button ID="btnSubmit" CssClass="textarea" runat="server" Text="Create Account" onclick="BtnSubmitClick" /></dd>

            </dl>


    </div>