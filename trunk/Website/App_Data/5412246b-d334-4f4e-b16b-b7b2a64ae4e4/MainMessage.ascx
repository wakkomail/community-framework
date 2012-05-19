<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMessage.ascx.cs" Inherits="nForum.usercontrols.nForum.includes.MainMessage" %>
<%@ Register src="MessageBox.ascx" tagname="MessageBox" tagprefix="nforum" %>


<div class="error" id="error" style="display:none;">
    <a onclick="document.getElementById('error').style.display = 'none'" href="#">
        <img alt="Remove message" src="/nforum/img/msg/close.png" />
    </a>
</div>
<nforum:MessageBox ID="msgBox" ShowCloseButton="true" runat="server" Visible="false" />

