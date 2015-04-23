<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CC_Info.aspx.cs" Inherits="Pages_CC_Info" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table>
            <tr>
                <td colspan="2"><obout:OboutButton Width="280" runat="server" ID="txHeadContacts" Enabled="false" Text="Contacts" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td><obout:OboutButton Width="120" runat="server" ID="txLastUpdate" Enabled="false" Text="Last Update" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td><td><obout:OboutButton runat="server" ID="LastUpdate" Enabled="false" /></td>
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

        <br />

        <table>
            <tr>
                <td colspan="2"><obout:OboutButton Width="280" runat="server" ID="txHeadCampain" Enabled="false" Text="Campain" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
