<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leads_Sales.aspx.cs" Inherits="Pages_Leads_Sales" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_Net" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Universal Lead Salespage</title>
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
        .ob_gRETpl #ob_iTSuperForm1_ClientNoteContainer { height: 60px !important; width:1000px !important; }
        .ob_gRETpl #ob_iTSuperForm1_SalesNoteContainer { height: 140px !important; width:1000px!important; }
    </style>
        
</head>

<body>

    <form runat="server">
        <div>
    
		<div style="display: none"><asp:PlaceHolder ID="phDummy" runat="server"></asp:PlaceHolder></div>
        <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>

        <asp:SqlDataSource ID="sdsEmployee" runat="server" SelectCommand="SELECT * FROM Employee"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsPriority" runat="server" SelectCommand="SELECT * FROM _LeadPriority ORDER BY Sequence"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsGrid" runat="server"></asp:SqlDataSource> 
        <asp:SqlDataSource ID="sdsSuperForm" runat="server"></asp:SqlDataSource> 

        </div>
    </form>

</body>

</html>
