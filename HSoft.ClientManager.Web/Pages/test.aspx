<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="Pages_test" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.ComboBox" Assembly="obout_ComboBox" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Sales Grid</title>
	<style type="text/css">
		.tdText {
			font:11px Verdana;
			color:#333333;
		}
		.option2{
			font:11px Verdana;
			color:#0033cc;				
			padding-left:4px;
			padding-right:4px;
		}
		a {
			font:11px Verdana;
			color:#315686;
			text-decoration:underline;
		}

		a:hover {
			color:crimson;
		}
			
		.ob_fC table td
        {
            white-space: normal !important;
        }
        
        .command-row .ob_fRwF
        {
            padding-left: 200px !important;
        }
        
        .ob_gRETpl #ob_iTSuperForm1_ClientNoteContainer
        {
            height: 160px !important;
        }
        .ob_gRETpl #ob_iTSuperForm1_SalesNoteContainer
        {
            height: 160px !important;
        }
	</style>
</head>

<body>
    <form id="frmgrid" runat="server">
    <div>
    
        <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		

        <obout:Grid id="gridLeads" runat="server" AutoGenerateColumns="false">
        </obout:Grid>

<%--            SelectCommand ="SELECT * FROM [Lead] WHERE Id= @LeadId"--%>
        <asp:SqlDataSource ID="SqlDataSourcePriority" runat="server" />
        <asp:SqlDataSource ID="SqlDataSourceSuperForm" runat="server"
            ConnectionString="data source=.\MSSQL2012;User Id=rar;Password=rar;Initial Catalog=ClientManager" 
            ProviderName="System.Data.SqlClient" 
            SelectCommand ="SELECT * FROM [Lead] WHERE Id= 'c1921066-6837-43df-b8fa-629ea1a2755d'"
        >
            <SelectParameters>
                <asp:Parameter Name="LeadId" Type="String" DefaultValue="" />
            </SelectParameters>
        </asp:SqlDataSource>


    </div>
    </form>
</body>
</html>
