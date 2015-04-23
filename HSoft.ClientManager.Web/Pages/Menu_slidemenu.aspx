<%@ Page Language="C#" Inherits="OboutInc.oboutAJAXPage" %>

<%@ Register TagPrefix="osm" Namespace="OboutInc.SlideMenu" Assembly="obout_SlideMenu3_Pro_Net" %>
<%@ Register TagPrefix="oajax" Namespace="OboutInc" Assembly="obout_AJAXPage" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head runat="server">
        <title></title>
        <script language="C#" runat="server">
	        public void UpdateSlideMenu(string cId)
	        {
		        pro7.SelectedId = cId;	
		        UpdatePanel("cp_slidemenu");
	        }
        </script>
		<script type="text/javascript" language="javascript">
		    function LoadMainPage(id,parm) {

		        // select the click-ed node from the slidemenu using a callback to the server and a callbackpanel for update
		        ob_post.post(null, 'UpdateSlideMenu', function () { }, { "cId": id });

		        if (typeof parm === 'undefined') {
		            parm = ''
		        }
		        else
		        {
		            parm = '?parm='+escape(parm)
		        }

		        // load the right content page with the details
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
						SelectedId = "x_x"
                        AllExpanded ="true" 
                        KeepExpanded =" true"    
                    >
					<menuitems>
					    <osm:Parent id="a">Contacts</osm:Parent>
                            <osm:Child id="a_a" Visible="false" OnClientClick="LoadMainPage('Info')">Contacts</osm:Child>
                            <osm:Child id="a_z" OnClientClick="LoadMainPage('Dial800')">Phone Leads</osm:Child>
					    <osm:Parent id="Parent1">Sales</osm:Parent>
                            <osm:Child id="b_0" OnClientClick="LoadMainPage('ltest')">Leads Sales</osm:Child>
                            <osm:Child id="b_a" Visible="false" OnClientClick="LoadMainPage('Lead_Sales_Flat')">Leads Sales</osm:Child>
                            <osm:Child id="b_b" OnClientClick="LoadMainPage('Lead_Assign')">Assign Leads</osm:Child>
                            <osm:Child id="b_c" Visible="false" OnClientClick="LoadMainPage('Lead_Email')">Lead Emails</osm:Child>
					    <osm:Parent id="c">Surveys</osm:Parent>
                            <osm:Child id="c_a" OnClientClick="LoadMainPage('Survey','Copy of Income Challenge 85000')">Copy of Income Challenge 85000</osm:Child>
                            <osm:Child id="c_b" Visible="false" OnClientClick="LoadMainPage('Info','Income Estimate')">Income Estimate</osm:Child>
                            <osm:Child id="c_c" Visible="false" OnClientClick="LoadMainPage('Info','Aptitude Test 1')">Aptitude Test 1</osm:Child>
					    <osm:Parent id="x" >Configuration</osm:Parent>
						    <osm:Child id="x_a" OnClientClick="LoadMainPage('EmailEditor')">Emails</osm:Child>
						    <osm:Child id="x_b" Visible="false" OnClientClick="LoadMainPage('Info')">Status</osm:Child>
						    <osm:Child id="x_x" OnClientClick="LoadMainPage('Info')">Info</osm:Child>
						    <osm:Child id="x_z" OnClientClick="LoadMainPage('Tracker')">Tracker</osm:Child>
					    <osm:Parent id="z">Resources</osm:Parent>
							<osm:Child id="z_a" Url="http://rakerappliancerepair.com" urlTarget="RightContent">Raker Website</osm:Child>
							<osm:Child id="z_b" Url="https://login.constantcontact.com/login/login.sdo?goto=https%3A%2F%2Fwww.constantcontact.com%2Fprocessing_login.jsp" urlTarget="RightContent">ConstantContact</osm:Child>
							<osm:Child id="z_c" Url="http://apps.dial800.com/CallView/?view=analytics" urlTarget="RightContent">Dial 800</osm:Child>
					</menuitems>
				</osm:SlideMenu>
			</content>
		</oajax:CallbackPanel>	

	</body>
</html>