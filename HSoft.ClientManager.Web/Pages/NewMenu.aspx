<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewMenu.aspx.cs" Inherits="Pages_NewMenu" %>

<%@ Register TagPrefix="obspl" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net" %>
<%@ Register TagPrefix="spl" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Client Manager</title>
    <meta name="viewport" content="width=device-width" />	<style type="text/css">
        .tdText { font:11px Verdana; color:#333333; }
        body { font-family:Verdana; font-size:8pt; }			
        .text { background-color:#ebe9ed; font-size:11px; text-align:center; }
        .textContent { font-size:11px; text-align:center; }
    </style>
</head>
<body>
    <form id="formNM" runat="server">

        <obspl:Splitter id="mySpl" runat="server" StyleFolder="styles/default_light/splitter" CookieDays="0">
            <LeftPanel WidthDefault="170" WidthMin="0" WidthMax="170">
		        <content Url="NewMenu_left.aspx" />
			    <footer height="30">
				    <div style="width:100%;height:100%;background-color:#ebe9ed;align-content:center;text-align:center;" class="tdText">
                            &copy; <%: DateTime.Now.Year %> - Harry D. Raker<br />Website by HSoft
				    </div>
        		</footer>
            </LeftPanel>
	        <RightPanel ID="RightContent">
		        <content Url="TabInfo.aspx" />
            </RightPanel>
        </obspl:Splitter>

    </form>
</body>
</html>
