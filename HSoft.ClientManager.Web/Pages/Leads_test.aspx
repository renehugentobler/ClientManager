<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leads_test.aspx.cs" Inherits="Pages_Leads_test" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
		<style type="text/css">
			.tdText {
				font:11px Verdana;
				color:#333333;
			}
			.floating
			{
			    float: left;
			    padding-right: 10px;
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
			
			.separator
			{
			    width: 8%;
			    color: #000;
			    display:-moz-inline-stack;
                display:inline-block;
                zoom:1;
                *display:inline;
                text-align:center;
                top: -7px;
                position: relative;
                font-size: 10px;
                height: 5px;
			}
		</style>

</head>
<body>

    <form runat="server">
    <div>

		<asp:PlaceHolder ID="phgridLeads" runat="server"></asp:PlaceHolder>
    
    </div>
    </form>
</body>
</html>
