<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="CC_Survey.aspx.cs" Inherits="Pages_CC_Campain" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Ajax.UI.FileUpload" Assembly="Obout.Ajax.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <obout:OboutButton runat="server" OnCommand="Btn_Command" Width="80" CommandName="Refresh" ID="Refresh" FolderStyle="styles/interface/premiere_blue/OboutButton" Text="Refresh" />
        <br />
        <br />

        <table id='campaigns' runat='server' border='1'>
            <tr>
                <td colspan="4"><obout:OboutButton Width="100%" runat="server" ID="txtHeadCampain" Enabled="false" Text="Campaigns" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
            <tr>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtCampainName" Enabled="false" Text="Campaign" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtCampainRunAt" Enabled="false" Text="Run at" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtCampainLastLoad" Enabled="false" Text="Last Update" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
                <td colspan="1"><obout:OboutButton Width="100%" runat="server" ID="txtLoad" Enabled="false" Text="Filedate" FolderStyle="styles/interface/premiere_blue/OboutButton" /></td>
            </tr>
        </table>
        <br/><br/><br/>
        <table runat='server' border='1'>
            <tr>
                <td colspan="1">
                    <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
                    <center>
                        <obout:FileUpload  OnClientBeforeUpload="clearResult"
                            OnClientAfterServerResponse="onServerPesponse" Accept="text/*"
                            ValidFileExtensions="csv" MaximumTotalFileSize="10240"
                            Width="250px" runat="server" id="fileUpload1" BrowseFieldDescription="Images"
                        />
                        <br />
                        <asp:Button runat="server" ID="upload" Text="Upload and save files" OnClientClick="return send();" />
                        <br /><br />
                        <asp:Label runat="server" ID="label" Text="" />
                    </center>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
