<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumSpamReport.ascx.cs" Inherits="nForum.usercontrols.nForum.ForumSpamReport" %>

        <asp:LoginView ID="lvSubmitSpam" runat="server">
            <LoggedInTemplate>
                <h2>Report Potential Spam Post</h2>
                <p>Please explain why you are reporting this post?</p>
                <p><asp:TextBox ID="tbSpamReport" runat="server" CssClass="reportspamtextbox" TextMode="MultiLine" Rows="14"></asp:TextBox></p>
                <p class="submitbutton"><asp:Button ID="btnSubmitPost" runat="server" Text="Submit Report" onclick="BtnSubmitPostClick" /></p>

            </LoggedInTemplate>
        </asp:LoginView>