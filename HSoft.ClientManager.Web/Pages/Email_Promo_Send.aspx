<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Email_Promo_Send.aspx.cs" Inherits="Pages_Email_Promo_Send" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Leads Manager</title>
	<style type="text/css">
	    .tdText { font:11px Verdana; color:#333333; }
	    .option2{ font:11px Verdana; color:#0033cc; padding-left:4px; padding-right:4px; }
	    a { font:11px Verdana; color:#315686; text-decoration:underline; }
	    a:hover { color:crimson; }
	    .ob_fC table td { white-space: normal !important; }
        .command-row .ob_fRwF { padding-left: 200px !important; }
        .ob_gRETpl #ob_iTSuperForm1_LeadNoteContainer { height: 75px !important; }
        .ob_gRETpl #ob_iTSuperForm1_SalesNoteContainer { height: 150px !important; }
        .ob_gFEC { left: 0px !important; right: auto !important; }
        .ob_gFALC { float: left !important; }
	    .ob_iCboTC_T { margin-top: -3px} 
	    .separator { width: 8%; color: #000; display:-moz-inline-stack; display:inline-block; zoom:1; *display:inline; text-align:center; top: 2px; position: relative; font-size: 10px; height: 5px; }	
	</style>
	<link type="text/css" rel="stylesheet" href="styles/premiere_blue/Combobox/style.css" />
    <script type="text/javascript">
        function onClientSelect(selectedRecords) {
            var index = -1;
            for (var i = 0; i < grid1.Rows.length; i++) {
                if (grid1.Rows[i].Cells[1].Value == selectedRecords[0].Id) {
                    index = i;
                    break;
                }
            }

            if (index != -1) {
                grid1.editRecord(index);
            }
        }
        function OpenWindowWithHtml(html, title) {
            var myWindow = window.open('', title);
            myWindow.document.write(html);
            myWindow.focus();
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
