<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabSurveyDisplay.aspx.cs" Inherits="TabSurveyDisplay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        table, th, td { border: 1px solid black; border-spacing: 0; border-collapse: separate; font-family: Tahoma;font-size: 11px;}
        td { vertical-align: top; padding: 2px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <table id='clientinfo' runat='server'>
            <tr>
                <td>Name</td>
                <td>Email</td>
                <td>Phone</td>
                <td>Submission</td>
            </tr>
        </table>
        <br />
        <table id='campaign' runat='server'>
        </table>

    </div>
    </form>
</body>
</html>
