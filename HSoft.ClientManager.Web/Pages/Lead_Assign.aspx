<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lead_Assign.aspx.cs" Inherits="Pages_Lead_Assign" %>
<%@ Register TagPrefix="obout" Namespace="Obout.ComboBox" Assembly="obout_ComboBox" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar2" Assembly="obout_Calendar2_Net" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Universal Lead Assignmentpage</title>
	<style type="text/css">
		.tdText { font:11px Verdana; color:#333333; }
		.option2{ font:11px Verdana; color:#0033cc; padding-left:4px; padding-right:4px; }
		a { font:11px Verdana; color:#315686; text-decoration:underline; }
		a:hover { color:crimson; }
		.ob_fC table td { white-space: normal !important; }
        .command-row .ob_fRwF { padding-left: 200px !important; }
        .ob_gRETpl #ob_iTSuperForm1_LeadNoteContainer { height: 75px !important; }
        .ob_gRETpl #ob_iTSuperForm1_SalesNoteContainer { height: 150px !important; }
	    .ob_gFEC { width: 100% !important; } 
        .ob_gFAL { width: 1320px !important; }
	    .ob_iCboTC_T { margin-top: 7px} 
	</style>		
</head>
<body>
    <form runat="server">
    <div>

        <div>
            <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
        </div>

    <obout:Grid 
        id="grid1" 
        CallbackMode="true"
        runat="server" 
        Serialize="true" 
        AutoGenerateColumns="false"
		FolderStyle="styles/grid/premiere_blue" 
        DataSourceID="sdsLead_Flat" 
        AllowAddingRecords="false"
        OnRebind="RebindGrid"
        OnUpdateCommand="OnItemUpdated"
        PageSize="-1">
	    <Columns>			
		    <obout:Column 
                DataField = ""
                Visible = "true"
                HeaderText = ""
                Width = "100"
                AllowEdit = "true"
                AllowDelete = "false"/>
            <obout:Column 
                ID="ID"
                DataField="Id"
                Visible="false">
            </obout:Column>
		    <obout:Column 
                ID="CUSTOMERID"
                DataField="CustomerId"
                Visible="false"/>
            <obout:Column 
                ID="Name"
                DataField="Name"
                HeaderText="Name" 
                Visible="true" 
                ReadOnly="true" 
                Width = "120"
                AllowSorting="true"
                Wrap="true"
                AllowFilter="true">
            </obout:Column>        
		    <obout:Column 
                ID="Email"
                DataField="Email"
                HeaderText="Email" 
                Visible="true" 
                ReadOnly="true" 
                Width = "180"
                AllowSorting="true"
                AllowFilter="true">
            </obout:Column>        
		    <obout:Column 
                ID="Phone"
                DataField="Phone"
                HeaderText="Phone" 
                Visible="true" 
                ReadOnly="true" 
                Width = "80"
                AllowSorting="false"
                AllowFilter="false">
            </obout:Column>        
		    <obout:Column 
                ID="TimeZone"
                DataField="TimeZone"
                HeaderText="Zone" 
                Visible="true" 
                ReadOnly="true" 
                Width = "50"
                AllowSorting="false"
                AllowFilter="false">
                </obout:Column>        
		    <obout:Column 
                ID="EntryDate"
                DataField="EntryDate"
                HeaderText="Entry Date" 
                Visible="true" 
                ReadOnly="true" 
                Width = "120"
                AllowSorting = "true"
                AllowFilter = "true"
                NullDisplayText = "missing!"
                DataFormatString = "{0:yyyy/MM/dd}">
            </obout:Column>        
		    <obout:Column 
                ID="CallLaterDate"
                DataField="CallLaterDate"
                HeaderText="Call Later" 
                Visible="true" 
                ReadOnly="false" 
                Width = "120"
                AllowSorting = "true"
                AllowFilter = "true"
                NullDisplayText = "missing!"
                DataFormatString = "{0:yyyy/MM/dd}"
                ApplyFormatInEditMode ="true" >
                <TemplateSettings EditTemplateId="tplDatePicker" />
            </obout:Column>        
		    <obout:Column 
                ID="Source"
                DataField="Source"
                HeaderText="Source" 
                Visible="true" 
                ReadOnly="true" 
                Width = "80"
                AllowSorting = "true"
                AllowFilter = "true">
            </obout:Column>        
		    <obout:Column 
                ID="Status"
                DataField="Status"
                HeaderText="Status" 
                Visible="true" 
                ReadOnly="true" 
                Width = "90"
                AllowSorting = "true"
                AllowFilter = "true">
            </obout:Column>        
		    <obout:Column 
                ID="Priority"
                DataField="Priority"
                HeaderText="Priority" 
                Visible="true" 
                ReadOnly="false" 
                Width = "110"
                AllowSorting = "true"
                AllowFilter = "true">
                <TemplateSettings EditTemplateId="tplPriority" />
            </obout:Column>        
		    <obout:Column 
                ID="AssignedTo"
                DataField="AssignedTo"
                HeaderText="AssignedTo" 
                Visible="true" 
                ReadOnly="false" 
                Width = "140"
                AllowSorting = "true"
                AllowFilter = "true">
                <TemplateSettings EditTemplateId="tplAssignedTo" />
            </obout:Column>        
		    <obout:Column 
                ID="LeadNote"
                DataField="LeadNote"
                HeaderText="Lead Note" 
                Visible="true" 
                ReadOnly="true" 
                Width = "300"
                Wrap = "true"
                AllowSorting = "false"
                AllowFilter = "true">
            </obout:Column>        
		    <obout:Column 
                ID="SalesNote"
                DataField="SalesNote"
                HeaderText="Sales Note" 
                Visible="true" 
                ReadOnly="true" 
                Width = "300"
                Wrap = "true"
                AllowSorting = "false"
                ParseHTML="true"
                AllowFilter = "true">
            </obout:Column>        
    	</Columns>
        <TemplateSettings HeadingTemplateId="HeadingTemplate1" />
		<Templates>								
            <obout:GridTemplate runat="server" ID="HeadingTemplate1">
            </obout:GridTemplate>
            <obout:GridTemplate runat="server" ID="tplDatePicker" ControlID="txtOrderDate" ControlPropertyName="value">
			    <Template>
			        <table width="100%" cellspacing="0" cellpadding="0" style="border-collapse:collapse;">
			            <tr>
			                <td valign="middle">
			                    <obout:OboutTextBox runat="server" ID="txtOrderDate" Width="100%" FolderStyle="styles/premiere_blue/interface/OboutTextBox" />
			                </td>
			                <td valign="middle" width="30">			                        
			                    <obout:Calendar ID="cal1" runat="server" 
									StyleFolder="styles/default/calendar" 
									DatePickerMode="true"
									ShowYearSelector="false"
                                    DateFormat= "yyyy/MM/dd"
									DatePickerImagePath ="styles/calendar/icon2.gif" />
							</td>
					    </tr>
					</table>
			    </Template>
			</obout:GridTemplate>
            <obout:GridTemplate runat="server" ID="tplAssignedTo" ControlID="ComboBox1" ControlPropertyName="value">
                <Template>
                    <obout:ComboBox 
                        runat="server" 
                        ID="ComboBox1" 
                        Width="100%" 
                        FolderStyle="styles/premiere_blue/Combobox">
                        <Items>
                            <obout:ComboBoxItem ID="Option1" runat="server" Value="Anna Drobotova" Text="Anna Drobotova" />
                            <obout:ComboBoxItem ID="Option2" runat="server" Value="Bob Pettit" Text="Bob Pettit" />
                            <obout:ComboBoxItem ID="Option3" runat="server" Value="Frank Marchant" Text="Frank Marchant" />
                            <obout:ComboBoxItem ID="Option4" runat="server" Value="Harry Raker" Text="Harry Raker" />
                            <obout:ComboBoxItem ID="Option5" runat="server" Value="Trent Howell" Text="Trent Howell" />
                        </Items>
                    </obout:ComboBox>
                </Template>
            </obout:GridTemplate>
            <obout:GridTemplate runat="server" ID="tplPriority" ControlID="ComboBox2" ControlPropertyName="value">
                <Template>
                    <obout:ComboBox 
                        runat="server" 
                        ID="ComboBox2" 
                        Width="100%" 
                        FolderStyle="styles/premiere_blue/Combobox">
                        <Items>
                            <obout:ComboBoxItem ID="Option1" runat="server" Value="Hot Lead" Text="Hot Lead" />
                            <obout:ComboBoxItem ID="Option2" runat="server" Value="Interested" Text="Interested" />
                            <obout:ComboBoxItem ID="Option3" runat="server" Value="Maybe Later" Text="Maybe Later" />
                            <obout:ComboBoxItem ID="Option4" runat="server" Value="Not Right" Text="Not Right" />
                            <obout:ComboBoxItem ID="Option5" runat="server" Value="Sold" Text="Sold" />
                            <obout:ComboBoxItem ID="Option6" runat="server" Value="Special" Text="Special" />
                            <obout:ComboBoxItem ID="Option7" runat="server" Value="Spam" Text="Spam" />
                            <obout:ComboBoxItem ID="Option8" runat="server" Value="Black List" Text="Black List" />
                            <obout:ComboBoxItem ID="Option9" runat="server" Value="Undefined" Text="Undefined" />
                        </Items>
                    </obout:ComboBox>
                </Template>
            </obout:GridTemplate>
        </Templates>
    </obout:Grid>

        <asp:SqlDataSource ID="sdsLead_Flat" 
            runat="server"> 
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="sdsPriority" runat="server" 
            SelectCommand="SELECT Name Priority FROM [_LeadPriority] ORDER BY Sequence ASC">
        </asp:SqlDataSource>            

        <asp:SqlDataSource ID="sdsEmployee" runat="server" 
            SelectCommand="SELECT DisplayName AssignedTo FROM [Employee] ORDER BY DisplayName ASC">
        </asp:SqlDataSource>            

        <asp:SqlDataSource ID="sdsSalesPeople" runat="server" 
            SelectCommand="SELECT DisplayName AssignedTo FROM [Employee] WHERE isSales=1 ORDER BY DisplayName ASC">
        </asp:SqlDataSource>            
                            
    </div>
    </form>
</body>
</html>
