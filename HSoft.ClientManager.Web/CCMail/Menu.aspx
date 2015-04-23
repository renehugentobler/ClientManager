<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="CCMail_Menu" %>

<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CC Mail Menu</title>
	<script type="text/javascript">
	    function requestPermission(txInfo) {
	        return confirm("Are you sure you want to " + txInfo + "?");
	    }
	</script>
</head>

<body>
    <form id="form1" runat="server">
    <div>

    <table>
        <tr>
		    <td><obout:OboutButton runat="server" OnClientClick = "return requestPermission('Reset Last Load Date');" OnCommand="Btn_Command" Width="280" CommandName="ResetLoadDate" ID="ResetLoadDate" FolderStyle="../Pages/styles/interface/premiere_blue/OboutButton" Text="Reset Last Load Date" /></td> 
        </tr>
        <tr>
            <td><obout:OboutButton runat="server" OnClientClick = "return requestPermission('Reset Constant Contact Local Database');" OnCommand="Btn_Command" Width="280" CommandName="ResetDatabase" ID="ResetDatabase" FolderStyle="../Pages/styles/interface/premiere_blue/OboutButton" Text="Reset Constant Contact Local Database" /></td> 
        </tr>
        <tr>
            <td><obout:OboutButton runat="server" OnClientClick = "return requestPermission('Update Local Database from Constant Contact');" OnCommand="Btn_Command" Width="280" CommandName="UpdateDatabase" ID="UpdateDatabase" FolderStyle="../Pages/styles/interface/premiere_blue/OboutButton" Text="Update Local Database from Constant Contact" /></td>
            <td></td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
		    <td><obout:OboutButton Width="120" runat="server" ID="txtContacts" Enabled="false" Text="Contacts" /></td> 
            <td><obout:OboutButton runat="server" ID="LastDateContacts" Enabled="false" /></td>
            <td><obout:OboutButton runat="server" ID="RecordCountContacts" Enabled="false" /></td>
        </tr>
        <tr>
		    <td><obout:OboutButton Width="120" runat="server" ID="txtContactLists" Enabled="false" Text="ContactLists" /></td> 
            <td><obout:OboutButton runat="server" ID="LastDateContactLists" Enabled="false" /></td>
            <td><obout:OboutButton runat="server" ID="RecordCountContactLists" Enabled="false" /></td>
        </tr>
        <tr>
		    <td><obout:OboutButton Width="120" runat="server" ID="txtEmailCampaigns" Enabled="false" Text="EmailCampaigns" /></td> 
            <td><obout:OboutButton runat="server" ID="LastDateEmailCampaigns" Enabled="false" /></td>
            <td><obout:OboutButton runat="server" ID="RecordCountEmailCampaigns" Enabled="false" /></td>
        </tr>
        <tr>
		    <td><obout:OboutButton Width="120" runat="server" ID="txtOpenActivities" Enabled="false" Text="OpenActivities" /></td> 
            <td><obout:OboutButton runat="server" ID="LastDateOpenActivities" Enabled="false" /></td>
            <td><obout:OboutButton runat="server" ID="RecordCountOpenActivities" Enabled="false" /></td>
        </tr>
    </table>
        
    </div>
    </form>
</body>
</html>
