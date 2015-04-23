<%@ Page Language="C#" AutoEventWireup="true" CodeFile="screentest1.aspx.cs" Inherits="Pages_screentest1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script id="Redirect" type="text/javascript">
        if (window.top.location != window.location) { window.top.location = window.location }
        function Redirect() {
            window.location = '/Pages/Screentest2.aspx?'+ 
            'wx=' + window.innerWidth + 
            '&wy=' + window.innerHeight +
            '&oh=' + window.outerHeight +
            '&ow=' + window.outerWidth+
            '&ox=' + window.pageXOffset+
            '&oy=' + window.pageYOffset+
            '&sx=' + window.screen.availHeight+
            '&sy=' + window.screen.availWidth+
            '&hx=' + window.screen.height+
            '&hy=' + window.screen.width +
            '&ua=' + navigator.userAgent +
            '&av=' + navigator.appVersion
            ;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Screen Test 1

    </div>
    </form>
</body>
</html>
