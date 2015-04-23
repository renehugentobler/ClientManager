<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EMail_Promo.aspx.cs" Inherits="Pages_EMail_Promo" validateRequest="false" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>

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
        function onAddEdit(selectedRecords) {
            // get the Editor component
            var index = -1;
            for (var i = 0; i < grid1.Rows.length; i++) {
                if (grid1.Rows[i].Cells[1].Value == selectedRecords.Id) {
                    index = i;
                    break;
                }
            }
            alert(document.getElementById("EditorID"));
            alert(document.getElementById("EditorID").value);
            var editorObject = $find(document.getElementById("EditorID").value);
            // set the Editor's content
            editorObject.set_content(document.getElementById('EditorContent').value);

            // trick against weird behavior of IE with version number more than or equal 8
            if (Obout.Ajax.UI.HTMLEditor.isIE && Sys.Browser.version >= 8) {
                // get the EditPanel of the Editor
                var editPanel = editorObject.get_editPanel();
                // get active edit mode
                var activeMode = editPanel.get_activeMode();
                // for Design and Preview modes only
                if (activeMode == Obout.Ajax.UI.HTMLEditor.ActiveModeType.Design ||
                    activeMode == Obout.Ajax.UI.HTMLEditor.ActiveModeType.Preview
                ) {
                    // preserve the content
                    var content = editPanel.getContent();
                    // currently active panel in EditPanel
                    var panel = editPanel.get_activePanel();
                    // deactivate it
                    panel._deactivate();
                    // activate the panel again with the preserved content
                    panel._activate(content);
                }
            }
        }    </script>

</head>

<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />

        <obout:EditorPopupHolder runat="Server" ID="popupHolder" />
        <asp:HiddenField ID="EditorID" runat="server" />

		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>
