<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tracker.aspx.cs" Inherits="Pages_Tracker" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tracker</title>
    <style>
     table,th,td
         {
             border:1px solid black;
            border-collapse:collapse;
            border-spacing:5px;     
            padding: 1px 5px 1px 3px;
         }
     </style>
</head>
<body>
    <form runat="server">
        <div>
            <asp:PlaceHolder ID="cbName" runat="server"></asp:PlaceHolder>		
        </div>
    </form>

    <br /><br />

    <b>To Do</b>
    <br /><br />

        <u>Database</u>
        <ul>
            <li><b>Dial 800 : New website to allow Dial800 to push their data</b></li>
            <li>Constant Contact : Automatic and on demand import of new data</li>
            <li><b>Redirect Old Leads pages to new Leadsreceiver page (Video Leads done</b></li>
        </ul>
        <u><b>Leads SalesPerson Page</b></u>
        <ul>
            <li>Limit Filter otions to sensible option per field</li>
            <li>Implement Fuzzy logic filter for Notes fields</li>
            <li>Implement drop down filter for fields with a fixed selection</li>
            <li>Implement date range filter for date fields</li>
            <li><b>Add Dial800 Voicemail if available</b></li>
            <li><b>Add Survey record if available</b></li>
        </ul>
        <u><b>Leads Assignment Page LIVE!</b></u>
        <ul>
            <li>Limit Filter otions to sensible option per field</li>
            <li>Implement Fuzzy logic filter for Notes fields</li>
            <li>Implement drop down filter for fields with a fixed selection</li>
            <li>Implement date range filter for date fields</li>
            <li>Prevent Call Later Dates in the past</li>
        </ul>
        <u>Dial 800 Page</u>
        <ul>
            <li>Recode audio button to be HTML5 compliant with direct click sound</li>
        </ul>
        <u>Email Maintenace</u>
        <ul>
            <li>Activate Save functionality</li>
        </ul>
        <u>Additional Pages</u>
        <ul>
            <li><b>Leads Sales Email</b></li>
            <li>Customer Maintenance Page (aka Constant Contact)</li>
            <li>Sales</li>
            <li>Administration</li>
            <li>Configuration</li>
            <li><b>Credential Maintenance</b></li>
            <li>Radio Sales Info Page</li>
        </ul>
        <u>Misc</u>
        <ul>
        </ul>

    <br />
    <b>Known Bugs</b>
    <br />
    <table title="History">
        <tr>
            <td><b>0.9.4</b></td>
            <td>2014-04-19</td>
            <td>Info : Selecting the last timekeeper before ALL does not work after selecting ALL and visiting a Sales page. Select the first timekeeper, press Select and then select the last timekeeper to remedy.</td>
        </tr>
    </table>

    <br />
    <b>History</b>
    <br />

    <table title="History">
        <tr>
            <td><b>0.9.4</b></td>
            <td>2014-04-19</td>
            <td>Increased menu font size</td>
        </tr>
        <tr> <td></td> <td></td> <td>Increased menu font size</td> </tr>
        <tr> <td></td> <td></td> <td>Moved Filter links to the left</td> </tr>
        <tr> <td></td> <td></td> <td>Eased security for the leadspage to allow HTML code in the sales notes</td> </tr>
        <tr> <td></td> <td></td> <td>Added all button the the employee select box</td> </tr>
        <tr>
            <td><b>0.9.3</b></td>
            <td>2014-04-15</td>
            <td>Coded LeadsReceiver redirect</td>
        </tr>
        <tr>
            <td><b></b></td>
            <td>2014-04-14</td>
            <td>Added Leads Assignment Page</td>
        </tr>
        <tr>
            <td><b>0.9.2</b></td>
            <td>2014-04-12</td>
            <td>Added Leads Email Maintenace Page</td>
        </tr>
        <tr>
            <td><b>0.9.1a</b></td>
            <td>2014-04-11</td>
            <td>Circumvented undeselectable last employee entry bug</td>
        </tr>
        <tr>
            <td><b>0.9.1</b></td>
            <td>2014-04-11</td>
            <td>Activated Salesperson multi selector option</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>Added Employee filtering to SalesPerson page</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>Added AssignedTo field to SalesPerson page</td>
        </tr>
        <tr>
            <td><b>0.9.0</b></td>
            <td>2014-04-10</td>
            <td>Added Tracker Page</td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>Added Survey Page</td>
        </tr>
        <tr>
            <td><b>0.8.1</b></td>
            <td>2014-04-09</td>
            <td>Added Command Line Survey Importer (based on manual extracted csv file)</td>
        </tr>
        <tr>
            <td><b>0.8.0</b></td>
            <td>2014-04-08</td>
            <td>Added Leads Salesperson page</td>
        </tr>
        <tr>
            <td></td>
            <td>2014-04-07</td>
            <td>Added Dial 800 page</td>
        </tr>
    </table>

</body>
</html>
