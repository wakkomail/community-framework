<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetMembers.ascx.cs" Inherits="nForum.usercontrols.nForum.SetMembers" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>


<asp:Repeater ID="rptMembers" runat="server" EnableViewState="false">       
    <ItemTemplate>
        <asp:CheckBox ID="chkMember" CssClass="isMember" Visible='<%# CurrentMemberIsAdmin %>' Text='<%# ((Member)Container.DataItem).LoginName %>' Checked='<%# IsMember(((Member)Container.DataItem).LoginName) %>'  runat="server" />        
        <asp:HyperLink ID="lnkMember" runat="server" Text='<%# ((Member)Container.DataItem).LoginName %>'
                                                    Visible='<%# !CurrentMemberIsAdmin && IsMember(((Member)Container.DataItem).LoginName) %>'
                                                    NavigateUrl='<%# "/memberprofile.aspx?mem=" + ((Member)Container.DataItem).Id %>' ></asp:HyperLink>
        <br />
    </ItemTemplate>
</asp:Repeater>

<asp:Button ID="btnSetMember" CssClass="btnsubmittopic" runat="server" 
    Text="Lidmaatschappen opslaan" onclick="btnSetMember_Click" />