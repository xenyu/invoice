<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.master" AutoEventWireup="true"
    CodeFile="Report.aspx.cs" Inherits="Member_Report" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/report.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content">
        <h1 class="pagetitle">
            進銷作業 - 報表製作</h1>
        <div class="post">
            <table class="reportTable">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelStart" runat="server">
                            <ContentTemplate>
                                <asp:Calendar ID="CalendarStart" runat="server" DayNameFormat="Shortest" BackColor="White"
                                    BorderColor="White" BorderWidth="1px" Font-Size="Medium" ForeColor="Blue" Height="190px"
                                    Width="350px" Caption="起始日期" Font-Bold="true" ShowGridLines="True" Visible="False">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                    <NextPrevStyle Font-Size="8pt" ForeColor="#333333" Font-Bold="True" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True"
                                        Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanelEnd" runat="server">
                            <ContentTemplate>
                                <asp:Calendar ID="CalendarEnd" runat="server" DayNameFormat="Shortest" BackColor="White"
                                    BorderColor="White" BorderWidth="1px" Font-Size="Medium" ForeColor="Blue" Height="190px"
                                    Width="350px" Caption="結束日期" Font-Bold="true" ShowGridLines="True" Visible="False">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                    <NextPrevStyle Font-Size="8pt" ForeColor="#333333" Font-Bold="True" VerticalAlign="Bottom" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True"
                                        Font-Size="12pt" ForeColor="#333399" />
                                    <TodayDayStyle BackColor="#CCCCCC" />
                                </asp:Calendar>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="post">
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="PanelControl" Visible="False" CssClass="control">
                        <ul>
                            <li>
                                <asp:Button ID="ButtonReport" runat="server" Text="取得/更新列表" CssClass="controlButton"
                                    OnClick="ButtonReport_Click" ClientIDMode="Static" Width="133px" />
                            </li>
                        </ul>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="MenuReport" EventName="MenuItemClick" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxReport" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="post">
            <asp:UpdateProgress ID="UpdateProgressReport" runat="server" AssociatedUpdatePanelID="UpdatePanelReport"
                ClientIDMode="Static">
                <ProgressTemplate>
                    <img alt="處理中..." src="../Images/progress.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanelReport" runat="server" ChildrenAsTriggers="False"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LabelMessage" runat="server" Visible="False" Font-Size="Large"></asp:Label>
                    <asp:Panel ID="PanelAccount" runat="server" Visible="False">
                        <table class="reportAccountTable">
                            <tr>
                                <th colspan="2">
                                    <asp:Label ID="LabelAccountSelfName" runat="server" Font-Size="Large"></asp:Label>
                                    <br />
                                    <asp:Label ID="LabelAccount" runat="server" Font-Overline="False" Font-Size="Medium"
                                        Font-Underline="True" Text="對帳單"></asp:Label>
                                    <br />
                                </th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    公司名稱：<asp:Label ID="LabelAccountCompanyName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    電話：<asp:Label ID="LabelAccountCompanyPhone" runat="server"></asp:Label>
                                </td>
                                <td style="width: 177px;">
                                    起始日期：<asp:Label ID="LabelAccountStart" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    地址：<asp:Label ID="LabelAccountCompanyAddress" runat="server"></asp:Label>
                                </td>
                                <td style="width: 177px;">
                                    結束日期：<asp:Label ID="LabelAccountEnd" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="GridViewAccount" runat="server" AutoGenerateColumns="false" CssClass="reportAccountGrid"
                                        OnDataBound="GridViewAccount_DataBound" OnRowDataBound="GridViewAccount_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="單據">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountType" runat="server" Text='<%# Eval("AccountType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="32px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="單據日期">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelSaveDate" runat="server" Text='<%# Eval("SaveDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="82px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="單據編號">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButtonAccountNumber" runat="server" Text='<%# Bind("AccountNumber") %>'
                                                        CommandArgument='<%# Bind("AccountId") %>' TabIndex='<%# Container.DataItemIndex %>'
                                                        ForeColor="Blue" OnClick="LinkButtonAccountNumber_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="144px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="數量">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="46px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="小計">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountSum" runat="server" Text='<%# Eval("Sum") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="稅金">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountTax" runat="server" Text='<%# Eval("Tax") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="總計">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="預付金額">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountPrePay" runat="server" Text='<%# Eval("PrePay") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="未付金額">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelAccountLeftPay" runat="server" Text='<%# Eval("LeftPay") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table class="reportAccountTotal">
                                        <tr>
                                            <th style="width: 32px;">
                                                <b>總計</b>
                                            </th>
                                            <th style="width: 222px;">
                                            </th>
                                            <th style="width: 46px; text-align: right;">
                                                <asp:Label ID="LabelAccountQuantity" runat="server" Text="0"></asp:Label>
                                            </th>
                                            <th style="width: 90px; text-align: right;">
                                                <asp:Label ID="LabelAccountSum" runat="server" Text="0.00"></asp:Label>
                                            </th>
                                            <th style="width: 90px; text-align: right;">
                                                <asp:Label ID="LabelAccountTax" runat="server" Text="0.00"></asp:Label>
                                            </th>
                                            <th style="width: 88px; text-align: right;">
                                                <asp:Label ID="LabelAccountTotal" runat="server" Text="0.00"></asp:Label>
                                            </th>
                                            <th style="width: 88px; text-align: right;">
                                                <asp:Label ID="LabelAccountPrePay" runat="server" Text="0.00"></asp:Label>
                                            </th>
                                            <th style="width: 88px; text-align: right;">
                                                <asp:Label ID="LabelAccountLeftPay" runat="server" Text="0.00"></asp:Label>
                                            </th>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                    電話：<asp:Label ID="LabelAccountSelfPhone" runat="server"></asp:Label>
                                    &nbsp; 傳真：<asp:Label ID="LabelAccountSelfFax" runat="server"></asp:Label>
                                    &nbsp;如對本單據有任何疑問敬請聯絡，謝謝。
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelReport" runat="server" Visible="False">
                        <table class="reportAccountTable">
                            <tr>
                                <th colspan="3">
                                    <asp:Label ID="LabelReport" runat="server" Font-Overline="False" Font-Size="Medium"
                                        Font-Underline="True" Text="報表"></asp:Label>
                                    <br />
                                </th>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Panel ID="PanelDate" runat="server">
                                        起始日期：<asp:Label ID="LabelReportStart" runat="server"></asp:Label>
                                        &nbsp;-&nbsp;結束日期：<asp:Label ID="LabelReportEnd" runat="server"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="GridViewReport" runat="server" AutoGenerateColumns="false" CssClass="reportAccountGrid"
                                        OnRowDataBound="GridViewReport_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="日期">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportDate" runat="server" Text='<%# Eval("SaveDate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="74px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="公司名稱">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="166px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="發票號碼">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="166px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="鞋型">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="96px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="單價">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="數量">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="32px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="合計">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportSum" runat="server" Text='<%# Eval("Sum") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="成本單價">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportCosts" runat="server" Text='<%# Eval("Costs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="58px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="成本合計">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportCostsSum" runat="server" Text='<%# Eval("CostsSum") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                                <HeaderStyle Width="58px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="毛利">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportProfit" runat="server" Text='<%# Eval("Profit") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="應收">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="已收">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportPrePay" runat="server" Text='<%# Eval("PrePay") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="未收">
                                                <ItemTemplate>
                                                    <asp:Label ID="LabelReportLeftPay" runat="server" Text='<%# Eval("LeftPay") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="centerCell" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table class="totalTable">
                                        <tr>
                                            <td>
                                                <b>總計</b>
                                            </td>
                                            <td>
                                                <b>數量</b>
                                            </td>
                                            <td>
                                                <b>合計</b>
                                            </td>
                                            <td>
                                                成本 <b></b>
                                            </td>
                                            <td>
                                                <b>毛利</b>
                                            </td>
                                            <td>
                                                <b>應收</b>
                                            </td>
                                            <td>
                                                <b>已收</b>
                                            </td>
                                            <td>
                                                <b>未收</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalQuantity" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalSum" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalCostsSum" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalProfit" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalTotal" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalPrepay" runat="server" Text="Label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotalLeftPay" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonReport" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="MenuReport" EventName="MenuItemClick" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxReport" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="sidebar">
        <ul>
            <li>
                <asp:UpdatePanel ID="UpdatePanelMenu" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>
                            報表選單</h2>
                        <div class="menuReport">
                            <asp:Menu ID="MenuReport" runat="server" DataSourceID="XmlDataSourceReport" SkipLinkText=""
                                BackColor="#E3EAEB" Width="208px" ForeColor="Black" Font-Size="Medium" RenderingMode="Table"
                                OnMenuItemClick="MenuReport_MenuItemClick">
                                <DataBindings>
                                    <asp:MenuItemBinding DataMember="menuMapNode" TextField="title" ToolTipField="description"
                                        ValueField="value" />
                                </DataBindings>
                                <StaticHoverStyle BackColor="#0094FF" ForeColor="Yellow" Font-Underline="true" />
                                <StaticSelectedStyle BackColor="#0094FF" ForeColor="Yellow" />
                            </asp:Menu>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </li>
            <li>
                <asp:UpdateProgress ID="UpdateProgressListBox" runat="server" AssociatedUpdatePanelID="UpdatePanelListBox"
                    ClientIDMode="Static">
                    <ProgressTemplate>
                        <img alt="處理中..." src="../Images/progress.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanelListBox" runat="server" ChildrenAsTriggers="False"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelListBox" runat="server" Visible="False">
                            <h2>
                                公司一覽</h2>
                            <asp:ListBox ID="ListBoxReport" runat="server" AutoPostBack="true" CssClass="reportListBox"
                                OnSelectedIndexChanged="ListBoxReport_SelectedIndexChanged"></asp:ListBox>
                        </asp:Panel>
                        <asp:Panel ID="PanelSearch" runat="server" Visible="False">
                            <h2>
                                搜尋公司</h2>
                            <ul>
                                <li>
                                    <asp:TextBox ID="TextBoxSearch" runat="server" AutoCompleteType="Search" Width="128px"></asp:TextBox>
                                    <asp:Button ID="ButtonSearch" runat="server" Text="搜尋" Width="48px" OnClick="ButtonSearch_Click" />
                                </li>
                                <li>
                                    <asp:ListBox ID="ListBoxCompanies" runat="server" CssClass="invoiceListBox" DataSourceID="SqlDataSourceCompaniesCondition"
                                        DataTextField="CompanyName" DataValueField="CompanyId" AutoPostBack="True" Visible="false"
                                        OnSelectedIndexChanged="ListBoxCompanies_SelectedIndexChanged" OnDataBinding="ListBoxCompanies_DataBinding"
                                        OnDataBound="ListBoxCompanies_DataBound"></asp:ListBox>
                                </li>
                            </ul>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonReport" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="MenuReport" EventName="MenuItemClick" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanelPrint" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="ButtonPrint" runat="server" Text="列印報表" Visible="false" CssClass="controlButton"
                            OnClick="ButtonPrint_Click" ForeColor="Red" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MenuReport" EventName="MenuItemClick" />
                        <asp:AsyncPostBackTrigger ControlID="ListBoxReport" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
        </ul>
    </div>
    <asp:XmlDataSource ID="XmlDataSourceReport" runat="server" DataFile="~/Menu/Report.xml"
        XPath="menuMap/menuMapNode"></asp:XmlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceCompanies" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT CompanyId, CompanyName FROM aspnet_Company WHERE (OwnerId = @OwnerId) AND (CompanyId <> @CompanyId) AND (DeleteDate IS NULL) ORDER BY CompanyType, CompanyName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ProfileParameter Name="CompanyId" PropertyName="PrincipalCompanyGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceCompaniesCondition" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT CompanyId, CompanyName FROM aspnet_Company WHERE (OwnerId = @OwnerId) AND (CompanyId <> @CompanyId) AND ((CompanyName LIKE '%' + @Condition + '%') OR (CompanyNumber LIKE '%' + @Condition + '%') OR (CompanyPhone LIKE '%' + @Condition + '%') OR (CompanyBusiness LIKE '%' + @Condition + '%') OR (Note LIKE '%' + @Condition + '%') OR (CompanyAddress LIKE '%' + @Condition + '%') OR (BillAddress LIKE '%' + @Condition + '%') OR (DeliverAddress LIKE '%' + @Condition + '%') OR (CompanyFax LIKE '%' + @Condition + '%')) AND (DeleteDate IS NULL) ORDER BY CompanyType, CompanyName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ProfileParameter Name="CompanyId" PropertyName="PrincipalCompanyGuid" />
            <asp:ControlParameter Name="Condition" ControlID="TextBoxSearch" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
