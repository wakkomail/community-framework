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
        <ul>        
            <asp:Repeater ID="rprGroups" runat="server" >                
 			    <ItemTemplate>
                <li>
                     <a href='<%# "membergroupmanagement.aspx?memid=" + ((ForumCategory)Container.DataItem).Id + "&memgsearch=" + txtSearchMembergroup.Text  %>' visible='<%# !IsMembergroupSelected(((ForumCategory)Container.DataItem).Id) %>'>
                     <%# ((ForumCategory)Container.DataItem).Name %>
                     </a> 
                    <asp:Label ID="selectedmembergroup" runat="server" CssClass="selectedmembergroup" Visible='<%# IsMembergroupSelected(((ForumCategory)Container.DataItem).Id) %>'><%# ((ForumCategory)Container.DataItem).Name %></asp:Label>
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
        <div id="ledenContainer" class="fatBorder">	        
	        <ul>
		        <asp:Repeater ID="rprMembers" runat="server">
			        <ItemTemplate>
				        <li>					        
                            <asp:CheckBox ID="chkMember" CssClass="isMember" Text='<%# ((Member)Container.DataItem).LoginName %>' 
                            Checked='<%# IsMember(((Member)Container.DataItem).LoginName) %>'  runat="server" /> 
                            <p>
                                <%# ((Member)Container.DataItem).Email %>
                            </p>
				        </li>
			        </ItemTemplate>
		        </asp:Repeater> 
	        </ul>      
        </div>    
    </div>
</div>

<asp:Button ID="save" runat="server" Text="Opslaan" CssClass="button" onclick="save_Click" />

 <a href="/Control-panel.aspx" class="button">Terug naar het beheerscherm</a> 