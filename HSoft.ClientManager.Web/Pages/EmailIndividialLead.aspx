<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailIndividialLead.aspx.cs" Inherits="EmailIndividialLead" %>
<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.HTMLEditor" TagPrefix="obout" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Emailer 1.00</title>
    <style type="text/css">
            a {
                   font-size:10pt;font-family:Tahoma
            }
            a:hover {
                   color:crimson;
            }
            .tdText {
                    font:11px Verdana;
                    color:#333333;
            }
    </style>
</head>
<body style="font:12px Verdana;">
<script type="text/JavaScript">
    function enDisClicked() {
        $find("<%= editor.ClientID %>").get_editPanel().setCancelOnPostback();
        return true;
    }
</script>
    <br /><br />
    <form id="form1" runat="server">
        <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />

        <asp:UpdatePanel ID="updatePanel1" runat="server">
            <ContentTemplate>
               <obout:OboutTextBox runat="server" ID="subject" Width="400px" />
               <obout:Editor runat="server" Id="editor" Height="500px" Width="100%">
                 <EditPanel AutoFocus="true" ActiveMode="design" IndentInDesignMode="20" />
                 <BottomToolBar ShowDesignButton="true" ShowHtmlTextButton="false" ShowPreviewButton="true" >
                 </BottomToolBar>
               </obout:Editor>
               <br />
               <asp:Button runat="server" Text="Send Mail" ID="Submit" OnClick="Submit_click" />
               <asp:Button runat="server" Text="Close Window" ID="Close" OnClick="Close_click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>