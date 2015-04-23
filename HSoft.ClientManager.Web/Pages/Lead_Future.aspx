<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lead_Future.aspx.cs" Inherits="Pages_Lead_Future" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Refresh" content="60" />
	<title>Leads Manager</title>
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
	</style>
	<link type="text/css" rel="stylesheet" href="styles/premiere_blue/Combobox/style.css" />
    <script type="text/javascript">
        function updateAssignedTo(AssignedToId, iRowIndex) {
            var oRecord = new Object();
            
            oRecord.Id = grid1.Rows[iRowIndex].Cells[0].Value;
            oRecord.AssignedToId = AssignedToId;
            // alert(JSON.stringify(oRecord));
            grid1.executeUpdate(oRecord);
        }
    </script>

</head>

<body>
    <form id="form1" runat="server">
    <div>

		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>
