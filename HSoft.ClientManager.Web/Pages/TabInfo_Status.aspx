<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabInfo_Status.aspx.cs" Inherits="TabInfo_Status" validateRequest="false" %>

<%@ Register TagPrefix="obout" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="owd" Namespace="OboutInc.Window" Assembly="obout_Window_NET" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<%--    <meta http-equiv="Refresh" content="600" />--%>
    <title>Leads Assignment</title>
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
            // alert(JSON.stringify(records[0]));
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
            // get the Harry marker out
            SuperForm1_SalesNote.value(records[0].SalesNote.replace('<font color=red><b>HDR</b></font>', 'HDR'));
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
            // get the Harry marker out
            SuperForm1_SalesNote.value(record.SalesNote.replace('<font color=red><b>HDR</b></font>', 'HDR'));
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
            Grid1.executeUpdate(data);
        }
        function survey() {
            var windowWidth = 900;
            var windowHeight = 600;
            var centerLeft = (window.screen.availWidth - windowWidth) / 3;
            var centerTop = (window.screen.availHeight - windowHeight) / 3;
            //            window.open('TabSurvey.aspx?email=' + SuperForm1_EMail.value(), 'Survey', 'width=' + windowWidth + ',height=' + windowHeight + ',left=' + centerLeft + ',top=' + centerTop + ',toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,modal=yes');
            window.open('TabSurvey.aspx?email=' + SuperForm1_EMail.value(), 'Survey', 'width=' + windowWidth + ', height=' + windowHeight + ', left=50, top=50, location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,modal=yes');
            window.focus();
        }
        function email() {
            var Id = document.getElementById('Id').value;
            var windowWidth = 900;
            var windowHeight = 600;
            var centerLeft = (window.screen.availWidth - windowWidth) / 3;
            var centerTop = (window.screen.availHeight - windowHeight) / 3;
            //            window.open('TabSurvey.aspx?email=' + SuperForm1_EMail.value(), 'Survey', 'width=' + windowWidth + ',height=' + windowHeight + ',left=' + centerLeft + ',top=' + centerTop + ',toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,modal=yes');
            window.open('EmailIndividialLead.aspx?Id=' + Id, 'EMail', 'width=' + windowWidth + ', height=' + windowHeight + ', left=50, top=50, location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=no,modal=yes');
            window.focus();
        }
        function cancelChanges() {
            Window1.Close();
        }
    </script>
</head>
<body>

<form id="Form1" runat="server">
    <div>

        <div>
            <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
        </div>

        <asp:PlaceHolder runat="server" ID="Grid1Container" /> 

        <asp:PlaceHolder runat="server" ID="SuperForm1Container" /> 

        <asp:SqlDataSource ID="sdsLeadFlat" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ClientManager %>" 
            SelectCommand="SELECT * FROM [Lead_Flat] ORDER BY EntryDate DESC"
            UpdateCommand="UPDATE [Lead_Flat] 
                              SET Name=@Name,
                                  EMail=@EMail,
                                  Phone=@Phone,
                                  TimeZone=@TimeZone,
                                  EntryDate=@EntryDate,
                                  CallLaterDate=@CallLaterDate,
                                  Source=@Source,
                                  SourceId=(SELECT Id FROM _LeadSource WHERE Name=@Source AND isdeleted=0),
                                  AssignedTo=@AssignedTo,
                                  AssignedToId=(SELECT Id FROM Employee WHERE DisplayName=@AssignedTo AND isdeleted=0),
                                  Priority=@Priority,
                                  PriorityId=(SELECT Id FROM _LeadPriority WHERE Name=@Priority AND isdeleted=0),
                                  MsgHistory=@MsgHistory,
                                  LeadNote=@LeadNote,
                                  SalesNote=@SalesNote,
                                  updatedate=getdate() 
                            WHERE Id=@Id">
            <UpdateParameters>
                <asp:Parameter Name="Id" Type="String" />
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="EMail" Type="String" />
                <asp:Parameter Name="Phone" Type="String" />
                <asp:Parameter Name="TimeZone" Type="String" />
                <asp:Parameter Name="EntryDate" Type="String" />
                <asp:Parameter Name="CallLaterDate" Type="DateTime" />
                <asp:Parameter Name="Source" Type="String" />
                <asp:Parameter Name="Priority" Type="String" />
                <asp:Parameter Name="AssignedTo" Type="String" />
                <asp:Parameter Name="MsgHistory" Type="String" />
                <asp:Parameter Name="LeadNote" Type="String" />
                <asp:Parameter Name="SalesNote" Type="String" />
            </UpdateParameters>
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
