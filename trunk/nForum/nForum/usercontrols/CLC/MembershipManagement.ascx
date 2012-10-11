<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipManagement.ascx.cs" Inherits="nForum.usercontrols.CLC.MembershipManagement" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>
<%@ Import Namespace="nForum.global" %>
<%@ Import Namespace="nForum.BusinessLogic" %>
<%@ Import Namespace="nForum.BusinessLogic.Models" %>
<%@ Import Namespace="umbraco.NodeFactory" %>
<%@ Import Namespace="umbraco.cms.businesslogic.web" %>

<div class="setmembercontainer">
    <div id="selectProject" class="halfpage" runat="server" visible="false">
        <h3 id="h2" class="kaffeeSatz c2" runat="server">Stap 1: project selecteren</h3>
        <asp:TextBox ID="txtSearchProject" runat="server" Width="300"></asp:TextBox>        
        <asp:Button ID="searchProjects" runat="server" CssClass="button" 
            Text="Projecten zoeken" onclick="searchProjects_Click" />
        <div id="Div2">
        <h3 id="H3" class="kaffeeSatz c2" runat="server" Visible="false">20 nieuwste projecten</h3>
        <ul>        
            <asp:Repeater ID="rprProjects" runat="server" >                
 			    <ItemTemplate>
                <li>
                     <a href='<%# "MembershipManagement.aspx?groupid=" + ((Document)Container.DataItem).Id + "&projsearch=" + txtSearchProject.Text + "&documenttype=" + GlobalConstants.ProjectAlias  %>'
                     class='<%# (IsGroupSelected(((Document)Container.DataItem).Id)) ? "selectedsearchresult" : "normalsearchresult"   %>'>
                     <%# ((Document)Container.DataItem).Text %>
                     </a>                     
                </li>
			    </ItemTemplate>               
            </asp:Repeater>
        </ul>
        </div>
    </div>
    <div id="selectMembergroup" class="halfpage" runat="server">
        <h3 id="h1" class="kaffeeSatz c2" runat="server">Stap 1: Kennisgroep selecteren</h3>
        <asp:TextBox ID="txtSearchMembergroup" runat="server" Width="300"></asp:TextBox>        
        <asp:Button ID="searchMembergroups" runat="server" CssClass="button" Text="Groepen zoeken" 
            onclick="searchMembergroups_Click" />
        <div id="groupContainer">
        <h3 id="lastGroups" class="kaffeeSatz c2" runat="server" Visible="false">20 nieuwste kennisgroepen</h3>
        <ul>        
            <asp:Repeater ID="rprGroups" runat="server" >                
 			    <ItemTemplate>
                <li>
                     <a href='<%# "MembershipManagement.aspx?groupid=" + ((ForumCategory)Container.DataItem).Id + "&memgsearch=" + txtSearchMembergroup.Text  + "&documenttype=" + GlobalConstants.MembergroupAlias  %>'
                     class='<%# (IsGroupSelected(((ForumCategory)Container.DataItem).Id)) ? "selectedsearchresult" : "normalsearchresult"   %>'>
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
                            <asp:CheckBox ID="chkManager" Text='beheerder' runat="server" CssClass="isManager"
                            Checked='<%# IsManager(((Member)Container.DataItem).LoginName) %>' />                                
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
<asp:Panel ID="pnlEditGroup" runat="server" Visible="false">
        <fieldset id="membergroup">
        <legend>Gegevens wijzigen</legend>
            <p>
                <asp:Label ID="lblGroupName" runat="server" Text="Naam van de kennisgroep" ></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtGroupName" MaxLength="30" runat="server" CssClass="maxWidthInput"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="lblGroupDescription" runat="server" Text="Omschrijving van de kennisgroep"></asp:Label>
            </p>
            <p>
                <asp:TextBox ID="txtGroupDescription" MaxLength="30" runat="server" TextMode="MultiLine" Rows="3" CssClass="maxWidthInput"></asp:TextBox>
            </p>     
        </fieldset>    
</asp:Panel>
<div class="buttonbar">
 <asp:Button ID="save" runat="server" Text="Opslaan" CssClass="button" onclick="save_Click" /> 
 <a href="/Control-panel.aspx" class="button">Terug naar het beheerscherm</a> 
 <a href='<%# "MembershipManagement.aspx?documenttype=" + Request.QueryString["documenttype"].ToString() %>' class="button">Velden leegmaken</a>

 <asp:HyperLink ID="lnkDeleteGroup" runat="server" CssClass="deleteGroup button" NavigateUrl="">
    Groep verwijderen
	<div class="confirmDeleteGroup" style="display: none;">
		<p>Weet u zeker dat u de geselecteerde groep wilt verwijderen?</p>		
		<asp:Button ID="delete" runat="server" Text="Verwijderen" CssClass="button" onclick="delete_Click" />
        <asp:Button ID="cancelbutton" CssClass="cancel button" Text="Annuleren" runat="server" />
	</div>
</asp:HyperLink>
</div>

