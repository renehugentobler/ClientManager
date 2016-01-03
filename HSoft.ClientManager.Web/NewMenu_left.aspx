<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewMenu_left.aspx.cs" Inherits="NewMenu_left" %>

<%@ Register TagPrefix="osm" Namespace="OboutInc.SlideMenu" Assembly="obout_SlideMenu3_Pro_Net" %>
<%@ Register TagPrefix="oajax" Namespace="OboutInc" Assembly="obout_AJAXPage" %> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    	<script type="text/javascript">
	    function LoadMainPage(id, parm)
	    {
	        if (typeof parm === 'undefined') {
	            parm = ''
	        }
	        else {
	            parm = '?parm=' + escape(parm)
	        }
		    // load the right content page with the details
		    // window.parent.mySpl.loadPage('RightContent', 'NewMenu_Main.aspx?cId=' + cId)
		    window.parent.mySpl.loadPage("RightContent", id + '.aspx' + parm)
        }
	</script>
</head>
<body>
    <oajax:CallbackPanel id="cp_slidemenu" runat="server">
	    <content>
		    <osm:SlideMenu
				    id = "pro7"
				    runat = "server"
				    StyleFolder = "styles/pro_7/slidemenu"
				    Height = "-1"
				    SelectedId = "StartInfo"
                    AllExpanded ="true" 
                    KeepExpanded =" true">
			    <menuitems>
				    <osm:Parent id="MyPages">My Pages</osm:Parent>
					    <osm:Child id="StartInfo" OnClientClick="LoadMainPage('TabInfo')">My Info</osm:Child>
				    <osm:Parent id="Lead">Sales</osm:Parent>
                        <osm:Child id="LeadSales" OnClientClick="LoadMainPage('ltest')">Leads Sales</osm:Child>
                        <osm:Child id="LeadSales2" OnClientClick="LoadMainPage('LeadSales')">Quick Leads</osm:Child>
                        <osm:Child ID="LeadPromoEmails" OnClientClick="LoadMainPage('Email_Promo_Send')">Promo Emails</osm:Child>
                    <osm:Parent id="Survey">Surveys</osm:Parent>
				    <osm:Parent id="Configuration" >Configuration</osm:Parent>
<%--                    <osm:Child id="Email" OnClientClick="LoadMainPage('EmailEditor')">Emails</osm:Child>--%>
					    <osm:Child id="SalesConfig" OnClientClick="LoadMainPage('Info')">Impersonate</osm:Child>
<%--					    <osm:Child id="PromoEmails" OnClientClick="LoadMainPage('EMail_Promo')">Promo Emails</osm:Child>--%>
<%--                    <osm:Child id="Tracker" OnClientClick="LoadMainPage('Tracker')">Tracker</osm:Child>--%>
				    <osm:Parent id="Resources">Resources</osm:Parent>
					    <osm:Child id="RakerURL" Url="http://rakerappliancerepair.com" urlTarget="RightContent">Raker Website</osm:Child>
			    </menuitems>
		    </osm:SlideMenu>
	    </content>
    </oajax:CallbackPanel>	

	</body>
</html>
