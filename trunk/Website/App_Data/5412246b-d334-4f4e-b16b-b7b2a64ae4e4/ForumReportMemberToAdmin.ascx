<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumReportMemberToAdmin.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.ForumReportMemberToAdmin" %>

        <asp:LoginView ID="lvReportMember" runat="server">
            <LoggedInTemplate>
                <h2>Report Member</h2>
                <p>Please explain why you are reporting this member?</p>
                <p><asp:TextBox ID="tbSpamReport" runat="server" CssClass="reportspamtextbox" TextMode="MultiLine" Rows="14"></asp:TextBox></p>
                <p class="submitbutton"><asp:Button ID="btnSubmitPost" runat="server" Text="Submit Report" onclick="BtnSubmitPostClick" /></p>
            </LoggedInTemplate>
        </asp:LoginView>