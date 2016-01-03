<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="Lead_Mobile.aspx.cs" Inherits="Lead_Mobile" %>

<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mobile Leads</title>
    <style type="text/css">
        div.ob_gCc2, div.ob_gCc2C, div.ob_gCc2R 
        { padding-left: 0px !important; }
    </style>
    <script type="text/javascript">
        function openemail() {
            var Id = id;
            alert('test');
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
        </div>

        <asp:PlaceHolder runat="server" ID="phGrid1" /> 
            
    </div>
    </form>
</body>
</html>
