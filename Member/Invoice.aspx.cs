using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;

public partial class Member_Invoice : System.Web.UI.Page
{
    #region SQL command
    //protected static String selectCompanyData = "SELECT CompanyType, CompanyName, CompanyNumber, CompanyPhone, DeliverAddress, Note FROM aspnet_Company WHERE (CompanyId = @CompanyId) AND (OwnerId = @OwnerId) AND (DeleteDate IS NULL)";
    protected static String selectCompanyData = "SELECT CompanyType, CompanyName, CompanyNumber, CompanyPhone, CompanyAddress, Note FROM aspnet_Company WHERE (CompanyId = @CompanyId) AND (OwnerId = @OwnerId) AND (DeleteDate IS NULL)";
    protected static String selectInvoice = "SELECT AccountType, AccountNumber, BillNumber, Tax, PrePay, PayDays, SaveDate, Note FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL)";
    protected static String selectInvoiceNotes = "SELECT TOP 50 Note, AccountId FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL) AND (Note IS NOT NULL) AND (Note <> '') ORDER BY SaveDate DESC";

    protected static String countAccountId = "SELECT COUNT(*) FROM aspnet_Account WHERE (AccountId = @AccountId)";
    protected static String selectAccount = "SELECT AccountType, AccountNumber, BillNumber, Tax, PrePay, PayDays, SaveDate, Note FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL)";
    protected static String selectRecords = "SELECT record.ProductId, product.ProductGroup, product.ProductColor, size.Score, record.Price, record.Quantity FROM aspnet_Account account, aspnet_Product product, aspnet_Record record, aspnet_ProductSizeType size WHERE (account.AccountId = @AccountId) AND (account.OwnerId = @OwnerId) AND (record.AccountId = account.AccountId) AND (product.ProductId = record.ProductId) AND (size.TypeId = product.ProductSize) AND (account.DeleteDate IS NULL) AND (product.DeleteDate IS NULL) AND (record.DeleteDate IS NULL) AND (size.DeleteDate IS NULL) ORDER BY record.Row";

    protected static String selectLastPrice = "SELECT TOP 1 record.Price FROM aspnet_Account account, aspnet_Record record, aspnet_Product product WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (account.AccountType = @AccountType) AND (record.AccountId = account.AccountId) AND (product.ProductGroup = @Group) AND (product.ProductColor = @Color) AND (product.ProductId = record.ProductId) AND (record.DeleteDate IS NULL) ORDER BY account.SaveDate DESC";
    protected static String selectProductIds = "SELECT product.ProductId FROM aspnet_Product product, aspnet_ProductSizeType size WHERE (product.OwnerId = @OwnerId) AND (product.ProductGroup = @ProductGroup) AND (product.ProductColor = @ProductColor) AND (product.ProductSize = size.TypeId) AND (product.DeleteDate IS NULL) ORDER BY size.Score";

    protected static String selectAccountType = "SELECT AccountType FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId) AND (DeleteDate IS NULL)";
    protected static String selectDeliverDate = "SELECT DeliverDate FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId) AND (DeleteDate IS NULL)";

    protected static String insertAccount = "INSERT INTO aspnet_Account (OwnerId, AccountId, AccountType, CompanyId, AccountNumber, BillNumber, Tax, PrePay, SaveDate, Note) VALUES (@OwnerId, @AccountId, @AccountType, @CompanyId, @AccountNumber, @BillNumber, @Tax, @PrePay, @SaveDate, @Note)";
    protected static String updateAccount = "UPDATE aspnet_Account SET AccountType = @AccountType, AccountNumber = @AccountNumber, BillNumber = @BillNumber, Tax = @Tax, PrePay = @PrePay, Note = @Note WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";
    protected static String deleteAccount = "UPDATE aspnet_Account SET DeleteDate = @DeleteDate WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";

    protected static String insertRecord = "INSERT INTO aspnet_Record (AccountId, ProductId, Price, Quantity, Row) VALUES (@AccountId, @ProductId, @Price, @Quantity, @Row)";
    protected static String deleteRecords = "UPDATE aspnet_Record SET DeleteDate = @DeleteDate WHERE (AccountId = @AccountId) AND (DeleteDate IS NULL)";

    protected static String inceaseProduct = "UPDATE aspnet_Product SET CurrentCount = CurrentCount + @Count WHERE (OwnerId = @OwnerId) AND (ProductId = @ProductId) AND (DeleteDate IS NULL)";
    protected static String deceaseProduct = "UPDATE aspnet_Product SET CurrentCount = CurrentCount - @Count WHERE (OwnerId = @OwnerId) AND (ProductId = @ProductId) AND (DeleteDate IS NULL)";

    protected static String updateCompanyNote = "UPDATE aspnet_Company SET Note = @Note WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId)";

    protected static String selectCompanyNameAndAccountIdFromInvoiceBillNumber = "SELECT company.CompanyId, company.CompanyName, account.AccountId, account.AccountNumber FROM aspnet_Account account, aspnet_Company company WHERE account.OwnerId = @OwnerId AND account.BillNumber = @BillNumber AND company.CompanyId = account.CompanyId";
    #endregion

    #region 公司類型 GUID
    protected static String customerGuid = "80951888-8C05-47DD-BC4D-B87DE0BDDB74";
    protected static String manufactureGuid = "AF639CBC-6B81-43EC-B0EB-F8CC0F96CB79";
    #endregion

    #region 單據類型 GUID
    protected static String customSales = "FDD998A1-D477-484B-B0EE-6D0DE75EE166";
    protected static String customReturns = "5E5A5BE7-46C5-4D2B-ABD8-491330921243";
    protected static String manufactureSales = "A6FC3F19-C524-4BA1-A3C0-8F180D8093C5";
    protected static String manufactureReturns = "26239577-49DF-44F9-AEBD-729870F0744F";
    #endregion

