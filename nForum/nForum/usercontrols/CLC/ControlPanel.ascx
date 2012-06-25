<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlPanel.ascx.cs" Inherits="nForum.usercontrols.CLC.ControlPanel" %>
<%@ Import Namespace="nForum.global" %>

<div id="options">
    <asp:Button ID="createMembergroup" ClientIDMode="Static" runat="server" CssClass="button"
        Text="Kennisgroep aanmaken" onclick="createMembergroup_Click" />
    <asp:Button ID="createProject" runat="server" Text="Project aanmaken" CssClass="button"
        onclick="createProject_Click" />    
    <asp:Button ID="createMember" runat="server" Text="Lid aanmaken" 
        CssClass="button" onclick="createMember_Click" />
    <a href='<%# "/control-panel/membergroupmanagement.aspx?documenttype=" + GlobalConstants.MembergroupAlias %>'  class="button">Kennisgroep leden koppelen</a> 
    <a href='<%# "/control-panel/membergroupmanagement.aspx?documenttype=" + GlobalConstants.ProjectAlias %>'  class="button">Project leden koppelen</a> 

    <asp:Button ID="publishAll" runat="server" Text="Alles publiseren" 
        CssClass="button" onclick="publishAll_Click" />
    
</div>
<div id="controlforms">    
<!-- Membergroup -->
    <asp:Panel ID="pnlMembergroup" runat="server">
        <fieldset id="membergroup">
        <legend>Kennisgroep aanmaken</legend>
            <p>
                <asp:Label ID="lblMembergroupName" runat="server" Text="Naam van de kennisgroep"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtMembergroupName" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>                
                <asp:Button ID="btnInsertMembergroup" runat="server" Text="Toevoegen" 
                    onclick="btnInsertMembergroup_Click" />
            </p>        
        </fieldset>
    </asp:Panel>
<!-- Project -->
    <asp:Panel ID="pnlProject" runat="server" Visible="false">
        <fieldset id="project">
        <legend>Project aanmaken</legend>
            <p>
                <asp:Label ID="lblProjectName" runat="server" Text="Naam van het project"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtProjectName" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnInsertProject" runat="server" Text="Toevoegen" 
                    onclick="btnInsertProject_Click" />
            </p>        
        </fieldset>
    </asp:Panel>
<!-- Member  -->
    <asp:Panel ID="pnlMember" runat="server" Visible="false">
        <fieldset id="member">
        <legend>Nieuw lid aanmaken</legend>
            <p>
                <asp:Label ID="lblMemberName" runat="server" Text="Naam van het lid"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtMemberName" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblMemberLoginName" runat="server" Text="Inlognaam"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtMemberLoginName" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblMemberEmail" runat="server" Text="Emailadres"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtMemberEmail" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblPassword" runat="server" Text="Wachtwoord"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtMemberPassword" MaxLength="30" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnInsertMember" runat="server" Text="Toevoegen" 
                    onclick="btnInsertMember_Click" />
            </p>        
        </fieldset>
    </asp:Panel>
    <div id="resultinfo">
        <asp:Label ID="lblResultInfo" runat="server" Text="" CssClass=""></asp:Label>
    </div>
</div>