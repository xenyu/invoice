<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.master" AutoEventWireup="true"
    CodeFile="Company.aspx.cs" Inherits="Member_Company" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/company.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content">
        <asp:UpdatePanel ID="UpdatePanelCaption" runat="server" UpdateMode="Conditional"
            ChildrenAsTriggers="False">
            <ContentTemplate>
                <h1 class="pagetitle">
                    公司資料 -
                    <asp:Label ID="LabelCaption" runat="server" Text=""></asp:Label>
                </h1>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="MenuCompany" EventName="MenuItemClick" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="post">
            <asp:UpdateProgress ID="UpdateProgressData" runat="server" AssociatedUpdatePanelID="UpdatePanelData">
                <ProgressTemplate>
                    <img alt="處理中..." src="../Images/progress.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanelData" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <table class="tableCompany">
                        <tr>
                            <td class="companyData">
                                <asp:Panel ID="PanelData" runat="server" Visible="False" CssClass="companyData">
                                    <table class="dataTable">
                                        <tr>
                                            <td colspan="3">
                                                公司類型：<asp:DropDownList ID="DropDownListCompanyType" runat="server" DataSourceID="SqlDataSourceCompanyType"
                                                    DataTextField="TypeName" DataValueField="TypeId">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                公司編碼：<asp:TextBox ID="TextBoxCompanyCode" runat="server" MaxLength="64" Width="144px"
                                                    AutoCompleteType="Disabled"></asp:TextBox>
                                            </td>
                                            <td>
                                                失聯天數：<asp:TextBox ID="TextBoxCompanyLost" runat="server" MaxLength="6" Width="54px"
                                                    AutoCompleteType="Disabled"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                公司名稱：<asp:TextBox ID="TextBoxCompanyName" runat="server" AutoCompleteType="Disabled"
                                                    MaxLength="42" Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                統一編號：<asp:TextBox ID="TextBoxCompanyNumber" runat="server" AutoCompleteType="Disabled"
                                                    MaxLength="32" Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                公司電話：<asp:TextBox ID="TextBoxCompanyPhone" runat="server" AutoCompleteType="Disabled"
                                                    MaxLength="32" Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                公司傳真：<asp:TextBox ID="TextBoxCompanyFax" runat="server" AutoCompleteType="Disabled"
                                                    MaxLength="32" Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                公司地址：<asp:TextBox ID="TextBoxCompanyAddress" runat="server" AutoCompleteType="Disabled"
                                                    MaxLength="256" Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                公司備註：<asp:TextBox ID="TextBoxNote" runat="server" AutoCompleteType="Disabled" MaxLength="1024"
                                                    Width="666px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                付款票期：<asp:DropDownList ID="DropDownListPayWay" runat="server" DataSourceID="SqlDataSourceCompanyType"
                                                    DataTextField="TypeName" DataValueField="TypeId">
                                                </asp:DropDownList>
                                                 <asp:DropDownList ID="DropDownListPayPeriod" runat="server" DataSourceID="SqlDataSourceCompanyType"
                                                    DataTextField="TypeName" DataValueField="TypeId">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table style="margin-top: 8px">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PanelDataMemo" runat="server" Visible="False" CssClass="companyData">
                                                備忘註記：<br />
                                                <asp:TextBox ID="TextBoxMemo" runat="server" AutoCompleteType="Disabled" Width="366px"
                                                    Height="200px" TextMode="MultiLine"></asp:TextBox>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="PanelCompanyControl" Visible="False" CssClass="companyControl">
                                                <ul>
                                                    <li>
                                                        <asp:Button ID="ButtonCompanyClear" runat="server" Text="清空" CssClass="companyButton"
                                                            OnClick="ButtonCompanyClear_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonCompanyInsert" runat="server" Text="新增" CssClass="companyButton"
                                                            OnClick="ButtonCompanyInsert_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonCompanyUpdate" runat="server" Text="更新" CssClass="companyButton"
                                                            OnClick="ButtonCompanyUpdate_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonCompanyDelete" runat="server" Text="刪除" CssClass="companyButton"
                                                            OnClick="ButtonCompanyDelete_Click" />
                                                    </li>
                                                </ul>
                                            </asp:Panel>
                                        </td>
                                        <td class="personData">
                                            <asp:Panel ID="PanelPerson" runat="server" Visible="false">
                                                <table class="companyPerson">
                                                    <tr>
                                                        <th colspan="2">
                                                            <b>人員資料</b>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="ListBoxPerson" runat="server" AutoPostBack="True" CssClass="dataListBox"
                                                                DataSourceID="SqlDataSourcePerson" DataTextField="PersonName" DataValueField="PersonId"
                                                                OnSelectedIndexChanged="ListBoxPerson_SelectedIndexChanged" OnDataBound="ListBoxPerson_DataBound"
                                                                Width="133px" Height="166px"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <ul>
                                                                <li>編碼：<asp:TextBox ID="TextBoxPersonCode" runat="server" Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>職稱：<asp:TextBox ID="TextBoxPersonTitle" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>姓名：<asp:TextBox ID="TextBoxPersonName" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>手機：<asp:TextBox ID="TextBoxPersonMobile" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>電話：<asp:TextBox ID="TextBoxPersonPhone" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>郵件：<asp:TextBox ID="TextBoxPersonMail" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                                <li>備註：<asp:TextBox ID="TextBoxPersonNote" runat="server" AutoCompleteType="Disabled"
                                                                    Width="133px"></asp:TextBox>
                                                                </li>
                                                            </ul>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="PanelPersonControl" Visible="False" CssClass="companyControl2">
                                                <ul>
                                                    <li>
                                                        <asp:Button ID="ButtonPersonClear" runat="server" Text="清空" CssClass="companyButton"
                                                            OnClick="ButtonPersonClear_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonPersonInsert" runat="server" Text="新增" CssClass="companyButton"
                                                            OnClick="ButtonPersonInsert_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonPersonUpdate" runat="server" Text="更新" CssClass="companyButton"
                                                            OnClick="ButtonPersonUpdate_Click" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="ButtonPersonDelete" runat="server" Text="刪除" CssClass="companyButton"
                                                            OnClick="ButtonPersonDelete_Click" />
                                                    </li>
                                                </ul>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="PanelProduct" runat="server" Visible="false" CssClass="companyProduct">
                        <table class="dataTable">
                            <tr>
                                <td class="dataLeft">
                                    <asp:ListBox ID="ListBoxProducts" runat="server" AutoPostBack="True" CssClass="dataListBox"
                                        Visible="False" DataSourceID="SqlDataSourceProducts" DataTextField="TypeName"
                                        DataValueField="TypeId" OnDataBound="ListBoxProducts_DataBound" OnSelectedIndexChanged="ListBoxProducts_SelectedIndexChanged"
                                        Width="200px" Height="220px"></asp:ListBox>
                                </td>
                                <td>
                                    <ul class="detailData">
                                        <li>類型：<asp:DropDownList ID="DropDownListProductType" runat="server" DataSourceID="SqlDataSourceProductType"
                                            DataTextField="TypeName" DataValueField="TypeId" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListProductType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </li>
                                        <li>顏色：<asp:DropDownList ID="DropDownListProductColor" runat="server" DataSourceID="SqlDataSourceProductColorType"
                                            DataTextField="TypeName" DataValueField="TypeId" 
                                                OnSelectedIndexChanged="DropDownListProductColor_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                        </asp:DropDownList>
                                            <asp:CheckBox ID="CheckBoxColor" runat="server" Text="全部顏色一併影響" Checked="True" />
                                        </li>
                                        <li>尺寸：<asp:DropDownList ID="DropDownListProductSize" runat="server" DataSourceID="SqlDataSourceProductSizeType"
                                            DataTextField="TypeName" DataValueField="TypeId" 
                                                OnSelectedIndexChanged="DropDownListProductSize_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                        </asp:DropDownList>
                                            <asp:CheckBox ID="CheckBoxSize" runat="server" Text="全部尺寸一併影響" Checked="True" />
                                        </li>
                                        <li>條碼：<asp:TextBox ID="TextBoxProductNumber" runat="server" AutoCompleteType="Disabled"></asp:TextBox></li>
                                        <li>名稱：<asp:TextBox ID="TextBoxProductName" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                        </li>
                                        <li>數量：<asp:TextBox ID="TextBoxProductCount" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                        </li>
                                        <li>備註：<asp:TextBox ID="TextBoxProductNote" runat="server" AutoCompleteType="Disabled"
                                            Width="384px"></asp:TextBox>
                                        </li>
                                        <li>成本：<asp:TextBox ID="TextBoxProductCosts" runat="server" AutoCompleteType="Disabled"
                                            MaxLength="6" Width="48px"></asp:TextBox>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelType" runat="server" Visible="false" CssClass="companyType">
                        <table class="dataTable">
                            <tr>
                                <td class="dataLeft">
                                    <asp:DropDownList ID="DropDownListType" runat="server" AutoPostBack="True" CssClass="dataListBox"
                                        OnSelectedIndexChanged="DropDownListType_SelectedIndexChanged" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2">
                                    <ul class="detailData">
                                        <li>類型名稱：<asp:TextBox ID="TextBoxType" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="ListBoxType" runat="server" AutoPostBack="True" Visible="false"
                                        CssClass="dataListBox" DataTextField="TypeName" DataValueField="TypeId" OnDataBinding="ListBoxType_DataBinding"
                                        OnSelectedIndexChanged="ListBoxType_SelectedIndexChanged" OnDataBound="ListBoxType_DataBound"
                                        Width="200px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="LabelMessage" runat="server" Visible="False"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonClear" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonInsert" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonUpdate" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonDelete" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonCompanyClear" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonCompanyInsert" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonCompanyUpdate" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonCompanyDelete" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonPersonClear" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonPersonInsert" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonPersonUpdate" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonPersonDelete" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListProductType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListProductColor" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListProductSize" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxPerson" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ListBoxProducts" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="MenuCompany" EventName="MenuItemClick" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="post">
            <asp:UpdatePanel ID="UpdatePanelControl" runat="server" UpdateMode="Conditional"
                ChildrenAsTriggers="False">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="PanelControl" Visible="False" CssClass="control">
                        <ul>
                            <li>
                                <asp:Button ID="ButtonClear" runat="server" Text="清空" CssClass="controlButton" OnClick="ButtonClear_Click" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonInsert" runat="server" Text="新增" CssClass="controlButton" OnClick="ButtonInsert_Click" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonUpdate" runat="server" Text="更新" CssClass="controlButton" OnClick="ButtonUpdate_Click" />
                            </li>
                            <li>
                                <asp:Button ID="ButtonDelete" runat="server" Text="刪除" CssClass="controlButton" OnClick="ButtonDelete_Click" />
                            </li>
                        </ul>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="MenuCompany" EventName="MenuItemClick" />
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
                        <asp:Button ID="ButtonSearch" runat="server" Text="搜尋" OnClick="ButtonSearch_Click"
                            Width="48px" />
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
                                    OnSelectedIndexChanged="ListBoxCompanies_SelectedIndexChanged" OnDataBinding="ListBoxCompanies_DataBinding"
                                    OnDataBound="ListBoxCompanies_DataBound"></asp:ListBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonClear" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonInsert" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonUpdate" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ButtonDelete" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li>
                <asp:UpdatePanel ID="UpdatePanelEdit" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PanelEdit" runat="server" Visible="false">
                            <h2>
                                公司選單</h2>
                            <div class="menuCompany">
                                <asp:Menu ID="MenuCompany" runat="server" DataSourceID="XmlDataSourceCompany" SkipLinkText=""
                                    BackColor="#E3EAEB" Width="208px" ForeColor="Black" Font-Size="Medium" RenderingMode="Table"
                                    OnMenuItemClick="MenuCompany_MenuItemClick">
                                    <DataBindings>
                                        <asp:MenuItemBinding DataMember="menuMapNode" TextField="title" ToolTipField="description"
                                            ValueField="value" />
                                    </DataBindings>
                                    <StaticHoverStyle BackColor="#0094FF" ForeColor="Yellow" Font-Underline="true" />
                                    <StaticSelectedStyle BackColor="#0094FF" ForeColor="Yellow" />
                                </asp:Menu>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
            <li>
                <asp:UpdateProgress ID="UpdateProgressRecord" runat="server" AssociatedUpdatePanelID="UpdatePanelRecord">
                    <ProgressTemplate>
                        <img alt="處理中..." src="../Images/progress.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanelRecord" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                    <ContentTemplate>
                        <asp:Panel ID="PanelRecord" runat="server" Visible="false">
                            <h2>
                                最後交易記錄</h2>
                            <ul>
                                <li>
                                    <asp:GridView ID="GridViewRecord" runat="server" CssClass="recordGrid" OnRowDataBound="GridViewRecord_RowDataBound"
                                        OnDataBound="GridViewRecord_DataBound" AllowPaging="True" OnPageIndexChanging="GridViewRecord_PageIndexChanging"
                                        PageSize="16">
                                        <PagerStyle HorizontalAlign="Center" CssClass="defaultPager" />
                                    </asp:GridView>
                                </li>
                            </ul>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ListBoxCompanies" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonClear" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonInsert" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ButtonDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="MenuCompany" EventName="MenuItemClick" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewRecord" EventName="PageIndexChanging" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
        </ul>
    </div>
    <asp:XmlDataSource ID="XmlDataSourceCompany" runat="server" DataFile="~/Menu/Company.xml"
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
    <asp:SqlDataSource ID="SqlDataSourceCompanyType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_CompanyType WHERE (OwnerId = @OwnerId) ORDER BY Score">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourcePerson" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT PersonId, PersonName FROM aspnet_Person WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL) ORDER BY PersonName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ControlParameter ControlID="ListBoxCompanies" Name="CompanyId" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_ProductType WHERE (OwnerId = @OwnerId) AND (DeleteDate IS NULL) ORDER BY Score">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProductColorType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_ProductColorType WHERE (OwnerId = @OwnerId) AND (DeleteDate IS NULL) ORDER BY Score">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProductSizeType" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT TypeId, TypeName FROM aspnet_ProductSizeType WHERE (OwnerId = @OwnerId) AND (DeleteDate IS NULL) ORDER BY Score">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceProducts" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT DISTINCT pgroup.TypeName, pgroup.TypeId FROM aspnet_Product product, aspnet_ProductGroup pgroup WHERE (product.OwnerId = @OwnerId) AND (product.CompanyId = @CompanyId) AND (product.ProductGroup = pgroup.TypeId) AND (product.DeleteDate IS NULL) AND (pgroup.DeleteDate IS NULL) ORDER BY pgroup.TypeName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ControlParameter ControlID="ListBoxCompanies" Name="CompanyId" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    </asp:Content>
