<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="CC_Emails.aspx.cs" Inherits="Pages_CC_Emails" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<obout:OboutButton Enabled="true" runat="server" OnClientClick = "return requestPermission('Update Campaign List');" OnCommand="Btn_Command" Width="280" CommandName="UpdateCampaigns" ID="UpdateCampaigns" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Update Campaign List" /> 
		<obout:OboutButton runat="server" OnClientClick = "return requestPermission('Update Campaigns');" OnCommand="Btn_Command" Width="280" CommandName="Update" ID="Update" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Update Campaigns" /> 

        <table>
            <tr>
                <td colspan="2"><obout:OboutButton Width="280" runat="server" ID="txHeadCampaigns" Enabled="false" Text="Campaigns" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txLastUpdate" Enabled="false" Text="Last Update" /></td><td><obout:OboutButton runat="server" ID="LastUpdate" Enabled="false" /></td>
            </tr>
        </table>

        <table id='emails' runat='server' border='1'>
            <tr>
                <td colspan="7"><obout:OboutButton Width="100%" runat="server" ID="txtHeadEMail" Enabled="false" Text="emails" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtEMailId" Enabled="false" Text="Id" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtEMailName" Enabled="false" Text="EMail" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtEMailRunAt" Enabled="false" Text="Run at" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtLastSync" Enabled="false" Text="Last Sync" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtisTracked" Enabled="false" Text="Tracked" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtCkickCnt" Enabled="false" Text="Clicks" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtShort" Enabled="false" Text="Lead Text" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
        </table>

        <br />

        <table id='clicks' runat='server' border='1'>
            <tr>
                <td colspan="5"><obout:OboutButton Width="100%" runat="server" ID="txtHeadClick" Enabled="false" Text="clicks" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtName" Enabled="false" Text="Name" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtEmail" Enabled="false" Text="EMail" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtPhone" Enabled="false" Text="Phone" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtDate" Enabled="false" Text="Date" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtList" Enabled="false" Text="List" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
