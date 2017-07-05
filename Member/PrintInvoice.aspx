<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintInvoice.aspx.cs" Inherits="Members_PrintAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>遠端管理系統 - 進銷單據列印</title>
    <link href="~/Styles/Member.css" rel="stylesheet" type="text/css" />
</head>
<body class="printBody">
    <form id="form1" runat="server">
    <table class="printTable">
        <tr>
            <td>
                <table class="printTitle">
                    <tr>
                        <th class="printType">
                            <asp:Label ID="LabelAccountType" runat="server" Text="Label"></asp:Label>
                        </th>
                        <th class="printCompany">
                            <asp:Label ID="LabelSelfName" runat="server" Font-Bold="True"></asp:Label>
                        </th>
                        <th>
                            <div class="printInfo">統編：<asp:Label ID="LabelSelfNumber" runat="server"></asp:Label>
                                &nbsp;電話：<asp:Label ID="LabelSelfPhone" runat="server"></asp:Label>
                                &nbsp;傳真：<asp:Label ID="LabelSelfFax" runat="server"></asp:Label>
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2">
                            單據編號：
                            <asp:Label ID="LabelAccountNumber" runat="server"></asp:Label>
                        </td>
                        <td>
                            發票號碼：
                            <asp:Label ID="LabelBillNumber" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            公司名稱：
                            <asp:Label ID="LabelCompanyName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            統一編號：
                            <asp:Label ID="LabelCompanyNumber" runat="server"></asp:Label>
                        </td>
                        <td>
                            單據日期：
                            <asp:Label ID="LabelDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            公司電話：
                            <asp:Label ID="LabelCompanyPhone" runat="server"></asp:Label>
                        </td>
                        <td>
                            付款日期：
                            <asp:Label ID="LabelPayDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            送貨地址：
                            <asp:Label ID="LabelDeliverAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            單據備註：
                            <asp:Label ID="LabelNote" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="4" width="16">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridViewAccount" runat="server" AutoGenerateColumns="False" CssClass="printGrid">
                    <Columns>
                        <asp:TemplateField HeaderText="型號">
                            <ItemTemplate>
                                <asp:Label ID="LabelGroup" runat="server" Text='<%# Eval("Group") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellGroup" />
                            <ItemStyle CssClass="leftCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="顏色">
                            <ItemTemplate>
                                <asp:Label ID="LabelColor" runat="server" Text='<%# Eval("Color") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellColor" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="3&lt;hr/&gt;36&lt;hr/&gt;21">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize0" runat="server" Text='<%# Eval("Size0") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="4&lt;hr/&gt;37&lt;hr/&gt;22">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize1" runat="server" Text='<%# Eval("Size1") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="5&lt;hr/&gt;38&lt;hr/&gt;23">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize2" runat="server" Text='<%# Eval("Size2") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="6&lt;hr/&gt;39&lt;hr/&gt;24">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize3" runat="server" Text='<%# Eval("Size3") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="7&lt;hr/&gt;40&lt;hr/&gt;25">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize4" runat="server" Text='<%# Eval("Size4") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="8&lt;hr/&gt;41&lt;hr/&gt;26">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize5" runat="server" Text='<%# Eval("Size5") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="9&lt;hr/&gt;42&lt;hr/&gt;27">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize6" runat="server" Text='<%# Eval("Size6") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="10&lt;hr/&gt;43&lt;hr/&gt;28">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize7" runat="server" Text='<%# Eval("Size7") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="11&lt;hr/&gt;44&lt;hr/&gt;29">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize8" runat="server" Text='<%# Eval("Size8") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="12&lt;hr/&gt;45&lt;hr/&gt;30">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize9" runat="server" Text='<%# Eval("Size9") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="13&lt;hr/&gt;46&lt;hr/&gt;31">
                            <ItemTemplate>
                                <asp:Label ID="LabelSize10" runat="server" Text='<%# Eval("Size10") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellSize" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="單價">
                            <ItemTemplate>
                                <asp:Label ID="LabelPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellPrice" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="數量">
                            <ItemTemplate>
                                <asp:Label ID="LabelQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellQuantity" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合計">
                            <ItemTemplate>
                                <asp:Label ID="LabelAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="cellAmount" />
                            <ItemStyle CssClass="centerCell" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <div class="printSum">
                    <ul>
                        <li>總數：
                            <asp:Label ID="LabelTotalNum" runat="server"></asp:Label>
                        </li>
                        <li>稅金：
                            <asp:Label ID="LabelTax" runat="server"></asp:Label></li>
                        <li>小計：
                            <asp:Label ID="LabelSum" runat="server"></asp:Label>
                        </li>
                        <li>總計：
                            <asp:Label ID="LabelTotal" runat="server"></asp:Label>
                        </li>
                        <li>已付款：
                            <asp:Label ID="LabelPrePay" runat="server"></asp:Label>
                        </li>
                        <li>未付款：
                            <asp:Label ID="LabelLeftPay" runat="server"></asp:Label>
                        </li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table class="printBottom">
                    <tr>
                        <td>
                            助理：
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            送貨業務：
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            客戶簽收：
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
