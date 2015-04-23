<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeadsReceiver2.aspx.cs" Inherits="Pages_LeadsReceiver2" Debug="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Leads Receiver</title>
</head>

<body>

<%--    
    <!-- Google Code for LeadsReceiver Conversion Page -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 1072692919;
        var google_conversion_language = "en";
        var google_conversion_format = "3";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "CKl7CPmIwgkQt_2__wM";
        var google_remarketing_only = false;
        /* ]]> */
    </script>
--%>

    <asp:PlaceHolder ID="google_init" runat="server" />

    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>

    <noscript>
        <div style="display:inline;">
        <img runat="server" id="googleImg" height="1" width="1" style="border-style:none;" alt="" src="//www.googleadservices.com/pagead/conversion/1072692919/?label=CKl7CPmIwgkQt_2__wM&amp;guid=ON&amp;script=0"/>
        </div>
    </noscript> 

    <form runat="server">
        <table border="1">
        <tbody>
            <tr>
                <td><asp:Label runat="server" Text="Id:"></asp:Label></td>
                <td><asp:TextBox ID="txtId" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Name:"></asp:Label></td>
                <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Email Address:"></asp:Label></td>
                <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>                        
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Phone:"></asp:Label></td>
                <td><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" Text="Comments:"></asp:Label></td>
                <td><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
        </tbody>
        </table>
        <br />
        <table border="1">
        <tbody>
            <tr>
                <td ><asp:TextBox ID="txtError" runat="server" TextMode="MultiLine" Width="800px" Height="300px"></asp:TextBox></td>
            </tr>
        </tbody>
        </table>
    </form>
</body>
</html>
