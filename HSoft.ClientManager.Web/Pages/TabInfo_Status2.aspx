<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabInfo_Status2.aspx.cs" Inherits="Pages_TabInfo_Status" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Priority Statistics</title>
	<style type="text/css">
        .super-form { margin: 12px; }
        .ob_fC table td { white-space: normal !important; }
        .command-row .ob_fRwF { padding-left: 50px !important; }
		.tdText { font:11px Verdana; color:#333333; }
		.floating { float: left; padding-right: 10px; }
		.option2{ font:11px Verdana; color:#0033cc; padding-left:4px; padding-right:4px; }
		a { font:11px Verdana; color:#315686; text-decoration:underline; }
		a:hover { color:crimson; }
		.ob_iBC { display: block !important; }
        * HTML .ob_iBC { -display: inline !important;  }
        .command-row .ob_fRwF { padding-left: 200px !important; }
        .ob_fBfC #ob_iTSuperForm1_LeadNoteContainer { height: 75px !important; }
        .ob_fBfC #ob_iTSuperForm1_SalesNoteContainer { height: 220px !important; }
	    .FieldSet1 { width: 300px !important; }
        .ob_gFEC { left: 0px !important; right: auto !important; }
        .ob_gFALC { float: left !important; }
	    .ob_iCboTC_T { margin-top: -3px} 
	    .separator { width: 8%; color: #000; display:-moz-inline-stack; display:inline-block; zoom:1; *display:inline; text-align:center; top: 2px; position: relative; font-size: 10px; height: 5px; }	
    </style>
    <script type="text/javascript">
        function onClientSelect(sender, records) {
//            alert(JSON.stringify(records[0]));
            Window1.Open();
            Window1.screenCenter();
            document.getElementById('Id').value = records[0].Id;
            SuperForm1_Name.value(records[0].Name);
            SuperForm1_EMail.value(records[0].EMail);
            SuperForm1_Phone.value(records[0].Phone);
            SuperForm1_TimeZone.value(records[0].TimeZone);
            SuperForm1_EntryDate.value(records[0].EntryDate);
            SuperForm1_CallLaterDate.value(records[0].CallLaterDate);
            SuperForm1_Source.value(records[0].Source);
            SuperForm1_Priority.value(records[0].Priority);
            SuperForm1_AssignedTo.value(records[0].AssignedTo);
            SuperForm1_MsgHistory.value(records[0].MsgHistory);
            SuperForm1_LeadNote.value(records[0].LeadNote);
            SuperForm1_SalesNote.value(records[0].SalesNote);
            return false;
        }
        function Grid1_ClientEdit(sender, record) {
            Window1.Open();
            Window1.screenCenter();
            document.getElementById('Id').value = record.Id;
            SuperForm1_Name.value(record.Name);
            SuperForm1_EMail.value(record.EMail);
            SuperForm1_Phone.value(record.Phone);
            SuperForm1_TimeZone.value(record.TimeZone);
            SuperForm1_EntryDate.value(record.EntryDate);
            SuperForm1_CallLaterDate.value(record.CallLaterDate);
            SuperForm1_Source.value(record.Source);
            SuperForm1_Priority.value(record.Priority);
            SuperForm1_AssignedTo.value(record.AssignedTo);
            SuperForm1_MsgHistory.value(record.MsgHistory);
            SuperForm1_LeadNote.value(record.LeadNote);
            SuperForm1_SalesNote.value(record.SalesNote);
            return false;
        }
        function saveChanges() {
            Window1.Close();
            var Id = document.getElementById('Id').value;
            var data = new Object();
            data.Name = SuperForm1_Name.value();
            data.EMail = SuperForm1_EMail.value();
            data.Phone = SuperForm1_Phone.value();
            data.TimeZone = SuperForm1_TimeZone.value();
            data.EntryDate = SuperForm1_EntryDate.value();
            data.CallLaterDate = SuperForm1_CallLaterDate.value();
            data.Source = SuperForm1_Source.value();
            data.Priority = SuperForm1_Priority.value();
            data.AssignedTo = SuperForm1_AssignedTo.value();
            data.MsgHistory = SuperForm1_MsgHistory.value();
            data.LeadNote = SuperForm1_LeadNote.value();
            data.SalesNote = SuperForm1_SalesNote.value();
            data.Id = Id;
            alert(JSON.stringify(data));
            Grid1.executeUpdate(data);
        }
        function survey()
        {
            var windowWidth = 900;
            var windowHeight = 600;
            var centerLeft = (window.screen.availWidth - windowWidth) / 3;
            var centerTop = (window.screen.availHeight - windowHeight) / 3;
            window.open('TabSurvey.aspx?email=' + SuperForm1_EMail.value(), 'Survey', 'width='+windowWidth+', height='+windowHeight+', left=50, top=50, location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,modal=yes');
            window.focus();
        }
        function cancelChanges()
        {
           Window1.Close();
       }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<asp:PlaceHolder ID="phGrid1" runat="server"></asp:PlaceHolder>
    
        <asp:PlaceHolder runat="server" ID="SuperForm1Container" /> 

        <asp:SqlDataSource ID="sdsLeadFlat" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ClientManager %>" 
            UpdateCommand="UpdateRecord">
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="sdsSalesPeople" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ClientManager %>" 
            SelectCommand="SELECT DisplayName AssignedTo FROM [Employee] WHERE isSales=1 AND isdeleted=0 ORDER BY DisplayName ASC">
        </asp:SqlDataSource>            

        <asp:SqlDataSource ID="sdsPriority" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ClientManager %>" 
            SelectCommand="SELECT Name Priority FROM [_LeadPriority] WHERE isdeleted=0 ORDER BY Sequence ASC">
        </asp:SqlDataSource>            

    </div>
    </form>
</body>
</html>
