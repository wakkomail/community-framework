<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateMembergroup.ascx.cs" Inherits="nForum.usercontrols.nForum.CreateMembergroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>
            <asp:Label ID="lblName" runat="server" Text="Naam van de groep"></asp:Label>
        </p>
        <p>
            <asp:TextBox ID="txtMembergroupName" MaxLength="30" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="btnInsertMembergroup" runat="server" Text="Toevoegen" 
                onclick="btnInsertMembergroup_Click" />
        </p>        
        <p>
            <asp:Label ID="lblResultInfo" runat="server" Text=""></asp:Label>
        </p>
    </div>
    </form>
</body>
</html>
