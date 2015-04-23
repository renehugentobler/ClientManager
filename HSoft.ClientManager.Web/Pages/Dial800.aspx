<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dial800.aspx.cs" Inherits="Pages_Dial800" %>
<%@ Register TagPrefix="obout" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
    <div>
    
    <obout:Grid 
        id="grid1" 
        runat="server" 
        CallbackMode="true" 
        Serialize="true" 
        AutoGenerateColumns="false"
		FolderStyle="styles/grid/premiere_blue" 
        DataSourceID="sdsDial800" 
        AllowAddingRecords="false"
        PageSize="-1">
	    <Columns>				
		    <obout:Column 
                ID="DIAL800ID"
                DataField="dial800id"
                Visible="false"/>
		    <obout:Column 
                ID="DNIS"
                DataField="dnis"
                Visible="false"/>
		    <obout:Column 
                ID="ANI" 
                DataField="ani" 
                HeaderText="Phone#" 
                Visible="true" 
                Width="100"
                ReadOnly="true" 
                AllowSorting="true"
                AllowFilter="false"                 
                DataFormatString="{0:(###) ###-####}"/>
		    <obout:Column 
                ID="TARGET"
                DataField="target"
                Visible="false"/>
		    <obout:Column 
                ID="START"
                DataField="start"
                HeaderText="Call Date" 
                Visible="true" 
                Width="120"
                ReadOnly="true" 
                AllowSorting="true"
                AllowFilter="false"                 
                DataFormatString="{0:MM/dd/yyyy hh:mm tt}"/>
		    <obout:Column 
                ID="DURATION"
                DataField="duration"
                HeaderText="Duration" 
                Visible="true" 
                Width="70"
                ReadOnly="true" 
                AllowSorting="false"
                AllowFilter="false"                 
                DataFormatString="{0:mm:ss}"/>
		    <obout:Column 
                ID="PHONETYPE"
                DataField="phonetype"
                HeaderText="Linetype" 
                Visible="true" 
                Width="100"
                ReadOnly="true" 
                AllowSorting="false"
                AllowFilter="true"/>
		    <obout:Column 
                ID="STATUS"
                DataField="status"
                Visible="false"/>
		    <obout:Column 
                ID="TERMINATEDBY"
                DataField="terminatedby"
                Visible="false"/>
		    <obout:Column 
                ID="CITY"
                DataField="city"
                HeaderText="City" 
                Visible="true" 
                Width="140"
                ReadOnly="true" 
                AllowSorting="true"
                AllowFilter="true"/>
		    <obout:Column 
                ID="STATE"
                DataField="state"
                HeaderText="State" 
                Visible="true" 
                Width="50"
                ReadOnly="true" 
                AllowSorting="true"
                AllowFilter="true"/>
		    <obout:Column 
                ID="ZIPCODE"
                DataField="zipcode"
                HeaderText="ZIP" 
                Visible="true" 
                Width="50"
                ReadOnly="true" 
                AllowSorting="true"
                AllowFilter="false"/>
		    <obout:Column 
                ID="RECORDING"
                DataField="recording"
                HeaderText="Link" 
                Visible="true" 
                Width="50"
                ReadOnly="true" 
                AllowSorting="false"
                AllowFilter="false"                 
                ParseHTML="true"/>
        </Columns>
    </obout:Grid>
    <asp:SqlDataSource ID="sdsDial800" runat="server" SelectCommand="SELECT dial800id, dnis, ani, target, start, dateadd(second,duration,'1980-01-01 00:00') duration, phonetype, status, terminatedby, city, state, zipcode, totalsaleamount,dbo.tohtml(recording) recording FROM Dial800 ORDER BY start DESC"></asp:SqlDataSource>

    </div>
    </form>
</body>
</html>
