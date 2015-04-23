<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lead_Sales_Flat.aspx.cs" Inherits="Pages_Lead_Sales_Flat" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>
<%@ Register TagPrefix="obout" Namespace="Obout.SuperForm" Assembly="obout_SuperForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Universal Lead Salespage (Flat)</title>
	<style type="text/css">
		.tdText { font:11px Verdana; color:#333333; }
		.option2{ font:11px Verdana; color:#0033cc; padding-left:4px; padding-right:4px; }
		a { font:11px Verdana; color:#315686; text-decoration:underline; }
		a:hover { color:crimson; }
		.ob_fC table td { white-space: normal !important; }
        .command-row .ob_fRwF { padding-left: 200px !important; }
        .ob_gRETpl #ob_iTSuperForm1_LeadNoteContainer { height: 75px !important; }
        .ob_gRETpl #ob_iTSuperForm1_SalesNoteContainer { height: 150px !important; }
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
 OnUpdateCommand="OnItemUpdated"
         OnInsertCommand="OnItemUpdated"
         OnRebind="OnItemUpdated"
        PageSize="-1">
	    <Columns>			
		    <obout:Column 
                DataField = ""
                Visible = "true"
                HeaderText = ""
                Width = "90"
                AllowEdit = "true"
                AllowDelete = "false"/>
            <obout:Column 
                ID="Key"
                DataField="Id"
                Visible="false">
                <TemplateSettings 
                    RowEditTemplateControlId="Id" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_Name" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_Email" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_Phone" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_TimeZone" 
                    RowEditTemplateControlPropertyName="value"/>

                </obout:Column>        
		    <obout:Column 
                ID="EntryDate"
                DataField="EntryDate"
                HeaderText="Entry Date" 
                Visible="true" 
                ReadOnly="true" 
                Width = "70"
                AllowSorting = "true"
                AllowFilter = "true"
                NullDisplayText = "missing!"
                DataFormatString = "{0:yyyy/MM/dd}">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_EntryDate" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
		    <obout:Column 
                ID="CallLaterDate"
                DataField="CallLaterDate"
                HeaderText="Call Later" 
                Visible="true" 
                ReadOnly="false" 
                Width = "70"
                AllowSorting = "true"
                AllowFilter = "true"
                NullDisplayText = "missing!"
                DataFormatString = "{0:yyyy/MM/dd}"
                 ApplyFormatInEditMode ="true">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_CallLaterDate" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_Source" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
		    <obout:Column 
                ID="Priority"
                DataField="Priority"
                HeaderText="Priority" 
                Visible="true" 
                ReadOnly="false" 
                Width = "90"
                AllowSorting = "true"
                AllowFilter = "true">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_Priority" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
		    <obout:Column 
                ID="AssignedTo"
                DataField="AssignedTo"
                HeaderText="AssignedTo" 
                Visible="true" 
                ReadOnly="false" 
                Width = "90"
                AllowSorting = "true"
                AllowFilter = "true">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_AssignedTo" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
		    <obout:Column 
                ID="MsgHistory"
                DataField="MsgHistory"
                HeaderText="History" 
                Visible="true" 
                ReadOnly="true" 
                Width = "60"
                AllowSorting = "false"
                AllowFilter = "false">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_MsgHistory" 
                    RowEditTemplateControlPropertyName="value"/>
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
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_LeadNote" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
		    <obout:Column 
                ID="SalesNote"
                DataField="SalesNote"
                HeaderText="Sales Note" 
                Visible="true" 
                ReadOnly="false" 
                Width = "300"
                Wrap = "true"
                AllowSorting = "false"
                AllowFilter = "true">
                <TemplateSettings 
                    RowEditTemplateControlId="SuperForm1_SalesNote" 
                    RowEditTemplateControlPropertyName="value"/>
            </obout:Column>        
	    </Columns>
		    <TemplateSettings RowEditTemplateId="tplRowEdit" />
		    <Templates> 			    
			    <obout:GridTemplate runat="server" ID="tplRowEdit">
                    <Template>
                        <input type="hidden" id="Id" />
                        <obout:SuperForm ID="SuperForm1" runat="server" 
                            DataSourceID="sdsSuperForm1"
                            AutoGenerateRows="false"
                            AutoGenerateInsertButton ="false"
                            AutoGenerateEditButton="false"
                            AutoGenerateDeleteButton="false"   
                            OnItemCommand="OnItemUpdated"
                            OnItemInserted="OnItemUpdated"
                            OnItemInserting="OnItemUpdated"
                            OnItemUpdated="OnItemUpdated"
                            OnItemUpdating="OnItemUpdating"           
                            OnDataBound="OnDataBound"       
                            OnModeChanged="OnItemUpdated"
                            OnModeChanging="OnItemUpdated"
                            DataKeyNames="Id" DefaultMode="Insert" Width="99%">
                            <Fields>
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="Name" HeaderText="Name" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="Email" HeaderText="Email" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="Phone" HeaderText="Phone" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="TimeZone" HeaderText="Time Zone" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="EntryDate" HeaderText="Entry Date" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:DateField DataField="CallLaterDate" HeaderText="Call Later" FieldSetID="FieldSet1" ControlStyle-Width="250" DataFormatString="{0:yyyy/MM/dd}" ApplyFormatInEditMode="true" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="Source" HeaderText="Source" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                <obout:DropDownListField DataField="Priority" HeaderText="Priority" FieldSetID="FieldSet1" ControlStyle-Width="250" DataSourceID="sdsPriority" />
                                <obout:DropDownListField DataField="AssignedTo" HeaderText="AssignedTo" FieldSetID="FieldSet1" ControlStyle-Width="250" DataSourceID="sdsEmployee" />
                                <obout:BoundField AllowEdit="false" Enabled="false" DataField="MsgHistory" HeaderText="History" FieldSetID="FieldSet1" ControlStyle-Width="250" />
                                
                                <obout:MultiLineField AllowEdit="false" DataField="LeadNote" HeaderText="" FieldSetID="FieldSet2" HeaderStyle-Width="1" ControlStyle-Width="1100"  />
                                <obout:MultiLineField DataField="SalesNote" HeaderText="" FieldSetID="FieldSet2" HeaderStyle-Width="1" ControlStyle-Width="1100"  />
                                <obout:TemplateField FieldSetID="FieldSet4">
                                    <EditItemTemplate>
                                        <obout:OboutButton ID="OboutButton1" runat="server" Text="Save" OnClientClick="grid1.save(); return false;" Width="75" />
                                        <obout:OboutButton ID="OboutButton2" runat="server" Text="Cancel" OnClientClick="grid1.cancel(); return false;" Width="75" />
                                    </EditItemTemplate>
                                </obout:TemplateField>                            
                            </Fields>
                            <FieldSets>
                                <obout:FieldSetRow>
                                    <obout:FieldSet ID="FieldSet1" Title="Lead Information"/>
                                    <obout:FieldSet ID="FieldSet2" Title="Notes"/>
                                </obout:FieldSetRow>
                                <obout:FieldSetRow>
                                    <obout:FieldSet ID="FieldSet4" ColumnSpan="2" CssClass="command-row" />
                                </obout:FieldSetRow>
                            </FieldSets>
                        </obout:SuperForm>                       				                         
                    </Template>
                </obout:GridTemplate>
		    </Templates>
        </obout:Grid>

        <asp:SqlDataSource ID="sdsLead_Flat" 
            runat="server"> 
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="sdsSuperForm1" runat="server" 
            UpdateCommand="UPDATE [Lead_Flat] 
                              SET CallLaterDate=@CallLaterDate
                                  ,Priority=@Priority
                                  ,AssignedTo=@AssignedTo
                                  ,SalesNote=@SalesNote
                            WHERE Id=@Id"
            InsertCommand="UPDATE [Lead_Flat] 
                              SET CallLaterDate=@CallLaterDate
                                  ,Priority=@Priority
                                  ,AssignedTo=@AssignedTo
                                  ,SalesNote=@SalesNote
                            WHERE Id=@Id">
            <UpdateParameters>
                <asp:Parameter Name="CallLaterDate" Type="DateTime" />
                <asp:Parameter Name="Priority" Type="String" />
                <asp:Parameter Name="AssignedTo" Type="String" />
                <asp:Parameter Name="SalesNote" Type="String" />
                <asp:Parameter Name="Id" Type="String" />
            </UpdateParameters>        
            <InsertParameters>
                <asp:Parameter Name="CallLaterDate" Type="DateTime" />
                <asp:Parameter Name="Priority" Type="String" />
                <asp:Parameter Name="AssignedTo" Type="String" />
                <asp:Parameter Name="SalesNote" Type="String" />
                <asp:Parameter Name="Id" Type="String" />
            </InsertParameters>        
        </asp:SqlDataSource>            
        
        <asp:SqlDataSource ID="sdsPriority" runat="server" 
            SelectCommand="SELECT Name Priority FROM [_LeadPriority] ORDER BY Sequence ASC">
        </asp:SqlDataSource>            

        <asp:SqlDataSource ID="sdsEmployee" runat="server" 
            SelectCommand="SELECT DisplayName AssignedTo FROM [Employee] ORDER BY DisplayName ASC">
        </asp:SqlDataSource>            
                            
    </div>
    </form>
</body>
</html>
