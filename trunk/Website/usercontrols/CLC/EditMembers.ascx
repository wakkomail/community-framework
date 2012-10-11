<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMembers.ascx.cs" Inherits="nForum.usercontrols.CLC.EditMembers" %>
<%@ Import Namespace="umbraco.cms.businesslogic.member" %>

<div class="setmembercontainer">
    <div id="selectMembers" class="halfpage" runat="server">
        <h3 id="header" class="kaffeeSatz c2" runat="server">Selecteer lid</h3>
        <asp:TextBox ID="txtSearchMember" runat="server" Width="300"></asp:TextBox>
        <asp:Button ID="searchMember" runat="server" Text="Leden zoeken" 
            onclick="search_Click" CssClass="button" />
        <div id="ledenContainer">	
            <h3 id="lastMembers" class="kaffeeSatz c2" runat="server" Visible="false">20 nieuwste leden</h3>        
	        <ul>
		        <asp:Repeater ID="rprMembers" runat="server" EnableViewState="false"  OnItemCommand="rptMember_ItemCommand">
			        <ItemTemplate>
				        <li class="editmember">
                            <asp:HyperLink ID="selectMember" runat="server" CssClass='<%# IsSelected(((Member)Container.DataItem).Id) ? "selectedsearchresult" : "normalsearchresult" %>'
                            NavigateUrl='<%# "/CLCEditMembers.aspx?memid=" + ((Member)Container.DataItem).Id + "&memsearch=" + txtSearchMember.Text %>'
                            Text='<%# ((Member)Container.DataItem).LoginName + " - " + ((Member)Container.DataItem).Email %>'></asp:HyperLink>
                            <asp:HyperLink ID="lnkDeleteMember" runat="server" CssClass="deleteMember deletebutton" NavigateUrl="">
						        <div class="confirmDeleteMember" style="display: none;">
							        <p>
								        Weet u zeker dat u het lid "<%# ((Member)Container.DataItem).LoginName %>" uit alle kennisgroepen en projecten wilt verwijderen?</p>
							        <asp:Button ID="deletebutton" runat="server" Text="Verwijderen" CommandName="delete"
								        CommandArgument="<%# ((Member)Container.DataItem).Id %>" CssClass="button" />
							        <asp:Button ID="cancelbutton" CssClass="cancel button" Text="Annuleren" runat="server" />
						        </div>
					        </asp:HyperLink>			                                    
				        </li>
			        </ItemTemplate>
		        </asp:Repeater> 
	        </ul>      
        </div>    
     </div>
     <div class="halfpage">
             <asp:Panel ID="pnlMember" runat="server" Visible="false">
                <fieldset id="member">
                <legend>Lid wijzigen</legend>
                    <p>
                        <asp:Label ID="lblMemberLoginname" runat="server" Text="Loginnaam"></asp:Label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtLoginName" MaxLength="30" runat="server" CssClass="maxWidthInput"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblMemberName" runat="server" Text="Naam van het lid"></asp:Label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtMemberName" MaxLength="30" runat="server" CssClass="maxWidthInput"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblMemberEmail" runat="server" Text="Emailadres"></asp:Label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtMemberEmail" MaxLength="30" runat="server" CssClass="maxWidthInput"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="lblPassword" runat="server" Text="Wachtwoord"></asp:Label>
                    </p>
                    <p>
                        <asp:TextBox ID="txtMemberPassword" MaxLength="30" runat="server" CssClass="maxWidthInput"></asp:TextBox>
                    </p>       
                </fieldset>
            </asp:Panel>
        </div>
</div>


    <div id="resultinfo">
        <asp:Label ID="lblResultInfo" runat="server" Text="" CssClass="kaffeeSatz"></asp:Label>
    </div>
    <div class="buttonbar">
        <asp:Button ID="save" runat="server" Text="Opslaan" CssClass="button" onclick="save_Click" />
        <a href="/Control-panel.aspx" class="button">Terug naar het beheerscherm</a>      
    </div>

