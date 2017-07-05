using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Linq;

public partial class Member_Company : System.Web.UI.Page
{
    #region SQL command
    protected static String selectDistinctProductId = "SELECT DISTINCT record.ProductId FROM aspnet_Record record, aspnet_Account account WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (account.AccountId = record.AccountId) AND (record.DeleteDate IS NULL) AND (account.DeleteDate IS NULL)";
    protected static String selectLastProducts = "SELECT product.ProductName, record.Price, account.SaveDate FROM aspnet_Account account, aspnet_Record record, aspnet_product product WHERE (record.ProductId = @ProductId) AND (product.ProductId = @ProductId) AND (record.AccountId = account.AccountId) AND (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (product.DeleteDate IS NULL) AND (record.DeleteDate IS NULL) AND (account.DeleteDate IS NULL) ORDER BY account.SaveDate DESC";
    protected static String selectCompanyType = "SELECT CompanyType FROM aspnet_Company WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL) ";

    protected static String selectCompany = "SELECT * FROM aspnet_Company WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL)";
    protected static String selectCompanyCount = "SELECT COUNT(*) FROM aspnet_Company WHERE (Ownerid = @OwnerId) AND (CompanyName = @CompanyName) AND (DeleteDate IS NULL)";
    protected static String insertCompany = "INSERT INTO aspnet_Company (OwnerId, CompanyType, CompanyCode, CompanyNumber, CompanyName, CompanyAddress, CompanyPhone, CompanyFax, Note, Memo, CompanyLost) VALUES (@OwnerId, @CompanyType, @CompanyCode, @CompanyNumber, @CompanyName, @CompanyAddress, @CompanyPhone, @CompanyFax, @Note, @Memo, @CompanyLost)";
    protected static String updateCompany = "UPDATE aspnet_Company SET CompanyType = @CompanyType, CompanyCode = @CompanyCode, CompanyNumber = @CompanyNumber, CompanyName = @CompanyName, CompanyAddress = @CompanyAddress, CompanyPhone = @CompanyPhone, CompanyFax = @CompanyFax, Note = @Note, Memo = @Memo, CompanyLost = @CompanyLost WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId)";
    protected static String deleteCompany = "UPDATE aspnet_Company SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId)";

    protected static String selectPerson = "SELECT * FROM aspnet_Person WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (PersonId = @PersonId) AND (DeleteDate IS NULL)";
    protected static String insertPerson = "INSERT INTO aspnet_Person (OwnerId, PersonCode, CompanyId, PersonTitle, PersonName, PersonMobile, PersonPhone, PersonMail, Note) VALUES (@OwnerId, @PersonCode, @CompanyId, @PersonTitle, @PersonName, @PersonMobile, @PersonPhone, @PersonMail, @Note)";
    protected static String updatePerson = "UPDATE aspnet_Person SET PersonCode = @PersonCode, PersonTitle = @PersonTitle, PersonName = @PersonName, PersonMobile = @PersonMobile, PersonPhone = @PersonPhone, PersonMail = @PersonMail, Note = @Note WHERE (OwnerId = @OwnerId) AND (PersonId = @PersonId) AND (CompanyId = @CompanyId)";
    protected static String deletePerson = "UPDATE aspnet_Person SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (PersonId = @PersonId) AND (CompanyId = @CompanyId)";

    protected static String selectProductTop1 = "SELECT TOP 1 * FROM aspnet_Product WHERE (OwnerId = @OwnerId) AND (ProductGroup = @ProductGroup) AND (DeleteDate IS NULL)";
    protected static String selectProduct = "SELECT * FROM aspnet_Product WHERE (OwnerId = @OwnerId) AND (ProductGroup = @ProductGroup) AND (ProductType = @ProductType) AND (ProductColor = @ProductColor) AND (ProductSize = @ProductSize) AND (DeleteDate IS NULL)";
    protected static String insertProduct = "INSERT INTO aspnet_Product (OwnerId, CompanyId, ProductGroup, ProductType, ProductNumber, ProductName, CurrentCount, Note, ProductCosts, ProductColor, ProductSize) VALUES (@OwnerId, @CompanyId, @ProductGroup, @ProductType, @ProductNumber, @ProductName, @CurrentCount, @Note, @ProductCosts, @ProductColor, @ProductSize)";
    protected static String updateProduct = "UPDATE aspnet_Product SET ProductType = @ProductType, ProductNumber = @ProductNumber, ProductName = @ProductName, CurrentCount = @CurrentCount, Note = @Note, ProductCosts = @ProductCosts WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (ProductGroup = @ProductGroup) AND (ProductColor = @ProductColor) AND (ProductSize = @ProductSize)";
    protected static String deleteProduct = "UPDATE aspnet_Product SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (ProductGroup = @ProductGroup) AND (ProductType = @ProductType) AND (ProductColor = @ProductColor) AND (ProductSize = @ProductSize)";

    protected static String selectProductGroupCount = "SELECT COUNT(*) FROM aspnet_ProductGroup WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";
    protected static String insertProductGroup = "INSERT INTO aspnet_ProductGroup (OwnerId, TypeId, TypeName) VALUES (@OwnerId, @TypeId, @TypeName)";
    protected static String updateProductGroup = "UPDATE aspnet_ProductGroup SET TypeName = @TypeName WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";
    protected static String deleteProductGroup = "UPDATE aspnet_ProductGroup SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";

    protected static String insertProductType = "INSERT INTO aspnet_ProductType (OwnerId, TypeName) VALUES (@OwnerId, @TypeName)";
    protected static String selectProductTypeId = "SELECT TypeId FROM aspnet_ProductType WHERE (OwnerId = @OwnerId) AND (TypeName = @TypeName) AND (DeleteDate IS NULL)";
    protected static String updateProductType = "UPDATE aspnet_ProductType SET TypeName = @TypeName WHERE (OwnerId = @OWnerId) AND (TypeId = @TypeId)";
    protected static String deleteProductType = "UPDATE aspnet_ProductType SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";

    protected static String insertProductColorType = "INSERT INTO aspnet_ProductColorType (OwnerId, TypeName) VALUES (@OwnerId, @TypeName)";
    protected static String selectProductColorTypeId = "SELECT TypeId FROM aspnet_ProductColorType WHERE (OwnerId = @OwnerId) AND (TypeName = @TypeName) AND (DeleteDate IS NULL)";
    protected static String updateProductColorType = "UPDATE aspnet_ProductColorType SET TypeName = @TypeName WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";
    protected static String deleteProductColorType = "UPDATE aspnet_ProductColorType SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";

    protected static String insertProductSizeType = "INSERT INTO aspnet_ProductSizeType (OwnerId, TypeName) VALUES (@OwnerId, @TypeName)";
    protected static String selectProductSizeTypeId = "SELECT TypeId FROM aspnet_ProductSizeType WHERE (OwnerId = @OwnerId) AND (TypeName = @TypeName) AND (DeleteDate IS NULL)";
    protected static String updateProductSizeType = "UPDATE aspnet_ProductSizeType SET TypeName = @TypeName WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";
    protected static String deleteProductSizeType = "UPDATE aspnet_ProductSizeType SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (TypeId = @TypeId)";
    #endregion

