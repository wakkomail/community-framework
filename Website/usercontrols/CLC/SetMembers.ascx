<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetMembers.ascx.cs" Inherits="nForum.usercontrols.CLC.SetMembers" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>

<div id="selectMembers" class="halfpage" runat="server">
    <h3 id="header" class="kaffeeSatz c2" runat="server">Stap 2: Leden koppelen</h3>
    <asp:TextBox ID="txtSearchMember" runat="server" Width="300"></asp:TextBox>
    <asp:Button ID="searchMember" runat="server" Text="Leden zoeken" 
        onclick="search_Click" CssClass="button" />
    <div id="ledenContainer">	
        <h3 id="lastMembers" class="kaffeeSatz c2" runat="server" Visible="false">20 nieuwste leden</h3>        
	    <ul>
		    <asp:Repeater ID="rprMembers" runat="server" EnableViewState="false">
			    <ItemTemplate>
				    <li>					        
                        <asp:CheckBox ID="chkMember" CssClass="isMember" Text='<%# ((Member)Container.DataItem).LoginName %>' 
                        Checked='<%# IsMember(((Member)Container.DataItem).LoginName) %>'  runat="server" /> 
                        <b><%# ((Member)Container.DataItem).Email %></b>
                        <asp:CheckBox ID="chkManager" Text='beheerder' runat="server" CssClass="isManager"
                        Checked='<%# IsManager(((Member)Container.DataItem).LoginName) %>' />                                
				    </li>
			    </ItemTemplate>
		    </asp:Repeater> 
	    </ul>      
    </div>    
    <div id="resultinfo">
        <asp:Label ID="lblResultInfo" runat="server" Text="" CssClass="kaffeeSatz"></asp:Label>
    </div>
    <div class="buttonbar">
        <asp:Button ID="save" runat="server" Text="Opslaan" CssClass="button" onclick="save_Click" />
        <a href="#" class="button cancel">Annuleren</a>      
    </div>
</div>
