<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabSurvey.aspx.cs" Inherits="Pages_TabSurvey" %>
<%@ Register TagPrefix="oem" Namespace="OboutInc.EasyMenu_Pro" Assembly="obout_EasyMenu_Pro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Survey Page</title>
	<style type="text/css">
        .tdText { font:11px Verdana; color:#333333; }
    </style>
	<script type="text/javascript">
	    function SelectTab(tabID, itemID, itemParm, itemParm1) {
	        ob_em_SelectItem(tabID);
	        document.getElementById('tabIframe').src = "TabSurveyDisplay.aspx?Id=" + itemParm + '&sparm1=' + itemParm1;
//	        alert(document.getElementById('tabIframe').src);
	    }
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
	<table width="100%">
		<tr>
			<td>
				<table cellpadding="0" cellspacing="0" width="100%" >
					<tr>
						<td>
							<oem:EasyMenu id="SurveyTab" runat="server" ShowEvent="Always" StyleFolder="styles/Yahoo/TabStrip" Position="Horizontal" Width="100%">
								<Components>
								</Components>
							</oem:EasyMenu>
						</td>
					</tr>
					<tr>
						<td style="padding-left:3px;">
							<iframe scrolling="yes" style="border:3px solid #c0cff0" frameborder="1" width="100%" id="tabIframe" runat="server"></iframe>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>

    </div>
    </form>
</body>
</html>
