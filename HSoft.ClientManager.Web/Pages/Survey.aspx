<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Survey.aspx.cs" Inherits="Pages_Survey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Universal Surveypage</title>
    <style type="text/css">
		.tdText { font:11px Verdana; color:#333333; }
		.floating { float: left; padding-right: 10px; }
		.option2 { font:11px Verdana; color:#0033cc;background-color:#f6f9fc; padding-left:4px; padding-right:4px; }
		a { font:11px Verdana; color:#315686; text-decoration:underline; }
		a:hover { color:crimson; }
		.ob_iBC { display: block !important; }
        * HTML .ob_iBC { -display: inline !important; }
        .lead-information .ob_fRwH { width: 60px !important; }
        .lead-information .ob_fRwF { width: 200px !important; }
		.rowEditTable { }
		.rowEditTable td { font-family: Verdana; font-size: 10px; color: #4B555E; }
        #Grid1_ob_Grid1FooterContainer_Top .ob_gFALC { float: left !important; }
    </style>
</head>
<body>
    <form runat="server">
    <div>
    
        <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>

        <asp:SqlDataSource ID="sdsGrid" runat="server"></asp:SqlDataSource> 

    </div>
    </form>
</body>
</html>
