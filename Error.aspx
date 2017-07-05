<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>遠端管理系統 - 錯誤</title>
    <link href="~/Styles/Error.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
        <div class="dotted_line">
        </div>
        <div id="main">
            <div class="main_left">
                <img src="Images/error_image.jpg" alt="Transparent Blue by BryantSmith.com" />
                <h1>
                    發生錯誤</h1>
                <h3>
                    我們對此一情況表示歉意</h3>
            </div>
            <div class="main_right">
                <h2>
                    繼續使用</h2>
                <p>
                    我們已經記錄此錯誤發生的原因，並將在最短時間內修正這個問題。
                    <br />
                    <br />
                    您可以<asp:HyperLink ID="HyperLink1" runat="server" ForeColor="#0000CC">按此回到上一頁</asp:HyperLink>繼續操作。
                    <br />
                    <br />
                    注意！如果您是嘗試使用不合法的方式連線本系統，將有可能觸法。
                    <br />
                    您目前連線的ＩＰ：
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="0.0.0.0"></asp:Label>
                </p>
            </div>
            <div class="main_bottom">
            </div>
        </div>
        <div class="dotted_line">
        </div>
        <div id="footer">
            <p>
                本系統著作權屬於 <a href="http://www.governsoft.com/">臺灣控軟</a> 所有
            </p>
        </div>
    </div>
    </form>
</body>
</html>