    #region 公司類型 GUID
    protected static String customerGuid = "80951888-8C05-47DD-BC4D-B87DE0BDDB74";
    protected static String manufactureGuid = "AF639CBC-6B81-43EC-B0EB-F8CC0F96CB79";
    #endregion

    #region 定義取得公司最近交易紀錄的暫存資料結構
    protected struct LastData
    {
        public String productName;
        public Double price;
        public DateTime saveDate;
    }
    #endregion

    /// <summary>
    /// 重置公司資料欄位內容
    /// </summary>
    protected void ClearCompany()
    {
        TextBoxCompanyCode.Text = String.Empty;
        TextBoxCompanyLost.Text = "30";
        TextBoxCompanyName.Text = String.Empty;
        TextBoxCompanyNumber.Text = String.Empty;
        TextBoxCompanyPhone.Text = String.Empty;
        TextBoxCompanyFax.Text = String.Empty;
        TextBoxNote.Text = String.Empty;
        TextBoxMemo.Text = String.Empty;
        TextBoxCompanyAddress.Text = String.Empty;
    }

    /// <summary>
    /// 重置人員資料欄位內容
    /// </summary>
    protected void ClearPerson()
    {
        TextBoxPersonCode.Text = String.Empty;
        TextBoxPersonTitle.Text = String.Empty;
        TextBoxPersonName.Text = String.Empty;
        TextBoxPersonMobile.Text = String.Empty;
        TextBoxPersonPhone.Text = String.Empty;
        TextBoxPersonMail.Text = String.Empty;
        TextBoxPersonNote.Text = String.Empty;
    }

    /// <summary>
    /// 重置商品資料所有欄位內容
    /// </summary>
    protected void ClearProduct()
    {
        DropDownListProductType.SelectedIndex = -1;
        DropDownListProductColor.SelectedIndex = -1;
        DropDownListProductSize.SelectedIndex = -1;
        TextBoxProductNumber.Text = String.Empty;
        TextBoxProductName.Text = String.Empty;
        TextBoxProductCount.Text = "0";
        TextBoxProductNote.Text = String.Empty;
        TextBoxProductCosts.Text = "0";
    }

    /// <summary>
    /// 重置商品資料欄位內容 (不包含選項欄)
    /// </summary>
    protected void ClearProductData()
    {
        TextBoxProductNumber.Text = String.Empty;
        TextBoxProductName.Text = String.Empty;
        TextBoxProductCount.Text = "0";
        TextBoxProductNote.Text = String.Empty;
        TextBoxProductCosts.Text = "0";
    }

