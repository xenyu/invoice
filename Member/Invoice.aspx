<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.master" AutoEventWireup="true"
    CodeFile="Invoice.aspx.cs" Inherits="Member_Invoice" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="../Scripts/datetime.js" type="text/javascript"></script>--%>
    <script src="../Scripts/invoice.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content">
        <h1 class="pagetitle">
            進銷作業 - 單據資料</h1>
        <div class="post">
            <asp:UpdateProgress ID="UpdateProgressTitle" runat="server" AssociatedUpdatePanelID="UpdatePanelTitle">
                <ProgressTemplate>
                    <img alt="處理中..." src="../Images/progress.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanelTitle" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="PanelTitle" runat="server" Visible="False">
                        <table class="accountTitle">
                            <tr>
                                <th class="accountType">
                                    <asp:RadioButtonList ID="RadioButtonListAccountType" runat="server" RepeatDirection="Horizontal"
                                        DataSourceID="SqlDataSourceAccountTypeCustomer" DataTextField="TypeName" DataValueField="TypeId">
                                    </asp:RadioButtonList>
                                </th>
                                <th class="accountCompany">
                                    <asp:Label ID="LabelSelfName" runat="server" Font-Bold="True"></asp:Label>
                                </th>
                                <th>
                                    <span class="accountInfo">統編：<asp:Label ID="LabelSelfNumber" runat="server"></asp:Label>
                                        &nbsp;電話：<asp:Label ID="LabelSelfPhone" runat="server"></asp:Label>
                                        &nbsp;傳真：<asp:Label ID="LabelSelfFax" runat="server"></asp:Label>
                                    </span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    單據編號：
                                    <asp:Label ID="LabelAccountNumber" runat="server"></asp:Label>
                                </td>
                                <td>
                                    發票號碼：
                                    <asp:TextBox ID="TextBoxBillNumber" runat="server" AutoCompleteType="Disabled" MaxLength="32"
                                        Width="128px"></asp:TextBox>
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
                                    <asp:Label ID="LabelDate" runat="server" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    公司電話：
                                    <asp:Label ID="LabelCompanyPhone" runat="server"></asp:Label>
                                </td>
                                <td>
                                    付款日期：
                                    <asp:Label ID="LabelPayDate" runat="server" ClientIDMode="Static"></asp:Label>
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
                                    <asp:TextBox ID="TextBoxNote" runat="server" AutoCompleteType="Disabled" Width="525px" TextMode="MultiLine" Height="40px">
                                    </asp:TextBox>&nbsp;<asp:LinkButton ID="LinkButtonNote" runat="server" OnClick="LinkButtonNote_Click">選擇歷史備註</asp:LinkButton>
                                    <asp:DropDownList ID="DropDownListNote" runat="server" Width="525px" DataTextField="Note"
                                        DataValueField="AccountId" Visible="false">
                                    </asp:DropDownList>
                                    &nbsp;<asp:LinkButton ID="LinkButtonNoteSelect" runat="server" OnClick="LinkButtonNoteSelect_Click"
                                        Text="確定" Visible="false"></asp:LinkButton>&nbsp;<asp:LinkButton ID="LinkButtonNoteCancel"
                                            runat="server" OnClick="LinkButtonNoteCancel_Click" Text="取消" Visible="false"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonInvoice" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxInvoices" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonClearAccount" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonInsertAccount" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonUpdateAccount" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonDeleteAccount" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="post">
            <asp:UpdatePanel ID="UpdatePanelGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="PanelAccount" Visible="False">
                        <asp:GridView ID="GridViewAccount" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridViewAccount_RowDataBound"
                            CssClass="gridTable">
                            <Columns>
                                <asp:TemplateField HeaderText="型號">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="DropDownListGroup" runat="server" DataSourceID="SqlDataSourceGroup"
                                            ValidationGroup='<%# Eval("Group") %>' TabIndex='<%# Container.DataItemIndex %>'
                                            DataTextField="TypeName" DataValueField="TypeId" AutoPostBack="True" CssClass="listboxGroup"
                                            OnSelectedIndexChanged="DropDownListRow_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellGroup" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="顏色">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="DropDownListColor" runat="server" DataSourceID="SqlDataSourceColor"
                                            ValidationGroup='<%# Eval("Color") %>' TabIndex='<%# Container.DataItemIndex %>'
                                            DataTextField="TypeName" DataValueField="TypeId" AutoPostBack="True" CssClass="listboxColor"
                                            OnSelectedIndexChanged="DropDownListRow_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellColor" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="3&lt;hr/&gt;36&lt;hr/&gt;21">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize0" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size0") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="4&lt;hr/&gt;37&lt;hr/&gt;22">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize1" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size1") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="5&lt;hr/&gt;38&lt;hr/&gt;23">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize2" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size2") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="6&lt;hr/&gt;39&lt;hr/&gt;24">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize3" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size3") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="7&lt;hr/&gt;40&lt;hr/&gt;25">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize4" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size4") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="8&lt;hr/&gt;41&lt;hr/&gt;26">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize5" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size5") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="9&lt;hr/&gt;42&lt;hr/&gt;27">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize6" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size6") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="10&lt;hr/&gt;43&lt;hr/&gt;28">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize7" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size7") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="11&lt;hr/&gt;44&lt;hr/&gt;29">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize8" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size8") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="12&lt;hr/&gt;45&lt;hr/&gt;30">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize9" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size9") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="13&lt;hr/&gt;46&lt;hr/&gt;31">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSize10" runat="server" AutoCompleteType="Disabled" MaxLength="5"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Size10") %>' CssClass="textboxSize"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellSize" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="單價">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxPrice" runat="server" AutoCompleteType="Disabled" MaxLength="10"
                                            TabIndex='<%# Container.DataItemIndex %>' Text='<%# Eval("Price") %>' CssClass="textboxPrice"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellPrice" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="數量">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="labelQuantity"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellQuantity" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="合計">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelAmount" runat="server" Text='<%# Eval("Amount") %>' CssClass="labelAmount"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="cellAmount" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonDeleteRow" runat="server" TabIndex='<%# Container.DataItemIndex %>'
                                            OnClick="LinkButtonDeleteRow_Click">刪除</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           </asp:GridView>
                        <div class="sum">
                            <ul>
                                <li>總數：
                                    <asp:Label ID="LabelTotalNum" runat="server" ClientIDMode="Static"></asp:Label>
                                </li>
                                <li>稅金：
                                    <asp:TextBox ID="TextBoxTax" runat="server" AutoCompleteType="Disabled" MaxLength="16"
                                        Width="64px" ClientIDMode="Static"></asp:TextBox></li>
                                <li>小計：
                                    <asp:Label ID="LabelSum" runat="server" ClientIDMode="Static"></asp:Label>
                                </li>
                                <li>總計：
                                    <asp:Label ID="LabelTotal" runat="server" ClientIDMode="Static"></asp:Label>
                                </li>
                                <li>已付款：
                                    <asp:TextBox ID="TextBoxPrePay" runat="server" AutoCompleteType="Disabled" MaxLength="16"
                                        Width="72px" ClientIDMode="Static"></asp:TextBox>
                                </li>
                                <li>未付款：
                                    <asp:Label ID="LabelLeftPay" runat="server" ClientIDMode="Static"></asp:Label>
                                </li>
                            </ul>
                        </div>
                    </asp:Panel>
                    <asp:Label ID="LabelMessage" runat="server" Visible="False"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="post">
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server" UpdateMode="Conditional"
                ChildrenAsTriggers="False">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="PanelControl" Visible="False" CssClass="control">
                        <ul>
                            <li>
                                <asp:Button ID="ButtonClearAccount" runat="server" Text="清空" OnClick="ButtonClearAccount_Click"
                                    CssClass="controlButton" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonInsertAccount" runat="server" Text="新增" OnClick="ButtonInsertAccount_Click"
                                    CssClass="controlButton" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonUpdateAccount" runat="server" Text="更新" OnClick="ButtonUpdateAccount_Click"
                                    CssClass="controlButton" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonDeleteAccount" runat="server" Text="刪除" OnClick="ButtonDeleteAccount_Click"
                                    CssClass="controlButton" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonAppendRow" runat="server" Text="新增一列" OnClick="ButtonAppendRow_Click"
                                    CssClass="controlButton" />
                            </li>
                        </ul>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonInvoice" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="sidebar">
        <ul>
            <li>
                <h2>
                    搜尋公司</h2>
                <ul>
                    <li>
                        <asp:TextBox ID="TextBoxSearch" runat="server" AutoCompleteType="Search" Width="128px"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" runat="server" Text="公司搜尋" OnClick="ButtonSearch_Click"
                            Width="72px" />
                    </li>
                    <li style="display: block; line-height: 2px; min-height: 2px;"></li>
                    <li>
                        <asp:TextBox ID="TextBoxInvoice" runat="server" AutoCompleteType="Search" Width="128px"></asp:TextBox>
                        <asp:Button ID="ButtonInvoice" runat="server" Text="發票搜尋" OnClick="ButtonInvoice_Click"
                            Width="72px" />
                    </li>
                    <li>
                        <asp:UpdateProgress ID="UpdateProgressSearch" runat="server" AssociatedUpdatePanelID="UpdatePanelSearch">
                            <ProgressTemplate>
                                <img alt="處理中..." src="../Images/progress.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                            <ContentTemplate>
                                <asp:ListBox ID="ListBoxCompanies" runat="server" CssClass="invoiceListBox" DataSourceID="SqlDataSourceCompaniesCondition"
                                    DataTextField="CompanyName" DataValueField="CompanyId" AutoPostBack="True" Visible="false"
                                    OnSelectedIndexChanged="ListBoxCompanies_SelectedIndexChanged" OnDataBound="ListBoxCompanies_DataBound"
                                    OnDataBinding="ListBoxCompanies_DataBinding"></asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonInvoice" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li>
                <asp:UpdateProgress ID="UpdateProgressInvoice" runat="server" AssociatedUpdatePanelID="UpdatePanelInvoice">
                    <ProgressTemplate>
                        <img alt="處理中..." src="../Images/progress.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanelInvoice" runat="server" UpdateMode="Conditional"
                    ChildrenAsTriggers="False">
                    <ContentTemplate>
                        <asp:Panel ID="PanelInvoices" runat="server" Visible="false">
                            <h2>
                                單據列表</h2>
                            <ul>
                                <li>
                                    <asp:ListBox ID="ListBoxInvoices" runat="server" AutoPostBack="True" CssClass="invoiceListBox2"
                                        DataSourceID="SqlDataSourceInvoices" DataTextField="AccountNumber" DataValueField="AccountId"
                                        OnSelectedIndexChanged="ListBoxInvoices_SelectedIndexChanged" OnDataBound="ListBoxInvoices_DataBound"
                                        OnDataBinding="ListBoxInvoices_DataBinding"></asp:ListBox>
                                </li>
                            </ul>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonInvoice" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonClearAccount" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonInsertAccount" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonDeleteAccount" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
            <li>
                <ul>
                    <asp:UpdatePanel ID="UpdatePanelPrint" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <asp:Button ID="ButtonPrint" runat="server" Visible="false" Text="列印單據" OnClick="ButtonPrint_Click"
                                ForeColor="Red" CssClass="controlButton" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ListBoxInvoices" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ButtonClearAccount" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ButtonInsertAccount" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ButtonDeleteAccount" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ul>
            </li>
        </ul>
    </div>
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
    <asp:SqlDataSource ID="SqlDataSourceInvoices" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT AccountId, AccountNumber FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL) ORDER BY CAST(AccountNumber AS bigint) DESC">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ControlParameter Name="CompanyId" ControlID="ListBoxCompanies" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceGroup" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_ProductGroup WHERE (OwnerId = @OwnerId) AND (DeleteDate IS NULL) ORDER BY TypeName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceColor" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_ProductColorType WHERE (OwnerId = @OwnerId) AND (DeleteDate IS NULL) ORDER BY TypeName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceAccountTypeCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_AccountType WHERE (Score = 1) OR (Score = 3) ORDER BY Score">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceAccountTypeManufacture" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_AccountType WHERE (Score = 0) OR (Score = 2) ORDER BY Score">
    </asp:SqlDataSource>
</asp:Content>
