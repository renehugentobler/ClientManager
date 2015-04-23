<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Info.aspx.cs" Inherits="Pages_Info" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
    <div>
    
        <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>	
        <br /><br />
        <asp:PlaceHolder ID="lbEmployee" runat="server"></asp:PlaceHolder>	
        <obout:OboutButton ID="bHirarySelect" runat="server" Text="Select" OnClick="Postback" />
    </div>
    </form>
</body>
</html>
