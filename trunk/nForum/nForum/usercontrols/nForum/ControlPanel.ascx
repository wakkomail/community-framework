<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlPanel.ascx.cs" Inherits="nForum.usercontrols.ControlPanel" %>

<div id="options">
    <asp:Button ID="createMembergroup" ClientIDMode="Static" runat="server" 
        Text="Kennisgroep aanmaken" onclick="createMembergroup_Click" />
    <asp:Button ID="createProject" runat="server" Text="Kennisgroep aanmaken" 
        onclick="createProject_Click" />    
</div>
<div id="controlforms">
<!-- Membergroup -->
    <asp:Panel ID="pnlMembergroup" runat="server">
        <fieldset id="membergroup">
            <p>
                <asp:Label ID="lblMembergroupName" runat="server" Text="Naam van de groep"></asp:Label>
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
    <div id="resultinfo">
        <asp:Label ID="lblResultInfo" runat="server" Text=""></asp:Label>
    </div>
</div>