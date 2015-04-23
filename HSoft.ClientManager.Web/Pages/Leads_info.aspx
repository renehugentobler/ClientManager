<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leads_info.aspx.cs" Inherits="Pages_Leads_info" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Interface" Assembly="obout_Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
    <div>

<obout:Grid id="gridLeads" runat="server" AutoGenerateColumns="false" FolderStyle="styles/grid/style_5">
<%--			<Columns>
				<obout:Column ID="Column2" DataFormatString="{0:M/d/yyyy}" DataField="callLaterDate" HeaderText="Call Later" Width="200" runat="server">
				    <FilterOptions>
				        <obout:CustomFilterOption IsDefault="false" ID="Between_callLaterDate" Text="Between">
				            <TemplateSettings FilterTemplateId="callLaterDateBetweenFilter" 
				                FilterControlsIds="StartDate_callLaterDate,EndDate_callLaterDate"
				                FilterControlsPropertyNames="value,value" />
				        </obout:CustomFilterOption>
				    </FilterOptions>
				</obout:Column>
			</Columns>--%>

			<Templates>
			    <obout:GridTemplate runat="server" ID="callLaterDateBetweenFilter">
			        <Template>
			            <div style="width: 99%;padding: 0px;margin: 0px; font-size: 5px;">
			                <obout:OboutTextBox runat="server" ID="StartDate_callLaterDate"  Width="45%" />
			                <obout:OboutTextBox runat="server" ID="EndDate_callLaterDate" Width="45%" />
			            </div>
			        </Template>
			    </obout:GridTemplate>
			    
			</Templates>

</obout:Grid>
            
    </div>
    </form>
</body>
</html>
