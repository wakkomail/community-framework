<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAdmin.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.UserAdmin" %>

    <div id="forumloggedinstatus">
        <asp:LoginStatus ID="ctlLogin"
                     runat="server"
                     LoginText="User Login"
                     LogoutText="User Logout" OnLoggedOut="UmbracoLogout" /> / 
                     <asp:LoginName ID="ctlLoginName" runat="server" />
    </div>

    <div id="forumloggedinadmin">
        <ul>
            <li><a href="/activetopics.aspx">Active topics</a></li>
            <li><a href="/yourtopics.aspx">Topics you participated in</a></li>
            <li><a href="/editprofile.aspx">Edit profile</a></li>
        </ul>
    </div>