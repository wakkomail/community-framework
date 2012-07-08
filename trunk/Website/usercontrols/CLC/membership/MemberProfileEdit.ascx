<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberProfileEdit.ascx.cs" Inherits="nForum.usercontrols.CLC.membership.MemberProfileEdit" %>

<div id="membereditprofile" class="membereditprofile" runat="server">

            <dl class="form">
                
                <dt><label for="<%= tbLoginName.ClientID %>">Login/Username</label></dt>
                <dd><asp:TextBox ToolTip="Enter username" CssClass="required" ID="tbLoginName" runat="server" /></dd>

                <dt><label for="<%= tbName.ClientID %>">Name</label></dt>
                <dd><asp:TextBox ToolTip="Enter name" CssClass="required" ID="tbName" runat="server" /></dd>

                <dt><label for="<%= tbEmail.ClientID %>">Email</label></dt>
                <dd><asp:TextBox ToolTip="Enter email address" CssClass="required email" ID="tbEmail" runat="server" /></dd>

                <dt><label for="<%= tbTwitter.ClientID %>">Twitter Username</label></dt>
                <dd><asp:TextBox ID="tbTwitter" runat="server" /></dd>

                <dt><label for="<%= cbAllowPrivateMessages.ClientID %>">Enable Private Messages</label></dt>
                <dd><asp:CheckBox ID="cbAllowPrivateMessages" runat="server" /></dd>

                <dt> </dt>
                <dd><asp:Button ID="btnSubmit" runat="server" Text="Update Profile" onclick="BtnSubmitClick" /></dd>

            </dl>

</div>