    /// <summary>
    /// 建立使用者單據 Table 定義
    /// </summary>
    /// <returns></returns>
    protected DataTable CreateDataTable()
    {
        DataTable dataTable = new DataTable();

        dataTable.Columns.Add(new DataColumn("Group", typeof(String)));
        dataTable.Columns.Add(new DataColumn("Color", typeof(String)));
        dataTable.Columns.Add(new DataColumn("Size0", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size1", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size2", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size3", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size4", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size5", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size6", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size7", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size8", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size9", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Size10", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Price", typeof(Double)));
        dataTable.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
        dataTable.Columns.Add(new DataColumn("Amount", typeof(Double)));

        Session["AccountDataTable"] = dataTable;

        return dataTable;
    }

    /// <summary>
    /// 建立使用者單據資料列表定義
    /// </summary>
    /// <returns></returns>
    protected List<AccountData> CreateAccountDataList()
    {
        List<AccountData> accountDataList = new List<AccountData>();

        Session["AccountDataList"] = accountDataList;

        return accountDataList;
    }

    /// <summary>
    /// 建立新單據號碼
    /// </summary>
    /// <returns>新單據號碼</returns>
    protected String NewAccountNumber()
    {
        String year = Convert.ToString(DateTime.Now.Year - 1911);
        String month = DateTime.Now.Month.ToString().PadLeft(2, '0');
        String day = DateTime.Now.Day.ToString().PadLeft(2, '0');
        String hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
        String minute = DateTime.Now.Minute.ToString().PadLeft(2, '0');
        String second = DateTime.Now.Second.ToString().PadLeft(2, '0');
        String ms = DateTime.Now.Millisecond.ToString().PadRight(3, '0');

        String number = year + month + day + hour + minute + second + ms;
        return number;
    }

    /// <summary>
    /// 初始化 GridViewAccount，一併 建立/重置 暫存單據 DateTable/AccountDataList
    /// </summary>
    protected void InitGridViewAccount()
    {
        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();
        else dataTable.Clear();

        List<AccountData> accountDataList = (List<AccountData>)Session["AccountDataList"];
        if (null == accountDataList) accountDataList = CreateAccountDataList();
        else accountDataList.Clear();

        GridViewAccount.DataSource = dataTable;
        GridViewAccount.DataBind();
    }

    /// <summary>
    /// 計算指定行合計金額
    /// </summary>
    /// <param name="row">指定行索引號</param>
    protected void Amount(Int32 row)
    {
        Int32 count;
        Int32 quantity = 0;

        TextBox tb;
        for (Int32 col = 2; col < 13; col++)
        {
            tb = (TextBox)GridViewAccount.Rows[row].Cells[col].Controls[1];
            count = Convert.ToInt32(tb.Text);
            tb.Text = String.Format("{0:N0}", count);

            quantity += count;
        }

        Label label = (Label)GridViewAccount.Rows[row].Cells[14].Controls[1];
        label.Text = String.Format("{0:N0}", quantity);

        tb = (TextBox)GridViewAccount.Rows[row].Cells[13].Controls[1];
        Double price = Convert.ToDouble(tb.Text);
        tb.Text = String.Format("{0:N}", price);

        Double amount = price * quantity;
        label = (Label)GridViewAccount.Rows[row].Cells[15].Controls[1];
        label.Text = String.Format("{0:N}", amount);
    }

    /// <summary>
    /// 總計單據
    /// </summary>
    /// <param name="culculate_tax">是否自動計算稅額</param>
    protected void Sum(Boolean culculate_tax)
    {
        Label label;
        Label label1;
        Double buffer;
        Double sum = 0;
        Int32 buffer1;
        Int32 num = 0;

        for (Int32 row = 0; row < GridViewAccount.Rows.Count; row++)
        {
            label = (Label)GridViewAccount.Rows[row].Cells[15].Controls[1];

            try { buffer = Convert.ToDouble(label.Text); }
            catch { buffer = 0; }
            sum += buffer;

            label1 = (Label)GridViewAccount.Rows[row].Cells[14].Controls[1];
            try { buffer1 = Convert.ToInt32(label1.Text); }
            catch { buffer1 = 0; }
            num += buffer1;
        }

        try { buffer = Convert.ToDouble(TextBoxTax.Text); }
        catch { buffer = 0; }

        Double tax = buffer;
        if (culculate_tax)// 計算稅額
            tax = sum * 0.05;

        try { buffer = Convert.ToDouble(TextBoxPrePay.Text); }
        catch { buffer = 0; }

        Double prepay = buffer;
        Double total = sum + tax;
        Double leftpay = total - prepay;

        LabelTotalNum.Text = string.Format("{0}", num);
        TextBoxTax.Text = String.Format("{0:N}", tax);
        LabelSum.Text = String.Format("{0:N}", sum);
        LabelTotal.Text = String.Format("{0:N}", total);
        TextBoxPrePay.Text = String.Format("{0:N}", prepay);
        LabelLeftPay.Text = String.Format("{0:N}", leftpay);
    }

    /// <summary>
    /// 清除總計資料
    /// </summary>
    protected void InitSum()
    {
        LabelTotalNum.Text = String.Format("{0}", 0);
        TextBoxTax.Text = String.Format("{0:N}", 0);
        LabelSum.Text = String.Format("{0:N}", 0);
        LabelTotal.Text = String.Format("{0:N}", 0);
        TextBoxPrePay.Text = String.Format("{0:N}", 0);
        LabelLeftPay.Text = String.Format("{0:N}", 0);
    }

    /// <summary>
    /// 重置單據備註 (公司資料不變)
    /// </summary>
    protected void InitNote()
    {
        TextBoxNote.Text = "";

        TextBoxNote.Visible = true;
        LinkButtonNote.Visible = true;

        DropDownListNote.Visible = false;
        LinkButtonNoteSelect.Visible = false;
        LinkButtonNoteCancel.Visible = false;
    }

    /// <summary>
    /// 重置單據標頭 (公司資料不變)
    /// </summary>
    protected void InitTitle()
    {
        RadioButtonListAccountType.SelectedIndex = 0;
        LabelAccountNumber.Text = NewAccountNumber();
        TextBoxBillNumber.Text = String.Empty;
        LabelDate.Text = DateTime.Now.ToShortDateString();
        LabelPayDate.Text = Convert.ToDateTime(LabelDate.Text).AddDays(30).ToShortDateString();

        //InitNote();
    }

    /// <summary>
    /// 依照 GridViewAccount 內容，重新建立 dataTable 及 accountDataList 內的資料
    /// </summary>
    protected void UpdateData()
    {
        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        TextBox tb;
        Int32 count;
        AccountData ad;
        DropDownList ddl;

        dataTable.Clear();
        for (Int32 i = 0; i < GridViewAccount.Rows.Count; i++)
        {
            if (accountDataList.Count < i - 1)
            {
                ad = new AccountData();
                accountDataList.Add(ad);
            }
            else ad = accountDataList[i];

            ad.Quantity = 0;

            ddl = GridViewAccount.Rows[i].Cells[0].Controls[1] as DropDownList;
            ad.Group = ddl.SelectedValue;

            ddl = GridViewAccount.Rows[i].Cells[1].Controls[1] as DropDownList;
            ad.Color = ddl.SelectedValue;

            tb = GridViewAccount.Rows[i].Cells[2].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size0 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[3].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size1 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[4].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size2 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[5].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size3 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[6].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size4 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[7].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size5 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[8].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size6 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[9].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size7 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[10].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size8 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[11].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size9 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[12].Controls[1] as TextBox;
            count = Convert.ToInt32(tb.Text); ;
            ad.Size10 = count;
            ad.Quantity += count;

            tb = GridViewAccount.Rows[i].Cells[13].Controls[1] as TextBox;
            ad.Price = Convert.ToDouble(tb.Text);

            //label = (Label)GridViewAccount.Rows[i].Cells[14].Controls[1];
            //ad.Quantity = Convert.ToInt32(label.Text);

            //label = (Label)GridViewAccount.Rows[i].Cells[15].Controls[1];
            //ad.Amount = Convert.ToDouble(label.Text);
            ad.Amount = ad.Quantity * ad.Price;

            dataTable.Rows.Add(ad.ToArray());
        }
    }

    /// <summary>
    /// 取得指定列產品 ID 及最後一次交易價格
    /// </summary>
    /// <param name="row">指定列索引號</param>
    protected void LastPriceAndProductIds(Int32 row)
    {
        LastPriceAndProductIds(GridViewAccount.Rows[row]);
    }

    /// <summary>
    /// 取得指定列產品 ID 及最後一次交易價格
    /// </summary>
    /// <param name="row">指定列</param>
    protected void LastPriceAndProductIds(GridViewRow row)
    {
        DropDownList group = row.Cells[0].Controls[1] as DropDownList;
        DropDownList color = row.Cells[1].Controls[1] as DropDownList;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            #region 取得 Size 對應的 ProductId
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable table = new DataTable();

                using (adapter.SelectCommand = new SqlCommand(selectProductIds, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductGroup", group.SelectedValue);
                    adapter.SelectCommand.Parameters.AddWithValue("ProductColor", color.SelectedValue);

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

                if (table.Rows.Count > 0)
                {
                    List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
                    if (null == accountDataList) accountDataList = CreateAccountDataList();

                    AccountData ad = accountDataList[row.RowIndex];

                    ad.Product0 = new Guid(table.Rows[0]["ProductId"].ToString());
                    ad.Product1 = new Guid(table.Rows[1]["ProductId"].ToString());
                    ad.Product2 = new Guid(table.Rows[2]["ProductId"].ToString());
                    ad.Product3 = new Guid(table.Rows[3]["ProductId"].ToString());
                    ad.Product4 = new Guid(table.Rows[4]["ProductId"].ToString());
                    ad.Product5 = new Guid(table.Rows[5]["ProductId"].ToString());
                    ad.Product6 = new Guid(table.Rows[6]["ProductId"].ToString());
                    ad.Product7 = new Guid(table.Rows[7]["ProductId"].ToString());
                    ad.Product8 = new Guid(table.Rows[8]["ProductId"].ToString());
                    ad.Product9 = new Guid(table.Rows[9]["ProductId"].ToString());
                    ad.Product10 = new Guid(table.Rows[10]["ProductId"].ToString());
                }
                else Message.LabelError(LabelMessage, "該型號的尺寸資料不完整，將無法寫入資料庫！");
            }
            #endregion

            // 取消此檢查，強制使用最後一次售價
            TextBox tb = row.Cells[13].Controls[1] as TextBox;
            //if (!String.IsNullOrEmpty(tb.Text))
            //{
            //    Double price = Convert.ToDouble(tb.Text);
            //    if (0 != price) return;
            //}

            #region 取得最後一次售價
            using (SqlCommand command = new SqlCommand(selectLastPrice, connection))
            {
                command.Parameters.AddWithValue("AccountType", RadioButtonListAccountType.SelectedValue);
                command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                command.Parameters.AddWithValue("Group", group.SelectedValue);
                command.Parameters.AddWithValue("Color", color.SelectedValue);
                command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                Object o;

                try
                {
                    connection.Open();

                    o = command.ExecuteScalar();
                }
                finally
                {
                    connection.Close();
                }

                Double price = 0;
                if (null != o && DBNull.Value != o)
                    price = Convert.ToDouble(o);

                tb.Text = String.Format("{0:N}", price);
            }
            #endregion
        }
    }

    /// <summary>
    /// 載入單據後，依序紀錄 ProductId
    /// </summary>
    /// <param name="index">索引號</param>
    protected void SetProductIds(Int32 index)
    {
        if (index == -1)
            return;

        if (GridViewAccount.Rows.Count < index)
            return;

        DropDownList ddlGroup = GridViewAccount.Rows[index].Cells[0].Controls[1] as DropDownList;
        DropDownList ddlColor = GridViewAccount.Rows[index].Cells[1].Controls[1] as DropDownList;
        if (null == ddlGroup.SelectedItem || null == ddlColor.SelectedItem)
            return;

        //String sAccountType = (String)Session["AccountType"];
        //Guid accountType = new Guid(sAccountType);

        UserData userData = Session["UserData"] as UserData;
        if (null == userData)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = new List<AccountData>();

        DataTable table = new DataTable();
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(selectProductIds, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@OwnerId", userData.PrincipalGuid);
                adapter.SelectCommand.Parameters.AddWithValue("@ProductGroup", ddlGroup.SelectedValue);
                adapter.SelectCommand.Parameters.AddWithValue("@ProductColor", ddlColor.SelectedValue);

                try
                {
                    connection.Open();
                    adapter.Fill(table);
                }
                finally { connection.Close(); }
            }
        }

        #region 取得Size對應的ProductId
        if (table.Rows.Count > 0)
        {
            AccountData ad = accountDataList[index];

            ad.Product0 = new Guid(table.Rows[0]["ProductId"].ToString());

            if (table.Rows.Count >= 2)
                ad.Product1 = new Guid(table.Rows[1]["ProductId"].ToString());

            if (table.Rows.Count >= 3)
                ad.Product2 = new Guid(table.Rows[2]["ProductId"].ToString());

            if (table.Rows.Count >= 4)
                ad.Product3 = new Guid(table.Rows[3]["ProductId"].ToString());

            if (table.Rows.Count >= 5)
                ad.Product4 = new Guid(table.Rows[4]["ProductId"].ToString());

            if (table.Rows.Count >= 6)
                ad.Product5 = new Guid(table.Rows[5]["ProductId"].ToString());

            if (table.Rows.Count >= 7)
                ad.Product6 = new Guid(table.Rows[6]["ProductId"].ToString());

            if (table.Rows.Count >= 8)
                ad.Product7 = new Guid(table.Rows[7]["ProductId"].ToString());

            if (table.Rows.Count >= 9)
                ad.Product8 = new Guid(table.Rows[8]["ProductId"].ToString());

            if (table.Rows.Count >= 10)
                ad.Product9 = new Guid(table.Rows[9]["ProductId"].ToString());

            if (table.Rows.Count >= 11)
                ad.Product10 = new Guid(table.Rows[10]["ProductId"].ToString());
        }
        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        Title = Profile.PrincipalCompanyName + " - 進銷作業";

        ButtonClearAccount.Attributes.Add("onclick", "return confirm('清空所有欄位，準備新增單據？')");
        ButtonInsertAccount.Attributes.Add("onclick", "return confirm('本單據新增到資料庫？')");
        ButtonUpdateAccount.Attributes.Add("onclick", "return confirm('已出貨數量會自動修正，確定更新本單據到資料庫？')");
        ButtonDeleteAccount.Attributes.Add("onclick", "return confirm('已出貨數量會自動增減，確定從資料庫刪除本單據？')");
        ButtonAppendRow.Attributes.Add("onclick", "return confirm('加入新一列？')");

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

            foreach (ListItem item in ListBoxCompanies.Items)
            {
                if (0 == companyId.CompareTo(item.Value))
                {
                    item.Selected = true;
                    ListBoxCompanies_SelectedIndexChanged(null, null);

                    ListBoxInvoices.DataBind();

                    break;
                }
            }
        }
        #endregion

        #region 檢查是否自動選取單據
        String accountNumber = Session["AccountNumber"] as String;
        String accountId = Session["AccountId"] as String;
        if (!String.IsNullOrEmpty(accountNumber) && !String.IsNullOrEmpty(accountId))
        {
            foreach (ListItem item in ListBoxInvoices.Items)
            {
                if (0 == accountId.CompareTo(item.Value))
                {
                    item.Selected = true;
                    ListBoxInvoices_SelectedIndexChanged(null, null);

                    break;
                }
            }
        }
        #endregion

        TextBoxSearch.Attributes.Add("onkeypress", "OnSearch('" + ButtonSearch.ClientID + "',event)");
        TextBoxSearch.Attributes.Add("onfocusin", "select();");
        TextBoxSearch.Focus();
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        ButtonPrint.Visible = false;
        PanelTitle.Visible = false;
        PanelControl.Visible = false;
        PanelAccount.Visible = false;
        PanelInvoices.Visible = false;
        ListBoxCompanies.Visible = false;
        TextBoxInvoice.Text = String.Empty;

        if (String.IsNullOrEmpty(TextBoxSearch.Text) || String.IsNullOrWhiteSpace(TextBoxSearch.Text))// 沒有搜尋條件則更換 SqlDataSource
            ListBoxCompanies.DataSourceID = "SqlDataSourceCompanies";
        else
            ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";// 預設 - 條件搜尋

        ListBoxCompanies.DataBind();// 列表公司

        #region Session 紀錄
        Session["CompanyName"] = TextBoxSearch.Text;

        if (IsPostBack)
        {
            Session.Remove("CompanyId");
            Session.Remove("AccountNumber");
            Session.Remove("AccountId");
        }
        #endregion
    }

