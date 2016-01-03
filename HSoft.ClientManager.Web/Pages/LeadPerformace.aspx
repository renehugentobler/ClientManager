<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeadPerformace.aspx.cs" Inherits="Pages_LeadPerformace" %>

<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml">

<head>
	<title>obout ASP.NET Grid examples</title>
	<style type="text/css">
        X.super-form { margin: 12px; }
        .ob_fC table td { white-space: normal !important; }
        X.command-row .ob_fRwF { padding-left: 50px !important; }
		.tdText { font:11px Verdana; color:#333333; }
		X.floating { float: left; padding-right: 10px; }
		.option2{ font:11px Verdana; color:#0033cc; padding-left:4px; padding-right:4px; }
		Xa { font:11px Verdana; color:#315686; text-decoration:underline; }
		a:hover { color:crimson; }
		.ob_iBC { display: block !important; }
        X.* HTML .ob_iBC { -display: inline !important;  }
        X.command-row .ob_fRwF { padding-left: 200px !important; }
        X.ob_fBfC #ob_iTSuperForm1_LeadNoteContainer { height: 75px !important; }
        X.ob_fBfC #ob_iTSuperForm1_SalesNoteContainer { height: 220px !important; }
	    X.FieldSet1 { width: 300px !important; }
        X.ob_gFEC { left: 0px !important; right: auto !important; }
        Xob_gFALC { float: left !important; }
	    .ob_iCboTC_T { margin-top: -3px} 
	    .separator { width: 8%; color: #000; display:-moz-inline-stack; display:inline-block; zoom:1; *display:inline; text-align:center; top: 2px; position: relative; font-size: 10px; height: 5px; }	
	</style>		
</head>

<body>	

	<form id="Form1" runat="server">

        <asp:SqlDataSource ID="sdsLeadFlat" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ClientManager %>" 
        >
        </asp:SqlDataSource>					

	    <asp:ObjectDataSource runat="server" ID="ods1" TypeName="LeadsDataPerformance"
		    SelectMethod="GetLeads" SelectCountMethod="GetLeadsCount" EnablePaging="true" SortParameterName="sortExpression" />

        <div><asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder></div>
	    <asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>	
		
	</form>

</body>

</html>
