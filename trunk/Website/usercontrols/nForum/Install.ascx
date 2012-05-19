<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Install.ascx.cs" Inherits="nForum.usercontrols.nForum.Install" %>

<asp:Panel ID="pnlFirstInfo" runat="server">
<h1>Almost Done!</h1>
<p>You are almost done installing nForum, there are a couple of things still left to 
    do - So please click the button below complete the install</p>
</asp:Panel>

<asp:Panel ID="pnlSecondInfor" runat="server" Visible="false">
<h1>Finished :)</h1>
<p><strong>Everything has now installed</strong>, don&#39;t forget to setup your SMTP 
    email details in the web.config</p>
    <p>
       &lt;smtp&gt;<br />
        &lt;network host=&quot;ADD HERE&quot; userName=&quot;ADD HERE&quot; password=&quot;ADD HERE&quot; /&gt;<br />
&lt;/smtp&gt;
    </p>
    <p>
        Please take the time to view the videos on my blog to see how to setup, use and 
        customise nForum</p>
<h3><a href="http://www.blogfodder.co.uk">http://www.blogfodder.co.uk</a></h3>
</asp:Panel>

<asp:Button ID="btnComplete" style="padding:6px 10px; font-size:17px;" runat="server" Text="Click Here To Finish nForum Install" onclick="Button1Click" />