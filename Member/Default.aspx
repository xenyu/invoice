<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Member.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Member_Default" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/default.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content">
        <h1 class="pagetitle">
            管理首頁</h1>
        <div class="post">
            <asp:UpdateProgress ID="UpdateProgressMain" runat="server" AssociatedUpdatePanelID="UpdatePanelMain">
                <ProgressTemplate>
                    <img alt="處理中..." src="../Images/progress.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="PanelAccount" runat="server" Visible="false">
                        <asp:GridView ID="GridViewAccount" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" DataKeyNames="AccountId" DataSourceID="SqlDataSourceAccount"
                            OnRowDataBound="GridViewAccount_RowDataBound" PageSize="36" CssClass="defaultGrid"
                            EmptyDataText="查詢無任何資料。" OnPageIndexChanged="GridViewAccount_PageIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="單據編號" SortExpression="AccountNumber">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonAccountNumber" runat="server" Text='<%# Bind("AccountNumber") %>'
                                            CommandArgument='<%# Bind("AccountId") %>' TabIndex='<%# Container.DataItemIndex %>'
                                            ForeColor="Blue" OnClick="LinkButtonAccountNumber_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="128px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Price" HeaderText="總金額" ReadOnly="True" SortExpression="Price"
                                    DataFormatString="{0:N}" />
                                <asp:BoundField DataField="PrePay" HeaderText="已付款" SortExpression="PrePay" DataFormatString="{0:N}" />
                                <asp:BoundField DataField="LeftPay" HeaderText="未付款" SortExpression="LeftPay" DataFormatString="{0:N}" />
                                <asp:TemplateField HeaderText="出貨完成" SortExpression="DeliverDate">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxDelivered" runat="server" Text='<%# Bind("DeliverDate") %>'
                                            TabIndex='<%# Container.DataItemIndex %>' AutoPostBack="true" OnCheckedChanged="CheckBoxAccount_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="64px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="單據完成" SortExpression="CheckDate">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxChecked" runat="server" Text='<%# Bind("CheckDate") %>'
                                            TabIndex='<%# Container.DataItemIndex %>' AutoPostBack="true" OnCheckedChanged="CheckBoxAccount_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="64px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonUpdateRow" runat="server" TabIndex='<%# Container.DataItemIndex %>'
                                            OnClick="LinkButtonUpdateRow_Click" ForeColor="Blue">寫入</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="48px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle ForeColor="Blue" />
                            <PagerStyle HorizontalAlign="Center" CssClass="defaultPager" />
                            <SelectedRowStyle BackColor="LightSalmon" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="PanelInventory" runat="server" Visible="false">
                        <div class="defaultDiv">
                            高於或等於數量：&nbsp;
                            <asp:TextBox ID="TextBoxInventory" runat="server" Width="48px" AutoCompleteType="Disabled">0</asp:TextBox>&nbsp;<asp:Button
                                ID="ButtonInventory" runat="server" Text="查詢" OnClick="ButtonInventory_Click"
                                Height="20px" Width="84px" />
                        </div>
                        <asp:GridView ID="GridViewInventory" runat="server" DataSourceID="SqlDataSourceInventory"
                            PageSize="35" CssClass="defaultGrid" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            EmptyDataText="查詢無任何資料。">
                            <Columns>
                                <asp:BoundField DataField="CompanyName" HeaderText="供貨商" SortExpression="CompanyName" />
                                <asp:BoundField DataField="ProductName" HeaderText="商品名稱" SortExpression="ProductName" />
                                <asp:BoundField DataField="ColorName" HeaderText="顏色" SortExpression="ColorName"
                                    HeaderStyle-Width="64px" />
                                <asp:BoundField DataField="SizeName" HeaderText="尺寸" SortExpression="SizeName" HeaderStyle-Width="64px" />
                                <asp:BoundField DataField="CurrentCount" HeaderText="目前數量" SortExpression="CurrentCount"
                                    DataFormatString="{0:N0}" HeaderStyle-Width="212px" />
                            </Columns>
                            <HeaderStyle ForeColor="Blue" />
                            <PagerStyle HorizontalAlign="Center" CssClass="defaultPager" />
                            <SelectedRowStyle BackColor="LightSalmon" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="PanelLost" runat="server" Visible="false">
                        <asp:GridView ID="GridViewLost" runat="server" CssClass="defaultGrid" DataSourceID="SqlDataSourceLost"
                            AllowPaging="True" AllowSorting="True" DataKeyNames="CompanyId" AutoGenerateColumns="False"
                            EmptyDataText="查詢無任何資料。" OnPageIndexChanged="GridViewLost_PageIndexChanged" PageSize="35">
                            <Columns>
                                <asp:BoundField DataField="Days" HeaderText="失聯天數" ReadOnly="True" SortExpression="Days"
                                    DataFormatString="{0:N0}" HeaderStyle-Width="72px" />
                                <asp:TemplateField HeaderText="公司名稱" SortExpression="CompanyName">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonCompanyName" runat="server" Text='<%# Bind("CompanyName") %>'
                                            CommandArgument='<%# Bind("CompanyId") %>' TabIndex='<%# Container.DataItemIndex %>'
                                            ForeColor="Blue" OnClick="LinkButtonCompanyName_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="288px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CompanyPhone" HeaderText="公司電話" SortExpression="CompanyPhone" />
                                <asp:BoundField DataField="CompanyFax" HeaderText="公司傳真" SortExpression="CompanyFax" />
                            </Columns>
                            <HeaderStyle ForeColor="Blue" />
                            <PagerStyle HorizontalAlign="Center" CssClass="defaultPager" />
                            <SelectedRowStyle BackColor="LightSalmon" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Label ID="LabelMessage" runat="server" Visible="False"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="MenuDefault" EventName="MenuItemClick" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="sidebar">
        <ul>
            <li>
                <h2>
                    公司選單</h2>
                <asp:UpdatePanel ID="UpdatePanelMenu" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="menuDefault">
                            <asp:Menu ID="MenuDefault" runat="server" DataSourceID="XmlDataSourceDefault" SkipLinkText=""
                                BackColor="#E3EAEB" Width="208px" ForeColor="Black" Font-Size="Medium" RenderingMode="Table"
                                OnMenuItemClick="MenuDefault_MenuItemClick">
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
        </ul>
    </div>
    <asp:XmlDataSource ID="XmlDataSourceDefault" runat="server" DataFile="~/Menu/Default.xml"
        XPath="menuMap/menuMapNode"></asp:XmlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceAccount" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT aspnet_Account.AccountId, aspnet_Account.AccountNumber, (SUM(aspnet_Record.Quantity * aspnet_Record.Price) + aspnet_Account.Tax) AS Price, aspnet_Account.PrePay, (SUM(aspnet_Record.Quantity * aspnet_Record.Price) + aspnet_Account.Tax - aspnet_Account.PrePay) AS LeftPay, aspnet_Account.CompanyId AS LeftPay, aspnet_Account.DeliverDate, aspnet_Account.CheckDate FROM aspnet_Account INNER JOIN aspnet_Record ON (aspnet_Record.AccountId = aspnet_Account.AccountId) AND (aspnet_Record.DeleteDate IS NULL) WHERE (aspnet_Account.OwnerId = @OwnerId) AND (aspnet_Account.DeliverDate IS NULL OR aspnet_Account.CheckDate IS NULL) AND (aspnet_Account.DeleteDate IS NULL) GROUP BY aspnet_Account.AccountId, aspnet_Account.AccountNumber, aspnet_Account.Tax, aspnet_Account.PrePay, aspnet_Account.CompanyId, aspnet_Account.DeliverDate, aspnet_Account.CheckDate">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceInventory" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT company.CompanyName, product.ProductName, size.TypeName AS SizeName, color.TypeName AS ColorName, product.CurrentCount FROM aspnet_Company company, aspnet_Product product, aspnet_ProductSizeType size, aspnet_ProductColorType color WHERE (company.OwnerId = @OwnerId) AND (product.OwnerId = @OwnerId) AND (company.CompanyId = product.CompanyId) AND (product.CurrentCount >= @Count) AND (size.TypeId = product.ProductSize) AND (color.TypeId = product.ProductColor) AND (product.DeleteDate IS NULL) ORDER BY product.ProductName, color.TypeName, size.TypeName">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
            <asp:ControlParameter Name="Count" ControlID="TextBoxInventory" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceLost" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT CompanyId, CompanyName, CompanyPhone, CompanyFax, DATEDIFF(day, LastDate, GETDATE()) AS Days FROM aspnet_Company WHERE (OwnerId = @OwnerId) AND DATEDIFF(day, LastDate, CURRENT_TIMESTAMP) >= CompanyLost AND (DeleteDate IS NULL) ORDER BY LastDate">
        <SelectParameters>
            <asp:ProfileParameter Name="OwnerId" PropertyName="PrincipalGuid" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
