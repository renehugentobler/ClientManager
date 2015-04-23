<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabInfo_Logins.aspx.cs" Inherits="Pages_TabInfo_Logins" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Refresh" content="300" />
    <title>Login Statistics</title>
	<style type="text/css">
        .tdText { font:11px Verdana; color:#333333; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>
        <obout:OboutButton Width="160" runat="server" ID="txTimeZone" Enabled="false" Text="Eastern Standard Time" FolderStyle="styles/interface/premiere_blue/OboutButton" />
    </div>
    </form>
</body>
</html>
