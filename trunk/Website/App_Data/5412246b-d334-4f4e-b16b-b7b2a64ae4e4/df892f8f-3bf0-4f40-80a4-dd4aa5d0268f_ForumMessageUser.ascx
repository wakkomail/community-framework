<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumMessageUser.ascx.cs" Inherits="nForum.usercontrols.nForum.membership.ForumMessageUser" %>

        <asp:LoginView ID="lvPrivateMessage" runat="server" Visible="false">
            <LoggedInTemplate>
                
                <h2>Private Message</h2>

                <p>To: <asp:Literal ID="litMemberTo" runat="server" /></p>

                <p>Subject<br /><asp:TextBox ID="tbMessageSubject" runat="server" CssClass="privatemessagesubject required" ToolTip="Subject Required" ClientIDMode="Static" /></p>

                <p><asp:TextBox ID="txtPost" runat="server" CssClass="reportspamtextbox" TextMode="MultiLine" ToolTip="Message Required" Rows="14" ClientIDMode="Static" /></p>

                <p class="submitbutton"><asp:Button ID="btnSubmitMessage" runat="server" Text="Send Message" onclick="BtnSubmitMessageClick" /></p>

            </LoggedInTemplate>
        </asp:LoginView>