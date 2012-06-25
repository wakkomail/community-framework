<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembergroupManager.ascx.cs" Inherits="nForum.usercontrols.CLC.MembergroupManager" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>

<div class="setmembercontainer">
    <div id="selectMembergroup" class="halfpage" runat="server">
        <h3 id="h1" class="kaffeeSatz c2" runat="server">Stap 1: Kennisgroep leden koppelen</h3>
        <asp:TextBox ID="txtSearchMembergroup" runat="server" Width="300"></asp:TextBox>        
        <asp:Button ID="searchMembergroups" runat="server" CssClass="button" Text="Groepen zoeken" 
            onclick="searchMembergroups_Click" />
        <div id="groupContainer">
        <h3 id="lastGroups" class="kaffeeSatz c2" runat="server" Visible="false">20 nieuwste kennisgroepen</h3>
        <ul>        
            <asp:Repeater ID="rprGroups" runat="server" >                
 			    <ItemTemplate>
                <li>
                     <a href='<%# "membergroupmanagement.aspx?memid=" + ((ForumCategory)Container.DataItem).Id + "&memgsearch=" + txtSearchMembergroup.Text  %>'
                     class='<%# (IsMembergroupSelected(((ForumCategory)Container.DataItem).Id)) ? "selectedmembergroup" : "normalmembergroup"   %>'>
                     <%# ((ForumCategory)Container.DataItem).Name %>
                     </a>                     
                </li>
			    </ItemTemplate>               
            </asp:Repeater>
        </ul>
        </div>
    </div>
    <div class="halfpage" id="selectMembers" runat="server">
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
				        </li>
			        </ItemTemplate>
		        </asp:Repeater> 
	        </ul>      
        </div>    
    </div>
    <div id="resultinfo">
        <asp:Label ID="lblResultInfo" runat="server" Text="" CssClass="kaffeeSatz"></asp:Label>
    </div>
</div>


<asp:Button ID="save" runat="server" Text="Opslaan" CssClass="button" onclick="save_Click" />

 <a href="/Control-panel.aspx" class="button">Terug naar het beheerscherm</a> 
 <a href="membergroupmanagement.aspx" class="button">Velden leegmaken</a>