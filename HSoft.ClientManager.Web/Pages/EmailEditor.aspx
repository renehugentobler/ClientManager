<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailEditor.aspx.cs" Inherits="Pages_EmailEditor" %>

<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor.ToolbarButton" TagPrefix="obout" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_Net" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Editor Sample</title>
    <style type="text/css">
        .tdText { font: 11px Verdana; color: #333333; }
        .option2 { font: 11px Verdana; color: #0033cc; padding-left: 4px; padding-right: 4px; }
        a { font: 11px Verdana; color: #315686; text-decoration: underline; }
        a:hover { color: crimson; }
        .rowEditTable td { font-family: Verdana; font-size: 10px; color: #4B555E;}
        div.ob_gCc2, div.ob_gCc2C, div.ob_gCc2R { padding-left: 3px !important; }
    </style>
</head>
<body style="font:12px Verdana;">

    <script type="text/JavaScript">
        function onAddEdit() {
            // get the Editor component
            alert("<%= EditorID.ClientID %>");
            var editorObject = $find(document.getElementById("<%= EditorID.ClientID %>").value);
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
        }

        function onBeforeInsertUpdate() {
            var editorObject = $find(document.getElementById("<%= EditorID.ClientID %>").value);
            document.getElementById('EditorContent').value = editorObject.get_content();
            editorObject.clearHistory();
        }

        function onBeforeClientCancelEdit() {
            var editorObject = $find(document.getElementById("<%= EditorID.ClientID %>").value);
            var editPanel = editorObject.get_editPanel();
            editPanel.ensurePopupsClosed();
            editorObject.clearHistory();
        }
    </script>

    <form runat="server">

        <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
        <obout:EditorPopupHolder runat="Server" ID="popupHolder" />

        <asp:HiddenField ID="EditorID" runat="server" />
    
        <obout:Grid ID="grid1" runat="server" Serialize="true" AutoGenerateColumns="false" AllowFiltering="false" AllowSorting="false"
            FolderStyle="styles/grid/premiere_blue" OnRebind="RebindGrid" OnInsertCommand="InsertRecord" OnDeleteCommand="DeleteRecord" OnUpdateCommand="UpdateRecord">
            <ClientSideEvents OnClientEdit="onAddEdit" OnClientAdd="onAddEdit" OnBeforeClientUpdate="onBeforeInsertUpdate"
                OnBeforeClientInsert="onBeforeInsertUpdate" OnBeforeClientCancelEdit="onBeforeClientCancelEdit" />
            <Columns>
    		    <obout:Column Width = "90" AllowEdit = "true" AllowDelete = "true" >
                </obout:Column>
                <obout:Column DataField="Id" HeaderText="ID" ReadOnly="true" Visible="false">
                </obout:Column>
                <obout:Column DataField="EmailId" HeaderText="Id" Width="40"></obout:Column>
                <obout:Column DataField="EmailDescription" HeaderText="Name" Width="180" Wrap="true"></obout:Column>
                <obout:Column DataField="EmailSubject" HeaderText="Subject" Width="180" Wrap="true"></obout:Column>
                <obout:Column DataField="EmailBody" HeaderText="Body" Width="700" ParseHTML="true">
                    <TemplateSettings EditTemplateId="tmpEditor" />
                </obout:Column>
            </Columns>
            <Templates>
                <obout:GridTemplate runat="server" ControlID="EditorContent" ID="tmpEditor" ControlPropertyName="value">
                    <Template>
                        <obout:Editor runat="server" ID="Editor" EditPanel-Height="300px" Width="100%" PopupHolderID="popupHolder">
                            <BottomToolbar ShowDesignButton="false" ShowHtmlTextButton="false" ShowPreviewButton="false" />
                        </obout:Editor>
                        <input type="hidden" id="EditorContent" />
                    </Template>
                </obout:GridTemplate>
                <obout:GridTemplate runat="server" ID="ButtonsTemplate">
                    <Template>
                        <obout:OboutButton ID="OboutButton1" runat="server" Text="Edit" Width="70" />
                        <obout:OboutButton ID="OboutButton2" runat="server" Text="Delete" Width="70" OnClientClick="return deleteRecord(this);"  />
                    </Template>
                </obout:GridTemplate>
                <obout:GridTemplate runat="server" ID="ButtonsEditTemplate">
                    <Template>
                        <obout:OboutButton ID="BtnUpdate" runat="server" Text="Update" Width="70" OnClientClick="return updateRecord(this, 'BtnUpdate');"  /> 
                        <obout:OboutButton ID="BtnCancel" runat="server" Text="Cancel" Width="70" OnClientClick="return cancelEdit(this, 'BtnCancel');"  />
                    </Template>
                </obout:GridTemplate>            </Templates>
        </obout:Grid>

    </form>

</body>
</html>
