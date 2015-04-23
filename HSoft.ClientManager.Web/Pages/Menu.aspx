<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>
<%@ Register Tagprefix="obspl" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head runat="server">
	    <title><%: Page.Title %> - Client Manager</title>
        <meta name="viewport" content="width=device-width" />

		<style type="text/css">
            .tdText 
            {
		        font:11px Verdana;
		        color:#333333;
            }
        </style>

    </head>

	<body>
	    <form runat="server">

            <obspl:Splitter id="mySpl" runat="server" StyleFolder="styles/splitter/default_light" CookieDays="0">

                <LeftPanel WidthDefault="170" WidthMin="0" WidthMax="170">
				    <content Url="Menu_slidemenu.aspx" />
			        <footer height="30">
				        <div style="width:100%;height:100%;background-color:#ebe9ed;align-content:center" class="tdText">
                             &copy; <%: DateTime.Now.Year %> - Harry D. Raker<br /> Website by HSoft
				        </div>
        			</footer>
				</LeftPanel>

				<RightPanel>
					<content Url="Info.aspx" />					
				</RightPanel>

			</obspl:Splitter>

        </form>
    </body>

</html>

