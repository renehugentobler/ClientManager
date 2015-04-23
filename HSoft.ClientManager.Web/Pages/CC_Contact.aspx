<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="CC_Contact.aspx.cs" Inherits="Pages_CC_Contact" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		function requestPermission(txInfo) {
		    return confirm("Are you sure you want to " + txInfo + "?");
		}
	</script></head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<obout:OboutButton runat="server" OnClientClick = "return requestPermission('Reset Last Load Date');" OnCommand="Btn_Command" Width="280" CommandName="ResetLoadDate" ID="ResetLoadDate" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Reset Last Load Date" /> 
        <obout:OboutButton runat="server" OnClientClick = "return requestPermission('Reset Constant Contact Local Database');" OnCommand="Btn_Command" Width="280" CommandName="ResetDatabase" ID="ResetDatabase" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Reset Constant Contact Local Database" /> 
        <obout:OboutButton runat="server" OnClientClick = "return requestPermission('Update Local Database from Constant Contact');" OnCommand="Btn_Command" Width="280" CommandName="UpdateDatabase" ID="UpdateDatabase" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Update Local Database from Constant Contact" /> 
        <br />
        <obout:OboutButton runat="server" OnCommand="Btn_Command" Width="280" CommandName="RelinkDatabases" ID="RelinkDatabases" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Relink Leads with Constant Contact" />
        <obout:OboutButton runat="server" OnClientClick = "return requestPermission('Upload Local Database to Constant Contact');" OnCommand="Btn_Command" Width="280" CommandName="UpdateConstantContact" ID="UpdateConstantContact" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Upload Local Database to Constant Contact" />
        <br />
        <obout:OboutButton runat="server" OnCommand="Btn_Command" Width="80" CommandName="Refresh" ID="Refresh" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Refresh" />

        <br />
        <br />

        <table>
            <tr>
                <td colspan="2"><obout:OboutButton Width="280" runat="server" ID="txHeadContacts" Enabled="false" Text="Contacts" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txLastUpdate" Enabled="false" Text="Last Update" /></td><td><obout:OboutButton runat="server" ID="LastUpdate" Enabled="false" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txCountContacts" Enabled="false" Text="Total Contacts" /></td><td><obout:OboutButton runat="server" ID="CountContacts" Enabled="false" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txCountActive" Enabled="false" Text="Active Contacts" /></td><td><obout:OboutButton runat="server" ID="CountActive" Enabled="false" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txCountLoads" Enabled="false" Text="Total Leads" /></td><td><obout:OboutButton runat="server" ID="CountLeads" Enabled="false" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txLinked" Enabled="false" Text="Linked Leads" /></td><td><obout:OboutButton runat="server" ID="Linked" Enabled="false" /></td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