    /// <summary>
    /// 依照 ListBoxCompanies 取得公司類型 ID
    /// </summary>
    /// <returns>公司類型 ID</returns>
    protected String SelectCompanyType()
    {
        String guid = String.Empty;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlCommand command = new SqlCommand(selectCompanyType, connection))
            {
                command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                try
                {
                    connection.Open();

                    Object o = command.ExecuteScalar();
                    if (null != o && DBNull.Value != o)
                        guid = Convert.ToString(o);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        return guid;
    }

    /// <summary>
    /// 顯示公司資料
    /// </summary>
    protected void ShowData()
    {
        Session["CompanyMenu"] = "Company";
        LabelMessage.Visible = false;

        if (-1 == ListBoxCompanies.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "尚未選擇公司！");
            return;
        }

        LabelCaption.Text = "資料編輯";

        ClearCompany();

        PanelData.Visible = true;
        PanelDataMemo.Visible = true;
        PanelProduct.Visible = false;
        PanelType.Visible = false;

        #region 取得並列出公司資料
        TextBoxCompanyName.Text = ListBoxCompanies.SelectedItem.Text;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (adapter.SelectCommand = new SqlCommand(selectCompany, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                    try
                    {
                        connection.Open();

                        adapter.Fill(table);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            #region 列出公司資料
            if (1 == table.Rows.Count)
            {
                DataRow row = table.Rows[0];

                if (0 == DropDownListCompanyType.Items.Count)
                    DropDownListCompanyType.DataBind();

                String companyType = Convert.ToString(row["CompanyType"]);
                DropDownListCompanyType.SelectedIndex = -1;
                foreach (ListItem item in DropDownListCompanyType.Items)
                {
                    if (0 == item.Value.CompareTo(companyType))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                TextBoxCompanyNumber.Text = row["CompanyNumber"].ToString();
                TextBoxCompanyPhone.Text = row["CompanyPhone"].ToString();
                TextBoxCompanyFax.Text = row["CompanyFax"].ToString();
                TextBoxCompanyAddress.Text = row["CompanyAddress"].ToString();
                TextBoxNote.Text = row["Note"].ToString();
                TextBoxMemo.Text = row["Memo"].ToString();
                TextBoxCompanyLost.Text = row["CompanyLost"].ToString();
                TextBoxCompanyCode.Text = row["CompanyCode"].ToString();
            }
            #endregion
        }
        #endregion

        PanelCompanyControl.Visible = true;
        PanelControl.Visible = false;
    }

    /// <summary>
    /// 顯示人員資料
    /// </summary>
    protected void ShowPerson()
    {
        if (-1 == ListBoxCompanies.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "尚未選擇公司！");
            return;
        }

        ClearPerson();

        PanelPerson.Visible = true;
        PanelProduct.Visible = false;
        PanelType.Visible = false;

        PanelPersonControl.Visible = true;
        PanelControl.Visible = false;
    }

    /// <summary>
    /// 顯示商品資料
    /// </summary>
    protected void ShowProduct()
    {
        Session["CompanyMenu"] = "Product";
        LabelMessage.Visible = false;

        if (-1 == ListBoxCompanies.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "尚未選擇公司！");
            return;
        }

        LabelCaption.Text = "商品資料";

        ClearProduct();

        PanelData.Visible = false;
        PanelDataMemo.Visible = false;
        PanelPerson.Visible = false;
        PanelProduct.Visible = false;
        PanelType.Visible = false;

        PanelCompanyControl.Visible = false;
        PanelPersonControl.Visible = false;
        PanelControl.Visible = false;

        //String guid = SelectCompanyType();// 廠商才顯示商品編輯功能
        //if (0 == guid.CompareTo(manufactureGuid.ToLower()))
        //{
        //    PanelProduct.Visible = true;
        //    ListBoxProducts.DataBind();

        //    PanelControl.Visible = true;
        //}
        //else Message.LabelError(LabelMessage, "選擇公司非廠商無商品編輯。");

        // 無論廠商貨客戶都會顯示商品列表
        PanelProduct.Visible = true;
        ListBoxProducts.DataBind();

        PanelControl.Visible = true;
    }

    /// <summary>
    /// 顯示商品類型資料
    /// </summary>
    protected void ShowType()
    {
        Session["CompanyMenu"] = "Type";
        LabelMessage.Visible = false;

        if (-1 == ListBoxCompanies.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "尚未選擇公司！");
            return;
        }

        LabelCaption.Text = "商品類型";

        TextBoxType.Text = String.Empty;

        PanelData.Visible = false;
        PanelDataMemo.Visible = false;
        PanelPerson.Visible = false;
        PanelProduct.Visible = false;
        PanelType.Visible = false;

        PanelCompanyControl.Visible = false;
        PanelPersonControl.Visible = false;
        PanelControl.Visible = false;

        //String guid = SelectCompanyType();// 廠商才顯示商品類型編輯功能
        //if (0 == guid.CompareTo(manufactureGuid.ToLower()))
        //{
        //    PanelType.Visible = true;
        //    PanelControl.Visible = true;

        //    if (0 == DropDownListType.Items.Count)// 類型選項設定
        //    {
        //        DropDownListType.Items.Add(new ListItem("商品類型", "ProductType"));
        //        DropDownListType.Items.Add(new ListItem("顏色類型", "ProductColor"));
        //        DropDownListType.Items.Add(new ListItem("尺寸類型", "ProductSize"));

        //        DropDownListType.SelectedIndex = 0;
        //    }
        //}
        //else Message.LabelError(LabelMessage, "選擇公司非廠商無商品類型編輯。");

        // 無論廠商或客戶都會顯示類型列表
        PanelType.Visible = true;
        PanelControl.Visible = true;

        if (0 == DropDownListType.Items.Count)// 類型選項設定
        {
            DropDownListType.Items.Add(new ListItem("商品類型", "ProductType"));
            DropDownListType.Items.Add(new ListItem("顏色類型", "ProductColor"));
            DropDownListType.Items.Add(new ListItem("尺寸類型", "ProductSize"));

            DropDownListType.SelectedIndex = 0;
        }

        ListBoxType.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        Session["ListBoxCompaniesSelectedIndex"] = null;

        Title = Profile.PrincipalCompanyName + " - 公司資料";

        #region 按鈕操作警告訊息
        ButtonClear.Attributes.Add("onclick", "return confirm('清空所有欄位，準備新增資料？')");
        ButtonInsert.Attributes.Add("onclick", "return confirm('新增資料到資料庫？')");
        ButtonUpdate.Attributes.Add("onclick", "return confirm('更新資料到資料庫？')");
        ButtonDelete.Attributes.Add("onclick", "return confirm('確定從資料庫刪除資料？')");

        ButtonCompanyClear.Attributes.Add("onclick", "return confirm('清空所有欄位，準備新增公司？')");
        ButtonCompanyInsert.Attributes.Add("onclick", "return confirm('新增公司到資料庫？')");
        ButtonCompanyUpdate.Attributes.Add("onclick", "return confirm('更新公司到資料庫？')");
        ButtonCompanyDelete.Attributes.Add("onclick", "return confirm('確定從資料庫刪除公司？')");

        ButtonPersonClear.Attributes.Add("onclick", "return confirm('清空所有欄位，準備新增人員？')");
        ButtonPersonInsert.Attributes.Add("onclick", "return confirm('新增人員到資料庫？')");
        ButtonPersonUpdate.Attributes.Add("onclick", "return confirm('更新人員到資料庫？')");
        ButtonPersonDelete.Attributes.Add("onclick", "return confirm('確定從資料庫刪除人員？')");
        #endregion

        #region 檢查是否自動搜尋公司
        String companyId = Session["CompanyId"] as String;
        if (!String.IsNullOrEmpty(companyId))
        {
            TextBoxSearch.Text = Session["CompanyName"] as String;
            
            if (String.IsNullOrWhiteSpace(TextBoxSearch.Text))// 沒有搜尋條件則更換 SqlDataSource
                ListBoxCompanies.DataSourceID = "SqlDataSourceCompanies";
            else
                ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";// 預設 - 條件搜尋

            ListBoxCompanies.DataBind();

            String menu = Session["CompanyMenu"] as String;// 先取得選單紀錄

            foreach (ListItem item in ListBoxCompanies.Items)
            {
                if (0 == companyId.CompareTo(item.Value))
                {
                    item.Selected = true;
                    ListBoxCompanies_SelectedIndexChanged(null, null);

                    break;
                }
            }

            #region 決定選單
            if (String.IsNullOrEmpty(menu))
            {
                menu = "Company";
                Session["CompanyMenu"] = menu;
            }

            switch (menu)
            {
                case "Company":
                    {
                        MenuCompany.Items[0].Selected = true;
                        ShowData();
                        ShowPerson();
                        break;
                    }

                case "Product":
                    {
                        MenuCompany.Items[2].Selected = true;
                        ShowProduct();
                        break;
                    }

                case "Type":
                    {
                        MenuCompany.Items[3].Selected = true;
                        ShowType();
                        break;
                    }
            }
            #endregion
        }
        #endregion

        TextBoxSearch.Attributes.Add("onkeypress", "OnSearch('" + ButtonSearch.ClientID + "',event)");
        TextBoxSearch.Attributes.Add("onfocusin", "select();");
        TextBoxSearch.Focus();
    }

    protected void MenuCompany_MenuItemClick(object sender, MenuEventArgs e)
    {
        switch (e.Item.Value)
        {
            case "company":
                {
                    ShowData();
                    ShowPerson();
                    break;
                }

            case "product": { ShowProduct(); break; }
            case "type": { ShowType(); break; }
        }
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        Session["ListBoxCompaniesSelectedIndex"] = null;
        LabelCaption.Text = String.Empty;

        GridViewRecord.Visible = false;
        PanelEdit.Visible = false;
        PanelData.Visible = false;
        PanelControl.Visible = false;

        PanelRecord.Visible = false;
        PanelProduct.Visible = false;
        PanelType.Visible = false;

        PanelDataMemo.Visible = false;
        PanelCompanyControl.Visible = false;
        PanelPerson.Visible = false;
        PanelPersonControl.Visible = false;

        ListBoxCompanies.Visible = false;

        if (PanelProduct.Visible)// 清除現有資料繫結
        {
            ListBoxProducts.DataSourceID = String.Empty;
            ListBoxProducts.DataBind();
        }

        if (String.IsNullOrWhiteSpace(TextBoxSearch.Text))// 沒有搜尋條件則更換 SqlDataSource
            ListBoxCompanies.DataSourceID = "SqlDataSourceCompanies";
        else
            ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";// 預設 - 條件搜尋

        ListBoxCompanies.DataBind();// 列表公司

        #region Session 紀錄
        Session["CompanyName"] = TextBoxSearch.Text;
        Session.Remove("CompanyId");
        #endregion
    }

    protected void ListBoxCompanies_DataBinding(object sender, EventArgs e)
    {
        //ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";// 預設 - 條件搜尋

        //if (String.IsNullOrWhiteSpace(TextBoxSearch.Text))// 沒有搜尋條件則更換 SqlDataSource
        //    ListBoxCompanies.DataSourceID = "SqlDataSourceCompanies";
    }

