<%@ Page Language="C#" Title="Login" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ServiceDesk.WebApp.Account.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="description" content="@ViewBag.Description"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="shortcut icon" href="~/favicon.png" type="image/x-icon"/>
    <title>Login</title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
        <!--Basic Styles-->
        <!--Basic Styles-->
        <%: Styles.Render("~/css/bootstrap") %>
        <link id="bootstrap_rtl_link" href="" rel="stylesheet"/>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
        <%: Styles.Render("~/assets/css/weather-icons.min.css") %>
        <!--Fonts-->
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,400,600,700,300" rel="stylesheet" type="text/css"/>
        <!--Beyond styles-->
        <!--Beyond styles-->
        <%: Styles.Render("~/css/beyond") %>
        <link id="skin_link" href="" rel="stylesheet" type="text/css"/>
        <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
        <%: Scripts.Render("~/bundles/skin") %>
    </asp:PlaceHolder>
    <!--Skin Script: Place this script in head to load scripts for skins and rtl support-->
    <script type="text/javascript" src="/assets/js/skins.min.js"></script>
</head>
<body>
<form class="form-horizontal" runat="server" role="form">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="login-container animated fadeInDown">
        <div class="loginbox bg-white">
            <div class="loginbox-title"> SIGN IN </div>
            <div class="loginbox-social">
                <div class="social-title "> Sử dụng tài khoản Nhân sự <br>Используйте аккаунт программы управления Персоналом</div>
                   <%-- <div class="social-buttons">
                        <a href="#" class="button-facebook">
                            <i class="social-icon fa fa-facebook"></i>
                        </a>
                        <a href="#" class="button-twitter">
                            <i class="social-icon fa fa-twitter"></i>
                        </a>
                        <a href="#" class="button-google">
                            <i class="social-icon fa fa-google-plus"></i>
                        </a>
                    </div>--%>
            </div>
            <div class="loginbox-textbox">
                <%--<input type="text" class="form-control" placeholder="Email" />--%>
                <span class="input-icon icon-right">
                    <asp:TextBox ID="txtUserName" runat="server" class="form-control" placeholder="Danh số/ Таб.номер"/>
                    <i class="fa fa-user-circle blue" aria-hidden="true"></i>
                </span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                                            ForeColor="Red" Display="Dynamic" Font-Size="8pt">
                    <strong>*</strong>
                </asp:RequiredFieldValidator>
            </div>
            <div class="loginbox-textbox">
                <%--<input type="text" class="form-control" placeholder="******" />--%>
                <span class="input-icon icon-right">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" AutoCompleteType="None"
                                 class="form-control input-sm" placeholder="******"/>
                    <i class="fa fa-key blue" aria-hidden="true"></i>
                </span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                                            ForeColor="Red" Display="Dynamic" Font-Size="8pt">
                    <strong>*</strong>
                </asp:RequiredFieldValidator>
            </div>
            <div class="loginbox-textbox">
                <div class="checkbox">
                    <label>
                        <input runat="server" id="Remember" type="checkbox" class="colored-blue" checked="checked"/>
                        <span class="text">Remember</span>
                    </label>
                </div>
            </div>
            <%--<div class="loginbox-forgot">
                    <a href="#">Forgot Password?</a>
                </div>--%>
            <div class="loginbox-textbox">
                <telerik:RadCaptcha ID="reCaptchar" runat="server"></telerik:RadCaptcha>
            </div>
            <div class="loginbox-submit">
                <%--<input type="button" class="btn btn-primary btn-block" value="Login"/>--%>

                <telerik:RadButton ID="btnLogin" runat="server" Text="Login" Width="100%"
                                   Skin="Office2007" OnClick="btnLogin_Click">
                </telerik:RadButton>
            </div>
            <div class="loginbox-signup">
                <span id="divMsg" runat="server">
                    <asp:Label ID="lbNotice" runat="server" EnableViewState="False" Visible="False"></asp:Label>
                </span>
                <%--<a href="register.html">Sign Up With Email</a>--%>
            </div>
        </div>
       <%-- <div style="font-style: initial; margin-top: 20px; text-align: center;">
            <p>Copyright Vietsovpetro &copy; <%: DateTime.Now.Year %></p>
            <span>Version 1.1. By IT &amp; Communication Center </span>
        </div>--%>

    </div>
    <div style="margin: 0 auto; text-align: left; width: 50%;">

    </div>
</form>
<telerik:RadScriptBlock runat="server" ID="radscriptblock2">
    <%: Scripts.Render("~/bundles/jquery") %>
    <%: Scripts.Render("~/bundles/bootstrap") %>
    <!--Beyond Scripts-->
    <%: Scripts.Render("~/bundles/beyond") %>
</telerik:RadScriptBlock>
</body>
</html>