    protected void ButtonInvoice_Click(object sender, EventArgs e)
    {
        ButtonPrint.Visible = false;
        PanelTitle.Visible = false;
        PanelControl.Visible = false;
        PanelAccount.Visible = false;
        PanelInvoices.Visible = false;
        ListBoxCompanies.Visible = false;

        if (String.IsNullOrEmpty(TextBoxInvoice.Text))
            return;

        if (String.IsNullOrWhiteSpace(TextBoxInvoice.Text))
            return;

        #region Session 紀錄
        Session["CompanyName"] = TextBoxSearch.Text;

        if (IsPostBack)
        {
            Session.Remove("CompanyId");
            Session.Remove("AccountNumber");
            Session.Remove("AccountId");
        }
        #endregion

        #region 依照發票號碼取得公司及單據唯一編號
        String companyId, companyName, accountId, accountNumber;
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (adapter.SelectCommand = new SqlCommand(selectCompanyNameAndAccountIdFromInvoiceBillNumber, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("BillNumber", TextBoxInvoice.Text);

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

                if (1 != table.Rows.Count)
                    return;

                companyId = Convert.ToString(table.Rows[0]["CompanyId"]);
                companyName = Convert.ToString(table.Rows[0]["CompanyName"]);
                accountId = Convert.ToString(table.Rows[0]["AccountId"]);
                accountNumber = Convert.ToString(table.Rows[0]["AccountNumber"]);
            }
        }

        if (String.IsNullOrEmpty(companyId))
            return;

        if (String.IsNullOrEmpty(companyName))
            return;

        if (String.IsNullOrEmpty(accountId))
            return;

        if (String.IsNullOrEmpty(accountNumber))
            return;

        TextBoxSearch.Text = companyName;
        Session["CompanyId"] = companyId;
        Session["AccountId"] = accountId;
        Session["AccountNumber"] = accountId;

        ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";
        ListBoxCompanies.DataBind();// 列表公司
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
        PanelAccount.Visible = false;
        PanelTitle.Visible = false;

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

        String companyId = Session["CompanyId"] as String;
        if (String.IsNullOrEmpty(companyId))
            return;

        foreach (ListItem item in ListBoxCompanies.Items)
        {
            if (!item.Value.Equals(companyId))
                continue;

            item.Selected = true;
            ListBoxCompanies_SelectedIndexChanged(null, null);
            break;
        }
    }