    protected void ListBoxCompanies_DataBound(object sender, EventArgs e)
    {
        ListBoxCompanies.Visible = false;

        if (ListBoxCompanies.Items.Count > 0)// 確認資料繫結後，顯示 ListBoxInvoices
        {
            ListBoxCompanies.Visible = true;

            if (ListBoxCompanies.Items.Count < 2)
                ListBoxCompanies.Rows = 2;
            else if (ListBoxCompanies.Items.Count < 16)
                ListBoxCompanies.Rows = ListBoxCompanies.Items.Count;
            else
                ListBoxCompanies.Rows = 16;
        }
    }

    protected void ListBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
    {
        String index = Session["ListBoxCompaniesSelectedIndex"] as String;
        if (!String.IsNullOrEmpty(index))
        {
            if (ListBoxCompanies.SelectedIndex == Convert.ToInt32(index))
                return;
        }
        Session["ListBoxCompaniesSelectedIndex"] = Convert.ToString(ListBoxCompanies.SelectedIndex);

        LabelMessage.Visible = false;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            #region 取得最後一筆交易記錄
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataSet set = new DataSet();
                DataTable table = new DataTable();

                try
                {
                    connection.Open();

                    #region 取得交易過的商品 ID (Distinct)
                    using (adapter.SelectCommand = new SqlCommand(selectDistinctProductId, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        adapter.Fill(table);
                    }
                    #endregion

                    #region 取得每一個商品ID的對應資料 (ProductName, Price, SaveDate)
                    using (adapter.SelectCommand = new SqlCommand(selectLastProducts, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                        adapter.SelectCommand.Parameters.AddWithValue("ProductId", "");

                        foreach (DataRow row in table.Rows)
                        {
                            adapter.SelectCommand.Parameters["ProductId"].Value = row["ProductId"];

                            adapter.Fill(set);
                        }
                    }
                    #endregion
                }
                finally
                {
                    connection.Close();
                }

                #region 依照 SaveDate，剔除重複的 ProductName
                List<LastData> lastdataList = new List<LastData>();
                foreach (DataTable t in set.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        LastData lastData = new LastData();

                        lastData.productName = r["ProductName"].ToString();
                        lastData.price = Convert.ToDouble(r["Price"]);
                        lastData.saveDate = DateTime.Parse(r["SaveDate"].ToString());

                        lastdataList.Add(lastData);
                    }
                }
                #endregion

                #region 建立需要刪除的列表
                List<LastData> removeList = new List<LastData>();
                for (Int32 i = 0; i < lastdataList.Count; i++)
                {
                    for (Int32 j = 0; j < lastdataList.Count; j++)
                    {
                        if (i != j)
                        {
                            if (0 == lastdataList[i].productName.CompareTo(lastdataList[j].productName))
                            {
                                if (lastdataList[i].saveDate >= lastdataList[j].saveDate)
                                {
                                    removeList.Add(lastdataList[j]);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 依照刪除列表做刪除動作、建立 Table 並導入 GridviewRecord
                for (Int32 i = 0; i < removeList.Count; i++)
                    lastdataList.Remove(removeList[i]);

                table.Reset();
                table.Columns.Add("ProductName", typeof(String));
                table.Columns.Add("Price", typeof(Double));
                table.Columns.Add("SaveDate", typeof(DateTime));
                for (Int32 i = 0; i < lastdataList.Count; i++)
                {
                    DataRow r = table.NewRow();
                    r["ProductName"] = lastdataList[i].productName;
                    r["Price"] = lastdataList[i].price;
                    r["SaveDate"] = lastdataList[i].saveDate;
                    table.Rows.Add(r);
                }

                Session["CompanyDataTable"] = table;

                GridViewRecord.Visible = true;
                GridViewRecord.DataSource = table;
                GridViewRecord.DataBind();
                #endregion
            }
            #endregion
        }

        PanelEdit.Visible = true;
        PanelControl.Visible = true;

        #region 依照目前顯示的編輯類型更新內容
        if (0 == MenuCompany.Items.Count)
        {
            MenuCompany.DataBind();
            MenuCompany.Items[0].Selected = true;
        }

        switch (MenuCompany.SelectedValue)
        {
            case "company":
                {
                    ShowData();
                    ShowPerson();
                    break;
                }
            case "product": { ShowProduct(); break; }
            case "type": { ShowType(); break; }
        }
        #endregion

        #region Session 紀錄
        Session["CompanyId"] = ListBoxCompanies.SelectedValue;
        #endregion
    }

    protected void GridViewRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                {
                    e.Row.Cells[0].Text = "型號";
                    e.Row.Cells[1].Text = "價格";
                    e.Row.Cells[2].Text = "日期";
                    break;
                }

            case DataControlRowType.DataRow:
                {
                    e.Row.Cells[1].Text = String.Format("{0:N}", Convert.ToDouble(e.Row.Cells[1].Text));
                    e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToShortDateString();

                    break;
                }
        }
    }

    protected void GridViewRecord_DataBound(object sender, EventArgs e)
    {
        PanelRecord.Visible = false;

        if (0 != GridViewRecord.Rows.Count)
            PanelRecord.Visible = true;
    }

    protected void ListBoxPerson_DataBound(object sender, EventArgs e)
    {
        if (ListBoxPerson.Items.Count > 0)// 確認資料繫結後，顯示 ListBoxInvoices
        {
            if (ListBoxPerson.Items.Count < 2)
                ListBoxPerson.Rows = 2;
            else if (ListBoxPerson.Items.Count < 16)
                ListBoxPerson.Rows = ListBoxPerson.Items.Count;
            else
                ListBoxPerson.Rows = 16;
        }
    }

    protected void ListBoxPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region 取得並列出人員資料
        ClearPerson();

        TextBoxPersonName.Text = ListBoxPerson.SelectedItem.Text;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (adapter.SelectCommand = new SqlCommand(selectPerson, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                    adapter.SelectCommand.Parameters.AddWithValue("PersonId", ListBoxPerson.SelectedValue);

                    try
                    {
                        connection.Open();

                        adapter.Fill(table);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            #region 列出人員資料
            if (1 == table.Rows.Count)
            {
                DataRow row = table.Rows[0];

                TextBoxPersonCode.Text = row["PersonCode"].ToString();
                TextBoxPersonTitle.Text = row["PersonTitle"].ToString();
                TextBoxPersonMobile.Text = row["PersonMobile"].ToString();
                TextBoxPersonPhone.Text = row["PersonPhone"].ToString();
                TextBoxPersonMail.Text = row["PersonMail"].ToString();
                TextBoxPersonNote.Text = row["Note"].ToString();
            }
            #endregion
        }
        #endregion
    }

    protected void ListBoxProducts_DataBound(object sender, EventArgs e)
    {
        ListBoxProducts.Visible = false;

        if (ListBoxProducts.Items.Count > 0)// 確認資料繫結後，顯示 ListBoxInvoices
        {
            ListBoxProducts.Visible = true;

            if (ListBoxProducts.Items.Count < 2)
                ListBoxProducts.Rows = 2;
            else if (ListBoxProducts.Items.Count < 16)
                ListBoxProducts.Rows = ListBoxProducts.Items.Count;
            else
                ListBoxProducts.Rows = 16;
        }
    }

    protected void ListBoxProducts_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearProduct();

        TextBoxProductName.Text = ListBoxProducts.SelectedItem.Text;

        #region 取得並列出商品資料
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (adapter.SelectCommand = new SqlCommand(selectProductTop1, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductGroup", ListBoxProducts.SelectedValue);

                    try
                    {
                        connection.Open();

                        adapter.Fill(table);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            #region 列出商品資料
            if (1 == table.Rows.Count)
            {
                DataRow row = table.Rows[0];

                TextBoxProductNumber.Text = row["ProductNumber"].ToString();
                TextBoxProductCount.Text = row["CurrentCount"].ToString();
                TextBoxProductNote.Text = row["Note"].ToString();
                TextBoxProductCosts.Text = row["ProductCosts"].ToString();

                String productType = Convert.ToString(row["ProductType"]);
                DropDownListProductType.SelectedIndex = -1;
                foreach (ListItem item in DropDownListProductType.Items)
                {
                    if (0 == item.Value.CompareTo(productType))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                String productColor = Convert.ToString(row["ProductColor"]);
                DropDownListProductColor.SelectedIndex = -1;
                foreach (ListItem item in DropDownListProductColor.Items)
                {
                    if (0 == item.Value.CompareTo(productColor))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                String productSize = Convert.ToString(row["ProductSize"]);
                DropDownListProductSize.SelectedIndex = -1;
                foreach (ListItem item in DropDownListProductSize.Items)
                {
                    if (0 == item.Value.CompareTo(productSize))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            #endregion
        }
        #endregion

        CheckBoxColor.Checked = true;
        CheckBoxSize.Checked = true;
    }

    protected void DropDownListProductType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (-1 == ListBoxProducts.SelectedIndex)
            return;

        // 不做任何動作
    }

    protected void DropDownListProductColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (-1 == ListBoxProducts.SelectedIndex)
            return;

        CheckBoxColor.Checked = false;

        ClearProductData();

        TextBoxProductName.Text = ListBoxProducts.SelectedItem.Text;

        #region 依照選取的商品類型、顏色、尺寸，顯示目前商品資料
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (adapter.SelectCommand = new SqlCommand(selectProduct, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductGroup", ListBoxProducts.SelectedValue);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductType", DropDownListProductType.SelectedValue);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductColor", DropDownListProductColor.SelectedValue);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductSize", DropDownListProductSize.SelectedValue);

                    try
                    {
                        connection.Open();

                        adapter.Fill(table);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            #region 列出商品資料
            if (1 == table.Rows.Count)
            {
                DataRow row = table.Rows[0];

                TextBoxProductNumber.Text = row["ProductNumber"].ToString();
                TextBoxProductCount.Text = row["CurrentCount"].ToString();
                TextBoxProductNote.Text = row["Note"].ToString();
                TextBoxProductCosts.Text = row["ProductCosts"].ToString();
            }
            #endregion
        }
        #endregion
    }

    protected void DropDownListProductSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (-1 == ListBoxProducts.SelectedIndex)
            return;

        CheckBoxColor.Checked = false;

    }

    protected void DropDownListType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxType.Text = String.Empty;

        ListBoxType.DataBind();// 列出商品類型
    }

    protected void ListBoxType_DataBinding(object sender, EventArgs e)
    {
        switch (DropDownListType.SelectedValue)
        {
            case "ProductType": { ListBoxType.DataSourceID = "SqlDataSourceProductType"; break; }
            case "ProductColor": { ListBoxType.DataSourceID = "SqlDataSourceProductColorType"; break; }
            case "ProductSize": { ListBoxType.DataSourceID = "SqlDataSourceProductSizeType"; break; }
        }
    }

    protected void ListBoxType_DataBound(object sender, EventArgs e)
    {
        ListBoxType.Visible = false;

        if (ListBoxType.Items.Count > 0)// 確認資料繫結後，顯示 ListBoxInvoices
        {
            ListBoxType.Visible = true;

            if (ListBoxType.Items.Count < 2)
                ListBoxType.Rows = 2;
            else if (ListBoxType.Items.Count < 16)
                ListBoxType.Rows = ListBoxType.Items.Count;
            else
                ListBoxType.Rows = 16;
        }
    }

    protected void ListBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxType.Text = ListBoxType.SelectedItem.Text;
    }

    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        switch (MenuCompany.SelectedValue)
        {
            case "product":
                {
                    ClearProduct();
                    ListBoxProducts.SelectedIndex = -1;

                    CheckBoxColor.Checked = true;
                    CheckBoxSize.Checked = true;
                    break;
                }

            case "type":
                {
                    TextBoxType.Text = String.Empty;
                    ListBoxType.SelectedIndex = -1;
                    break;
                }
        }
    }

    protected void ButtonInsert_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        switch (MenuCompany.SelectedValue)
        {
            case "product":
                {
                    #region 新增商品
                    if (PanelProduct.Visible)
                    {
                        #region 條件檢查
                        if (String.IsNullOrWhiteSpace(TextBoxProductCount.Text))
                        {
                            Message.LabelError(LabelMessage, "必須設定商品數量！");
                            return;
                        }

                        try { Int32 count = Convert.ToInt32(TextBoxProductCount.Text); }
                        catch
                        {
                            Message.LabelError(LabelMessage, "商品數量數值不正確！");
                            return;
                        }

                        if (String.IsNullOrWhiteSpace(TextBoxProductName.Text))
                        {
                            Message.LabelError(LabelMessage, "必須提供商品名稱！");
                            return;
                        }
                        #endregion

                        #region 新增
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                #region 建立 ProductGroup
                                Guid productGroup = Guid.Empty;// 確認新增 ProductGroup ID 不重複
                                while (Guid.Empty == productGroup)
                                {
                                    productGroup = Guid.NewGuid();

                                    using (SqlCommand command = new SqlCommand(selectProductGroupCount, connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                        command.Parameters.AddWithValue("TypeId", productGroup);

                                        if (0 != Convert.ToInt32(command.ExecuteScalar()))
                                            productGroup = Guid.Empty;
                                    }
                                }

                                using (SqlCommand command = new SqlCommand(insertProductGroup, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeId", productGroup);
                                    command.Parameters.AddWithValue("TypeName", TextBoxProductName.Text);

                                    command.ExecuteNonQuery();
                                }
                                #endregion

                                #region 建立 Product
                                using (SqlCommand command = new SqlCommand(insertProduct, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                                    command.Parameters.AddWithValue("ProductGroup", productGroup);
                                    command.Parameters.AddWithValue("ProductType", DropDownListProductType.SelectedValue);
                                    command.Parameters.AddWithValue("ProductName", TextBoxProductName.Text);
                                    command.Parameters.AddWithValue("ProductNumber", TextBoxProductNumber.Text);
                                    command.Parameters.AddWithValue("CurrentCount", TextBoxProductCount.Text);
                                    command.Parameters.AddWithValue("Note", TextBoxProductNote.Text);
                                    command.Parameters.AddWithValue("ProductCosts", TextBoxProductCosts.Text);
                                    command.Parameters.AddWithValue("ProductColor", "");
                                    command.Parameters.AddWithValue("ProductSize", "");

                                    if (CheckBoxColor.Checked && CheckBoxSize.Checked)
                                    {
                                        #region 同時建立所有顏色及尺寸
                                        foreach (ListItem size in DropDownListProductSize.Items)
                                        {
                                            command.Parameters["ProductSize"].Value = size.Value;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (CheckBoxColor.Checked)
                                        {
                                            #region 同時建立單一尺寸、所有顏色
                                            command.Parameters["ProductSize"].Value = DropDownListProductSize.SelectedValue;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                        else if (CheckBoxSize.Checked)
                                        {
                                            #region 同時建立單一顏色、所有尺寸
                                            command.Parameters["ProductColor"].Value = DropDownListProductColor.SelectedValue;

                                            foreach (ListItem size in DropDownListProductSize.Items)
                                            {
                                                command.Parameters["ProductSize"].Value = size.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                transaction.Commit();
                                transaction = null;

                                Message.LabelMessage(LabelMessage, "新增完成。");
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "新增失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxProducts.DataBind();

                        return;
                    }
                    #endregion

                    break;
                }

            case "type":
                {
                    #region 新增類型
                    if (PanelType.Visible)
                    {
                        #region 條件檢查
                        if (String.IsNullOrWhiteSpace(TextBoxType.Text))
                        {
                            Message.LabelError(LabelMessage, "必須提供類型名稱！");
                            return;
                        }
                        #endregion

                        #region 決定指令字串
                        String commandString = String.Empty;
                        switch (DropDownListType.SelectedValue)
                        {
                            case "ProductType": { commandString = insertProductType; break; }
                            case "ProductColor": { commandString = insertProductColorType; break; }
                            case "ProductSize": { commandString = insertProductSizeType; break; }
                        }
                        #endregion

                        #region 新增
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                using (SqlCommand command = new SqlCommand(commandString, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeName", TextBoxType.Text);

                                    command.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                transaction = null;

                                Message.LabelMessage(LabelMessage, "新增完成。");
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "新增失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxType.DataBind();
                        switch (DropDownListType.SelectedIndex)
                        {
                            case 0: DropDownListProductType.DataBind(); break;
                            case 1: DropDownListProductColor.DataBind(); break;
                            case 2: DropDownListProductSize.DataBind(); break;
                        }

                        return;
                    }
                    #endregion

                    break;
                }
        }
    }

    protected void ButtonUpdate_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        switch (MenuCompany.SelectedValue)
        {
            case "product":
                {
                    #region 更新商品
                    if (PanelProduct.Visible)
                    {
                        #region 條件檢查
                        if (String.IsNullOrWhiteSpace(TextBoxProductCount.Text))
                        {
                            Message.LabelError(LabelMessage, "必須設定商品數量！");
                            return;
                        }

                        try { Int32 count = Convert.ToInt32(TextBoxProductCount.Text); }
                        catch
                        {
                            Message.LabelError(LabelMessage, "商品數量數值不正確！");
                            return;
                        }

                        if (String.IsNullOrWhiteSpace(TextBoxProductName.Text))
                        {
                            Message.LabelError(LabelMessage, "必須提供商品名稱！");
                            return;
                        }

                        if (-1 == ListBoxProducts.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品！");
                            return;
                        }

                        if (-1 == ListBoxCompanies.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品所屬公司！");
                            return;
                        }
                        #endregion

                        #region 更新
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                #region 更新ProductGroup
                                using (SqlCommand command = new SqlCommand(updateProductGroup, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeId", ListBoxProducts.SelectedValue);
                                    command.Parameters.AddWithValue("TypeName", TextBoxProductName.Text);

                                    command.ExecuteNonQuery();
                                }
                                #endregion

                                #region 更新商品
                                using (SqlCommand command = new SqlCommand(updateProduct, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("ProductType", DropDownListProductType.SelectedValue);
                                    command.Parameters.AddWithValue("ProductNumber", TextBoxProductNumber.Text);
                                    command.Parameters.AddWithValue("ProductName", TextBoxProductName.Text);
                                    command.Parameters.AddWithValue("CurrentCount", TextBoxProductCount.Text);
                                    command.Parameters.AddWithValue("Note", TextBoxProductNote.Text);
                                    command.Parameters.AddWithValue("ProductCosts", TextBoxProductCosts.Text);
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                                    command.Parameters.AddWithValue("ProductGroup", ListBoxProducts.SelectedValue);
                                    command.Parameters.AddWithValue("ProductColor", String.Empty);
                                    command.Parameters.AddWithValue("ProductSize", String.Empty);

                                    if (CheckBoxColor.Checked && CheckBoxSize.Checked)
                                    {
                                        #region 同時更新全部顏色及尺寸
                                        foreach (ListItem size in DropDownListProductSize.Items)
                                        {
                                            command.Parameters["ProductSize"].Value = size.Value;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (CheckBoxColor.Checked)
                                        {
                                            #region 同時更新單一尺寸、所有顏色
                                            command.Parameters["ProductSize"].Value = DropDownListProductSize.SelectedValue;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                        else if (CheckBoxSize.Checked)
                                        {
                                            #region 同時更新單一顏色、所有尺寸
                                            command.Parameters["ProductColor"].Value = DropDownListProductColor.SelectedValue;

                                            foreach (ListItem size in DropDownListProductSize.Items)
                                            {
                                                command.Parameters["ProductSize"].Value = size.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                transaction.Commit();
                                transaction = null;

                                Message.LabelMessage(LabelMessage, "更新完成。");
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "更新失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxProducts.DataBind();

                        return;
                    }
                    #endregion

                    break;
                }

            case "type":
                {
                    #region 更新類型
                    if (PanelType.Visible)
                    {
                        #region 條件檢查
                        if (String.IsNullOrWhiteSpace(TextBoxType.Text))
                        {
                            Message.LabelError(LabelMessage, "必須提供類型名稱！");
                            return;
                        }

                        if (-1 == ListBoxType.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品類型！");
                            return;
                        }
                        #endregion

                        #region 決定指令字串
                        String selectIdString = String.Empty;
                        String updateString = String.Empty;
                        switch (DropDownListType.SelectedValue)
                        {
                            case "ProductType":
                                {
                                    selectIdString = selectProductTypeId;
                                    updateString = updateProductType;
                                    break;
                                }

                            case "ProductColor":
                                {
                                    selectIdString = selectProductColorTypeId;
                                    updateString = updateProductColorType;
                                    break;
                                }

                            case "ProductSize":
                                {
                                    selectIdString = selectProductSizeTypeId;
                                    updateString = updateProductSizeType;
                                    break;
                                }
                        }
                        #endregion

                        #region 更新
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                #region 取得類型 GUID
                                String guid = String.Empty;
                                using (SqlCommand command = new SqlCommand(selectIdString, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeName", TextBoxType.Text);

                                    Object o = command.ExecuteScalar();
                                    if (null != o && DBNull.Value != o)
                                        guid = Convert.ToString(o);
                                }
                                #endregion

                                if (!String.IsNullOrEmpty(guid))
                                {
                                    #region 更新
                                    using (SqlCommand command = new SqlCommand(updateString, connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("TypeName", TextBoxType.Text);
                                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                        command.Parameters.AddWithValue("TypeId", ListBoxType.SelectedValue);

                                        command.ExecuteNonQuery();

                                        Message.LabelMessage(LabelMessage, "更新完成。");
                                    }
                                    #endregion
                                }
                                else Message.LabelError(LabelMessage, "類型名稱已存在！");

                                transaction.Commit();
                                transaction = null;
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "更新失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxType.DataBind();
                        switch (DropDownListType.SelectedIndex)
                        {
                            case 0: DropDownListProductType.DataBind(); break;
                            case 1: DropDownListProductColor.DataBind(); break;
                            case 2: DropDownListProductSize.DataBind(); break;
                        }

                        return;
                    }
                    #endregion

                    break;
                }
        }
    }

    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        switch (MenuCompany.SelectedValue)
        {
            case "product":
                {
                    #region 刪除商品
                    if (PanelProduct.Visible)
                    {
                        #region 條件檢查
                        if (-1 == ListBoxProducts.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品！");
                            return;
                        }

                        if (-1 == ListBoxCompanies.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品所屬公司！");
                            return;
                        }
                        #endregion

                        #region 刪除
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                #region 刪除商品
                                using (SqlCommand command = new SqlCommand(deleteProduct, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                                    command.Parameters.AddWithValue("ProductGroup", ListBoxProducts.SelectedValue);
                                    command.Parameters.AddWithValue("ProductType", DropDownListProductType.SelectedValue);
                                    command.Parameters.AddWithValue("ProductColor", "");
                                    command.Parameters.AddWithValue("ProductSize", "");

                                    if (CheckBoxColor.Checked && CheckBoxSize.Checked)
                                    {
                                        #region 同時刪除所有顏色及尺寸
                                        foreach (ListItem size in DropDownListProductSize.Items)
                                        {
                                            command.Parameters["ProductSize"].Value = size.Value;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (CheckBoxColor.Checked)
                                        {
                                            #region 同時刪除單一尺寸、所有顏色
                                            command.Parameters["ProductSize"].Value = DropDownListProductSize.SelectedValue;

                                            foreach (ListItem color in DropDownListProductColor.Items)
                                            {
                                                command.Parameters["ProductColor"].Value = color.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                        else if (CheckBoxSize.Checked)
                                        {
                                            #region 同時刪除單一顏色、所有尺寸
                                            command.Parameters["ProductColor"].Value = DropDownListProductColor.SelectedValue;

                                            foreach (ListItem size in DropDownListProductSize.Items)
                                            {
                                                command.Parameters["ProductSize"].Value = size.Value;

                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion

                                #region 刪除 ProductGroup
                                using (SqlCommand command = new SqlCommand(deleteProductGroup, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeId", ListBoxProducts.SelectedValue);

                                    command.ExecuteNonQuery();
                                }
                                #endregion

                                transaction.Commit();
                                transaction = null;

                                Message.LabelMessage(LabelMessage, "刪除完成。");
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "刪除失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxProducts.DataBind();

                        ClearProduct();

                        return;
                    }
                    #endregion

                    break;
                }
            case "type":
                {
                    #region 刪除類型
                    if (PanelType.Visible)
                    {
                        #region 條件檢查
                        if (-1 == ListBoxType.SelectedIndex)
                        {
                            Message.LabelError(LabelMessage, "尚未選擇商品類型！");
                            return;
                        }
                        #endregion

                        #region 決定指令字串
                        String commandString = String.Empty;
                        switch (DropDownListType.SelectedValue)
                        {
                            case "ProductType": { commandString = deleteProductType; break; }
                            case "ProductColor": { commandString = deleteProductColorType; break; }
                            case "ProductSize": { commandString = deleteProductSizeType; break; }
                        }
                        #endregion

                        #region 刪除
                        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            SqlTransaction transaction = null;

                            try
                            {
                                connection.Open();
                                transaction = connection.BeginTransaction();

                                using (SqlCommand command = new SqlCommand(commandString, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                    command.Parameters.AddWithValue("TypeId", ListBoxType.SelectedValue);

                                    command.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                transaction = null;

                                Message.LabelMessage(LabelMessage, "刪除完成。");
                            }
                            catch (Exception exception)
                            {
                                Message.LabelError(LabelMessage, "刪除失敗！");

                                if (null != transaction)
                                    transaction.Rollback();

                                throw exception;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        #endregion

                        ListBoxType.DataBind();

                        TextBoxType.Text = String.Empty;

                        return;
                    }
                    #endregion

                    break;
                }
        }
    }

    protected void ButtonCompanyClear_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        ClearPerson();
        ClearCompany();

        ListBoxCompanies.SelectedIndex = -1;
        DropDownListCompanyType.SelectedIndex = 1;// 預選第二個公司類型

        GridViewRecord.DataSource = null;
        GridViewRecord.DataBind();
    }

    protected void ButtonCompanyInsert_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 新增公司
        if (PanelData.Visible)
        {
            #region 條件檢查
            if (String.IsNullOrWhiteSpace(TextBoxCompanyLost.Text))
            {
                Message.LabelError(LabelMessage, "必須設定失聯天數！");
                return;
            }

            try { Int32 count = Convert.ToInt32(TextBoxCompanyLost.Text); }
            catch
            {
                Message.LabelError(LabelMessage, "失聯天數數值不正確！");
                return;
            }

            if (String.IsNullOrWhiteSpace(TextBoxCompanyName.Text))
            {
                Message.LabelError(LabelMessage, "必須提供公司名稱！");
                return;
            }
            #endregion

            #region 新增
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    Int32 exists = 0;
                    using (SqlCommand command = new SqlCommand(selectCompanyCount, connection, transaction))
                    {
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("CompanyName", TextBoxCompanyName.Text);

                        exists = Convert.ToInt32(command.ExecuteScalar());
                    }

                    if (0 == exists)
                    {
                        #region 新增
                        using (SqlCommand command = new SqlCommand(insertCompany, connection, transaction))
                        {
                            command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                            command.Parameters.AddWithValue("CompanyType", DropDownListCompanyType.SelectedValue);
                            command.Parameters.AddWithValue("CompanyCode", TextBoxCompanyCode.Text);
                            command.Parameters.AddWithValue("CompanyNumber", TextBoxCompanyNumber.Text);
                            command.Parameters.AddWithValue("CompanyName", TextBoxCompanyName.Text);
                            command.Parameters.AddWithValue("CompanyAddress", TextBoxCompanyAddress.Text);
                            command.Parameters.AddWithValue("CompanyPhone", TextBoxCompanyPhone.Text);
                            command.Parameters.AddWithValue("CompanyFax", TextBoxCompanyFax.Text);
                            command.Parameters.AddWithValue("Note", TextBoxNote.Text);
                            command.Parameters.AddWithValue("Memo", TextBoxMemo.Text);
                            command.Parameters.AddWithValue("CompanyLost", TextBoxCompanyLost.Text);

                            command.ExecuteNonQuery();

                            Message.LabelMessage(LabelMessage, "新增完成。");
                        }
                        #endregion
                    }
                    else Message.LabelError(LabelMessage, "公司名稱重複！");

                    transaction.Commit();
                    transaction = null;
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "新增失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxCompanies.DataBind();

            GridViewRecord.DataSource = null;
            GridViewRecord.DataBind();

            return;
        }
        #endregion
    }

    protected void ButtonCompanyUpdate_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 更新公司
        if (PanelData.Visible)
        {
            #region 條件檢查
            if (String.IsNullOrWhiteSpace(TextBoxCompanyLost.Text))
            {
                Message.LabelError(LabelMessage, "必須設定失聯天數！");
                return;
            }

            try { Int32 count = Convert.ToInt32(TextBoxCompanyLost.Text); }
            catch
            {
                Message.LabelError(LabelMessage, "失聯天數數值不正確！");
                return;
            }

            if (String.IsNullOrWhiteSpace(TextBoxCompanyName.Text))
            {
                Message.LabelError(LabelMessage, "必須提供公司名稱！");
                return;
            }

            if (-1 == ListBoxCompanies.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇公司！");
                return;
            }
            #endregion

            #region 更新
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(updateCompany, connection, transaction))
                    {
                        command.Parameters.AddWithValue("CompanyType", DropDownListCompanyType.SelectedValue);
                        command.Parameters.AddWithValue("CompanyCode", TextBoxCompanyCode.Text);
                        command.Parameters.AddWithValue("CompanyNumber", TextBoxCompanyNumber.Text);
                        command.Parameters.AddWithValue("CompanyName", TextBoxCompanyName.Text);
                        command.Parameters.AddWithValue("CompanyAddress", TextBoxCompanyAddress.Text);
                        command.Parameters.AddWithValue("CompanyPhone", TextBoxCompanyPhone.Text);
                        command.Parameters.AddWithValue("CompanyFax", TextBoxCompanyFax.Text);
                        command.Parameters.AddWithValue("Note", TextBoxNote.Text);
                        command.Parameters.AddWithValue("Memo", TextBoxMemo.Text);
                        command.Parameters.AddWithValue("CompanyLost", TextBoxCompanyLost.Text);
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    transaction = null;

                    Message.LabelMessage(LabelMessage, "更新完成。");
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "更新失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxCompanies.DataBind();

            return;
        }
        #endregion
    }

    protected void ButtonCompanyDelete_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 刪除公司
        if (PanelData.Visible)
        {
            #region 條件檢查
            if (-1 == ListBoxCompanies.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇公司！");
                return;
            }
            #endregion

            #region 刪除
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(deleteCompany, connection, transaction))
                    {
                        command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    transaction = null;

                    Message.LabelMessage(LabelMessage, "刪除完成。");
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "刪除失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxCompanies.DataBind();

            GridViewRecord.DataSource = null;
            GridViewRecord.DataBind();

            ClearCompany();

            return;
        }
        #endregion
    }

    protected void ButtonPersonClear_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        ClearPerson();
        ListBoxPerson.SelectedIndex = -1;
    }

    protected void ButtonPersonInsert_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 新增人員
        if (PanelPerson.Visible)
        {
            #region 條件檢查
            if (String.IsNullOrWhiteSpace(TextBoxPersonName.Text))
            {
                Message.LabelError(LabelMessage, "必須提供人員名稱！");
                return;
            }
            #endregion

            #region 新增
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(insertPerson, connection, transaction))
                    {
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("PersonCode", TextBoxPersonCode.Text);
                        command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                        command.Parameters.AddWithValue("PersonTitle", TextBoxPersonTitle.Text);
                        command.Parameters.AddWithValue("PersonName", TextBoxPersonName.Text);
                        command.Parameters.AddWithValue("PersonMobile", TextBoxPersonMobile.Text);
                        command.Parameters.AddWithValue("PersonPhone", TextBoxPersonPhone.Text);
                        command.Parameters.AddWithValue("PersonMail", TextBoxPersonMail.Text);
                        command.Parameters.AddWithValue("Note", TextBoxPersonNote.Text);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    transaction = null;

                    Message.LabelMessage(LabelMessage, "新增完成。");
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "新增失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxPerson.DataBind();

            return;
        }
        #endregion
    }

    protected void ButtonPersonUpdate_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 更新人員
        if (PanelPerson.Visible)
        {
            #region 條件檢查
            if (String.IsNullOrWhiteSpace(TextBoxPersonName.Text))
            {
                Message.LabelError(LabelMessage, "必須提供人員名稱！");
                return;
            }

            if (-1 == ListBoxPerson.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇人員！");
                return;
            }

            if (-1 == ListBoxCompanies.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇人員所屬公司！");
                return;
            }
            #endregion

            #region 更新
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(updatePerson, connection, transaction))
                    {
                        command.Parameters.AddWithValue("PersonCode", TextBoxPersonCode.Text);
                        command.Parameters.AddWithValue("PersonTitle", TextBoxPersonTitle.Text);
                        command.Parameters.AddWithValue("PersonName", TextBoxPersonName.Text);
                        command.Parameters.AddWithValue("PersonMobile", TextBoxPersonMobile.Text);
                        command.Parameters.AddWithValue("PersonPhone", TextBoxPersonPhone.Text);
                        command.Parameters.AddWithValue("PersonMail", TextBoxPersonMail.Text);
                        command.Parameters.AddWithValue("Note", TextBoxPersonNote.Text);
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("PersonId", ListBoxPerson.SelectedValue);
                        command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    transaction = null;

                    Message.LabelMessage(LabelMessage, "更新完成。");
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "更新失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxPerson.DataBind();

            return;
        }
        #endregion
    }

    protected void ButtonPersonDelete_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 刪除人員
        if (PanelPerson.Visible)
        {
            #region 條件檢查
            if (-1 == ListBoxPerson.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇人員！");
                return;
            }

            if (-1 == ListBoxCompanies.SelectedIndex)
            {
                Message.LabelError(LabelMessage, "尚未選擇人員所屬公司！");
                return;
            }
            #endregion

            #region 刪除
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlCommand command = new SqlCommand(deletePerson, connection, transaction))
                    {
                        command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("PersonId", ListBoxPerson.SelectedValue);
                        command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    transaction = null;

                    Message.LabelMessage(LabelMessage, "刪除完成。");
                }
                catch (Exception exception)
                {
                    Message.LabelError(LabelMessage, "刪除失敗！");

                    if (null != transaction)
                        transaction.Rollback();

                    throw exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            #endregion

            ListBoxPerson.DataBind();

            ClearPerson();

            return;
        }
        #endregion
    }

    protected void GridViewRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gv = sender as GridView;
        DataTable table = Session["CompanyDataTable"] as DataTable;

        gv.PageIndex = e.NewPageIndex;// 換頁
        gv.SelectedIndex = -1;// 取消選擇列後才繫結資料

        gv.DataSource = table;
        gv.DataBind();
    }
}