<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>遠端管理系統 - 首頁</title>
    <link href="~/Styles/Host.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <div id="logo">
            <h1>
                <a href="#">遠端管理系統</a></h1>
        </div>
    </div>
    <div id="content">
        <div id="login" class="boxed">
            <h2 class="title">
                登入系統</h2>
            <asp:Login ID="Login" runat="server" DisplayRememberMe="False" TextLayout="TextOnTop"
                DestinationPageUrl="~/Member/Default.aspx" OnLoggedIn="Login_LoggedIn">
                <LayoutTemplate>
                    <table class="loginTable">
                        <tr>
                            <td class="loginTitle">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">帳號：</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="UserName" runat="server" AutoCompleteType="Disabled" Width="168px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="必須提供使用者名稱。" ToolTip="必須提供使用者名稱。" ValidationGroup="Login1" Font-Size="Large"
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="loginTitle">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密碼：</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" AutoCompleteType="Disabled"
                                    Width="168px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="必須提供密碼。" ToolTip="必須提供密碼。" ValidationGroup="Login1" Font-Size="Large"
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="loginMessage">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="loginButton">
                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="登入" ValidationGroup="Login1"
                                    Height="28px" Width="88px" />
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </div>
    <div id="content">
		<a href="http://invoice2.governsoft.com">舊版遠端管理系統/a>
	</div>
    <div id="footer">
        <p id="legal">
            本系統著作權屬於 <a href="http://www.governsoft.com/">臺灣控軟</a> 所有</p>
    </div>
    </form>
</body>
</html>