    protected void ListBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        TextBoxBillNumber.Text = String.Empty;

        InitSum();
        InitGridViewAccount();
        InitNote();

        #region 取得客戶/廠商資料填入 PanelTitle
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable table = new DataTable();
                using (adapter.SelectCommand = new SqlCommand(selectCompanyData, connection))
                {
                    try
                    {
                        connection.Open();

                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);

                        adapter.Fill(table);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                if (0 != table.Rows.Count)
                {
                    #region TitleTable 客戶/廠商資料
                    LabelCompanyName.Text = Convert.ToString(table.Rows[0]["CompanyName"]);
                    LabelCompanyNumber.Text = Convert.ToString(table.Rows[0]["CompanyNumber"]);
                    LabelCompanyPhone.Text = Convert.ToString(table.Rows[0]["CompanyPhone"]);
                    //LabelDeliverAddress.Text = Convert.ToString(table.Rows[0]["DeliverAddress"]);
                    LabelDeliverAddress.Text = Convert.ToString(table.Rows[0]["CompanyAddress"]);
                    TextBoxNote.Text = Convert.ToString(table.Rows[0]["Note"]);

                    // 依照公司類型決定 進貨/銷貨/出貨 選項
                    String guid = Convert.ToString(table.Rows[0]["CompanyType"]).ToUpper();
                    if (0 == customerGuid.CompareTo(guid))
                        RadioButtonListAccountType.DataSourceID = "SqlDataSourceAccountTypeCustomer";
                    else if (0 == manufactureGuid.CompareTo(guid))
                        RadioButtonListAccountType.DataSourceID = "SqlDataSourceAccountTypeManufacture";

                    RadioButtonListAccountType.DataBind();
                    #endregion
                }
            }
        }
        #endregion

        #region TitleTable 本身資料
        LabelSelfName.Text = Profile.PrincipalCompanyName;
        LabelSelfNumber.Text = Profile.PrincipalCompanyNumber;
        LabelSelfPhone.Text = Profile.PrincipalCompanyPhone;
        LabelSelfFax.Text = Profile.PrincipalCompanyFax;
        #endregion

        #region 其他變動資料
        RadioButtonListAccountType.SelectedIndex = 0;
        LabelAccountNumber.Text = NewAccountNumber();// 單據號碼

        LabelDate.Text = DateTime.Now.ToShortDateString();// 單據日期
        #endregion

        PanelTitle.Visible = true;
        PanelAccount.Visible = false;
        PanelControl.Visible = true;
        ButtonPrint.Visible = false;

        ListBoxInvoices.DataBind();// 列表單據

        #region Session 紀錄
        Session["CompanyId"] = ListBoxCompanies.SelectedValue;

        if (IsPostBack)
        {
            Session.Remove("AccountNumber");
            Session.Remove("AccountId");
        }
        #endregion
    }

    protected void ListBoxInvoices_DataBinding(object sender, EventArgs e)
    {
        ListBoxInvoices.DataSourceID = "SqlDataSourceInvoices";// 預設 - 未完成單據
    }

    protected void ListBoxInvoices_DataBound(object sender, EventArgs e)
    {
        PanelInvoices.Visible = false;

        if (ListBoxInvoices.Items.Count > 0)// 確認資料繫結後，顯示 ListBoxInvoices
        {
            PanelInvoices.Visible = true;

            if (ListBoxInvoices.Items.Count < 2)
                ListBoxInvoices.Rows = 2;
            else if (ListBoxInvoices.Items.Count < 16)
                ListBoxInvoices.Rows = ListBoxInvoices.Items.Count;
            else
                ListBoxInvoices.Rows = 16;
        }

        String accountId = Session["AccountId"] as String;
        String accountNumber = Session["AccountNumber"] as String;
        if (String.IsNullOrEmpty(accountId) || String.IsNullOrEmpty(accountNumber))
            return;

        foreach (ListItem item in ListBoxInvoices.Items)
        {
            if (!item.Value.Equals(accountId))
                continue;

            item.Selected = true;
            ListBoxInvoices_SelectedIndexChanged(null, null);
            break;
        }
    }

    protected void ListBoxInvoices_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        InitGridViewAccount();

        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        InitNote();

        #region 取得單據資料填入 GridViewAccount
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable recordTable = new DataTable();
            DataTable accountTable = new DataTable();

            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                try
                {
                    connection.Open();

                    using (adapter.SelectCommand = new SqlCommand(selectAccount, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                        adapter.Fill(accountTable);
                    }

                    using (adapter.SelectCommand = new SqlCommand(selectRecords, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);

                        adapter.Fill(recordTable);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            #region 處裡account資料
            if (accountTable.Rows.Count > 0)
            {
                DataRow row = accountTable.Rows[0];

                TextBoxNote.Text = row["Note"].ToString();// PanelTitel 顯示單據備註

                RadioButtonListAccountType.SelectedIndex = -1;// PanelAccount 顯示確認類型
                foreach (ListItem item in RadioButtonListAccountType.Items)
                {
                    if (0 == item.Value.CompareTo(Convert.ToString(row["AccountType"])))
                    {
                        item.Selected = true;
                        break;
                    }
                }

                LabelAccountNumber.Text = Convert.ToString(row["AccountNumber"]);// 單據編號
                TextBoxBillNumber.Text = Convert.ToString(row["BillNumber"]);// 發票號碼

                LabelDate.Text = Convert.ToDateTime(row["SaveDate"]).ToShortDateString();// 建單日期

                Double tax = Convert.ToDouble(row["Tax"]);
                //TextBoxTax.Text = String.Format("{0:0,0.0}", tax);// 稅金
                TextBoxTax.Text = String.Format("{0:N}", tax);// 稅金

                Double prePay = Convert.ToDouble(row["PrePay"]);
                //TextBoxPrePay.Text = String.Format("{0:0,0.0}", prePay);// 已付款
                TextBoxPrePay.Text = String.Format("{0:N}", prePay);// 已付款
            }
            #endregion

            #region 處裡record資料
            if (recordTable.Rows.Count <= 0)
            {
                LabelMessage.Text = "單據內無任何紀錄！";
                LabelMessage.Visible = true;
            }
            else
            {
                foreach (DataRow row in recordTable.Rows)
                {
                    String productGroup = row["ProductGroup"].ToString();
                    String productColor = row["ProductColor"].ToString();

                    AccountData accountData = null;//檢查對應 row 的 ProductGroup, ProductColor 是否已存在
                    for (Int32 i = 0; i < accountDataList.Count; i++)
                    {
                        if (0 == accountDataList[i].Group.CompareTo(productGroup) && 0 == accountDataList[i].Color.CompareTo(productColor))
                        {
                            accountData = accountDataList[i];
                            break;
                        }
                    }

                    if (null == accountData)
                    {
                        accountData = new AccountData();
                        accountData.Group = productGroup;
                        accountData.Color = productColor;
                        accountData.Price = Convert.ToDouble(row["Price"]);
                        accountDataList.Add(accountData);
                    }

                    #region 取得 Size 對應增加數量，並計算目前 Amount
                    switch (Convert.ToInt32(row["Score"]))
                    {
                        case 0: { accountData.Size0 += Convert.ToInt32(row["Quantity"]); break; }
                        case 1: { accountData.Size1 += Convert.ToInt32(row["Quantity"]); break; }
                        case 2: { accountData.Size2 += Convert.ToInt32(row["Quantity"]); break; }
                        case 3: { accountData.Size3 += Convert.ToInt32(row["Quantity"]); break; }
                        case 4: { accountData.Size4 += Convert.ToInt32(row["Quantity"]); break; }
                        case 5: { accountData.Size5 += Convert.ToInt32(row["Quantity"]); break; }
                        case 6: { accountData.Size6 += Convert.ToInt32(row["Quantity"]); break; }
                        case 7: { accountData.Size7 += Convert.ToInt32(row["Quantity"]); break; }
                        case 8: { accountData.Size8 += Convert.ToInt32(row["Quantity"]); break; }
                        case 9: { accountData.Size9 += Convert.ToInt32(row["Quantity"]); break; }
                        case 10: { accountData.Size10 += Convert.ToInt32(row["Quantity"]); break; }
                    }

                    accountData.Quantity = accountData.Size0;
                    accountData.Quantity += accountData.Size1;
                    accountData.Quantity += accountData.Size2;
                    accountData.Quantity += accountData.Size3;
                    accountData.Quantity += accountData.Size4;
                    accountData.Quantity += accountData.Size5;
                    accountData.Quantity += accountData.Size6;
                    accountData.Quantity += accountData.Size7;
                    accountData.Quantity += accountData.Size8;
                    accountData.Quantity += accountData.Size9;
                    accountData.Quantity += accountData.Size10;

                    accountData.Amount = accountData.Price * accountData.Quantity;

                    //TextBoxTax.Text = String.Format("{0:0,0.0}", tax);// 稅金
                    LabelTotalNum.Text = String.Format("{0}", accountData.Quantity);// 稅金
                    #endregion

                    #region accountDataList 對應到 dataTable
                    dataTable.Rows.Clear();
                    foreach (AccountData ad in accountDataList)
                        dataTable.Rows.Add(ad.ToArray());
                    #endregion

                    GridViewAccount.DataSource = dataTable;
                    GridViewAccount.DataBind();

                    for (Int32 i = 0; i < GridViewAccount.Rows.Count; i++)
                        SetProductIds(i);
                }
            }
            #endregion
        }
        #endregion

        PanelAccount.Visible = true;
        ButtonPrint.Visible = true;

        Sum(false);

        #region Session 紀錄
        Session["AccountNumber"] = ListBoxInvoices.SelectedItem.Text;
        Session["AccountId"] = ListBoxInvoices.SelectedValue;
        #endregion
    }

    protected void LinkButtonNote_Click(object sender, EventArgs e)
    {
        TextBoxNote.Visible = false;
        LinkButtonNote.Visible = false;

        DropDownListNote.Visible = true;
        LinkButtonNoteSelect.Visible = true;
        LinkButtonNoteCancel.Visible = true;

        #region 取得單據備註記錄
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            using (adapter.SelectCommand = new SqlCommand(selectInvoiceNotes, connection))
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

            DropDownListNote.DataSource = table;
            DropDownListNote.DataBind();

            DropDownListNote.Items.Insert(0, new ListItem(TextBoxNote.Text, Guid.Empty.ToString()));// 目前編輯內容插入最上端
        }
        #endregion
    }

    protected void LinkButtonNoteSelect_Click(object sender, EventArgs e)
    {
        TextBoxNote.Text = DropDownListNote.SelectedItem.Text;

        TextBoxNote.Visible = true;
        LinkButtonNote.Visible = true;

        DropDownListNote.Visible = false;
        LinkButtonNoteSelect.Visible = false;
        LinkButtonNoteCancel.Visible = false;
    }

    protected void LinkButtonNoteCancel_Click(object sender, EventArgs e)
    {
        TextBoxNote.Visible = true;
        LinkButtonNote.Visible = true;

        DropDownListNote.Visible = false;
        LinkButtonNoteSelect.Visible = false;
        LinkButtonNoteCancel.Visible = false;
    }

    protected void GridViewAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        #region 選擇 Group
        DropDownList group = (DropDownList)e.Row.Cells[0].Controls[1];
        if (null != group)
        {
            foreach (ListItem item in group.Items)
            {
                if (0 == group.ValidationGroup.CompareTo(item.Value))
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region 選擇 Color
        DropDownList color = (DropDownList)e.Row.Cells[1].Controls[1];
        if (null != color)
        {
            foreach (ListItem item in color.Items)
            {
                if (0 == color.ValidationGroup.CompareTo(item.Value))
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region 千位號檢查
        TextBox tb;
        for (int col = 2; col < 13; col++)
        {
            tb = (TextBox)e.Row.Cells[col].Controls[1];// 數量為 Int 型態
            tb.Text = String.Format("{0:N0}", Convert.ToInt32(tb.Text));
        }

        tb = (TextBox)e.Row.Cells[13].Controls[1];// 單價為 Double 型態
        tb.Text = String.Format("{0:N}", Convert.ToDouble(tb.Text));

        Label label = (Label)e.Row.Cells[14].Controls[1];// 數量為 Int 型態
        label.Text = String.Format("{0:N0}", Convert.ToInt32(label.Text));

        label = (Label)e.Row.Cells[15].Controls[1];// 合計為 Double 型態
        label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));
        #endregion

        if (0 == Convert.ToDouble(tb.Text))// 如果沒有單價，從資料庫內取最後紀錄
            LastPriceAndProductIds(e.Row);

        #region 刪除按鈕附加詢問訊息
        LinkButton lb = (LinkButton)e.Row.Cells[16].Controls[1];
        if (null != lb)
        {
            lb.Attributes.Add("onclick", "return confirm('確定刪除此列？')");
        }
        #endregion
    }

    protected void DropDownListRow_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = sender as DropDownList;
        Int32 row = list.TabIndex;
        GridViewAccount.SelectedIndex = row;

        LastPriceAndProductIds(row);
        Amount(row);
        Sum(true);
    }

    protected void ButtonClearAccount_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();
        else dataTable.Clear();

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = CreateAccountDataList();
        else accountDataList.Clear();

        InitTitle();
        InitSum();
        InitGridViewAccount();

        PanelAccount.Visible = false;
        ButtonPrint.Visible = false;

        ListBoxInvoices.SelectedIndex = -1;
    }

    protected void ButtonInsertAccount_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 寫入檢查
        if (-1 != ListBoxInvoices.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "禁止重複新增單據");
            return;
        }

        if (-1 == ListBoxCompanies.SelectedIndex)
        {
            Message.LabelError(LabelMessage, "未選取任何公司");
            return;
        }

        if (GridViewAccount.Rows.Count <= 0)
        {
            Message.LabelError(LabelMessage, "未建立單據內容");
            return;
        }
        #endregion

        UpdateData();// GridViewAccount 資料寫回 accountDataList

        List<AccountData> accountDataList = (List<AccountData>)Session["AccountDataList"];
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        if (accountDataList.Count < 0) return;

        if (DropDownListNote.Visible)// 確認備註內容 
            TextBoxNote.Text = DropDownListNote.SelectedItem.Text;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            Guid accountId = Guid.Empty;
            SqlTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                #region 建立單據 AccountId，並確認唯一性
                Int32 count = 1;
                while (count > 0)
                {
                    accountId = Guid.NewGuid();
                    using (SqlCommand command = new SqlCommand(countAccountId, connection, transaction))
                    {
                        command.Parameters.AddWithValue("AccountId", accountId);
                        count = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                #endregion

                #region 建立Account
                using (SqlCommand command = new SqlCommand(insertAccount, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("AccountId", accountId);
                    command.Parameters.AddWithValue("AccountType", RadioButtonListAccountType.SelectedValue);
                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                    command.Parameters.AddWithValue("AccountNumber", LabelAccountNumber.Text);
                    command.Parameters.AddWithValue("BillNumber", TextBoxBillNumber.Text);
                    command.Parameters.AddWithValue("SaveDate", Convert.ToDateTime(LabelDate.Text));
                    command.Parameters.AddWithValue("Tax", Convert.ToDecimal(TextBoxTax.Text));
                    command.Parameters.AddWithValue("PrePay", Convert.ToDecimal(TextBoxPrePay.Text));
                    command.Parameters.AddWithValue("Note", TextBoxNote.Text);

                    command.ExecuteNonQuery();
                }
                #endregion

                #region 建立Records
                using (SqlCommand command = new SqlCommand(insertRecord, connection, transaction))
                {
                    command.Parameters.AddWithValue("AccountId", accountId);
                    command.Parameters.AddWithValue("Price", 0);
                    command.Parameters.AddWithValue("ProductId", String.Empty);
                    command.Parameters.AddWithValue("Quantity", 0);
                    command.Parameters.AddWithValue("Row", 0);

                    Int32 row = 0;
                    foreach (AccountData ad in accountDataList)
                    {
                        command.Parameters["Price"].Value = ad.Price;
                        command.Parameters["Row"].Value = row;
                        row++;

                        if (ad.Size0 > 0)
                        {
                            command.Parameters["ProductId"].Value = ad.Product0;
                            command.Parameters["Quantity"].Value = ad.Size0;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size1 > 0 && ad.Product1 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product1;
                            command.Parameters["Quantity"].Value = ad.Size1;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size2 > 0 && ad.Product2 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product2;
                            command.Parameters["Quantity"].Value = ad.Size2;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size3 > 0 && ad.Product3 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product3;
                            command.Parameters["Quantity"].Value = ad.Size3;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size4 > 0 && ad.Product4 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product4;
                            command.Parameters["Quantity"].Value = ad.Size4;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size5 > 0 && ad.Product5 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product5;
                            command.Parameters["Quantity"].Value = ad.Size5;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size6 > 0 && ad.Product6 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product6;
                            command.Parameters["Quantity"].Value = ad.Size6;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size7 > 0 && ad.Product7 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product7;
                            command.Parameters["Quantity"].Value = ad.Size7;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size8 > 0 && ad.Product8 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product8;
                            command.Parameters["Quantity"].Value = ad.Size8;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size9 > 0 && ad.Product9 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product9;
                            command.Parameters["Quantity"].Value = ad.Size9;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size10 > 0 && ad.Product10 != Guid.Empty)
                        {
                            command.Parameters["ProductId"].Value = ad.Product10;
                            command.Parameters["Quantity"].Value = ad.Size10;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                #region 更新公司備註記錄
                using (SqlCommand command = new SqlCommand(updateCompanyNote, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                    command.Parameters.AddWithValue("Note", TextBoxNote.Text);

                    command.ExecuteNonQuery();
                }
                #endregion

                transaction.Commit();
                transaction = null;
            }
            catch (Exception exception)
            {
                Message.LabelError(LabelMessage, "新增單據失敗。");

                if (null != transaction)
                    transaction.Rollback();

                throw exception;
            }
            finally
            {
                connection.Close();
            }
        }

        ButtonPrint.Visible = false;

        ListBoxInvoices.DataBind();// 重新列表單據
        ListBoxInvoices.SelectedIndex = 0;// 選取第一筆 (新增單據在第一筆)

        ListBoxInvoices_SelectedIndexChanged(null, null);// 必須再讀取一次，否則螢幕會顯示 JavaScript 變更前資料
        Message.LabelMessage(LabelMessage, "新增單據完成。");
    }

    protected void ButtonUpdateAccount_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 更新檢查
        if (null == ListBoxInvoices.SelectedItem)
        {
            Message.LabelError(LabelMessage, "未選取任何單據");
            return;
        }
        #endregion

        UpdateData();// GridViewAccount 資料寫回 accountDataList

        List<AccountData> accountDataList = (List<AccountData>)Session["AccountDataList"];
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        if (accountDataList.Count < 0) return;

        if (DropDownListNote.Visible)// 確認備註內容 
            TextBoxNote.Text = DropDownListNote.SelectedItem.Text;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            SqlTransaction transaction = null;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                #region 取得原單據類型及出貨時間 (是否已出貨)
                String accountType = String.Empty;
                DateTime deliverDate = DateTime.MinValue;
                using (SqlCommand command = new SqlCommand(selectAccountType, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    accountType = Convert.ToString(command.ExecuteScalar());

                    command.CommandText = selectDeliverDate;
                    Object o = command.ExecuteScalar();
                    if (null != o && DBNull.Value != o)
                    {
                        try { deliverDate = Convert.ToDateTime(o); }
                        catch { }
                    }
                }
                #endregion

                #region 如果原單據已出貨，依照單據類型，Product 增加+(廠商退貨/客戶銷貨)或減少-(廠商進貨/客戶退貨)Records 紀錄的 Quantity
                if (DateTime.MinValue != deliverDate)
                {
                    using (adapter.SelectCommand = new SqlCommand(selectRecords, connection, transaction))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);

                        adapter.Fill(table);
                    }

                    using (SqlCommand command = new SqlCommand(String.Empty, connection, transaction))
                    {
                        if (0 == accountType.CompareTo(manufactureReturns) || 0 == accountType.CompareTo(customSales))
                            command.CommandText = inceaseProduct;
                        else
                            command.CommandText = deceaseProduct;

                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("ProductId", "");
                        command.Parameters.AddWithValue("Count", 0);

                        foreach (DataRow row in table.Rows)
                        {
                            command.Parameters["ProductId"].Value = row["ProductId"];
                            command.Parameters["Count"].Value = Convert.ToInt32(row["Quantity"]);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                #region 刪除Records
                using (SqlCommand command = new SqlCommand(deleteRecords, connection, transaction))
                {
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                #endregion

                #region 建立Records
                using (SqlCommand command = new SqlCommand(insertRecord, connection, transaction))
                {
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    command.Parameters.AddWithValue("Price", 0);
                    command.Parameters.AddWithValue("ProductId", String.Empty);
                    command.Parameters.AddWithValue("Quantity", 0);
                    command.Parameters.AddWithValue("Row", 0);

                    Int32 row = 0;
                    foreach (AccountData ad in accountDataList)
                    {
                        command.Parameters["Price"].Value = ad.Price;
                        command.Parameters["Row"].Value = row;
                        row++;

                        if (ad.Size0 > 0)
                        {
                            command.Parameters["ProductId"].Value = ad.Product0;
                            command.Parameters["Quantity"].Value = ad.Size0;
                            command.ExecuteNonQuery();
                        }

                        if (ad.Size1 > 0)
                        {
                            if (ad.Product1 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product1;
                                command.Parameters["Quantity"].Value = ad.Size1;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size2 > 0)
                        {
                            if (ad.Product2 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product2;
                                command.Parameters["Quantity"].Value = ad.Size2;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size3 > 0)
                        {
                            if (ad.Product3 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product3;
                                command.Parameters["Quantity"].Value = ad.Size3;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size4 > 0)
                        {
                            if (ad.Product4 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product4;
                                command.Parameters["Quantity"].Value = ad.Size4;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size5 > 0)
                        {
                            if (ad.Product5 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product5;
                                command.Parameters["Quantity"].Value = ad.Size5;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size6 > 0)
                        {
                            if (ad.Product6 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product6;
                                command.Parameters["Quantity"].Value = ad.Size6;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size7 > 0)
                        {
                            if (ad.Product7 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product7;
                                command.Parameters["Quantity"].Value = ad.Size7;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size8 > 0)
                        {
                            if (ad.Product8 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product8;
                                command.Parameters["Quantity"].Value = ad.Size8;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size9 > 0)
                        {
                            if (ad.Product9 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product9;
                                command.Parameters["Quantity"].Value = ad.Size9;
                                command.ExecuteNonQuery();
                            }
                        }

                        if (ad.Size10 > 0)
                        {
                            if (ad.Product10 != Guid.Empty)
                            {
                                command.Parameters["ProductId"].Value = ad.Product10;
                                command.Parameters["Quantity"].Value = ad.Size10;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                #endregion

                #region 如果原單據已送貨，依照單據類型，Product -(廠商退貨/客戶銷貨)或+(廠商進貨/客戶退貨) Records 紀錄的 Quantity
                if (DateTime.MinValue != deliverDate)
                {
                    using (SqlCommand command = new SqlCommand("", connection, transaction))
                    {
                        if (0 == accountType.CompareTo(manufactureReturns) || 0 == accountType.CompareTo(customSales))
                            command.CommandText = deceaseProduct;
                        else
                            command.CommandText = inceaseProduct;

                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("ProductId", "");
                        command.Parameters.AddWithValue("Count", 0);

                        foreach (AccountData ad in accountDataList)
                        {
                            if (ad.Size0 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product0;
                                command.Parameters["Count"].Value = ad.Size0;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size1 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product1;
                                command.Parameters["Count"].Value = ad.Size1;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size2 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product2;
                                command.Parameters["Count"].Value = ad.Size2;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size3 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product3;
                                command.Parameters["Count"].Value = ad.Size3;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size4 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product4;
                                command.Parameters["Count"].Value = ad.Size4;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size5 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product5;
                                command.Parameters["Count"].Value = ad.Size5;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size6 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product6;
                                command.Parameters["Count"].Value = ad.Size6;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size7 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product7;
                                command.Parameters["Count"].Value = ad.Size7;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size8 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product8;
                                command.Parameters["Count"].Value = ad.Size8;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size9 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product9;
                                command.Parameters["Count"].Value = ad.Size9;
                                command.ExecuteNonQuery();
                            }

                            if (ad.Size10 > 0)
                            {
                                command.Parameters["ProductId"].Value = ad.Product10;
                                command.Parameters["Count"].Value = ad.Size10;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                #endregion

                #region 更新 Account
                using (SqlCommand command = new SqlCommand(updateAccount, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    command.Parameters.AddWithValue("AccountType", RadioButtonListAccountType.SelectedValue);
                    command.Parameters.AddWithValue("AccountNumber", LabelAccountNumber.Text);
                    command.Parameters.AddWithValue("BillNumber", TextBoxBillNumber.Text);
                    command.Parameters.AddWithValue("Tax", Convert.ToDecimal(TextBoxTax.Text));
                    command.Parameters.AddWithValue("PrePay", Convert.ToDecimal(TextBoxPrePay.Text));
                    command.Parameters.AddWithValue("Note", TextBoxNote.Text);

                    command.ExecuteNonQuery();
                }
                #endregion

                #region 更新公司備註記錄
                using (SqlCommand command = new SqlCommand(updateCompanyNote, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);
                    command.Parameters.AddWithValue("Note", TextBoxNote.Text);

                    command.ExecuteNonQuery();
                }
                #endregion

                transaction.Commit();
                transaction = null;
            }
            catch (Exception exception)
            {
                Message.LabelError(LabelMessage, "更新單據失敗！");

                if (null != transaction)
                    transaction.Rollback();

                throw exception;
            }
            finally
            {
                connection.Close();
            }
        }

        ListBoxInvoices_SelectedIndexChanged(null, null);// 必須再讀取一次，否則螢幕會顯示 JavaScript 變更前資料
        Message.LabelMessage(LabelMessage, "更新單據完成。");
    }

    protected void ButtonDeleteAccount_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        #region 更新檢查
        if (null == ListBoxInvoices.SelectedItem)
        {
            Message.LabelError(LabelMessage, "未選取任何單據");
            return;
        }
        #endregion

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            SqlTransaction transaction = null;
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                #region 取得原單據類型及出貨時間 (是否已出貨)
                String accountType = String.Empty;
                DateTime deliverDate = DateTime.MinValue;
                using (SqlCommand command = new SqlCommand(selectAccountType, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    accountType = Convert.ToString(command.ExecuteScalar());

                    command.CommandText = selectDeliverDate;
                    Object o = command.ExecuteScalar();
                    if (null != o && DBNull.Value != o)
                    {
                        try { deliverDate = Convert.ToDateTime(o); }
                        catch { }
                    }
                }
                #endregion

                #region 如果單據已送貨，依照單據類型，Product 增加+(廠商退貨/客戶銷貨)或減少-(廠商進貨/客戶退貨) Records 紀錄的 Quantity
                if (DateTime.MinValue != deliverDate)
                {
                    using (adapter.SelectCommand = new SqlCommand(selectRecords, connection, transaction))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);

                        adapter.Fill(table);
                    }

                    using (SqlCommand command = new SqlCommand("", connection, transaction))
                    {
                        if (0 == accountType.CompareTo(manufactureReturns) || 0 == accountType.CompareTo(customSales))
                            command.CommandText = inceaseProduct;
                        else
                            command.CommandText = deceaseProduct;

                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("ProductId", "");
                        command.Parameters.AddWithValue("Count", 0);

                        foreach (DataRow row in table.Rows)
                        {
                            command.Parameters["ProductId"].Value = row["Productid"];
                            command.Parameters["Count"].Value = Convert.ToInt32(row["Quantity"]);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                #region 刪除Records
                using (SqlCommand command = new SqlCommand(deleteRecords, connection, transaction))
                {
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                #endregion

                #region 刪除Account
                using (SqlCommand command = new SqlCommand(deleteAccount, connection, transaction))
                {
                    command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    command.Parameters.AddWithValue("AccountId", ListBoxInvoices.SelectedValue);
                    command.Parameters.AddWithValue("DeleteDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                #endregion

                transaction.Commit();
                transaction = null;

                Message.LabelMessage(LabelMessage, "刪除單據完成。");
            }
            catch (Exception exception)
            {
                Message.LabelError(LabelMessage, "刪除單據失敗！");

                if (null != transaction)
                    transaction.Rollback();

                throw exception;
            }
            finally
            {
                connection.Close();
            }
        }

        ListBoxInvoices.DataBind();

        InitTitle();
        InitSum();
        InitGridViewAccount();

        PanelAccount.Visible = false;
        ButtonPrint.Visible = false;
    }

    protected void ButtonAppendRow_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        if (GridViewAccount.Rows.Count > 0)
            UpdateData();// 目前 GridViewAccount 有資料，更新 dataTable 及 accountDataList

        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        AccountData emptyAD = new AccountData();//新增一列
        accountDataList.Add(emptyAD);
        dataTable.Rows.Add(emptyAD.ToArray());

        GridViewAccount.DataSource = dataTable;
        GridViewAccount.DataBind();

        LastPriceAndProductIds(GridViewAccount.Rows.Count - 1);

        Sum(true);
        PanelAccount.Visible = true;
    }

    protected void LinkButtonDeleteRow_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        LinkButton lb = (LinkButton)sender;
        GridViewAccount.SelectedIndex = lb.TabIndex;

        if (GridViewAccount.SelectedIndex == -1)
        {
            Message.LabelError(LabelMessage, "未選取任何資料列");
            return;
        }

        DataTable dataTable = Session["AccountDataTable"] as DataTable;
        if (null == dataTable) dataTable = CreateDataTable();

        List<AccountData> accountDataList = Session["AccountDataList"] as List<AccountData>;
        if (null == accountDataList) accountDataList = CreateAccountDataList();

        dataTable.Rows.RemoveAt(GridViewAccount.SelectedIndex);
        accountDataList.RemoveAt(GridViewAccount.SelectedIndex);

        GridViewAccount.DataSource = dataTable;
        GridViewAccount.DataBind();

        if (GridViewAccount.Rows.Count <= 0)
        {
            InitSum();
            InitGridViewAccount();
            PanelAccount.Visible = false;
        }
        else Sum(true);
    }

    protected void ButtonPrint_Click(object sender, EventArgs e)
    {
        #region CompanyData
        CompanyData companyData = new CompanyData();
        companyData.AccountNumber = LabelAccountNumber.Text;
        companyData.AccountType = RadioButtonListAccountType.SelectedItem.Text;
        companyData.BillNumber = TextBoxBillNumber.Text;
        companyData.CompanyName = LabelCompanyName.Text;
        companyData.CompanyNumber = LabelCompanyNumber.Text;
        companyData.CompanyPhone = LabelCompanyPhone.Text;
        companyData.Date = LabelDate.Text;
        companyData.DeliverAddress = LabelDeliverAddress.Text;
        companyData.PayDate = LabelPayDate.Text;
        companyData.SelfFax = LabelSelfFax.Text;
        companyData.SelfName = LabelSelfName.Text;
        companyData.SelfNumber = LabelSelfNumber.Text;
        companyData.SelfPhone = LabelSelfPhone.Text;
        companyData.Note = TextBoxNote.Text;
        #endregion

        #region RecordData
        List<String[]> recordDataList = new List<String[]>();
        foreach (GridViewRow row in GridViewAccount.Rows)
        {
            String[] recordData = new String[16];

            DropDownList ddl = (DropDownList)row.Cells[0].Controls[1];
            recordData[0] = ddl.SelectedItem.Text;// Group

            ddl = (DropDownList)row.Cells[1].Controls[1];
            recordData[1] = ddl.SelectedItem.Text;// Color

            TextBox tb;// Size
            for (Int32 i = 2; i < 13; i++)
            {
                tb = (TextBox)row.Cells[i].Controls[1];
                if (String.IsNullOrWhiteSpace(tb.Text) || 0 == Convert.ToInt32(tb.Text))
                    recordData[i] = String.Empty;
                else
                    recordData[i] = tb.Text;
            }

            tb = (TextBox)row.Cells[13].Controls[1];
            recordData[13] = tb.Text;// Price

            Label label = (Label)row.Cells[14].Controls[1];
            recordData[14] = label.Text;// Quantity

            label = (Label)row.Cells[15].Controls[1];
            recordData[15] = label.Text;// Amount

            recordDataList.Add(recordData);
        }
        #endregion

        #region AmountData
        AmountData amountData = new AmountData();
        amountData.Num = LabelTotalNum.Text;
        amountData.Sum = LabelSum.Text;
        amountData.LeftPay = LabelLeftPay.Text;
        amountData.PrePay = TextBoxPrePay.Text;
        amountData.Tax = TextBoxTax.Text;
        amountData.Total = LabelTotal.Text;
        #endregion

        InvoiceData invoiceData = new InvoiceData();
        invoiceData.amountData = amountData;
        invoiceData.companyData = companyData;
        invoiceData.recordDataList = recordDataList;
        Session["InvoiceData"] = invoiceData;// 暫存列印資料

        String script = "window.open('PrintInvoice.aspx');";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PrintInvoice", script, true);
    }
}