<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="CC_EmailLists.aspx.cs" Inherits="CC_EmailLists" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <obout:OboutButton runat="server" OnClientClick = "return requestPermission('Create Next 500 CC No Leads Contacts');" OnCommand="Btn_Command" Width="280" CommandName="CC_Lead_500" ID="CC_Lead_500" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Create List with Next 500 no Leads Contacts" /> 
        <obout:OboutButton runat="server" OnClientClick = "return requestPermission('Create Next 500 CC Unsubscribed');" OnCommand="Btn_Command" Width="280" CommandName="CC_Unsubscribed_500" ID="OboutButton1" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Create List with Next 500 Unsubscribed Contacts" /> 

        <br />
        <br />

        <table runat="server" id="list"  border='1'>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="hdrListName" Enabled="false" Text="List Name" /></td>
                <td><obout:OboutButton Width="120" runat="server" ID="hdrCount" Enabled="false" Text="Contact Count" /></td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
