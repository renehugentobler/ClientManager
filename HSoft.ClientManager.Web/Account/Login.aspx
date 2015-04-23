<%@ Page Title="Log in" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title><%: Page.Title %> - Login</title>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">      
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference ID="BundleReference1" runat="server" Path="~/Content/css" /> 
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <script id="Redirect" type="text/javascript">
        if (window.top.location != window.location) {window.top.location = window.location }
        function Redirect()
        {
            window.location = '/Pages/NewMenu.aspx?wx=' + window.innerWidth + '&wy=' + window.innerHeight;
        }
    </script>
</head>

<body>
    <form runat="server">
    <header>
        <div class="content-wrapper">
            <div class="float-left">
                <p class="site-title">
                    <a id="A1" runat="server" href="Account/login.aspx">Raker Client Manager</a></p>
            </div>
            <div class="float-right">
                <section id="login">
                            <ul>
                                <li><a id="registerLink" runat="server" href="~/Account/Register" onclick="return false;" >Register</a></li>
<%--                                <li><a id="loginLink" runat="server" href="~/Account/Login">Log in</a></li>--%>
                            </ul>
                </section>
                <nav>
                    <ul id="menu">
                        <li><a  id="A4" runat="server" href="~/About">About</a></li>
                        <li><a id="A5" runat="server" href="~/Contact">Contact</a></li>
                    </ul>
                </nav>
            </div>
        </div>
    </header>

    <div id="body">
        <section class="content-wrapper main-content clear-fix">

    <section id="loginForm">
                <p class="validation-summary-errors">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend>Log in Form</legend>
                    <ol>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
                            <asp:TextBox runat="server" ID="UserName" Enabled="true" TabIndex="1" Visible="true" TextMode="SingleLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="The user name field is required." />
                        </li>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" TabIndex="2" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="The password field is required." />
                        </li>
                        <li>
                            <asp:CheckBox Enabled="false" runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
                            &nbsp;
                            <asp:CheckBox Enabled="false" runat="server" ID="RememberDevice" />
                            <asp:Label runat="server" AssociatedControlID="RememberDevice" CssClass="checkbox">Remember Device?</asp:Label>
                        </li>
                        <li><asp:Literal runat="server" ID="error" Visible="false" Text="Please enter a valid username and password" /></li>
                    </ol>
                    <asp:Button runat="server" CommandName="Login" Text="Log in" OnCommand="Login_Click" />
                </fieldset>
    </section>

    <section id="socialLoginForm">
        <h2>Use another service to log in.</h2>
<%--        <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />--%>
    </section>

        </section>
    </div>


    <footer>
        <div class="content-wrapper">
            <div class="float-left">
                <p>
                    &copy; <%: DateTime.Now.Year %> - Harry D. Raker, Website by HSoft
                </p>
            </div>
        </div>
    </footer>

    </form>
</body>

</html>

