using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using System.Globalization;

public partial class Member_Report : System.Web.UI.Page
{
    #region SQL command
    protected static String selectAccountsNotPayed = "SELECT account.AccountId FROM aspnet_Account account, aspnet_Company company, aspnet_Record record WHERE (account.OwnerId = @OwnerId) AND (account.DeleteDate IS NULL) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (company.CompanyId = account.CompanyId) AND (record.AccountId = account.AccountId) GROUP BY account.AccountId, account.Tax, account.PrePay, company.CompanyName, account.SaveDate HAVING (SUM(record.Price * record.Quantity) + account.Tax <> account.PrePay) ORDER BY company.CompanyName, account.SaveDate";
    protected static String selectAccountsRange = "SELECT account.AccountId FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.DeleteDate IS NULL) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (company.CompanyId = account.CompanyId) ORDER BY company.CompanyName, account.SaveDate";

    protected static String selectAccountsAll = "SELECT account.AccountId FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.DeleteDate IS NULL) AND (company.CompanyId = account.CompanyId) ORDER BY company.CompanyName, account.SaveDate";

    protected static String selectCompany = "SELECT * FROM aspnet_Company WHERE (OwnerId = OwnerId) AND (CompanyId = @CompanyId)";
    protected static String selectAccountsByCompany = "SELECT AccountType, SaveDate, AccountNumber, Tax, PrePay, AccountId FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (DeleteDate IS NULL) AND (SaveDate >= @StartDate) AND (SaveDate < @EndDate) ORDER BY SaveDate";
    protected static String selectAccounts = "SELECT AccountType, SaveDate, AccountNumber, Tax, PrePay, AccountId FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId) AND (SaveDate >= @StartDate) AND (SaveDate < @EndDate) AND (DeleteDate IS NULL) ORDER BY SaveDate";
    protected static String selectRecords = "SELECT Price, Quantity FROM aspnet_Record WHERE (AccountId = @AccountId) AND (DeleteDate IS NULL)";

    protected static String selectCompanyFromAccount = "SELECT company.CompanyId, company.CompanyName FROM aspnet_Company company, aspnet_Account account WHERE (account.OwnerId = @OwnerId) AND (account.AccountId = @AccountId) AND (company.OwnerId = @OwnerId) AND (company.CompanyId = account.CompanyId)";
    protected static String selectCompanyFromAccountTypeChecked = "SELECT company.CompanyId, company.CompanyName FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = company.CompanyId) AND (account.CheckDate IS NOT NULL) AND (account.AccountType = @AccountType1 OR account.AccountType = @AccountType2) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (account.DeleteDate IS NULL) GROUP BY company.CompanyId, company.CompanyName ORDER BY company.CompanyName, account.SaveDate";
    protected static String selectCompanyFromAccountTypeNotChecked = "SELECT company.CompanyId, company.CompanyName FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = company.CompanyId) AND (account.CheckDate IS NULL) AND (account.AccountType = @AccountType1 OR account.AccountType = @AccountType2) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (account.DeleteDate IS NULL) GROUP BY company.CompanyId, company.CompanyName ORDER BY company.CompanyName, account.SaveDate";
    protected static String selectReportRowSum = "SELECT SUM(record.Price * record.Quantity) + SUM(account.Tax) FROM aspnet_Account account, aspnet_Record record WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (account.AccountType = @AccountType) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (account.DeleteDate IS NULL) AND (record.AccountId = account.AccountId) AND (record.DeleteDate IS NULL)";

    //protected static String selectReportSingle = "SELECT account.AccountId, account.SaveDate, company.CompanyName, account.Tax, account.PrePay FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (account.DeleteDate IS NULL) AND (company.CompanyId = @CompanyId) AND (company.OwnerId = @OwnerId) ORDER BY account.SaveDate";
    protected static String selectReportSingle = "SELECT account.AccountId, account.SaveDate, company.CompanyName, account.Tax, account.PrePay FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.CompanyId = @CompanyId) AND (account.DeleteDate IS NULL) AND (company.CompanyId = @CompanyId) AND (company.OwnerId = @OwnerId) ORDER BY account.SaveDate DESC";

    //protected static String selectReportAccountsNotPayed = "SELECT account.AccountId, account.SaveDate, company.CompanyName, account.Tax, account.PrePay FROM aspnet_Company company, aspnet_Account account, aspnet_Record record WHERE (account.OwnerId = @OwnerId) AND (account.DeleteDate IS NULL) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (record.AccountId = account.AccountId) AND (company.CompanyId = account.CompanyId) AND (company.OwnerId = @OwnerId) GROUP BY account.AccountId, account.SaveDate, company.CompanyName, account.Tax, account.PrePay HAVING (SUM(record.Price * record.Quantity) + account.Tax <> account.PrePay) ORDER BY company.CompanyName, account.SaveDate";
    protected static String selectReportAccountsNotPayed = "SELECT account.AccountId, account.SaveDate, company.CompanyName, account.BillNumber, account.Tax, account.PrePay FROM aspnet_Company company, aspnet_Account account, aspnet_Record record WHERE (account.OwnerId = @OwnerId) AND (account.DeleteDate IS NULL) AND (record.DeleteDate IS NULL) AND (record.AccountId = account.AccountId) AND (company.CompanyId = account.CompanyId) AND (company.OwnerId = @OwnerId) GROUP BY account.AccountId, account.SaveDate, company.CompanyName, account.BillNumber, account.Tax, account.PrePay HAVING (SUM(record.Price * record.Quantity) + account.Tax <> account.PrePay) ORDER BY company.CompanyName, account.SaveDate DESC";
    protected static String selectReportAccounts = "SELECT account.AccountId, account.SaveDate, company.CompanyName, account.BillNumber, account.Tax, account.PrePay FROM aspnet_Account account, aspnet_Company company WHERE (account.OwnerId = @OwnerId) AND (account.SaveDate >= @StartDate) AND (account.SaveDate < @EndDate) AND (account.DeleteDate IS NULL) AND (company.CompanyId = account.CompanyId) AND (company.OwnerId = @OwnerId) ORDER BY company.CompanyName, account.SaveDate";
    protected static String selectReportRecords = "SELECT pg.TypeName, record.Quantity, record.Price, product.ProductCosts FROM aspnet_Record record, aspnet_Product product, aspnet_ProductGroup pg WHERE (record.AccountId = @AccountId) AND (record.DeleteDate IS NULL) AND (product.ProductId = record.ProductId) AND (product.OwnerId = @OwnerId) AND (pg.TypeId = product.ProductGroup) AND (pg.OwnerId = @OwnerId) ORDER BY record.Row";
    #endregion

    #region 單據類型 GUID
    protected static String customSales = "FDD998A1-D477-484B-B0EE-6D0DE75EE166";
    protected static String customReturns = "5E5A5BE7-46C5-4D2B-ABD8-491330921243";
    protected static String manufactureSales = "A6FC3F19-C524-4BA1-A3C0-8F180D8093C5";
    protected static String manufactureReturns = "26239577-49DF-44F9-AEBD-729870F0744F";
    #endregion

    #region 公司類型 GUID
    protected static String companySelf = "DE9DFF94-D08E-44DA-A21D-991C8D3133F9";
    protected static String companyFirm = "AF639CBC-6B81-43EC-B0EB-F8CC0F96CB79";
    protected static String companyClient = "80951888-8C05-47DD-BC4D-B87DE0BDDB74";
    #endregion

    #region 公司類型 GUID
    protected static String customerGuid = "80951888-8C05-47DD-BC4D-B87DE0BDDB74";
    protected static String manufactureGuid = "AF639CBC-6B81-43EC-B0EB-F8CC0F96CB79";
    #endregion

    /// <summary>
    /// 顯示民國年日期字串
    /// </summary>
    /// <param name="date">西元年日期</param>
    /// <param name="format">民國年字串格式</param>
    /// <returns>民國年字串</returns>
    protected String GetTaiwanCalendar(DateTime date, String format)
    {
        CultureInfo culture = new CultureInfo("zh-TW");
        culture.DateTimeFormat.Calendar = new TaiwanCalendar();

        return date.ToString(format, culture);
    }

    /// <summary>
    /// 選單選擇後顯示
    /// </summary>
    protected void MenuClick()
    {
        LabelMessage.Visible = false;
        ListBoxReport.SelectedIndex = -1;
        ListBoxCompanies.SelectedIndex = -1;

        PanelAccount.Visible = false;
        PanelReport.Visible = false;
        switch (MenuReport.SelectedValue)
        {
            case "account":
                {
                    CalendarStart.Visible = true;
                    CalendarEnd.Visible = true;

                    ButtonPrint.Visible = false;
                    PanelControl.Visible = true;
                    PanelListBox.Visible = false;
                    PanelSearch.Visible = false;
                    ButtonReport.Visible = true;
                    ButtonReport.Text = "建立對帳單";

                    Session["ReportMenu"] = "Account";
                    break;
                }

            case "list":
                {
                    CalendarStart.Visible = false;
                    CalendarEnd.Visible = false;

                    ButtonPrint.Visible = false;
                    PanelControl.Visible = false;
                    PanelListBox.Visible = false;

                    PanelSearch.Visible = true;
                    ListBoxCompanies.Visible = false;

                    #region 檢查是否自動搜尋公司
                    String companyId = Session["CompanyId"] as String;
                    if (!String.IsNullOrEmpty(companyId))
                    {
                        TextBoxSearch.Text = Session["CompanyName"] as String;
                        ListBoxCompanies.DataBind();

                        String menu = Session["CompanyMenu"] as String;// 先取得選單紀錄

                        foreach (ListItem item in ListBoxCompanies.Items)
                        {
                            if (0 == companyId.CompareTo(item.Value))
                            {
                                item.Selected = true;
                                ListBoxCompanies_SelectedIndexChanged(null, null);

                                LabelMessage.Visible = false;
                                PanelDate.Visible = false;
                                CreateReport(selectReportSingle);
                                PanelReport.Visible = true;

                                break;
                            }
                        }
                    }
                    #endregion

                    //PanelReport.Visible = true;
                    //UpdatePanelReport.Update();

                    Session["ReportMenu"] = "List";
                    break;
                }

            case "debt":
                {
                    CalendarStart.Visible = false;
                    CalendarEnd.Visible = false;

                    ButtonPrint.Visible = false;
                    PanelControl.Visible = true;
                    PanelListBox.Visible = false;
                    ButtonReport.Visible = true;
                    ButtonReport.Text = "開始";

                    PanelSearch.Visible = false;
                    //PanelAccount.Visible = true;
                    //Message.LabelMessage(LabelMessage, "此報表會耗費較長時間。按下[開始]後開始建立並列印報表。");

                    Session["ReportMenu"] = "Debt";
                    Session.Remove("ReportCompanyId");

                    Session["ReportMenu"] = "Debt";
                    break;
                }

            case "report":
                {
                    CalendarStart.Visible = true;
                    CalendarEnd.Visible = true;

                    ButtonPrint.Visible = false;
                    PanelControl.Visible = true;
                    PanelListBox.Visible = false;
                    PanelSearch.Visible = false;
                    ButtonReport.Text = "列印報表";

                    Session["ReportMenu"] = "Report";
                    break;
                }
        }
    }

    /// <summary>
    /// 交易 廠商/客戶 列表
    /// </summary>
    /// <param name="sqlcommand">指定資料庫查詢指令</param>
    protected void ListAccounts(String sqlcommand)
    {
        ListBoxReport.Items.Clear();

        Int32 i;
        Boolean exists;
        String name, id;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable tableAccounts = new DataTable();
                DataTable tableCompany = new DataTable();

                using (adapter.SelectCommand = new SqlCommand(sqlcommand, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                    adapter.SelectCommand.Parameters.AddWithValue("StartDate", CalendarStart.SelectedDate);
                    adapter.SelectCommand.Parameters.AddWithValue("EndDate", CalendarEnd.SelectedDate.AddDays(1));

                    try
                    {
                        connection.Open();
                        adapter.Fill(tableAccounts);

                        adapter.SelectCommand.CommandText = selectCompanyFromAccount;
                        adapter.SelectCommand.Parameters.Clear();
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", String.Empty);

                        foreach (DataRow row in tableAccounts.Rows)
                        {
                            adapter.SelectCommand.Parameters["AccountId"].Value = row["AccountId"];

                            tableCompany.Reset();
                            adapter.Fill(tableCompany);

                            exists = false;
                            for (i = 0; i < ListBoxReport.Items.Count; i++)
                            {
                                if (0 == ListBoxReport.Items[i].Value.CompareTo(Convert.ToString(tableCompany.Rows[0]["CompanyId"])))
                                {
                                    exists = true;
                                    break;
                                }
                            }

                            if (!exists)
                            {
                                name = Convert.ToString(tableCompany.Rows[0]["CompanyName"]);
                                id = Convert.ToString(tableCompany.Rows[0]["CompanyId"]);
                                ListBoxReport.Items.Add(new ListItem(name, id));
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                if (ListBoxReport.Items.Count > 0)
                {
                    if (ListBoxReport.Items.Count < 2)
                        ListBoxReport.Rows = 2;
                    else if (ListBoxReport.Items.Count < 16)
                        ListBoxReport.Rows = ListBoxReport.Items.Count;
                    else
                        ListBoxReport.Rows = 16;

                    PanelListBox.Visible = true;
                }
                else Message.LabelError(LabelMessage, "查詢無任何資料");
            }
        }
    }

    /// <summary>
    /// 顯示交易資料
    /// </summary>
    /// <param name="full">False=未付款單據、True=所有單據</param>
    protected void ShowAccount(Boolean full)
    {
        LabelMessage.Visible = false;
        ButtonPrint.Visible = true;

        String sqlcommand = selectAccountsByCompany;
        LabelAccount.Text = "未付款對帳單";
        if (full)
        {
            GridViewAccount.Columns[1].Visible = true;
            GridViewAccount.Columns[3].Visible = true;
            GridViewAccount.Columns[4].Visible = true;
            GridViewAccount.Columns[6].Visible = true;

            LabelAccountQuantity.Visible = true;
            LabelAccountSum.Visible = true;
            LabelAccountTax.Visible = true;
            LabelAccountTotal.Visible = true;
            LabelAccountPrePay.Visible = true;

            sqlcommand = selectAccounts;
            LabelAccount.Text = "銷售對帳單";
        }
        else
        {
            GridViewAccount.Columns[1].Visible = false;
            GridViewAccount.Columns[3].Visible = false;
            GridViewAccount.Columns[4].Visible = false;
            GridViewAccount.Columns[6].Visible = false;

            LabelAccountQuantity.Visible = false;
            LabelAccountSum.Visible = false;
            LabelAccountTax.Visible = false;
            LabelAccountTotal.Visible = false;
            LabelAccountPrePay.Visible = false;
        }

        #region 取得客戶單據
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                DataTable tableCompany = new DataTable();
                DataTable tableAccount = new DataTable();

                try
                {
                    connection.Open();

                    #region 取得客戶資料
                    using (adapter.SelectCommand = new SqlCommand(selectCompany, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxReport.SelectedValue);

                        adapter.Fill(tableCompany);
                    }
                    #endregion

                    #region 取得單據資料
                    using (adapter.SelectCommand = new SqlCommand(sqlcommand, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxReport.SelectedValue);
                        adapter.SelectCommand.Parameters.AddWithValue("StartDate", CalendarStart.SelectedDate);
                        adapter.SelectCommand.Parameters.AddWithValue("EndDate", CalendarEnd.SelectedDate.AddDays(1));

                        adapter.Fill(tableAccount);
                    }
                    #endregion
                }
                finally
                {
                    connection.Close();
                }

                #region 填入報表資料
                if (0 != tableCompany.Rows.Count && 0 != tableAccount.Rows.Count)
                {
                    PanelAccount.Visible = true;

                    LabelAccountSelfName.Text = Profile.PrincipalCompanyName;
                    LabelAccountSelfPhone.Text = Profile.PrincipalCompanyPhone;
                    LabelAccountSelfFax.Text = Profile.PrincipalCompanyFax;

                    LabelAccountStart.Text = CalendarStart.SelectedDate.ToShortDateString();
                    LabelAccountEnd.Text = CalendarEnd.SelectedDate.ToShortDateString();

                    DataRow row = tableCompany.Rows[0];
                    LabelAccountCompanyName.Text = Convert.ToString(row["CompanyName"]);
                    LabelAccountCompanyPhone.Text = Convert.ToString(row["CompanyPhone"]);
                    LabelAccountCompanyAddress.Text = Convert.ToString(row["CompanyAddress"]);

                    Int32 length = Encoding.GetEncoding("Big5").GetByteCount(LabelAccountCompanyAddress.Text);

                    #region 建立繫結至 GridViewAccount 的 Table
                    DataTable tableReport = new DataTable();
                    tableReport.Columns.Add("AccountId");
                    tableReport.Columns.Add("AccountType");
                    tableReport.Columns.Add("SaveDate");
                    tableReport.Columns.Add("AccountNumber");
                    tableReport.Columns.Add("Quantity");
                    tableReport.Columns.Add("Sum");
                    tableReport.Columns.Add("Tax");
                    tableReport.Columns.Add("Total");
                    tableReport.Columns.Add("PrePay");
                    tableReport.Columns.Add("LeftPay");
                    #endregion

                    #region 列表每一張單據的資料
                    using (adapter.SelectCommand = new SqlCommand(selectRecords, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("AccountId", String.Empty);

                        DataRow dr;
                        Double sum, tax, quantity, prepay, q;
                        DataTable tableRecord = new DataTable();

                        try
                        {
                            connection.Open();

                            foreach (DataRow r in tableAccount.Rows)
                            {
                                tableRecord.Reset();
                                adapter.SelectCommand.Parameters["AccountId"].Value = r["AccountId"];
                                adapter.Fill(tableRecord);

                                sum = quantity = 0;
                                foreach (DataRow r2 in tableRecord.Rows)
                                {
                                    q = Convert.ToInt32(r2["Quantity"]);
                                    quantity += q;

                                    sum += Convert.ToDouble(r2["Price"]) * q;
                                }

                                tax = Convert.ToDouble(r["Tax"]);
                                prepay = Convert.ToDouble(r["PrePay"]);

                                // 沒有未付款，如果只需要列出檢查未付款，則繼續檢查下一筆
                                if (prepay == sum + tax && !full)
                                    continue;

                                #region 新增一列對帳單資料
                                dr = tableReport.NewRow();

                                dr["AccountId"] = r["AccountId"];
                                dr["AccountType"] = r["AccountType"];
                                dr["SaveDate"] = r["SaveDate"];
                                dr["AccountNumber"] = r["AccountNumber"];
                                dr["Tax"] = r["Tax"];
                                dr["PrePay"] = r["PrePay"];

                                dr["Sum"] = sum;
                                dr["Quantity"] = quantity;
                                dr["Total"] = sum + tax;

                                dr["LeftPay"] = sum + tax - prepay;

                                tableReport.Rows.Add(dr);
                                #endregion
                            }
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                    GridViewAccount.DataSource = tableReport;
                    GridViewAccount.DataBind();
                    #endregion
                }
                #endregion
            }
        }
        #endregion

        Session["PrintObject"] = PanelAccount;// 暫存列印表
    }

    /// <summary>
    /// 顯示報表
    /// </summary>
    /// <param name="sqlcommand">指定資料庫查詢指令</param>
    protected void CreateReport(String sqlcommand)
    {
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                #region 定義繫結至 GridViewReport 的 Table 欄位
                DataTable tableReport = new DataTable();
                tableReport.Columns.Add("SaveDate");
                tableReport.Columns.Add("CompanyName");
                tableReport.Columns.Add("InvoiceNumber");
                tableReport.Columns.Add("ProductName");
                tableReport.Columns.Add("Quantity");
                tableReport.Columns.Add("Price");
                tableReport.Columns.Add("Sum");
                tableReport.Columns.Add("Costs");
                tableReport.Columns.Add("CostsSum");
                tableReport.Columns.Add("Profit");
                tableReport.Columns.Add("Total");
                tableReport.Columns.Add("PrePay");
                tableReport.Columns.Add("LeftPay");
                #endregion

                DataRow dr;
                Int32 quantity = 0, index = 0, revised;
                String product_name, company_name, save_date, invoice_number;
                Double total, price, costs, sum = 0, csum = 0, prepay;

                DataTable tableAccounts = new DataTable();
                DataTable tableRecords = new DataTable();

                if (MenuReport.SelectedValue == "debt")
                {                
                    try
                    {
                        connection.Open();

                        #region 取得所有單據資料
                        using (adapter.SelectCommand = new SqlCommand(sqlcommand, connection))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                            adapter.SelectCommand.Parameters.AddWithValue("StartDate", CalendarStart.SelectedDate);
                            adapter.SelectCommand.Parameters.AddWithValue("EndDate", CalendarEnd.SelectedDate.AddDays(1));

                            if (-1 != ListBoxReport.SelectedIndex)
                                adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxReport.SelectedValue);

                            if (-1 != ListBoxCompanies.SelectedIndex)
                                adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                            adapter.Fill(tableAccounts);
                        }
                        #endregion

                        #region 取得所有單據內紀錄後建立報表資料
                        index = 0;
                        foreach (DataRow row in tableAccounts.Rows)
                        {
                            //save_date = Convert.ToDateTime(row["SaveDate"]).ToShortDateString();
                            save_date = GetTaiwanCalendar(Convert.ToDateTime(row["SaveDate"]), "yyy/MM/dd");
                            company_name = Convert.ToString(row["CompanyName"]);
                            invoice_number = Convert.ToString(row["BillNumber"]);

                            #region 檢查日期及公司名稱是否已存在
                            // 目前不實作
                            #endregion

                            total = prepay = 0;
                            using (adapter.SelectCommand = new SqlCommand(selectReportRecords, connection))
                            {
                                tableRecords.Reset();
                                adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                adapter.SelectCommand.Parameters.AddWithValue("AccountId", row["AccountId"]);
                                adapter.Fill(tableRecords);

                                sum = quantity = 0;
                                csum = 0;
                                index = 0;
                                foreach (DataRow row2 in tableRecords.Rows)
                                {
                                    price = Convert.ToDouble(row2["Price"]);
                                    quantity += Convert.ToInt32(row2["Quantity"]);
                                    sum += price * Convert.ToInt32(row2["Quantity"]);

                                    costs = Convert.ToDouble(row2["ProductCosts"]);
                                    csum += costs * Convert.ToInt32(row2["Quantity"]);

                                    index += 1;
                                    if (index == 1)// 第一列
                                    {
                                        total = Convert.ToDouble(row["Tax"]);
                                        prepay = Convert.ToDouble(row["PrePay"]);

                                        dr = tableReport.NewRow();

                                        dr["SaveDate"] = save_date;
                                        dr["CompanyName"] = company_name;
                                        dr["InvoiceNumber"] = invoice_number;
                                        dr["Price"] = String.Format("{0:N0}", price);
                                        dr["Quantity"] = Convert.ToString(quantity);
                                        dr["Sum"] = String.Format("{0:N0}", sum);
                                        dr["Costs"] = String.Format("{0:N0}", costs);
                                        dr["CostsSum"] = String.Format("{0:N0}", csum);
                                        dr["Profit"] = String.Format("{0:N0}", sum - csum);

                                        tableReport.Rows.Add(dr);
                                    }
                                }
                                total += sum; //最後加總完金額再把稅加上去

                                index = tableReport.Rows.Count - 1;

                                tableReport.Rows[index]["Quantity"] = Convert.ToString(quantity);
                                tableReport.Rows[index]["Sum"] = String.Format("{0:N0}", sum);
                                tableReport.Rows[index]["CostsSum"] = String.Format("{0:N0}", csum);
                                tableReport.Rows[index]["Total"] = String.Format("{0:N0}", total);
                                tableReport.Rows[index]["PrePay"] = String.Format("{0:N0}", prepay);
                                tableReport.Rows[index]["LeftPay"] = String.Format("{0:N0}", total - prepay);
                            }
                        }
                        #endregion
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    try
                    {
                        connection.Open();

                        #region 取得所有單據資料
                        using (adapter.SelectCommand = new SqlCommand(sqlcommand, connection))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                            adapter.SelectCommand.Parameters.AddWithValue("StartDate", CalendarStart.SelectedDate);
                            adapter.SelectCommand.Parameters.AddWithValue("EndDate", CalendarEnd.SelectedDate.AddDays(1));

                            if (-1 != ListBoxReport.SelectedIndex)
                                adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxReport.SelectedValue);

                            if (-1 != ListBoxCompanies.SelectedIndex)
                                adapter.SelectCommand.Parameters.AddWithValue("CompanyId", ListBoxCompanies.SelectedValue);

                            adapter.Fill(tableAccounts);
                        }
                        #endregion

                        #region 取得所有單據內紀錄後建立報表資料
                        index = 0;
                        foreach (DataRow row in tableAccounts.Rows)
                        {
                            //save_date = Convert.ToDateTime(row["SaveDate"]).ToShortDateString();
                            save_date = GetTaiwanCalendar(Convert.ToDateTime(row["SaveDate"]), "yyy/MM/dd");
                            company_name = Convert.ToString(row["CompanyName"]);
                            //invoice_number = Convert.ToString(row["BillNumber"]);

                            #region 檢查日期及公司名稱是否已存在
                            // 目前不實作
                            #endregion

                            total = prepay = 0;
                            using (adapter.SelectCommand = new SqlCommand(selectReportRecords, connection))
                            {
                                tableRecords.Reset();
                                adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                                adapter.SelectCommand.Parameters.AddWithValue("AccountId", row["AccountId"]);
                                adapter.Fill(tableRecords);

                                foreach (DataRow row2 in tableRecords.Rows)
                                {
                                    price = Convert.ToDouble(row2["Price"]);
                                    quantity = Convert.ToInt32(row2["Quantity"]);
                                    sum = price * quantity;

                                    costs = Convert.ToDouble(row2["ProductCosts"]);
                                    csum = costs * quantity;

                                    #region 檢查產品名稱是否已存在
                                    //product_name = Convert.ToString(row2["ProductName"]);
                                    product_name = Convert.ToString(row2["TypeName"]);
                                    for (revised = 1 + index; revised < tableReport.Rows.Count; revised++)
                                    {
                                        if (0 == Convert.ToString(tableReport.Rows[revised]["ProductName"]).CompareTo(product_name))
                                        {
                                            product_name = String.Empty;
                                            break;
                                        }
                                    }
                                    #endregion

                                    if (0 == total)// 第一列
                                    {
                                        total = Convert.ToDouble(row["Tax"]);
                                        prepay = Convert.ToDouble(row["PrePay"]);

                                        total += sum;

                                        dr = tableReport.NewRow();

                                        dr["SaveDate"] = save_date;
                                        dr["CompanyName"] = company_name;
                                        //dr["InvoiceNumber"] = invoice_number;
                                        dr["ProductName"] = row2["TypeName"];
                                        dr["Price"] = String.Format("{0:N0}", price);
                                        dr["Quantity"] = Convert.ToString(quantity);
                                        dr["Sum"] = String.Format("{0:N0}", sum);
                                        dr["Costs"] = String.Format("{0:N0}", costs);
                                        dr["CostsSum"] = String.Format("{0:N0}", csum);
                                        dr["Profit"] = String.Format("{0:N0}", sum - csum);

                                        tableReport.Rows.Add(dr);
                                    }
                                    else if (!String.IsNullOrEmpty(product_name))// 新增一列
                                    {
                                        total += sum;
                                        dr = tableReport.NewRow();

                                        //dr["ProductName"] = row2["productName"];
                                        dr["ProductName"] = row2["TypeName"];
                                        dr["Price"] = String.Format("{0:N0}", price);
                                        dr["Quantity"] = Convert.ToString(quantity);
                                        dr["Sum"] = String.Format("{0:N0}", sum);
                                        dr["Costs"] = String.Format("{0:N0}", costs);
                                        dr["CostsSum"] = String.Format("{0:N0}", csum);
                                        dr["Profit"] = String.Format("{0:N0}", sum - csum);

                                        tableReport.Rows.Add(dr);
                                    }
                                    else// 產品名稱重複，整合同一列
                                    {
                                        total += sum;
                                        dr = tableReport.Rows[revised];

                                        sum += Convert.ToDouble(dr["Sum"]);
                                        csum += Convert.ToDouble(dr["CostsSum"]);

                                        dr["Quantity"] = Convert.ToString(quantity + Convert.ToInt32(dr["Quantity"]));
                                        dr["Sum"] = String.Format("{0:N0}", sum);
                                        dr["CostsSum"] = String.Format("{0:N0}", csum);
                                        dr["Profit"] = String.Format("{0:N0}", sum - csum);
                                    }
                                }

                                index = tableReport.Rows.Count - 1;
                                tableReport.Rows[index]["Total"] = String.Format("{0:N0}", total);
                                tableReport.Rows[index]["PrePay"] = String.Format("{0:N0}", prepay);
                                tableReport.Rows[index]["LeftPay"] = String.Format("{0:N0}", total - prepay);
                            }
                        }
                        #endregion
                    }
                    finally
                    {
                        connection.Close();
                    }

                }

                GridViewReport.DataSource = tableReport;
                GridViewReport.DataBind();

                PanelReport.Visible = false;
                if (0 != tableReport.Rows.Count)
                {
                    #region 總整理
                    LabelReportStart.Text = CalendarStart.SelectedDate.ToShortDateString();
                    LabelReportEnd.Text = CalendarEnd.SelectedDate.ToShortDateString();

                    quantity = 0;
                    sum = csum = total = prepay = 0;

                    for (Int32 i = 0; i < tableReport.Rows.Count; i++)
                    {
                        try
                        {
                            Convert.ToInt32(tableReport.Rows[i]["Quantity"]);// 錯誤嘗試
                            quantity += Convert.ToInt32(tableReport.Rows[i]["Quantity"]);
                        }
                        catch { }

                        try
                        {
                            Convert.ToDouble(tableReport.Rows[i]["Sum"]);// 錯誤嘗試
                            sum += Convert.ToDouble(tableReport.Rows[i]["Sum"]);
                        }
                        catch { }

                        try
                        {
                            Convert.ToDouble(tableReport.Rows[i]["CostsSum"]);// 錯誤嘗試
                            csum += Convert.ToDouble(tableReport.Rows[i]["CostsSum"]);
                        }
                        catch { }

                        try
                        {
                            Convert.ToDouble(tableReport.Rows[i]["Total"]);// 錯誤嘗試
                            total += Convert.ToDouble(tableReport.Rows[i]["Total"]);
                        }
                        catch { }

                        try
                        {
                            Convert.ToDouble(tableReport.Rows[i]["PrePay"]);// 錯誤嘗試
                            prepay += Convert.ToDouble(tableReport.Rows[i]["PrePay"]);
                        }
                        catch { }

                    }

                    LabelTotalQuantity.Text = Convert.ToString(quantity);
                    LabelTotalSum.Text = String.Format("{0:N0}", sum);
                    LabelTotalCostsSum.Text = String.Format("{0:N0}", csum);
                    LabelTotalProfit.Text = String.Format("{0:N0}", sum - csum);
                    LabelTotalTotal.Text = String.Format("{0:N0}", total);
                    LabelTotalPrepay.Text = String.Format("{0:N0}", prepay);
                    LabelTotalLeftPay.Text = String.Format("{0:N0}", total - prepay);
                    #endregion

                    //GridViewReport.DataSource = tableReport;
                    //GridViewReport.DataBind();

                    #region 確認報表顯示欄位及長度
                    switch (MenuReport.SelectedValue)
                    {
                        case "list":
                            {
                                Label label;
                                LabelReport.Text = String.Empty;
                                foreach (GridViewRow r in GridViewReport.Rows)
                                {
                                    label = r.FindControl("LabelReportCompanyName") as Label;
                                    LabelReport.Text = label.Text;
                                    if (!String.IsNullOrEmpty(LabelReport.Text))
                                        break;
                                }

                                GridViewReport.Columns[1].HeaderStyle.Width = 166;

                                GridViewReport.Columns[1].Visible = false;
                                GridViewReport.Columns[4].Visible = false;
                                GridViewReport.Columns[7].Visible = false;
                                GridViewReport.Columns[8].Visible = false;
                                GridViewReport.Columns[9].Visible = false;

                                PanelReport.Visible = true;

                                if (0 != GridViewReport.Rows.Count)
                                    ButtonPrint.Visible = true;

                                break;
                            }

                        case "debt":
                            {
                                GridViewReport.Columns[1].HeaderStyle.Width = 166;

                                GridViewReport.Columns[1].Visible = true;
                                GridViewReport.Columns[3].Visible = false;
                                GridViewReport.Columns[4].Visible = false;
                                GridViewReport.Columns[7].Visible = false;
                                GridViewReport.Columns[8].Visible = false;
                                GridViewReport.Columns[9].Visible = false;
                                break;
                            }

                        case "report":
                            {
                                GridViewReport.Columns[1].HeaderStyle.Width = 88;
                                GridViewReport.Columns[1].Visible = true;
                                GridViewReport.Columns[3].Visible = false;
                                GridViewReport.Columns[4].Visible = true;
                                GridViewReport.Columns[7].Visible = true;
                                GridViewReport.Columns[8].Visible = true;
                                GridViewReport.Columns[9].Visible = true;
                                break;
                            }
                    }
                    #endregion

                    Session["PrintObject"] = PanelReport;// 暫存列印表
                }
                else Message.LabelError(LabelMessage, "查詢無任何資料。");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack) return;

        Title = Profile.PrincipalCompanyName + " - 報表製作";

        CalendarStart.SelectedDate = DateTime.Now.Date;
        CalendarEnd.SelectedDate = DateTime.Now.Date;

        #region 檢查是否為跳頁後返回
        String menu = Session["ReportMenu"] as String;
        if (String.IsNullOrEmpty(menu)) return;

        String dt = Session["CalendarStart"] as String;
        if (!String.IsNullOrEmpty(dt))
            CalendarStart.SelectedDate = DateTime.ParseExact(dt, "yyyyMMddHHmmtt", null);

        dt = Session["CalendarEnd"] as String;
        if (!String.IsNullOrEmpty(dt))
            CalendarEnd.SelectedDate = DateTime.ParseExact(dt, "yyyyMMddHHmmtt", null);

        if (0 == MenuReport.Items.Count)
        {
            MenuReport.DataBind();
            //MenuClick();
        }

        PanelAccount.Visible = false;
        PanelReport.Visible = false;
        switch (menu)
        {
            case "Account":
                {
                    MenuReport.Items[0].Selected = true;

                    String id = Session["ReportCompanyId"] as String;
                    if (!String.IsNullOrEmpty(id))
                    {
                        ListAccounts(selectAccountsNotPayed);
                        foreach (ListItem item in ListBoxReport.Items)
                        {
                            if (0 == item.Value.CompareTo(id))
                            {
                                item.Selected = true;
                                break;
                            }
                        }

                        ShowAccount(false);
                        PanelAccount.Visible = true;
                        PanelReport.Visible = false;
                    }
                    break;
                }

            case "Trade":
                {
                    break;
                }

            case "List":
                {
                    MenuReport.Items[1].Selected = true;
                    MenuClick();
                    break;
                }

            case "Debt":
                {
                    MenuReport.Items[2].Selected = true;
                    MenuClick();
                    break;
                }

            case "Report":
                {
                    MenuReport.Items[3].Selected = true;
                    MenuClick();
                    break;
                }
        }
        #endregion
    }

    protected void MenuReport_MenuItemClick(object sender, MenuEventArgs e)
    {
        MenuClick();
    }

    protected void ButtonReport_Click(object sender, EventArgs e)
    {
        LabelMessage.Visible = false;

        PanelAccount.Visible = false;
        PanelReport.Visible = false;

        Session["CalendarStart"] = CalendarStart.SelectedDate.ToString("yyyyMMddHHmmtt");
        Session["CalendarEnd"] = CalendarEnd.SelectedDate.ToString("yyyyMMddHHmmtt");

        switch (MenuReport.SelectedValue)
        {
            case "account":
                {
                    ListAccounts(selectAccountsNotPayed);// 取得未付款公司列表

                    Session["ReportMenu"] = "Account";
                    break;
                }

            case "trade":
                {
                    ListAccounts(selectAccountsRange);// 取得交易公司列表

                    Session["ReportMenu"] = "Trade";
                    break;
                }

            case "list":
                {
                    ListAccounts(selectAccountsAll);// 單一客戶交易報表

                    Session["ReportMenu"] = "List";
                    break;
                }

            case "debt":
                {
                    CreateReport(selectReportAccountsNotPayed);
                    LabelReport.Text = "應收帳款報表";

                    Session["ReportMenu"] = "Debt";
                    Session.Remove("ReportCompanyId");

                    if (GridViewReport.Rows.Count > 0)
                    {
                        String script = "window.open('PrintReport.aspx');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PrintReport", script, true);
                    }

                    break;
                }

            case "report":
                {
                    PanelDate.Visible = true;

                    CreateReport(selectReportAccounts);
                    LabelReport.Text = "營業報表";

                    Session["ReportMenu"] = "Report";
                    Session.Remove("ReportCompanyId");

                    if (GridViewReport.Rows.Count > 0)
                    {
                        String script = "window.open('PrintReport.aspx');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PrintReport", script, true);
                    }

                    break;
                }
        }
    }

    protected void ButtonPrint_Click(object sender, EventArgs e)
    {
        String script = "window.open('PrintReport.aspx');";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PrintReport", script, true);
    }

    protected void ListBoxReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (MenuReport.SelectedValue)
        {
            case "account":
                {
                    CalendarStart.Visible = false;
                    CalendarEnd.Visible = false;
                    ButtonReport.Visible = false;

                    ShowAccount(false);
                    PanelAccount.Visible = true;
                    PanelReport.Visible = false;

                    break;
                }

            case "trade":
                {
                    ShowAccount(true);
                    PanelAccount.Visible = true;
                    PanelReport.Visible = false;

                    break;
                }

            case "list":
                {
                    PanelDate.Visible = false;

                    CreateReport(selectReportSingle);
                    PanelAccount.Visible = false;
                    PanelReport.Visible = true;
                    LabelReport.Text = "銷售報表";

                    break;
                }
        }

        Session["ReportCompanyId"] = ListBoxReport.SelectedValue;
    }

    protected void GridViewAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow == e.Row.RowType)
        {
            Label label = e.Row.Cells[0].Controls[1] as Label;
            if (0 == label.Text.CompareTo(customSales.ToLower()))
            {
                label.Text = "銷";
            }
            else if (0 == label.Text.CompareTo(customReturns.ToLower()))
            {
                label.Text = "退";
            }
            else if (0 == label.Text.CompareTo(manufactureSales.ToLower()))
            {
                label.Text = "進";//目前廠商不確定是否需要製作
            }
            else if (0 == label.Text.CompareTo(manufactureReturns.ToLower()))
            {
                label.Text = "退";
            }

            label = (Label)e.Row.Cells[1].Controls[1];
            label.Text = Convert.ToDateTime(label.Text).ToShortDateString();

            label = (Label)e.Row.Cells[4].Controls[1];
            label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));

            label = (Label)e.Row.Cells[5].Controls[1];
            label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));

            label = (Label)e.Row.Cells[6].Controls[1];
            label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));

            label = (Label)e.Row.Cells[7].Controls[1];
            label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));

            label = (Label)e.Row.Cells[8].Controls[1];
            label.Text = String.Format("{0:N}", Convert.ToDouble(label.Text));
        }
    }

    protected void GridViewAccount_DataBound(object sender, EventArgs e)
    {
        Label label;

        Int32 quantity = 0;
        Double sum = 0;
        Double tax = 0;
        Double total = 0;
        Double pre_pay = 0;
        Double left_pay = 0;

        foreach (GridViewRow row in GridViewAccount.Rows)
        {
            label = (Label)row.Cells[3].Controls[1];
            quantity += Convert.ToInt32(label.Text);

            label = (Label)row.Cells[4].Controls[1];
            sum += Convert.ToDouble(label.Text);

            label = (Label)row.Cells[5].Controls[1];
            tax += Convert.ToDouble(label.Text);

            label = (Label)row.Cells[6].Controls[1];
            total += Convert.ToDouble(label.Text);

            label = (Label)row.Cells[7].Controls[1];
            pre_pay += Convert.ToDouble(label.Text);

            label = (Label)row.Cells[8].Controls[1];
            left_pay += Convert.ToDouble(label.Text);
        }

        LabelAccountQuantity.Text = Convert.ToString(quantity);
        LabelAccountSum.Text = String.Format("{0:N}", sum);
        LabelAccountTax.Text = String.Format("{0:N}", tax);
        LabelAccountTotal.Text = String.Format("{0:N}", total);
        LabelAccountPrePay.Text = String.Format("{0:N}", pre_pay);
        LabelAccountLeftPay.Text = String.Format("{0:N}", left_pay);
    }

    protected void LinkButtonAccountNumber_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewAccount.SelectedIndex = lb.TabIndex % GridViewAccount.PageSize;

        #region 取得單據所屬公司並記錄單據資料
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            using (adapter.SelectCommand = new SqlCommand(selectCompanyFromAccount, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                adapter.SelectCommand.Parameters.AddWithValue("AccountId", lb.CommandArgument);

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

            if (1 == table.Rows.Count)
            {
                Session["CompanyName"] = table.Rows[0]["CompanyName"];
                Session["CompanyId"] = Convert.ToString(table.Rows[0]["CompanyId"]);

                Session["AccountNumber"] = lb.Text;
                Session["AccountId"] = lb.CommandArgument;
            }
        }
        #endregion

        Response.Redirect("Invoice.aspx");
    }

    protected void GridViewReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Attributes["id"] = "HeaderRow";// 強制列印分頁
            return;
        }

        if (e.Row.RowType != DataControlRowType.DataRow) return;

        Label label = e.Row.Cells[1].Controls[1] as Label;
        if (0 == label.Text.CompareTo("CompanyName"))
        {
            //e.Row.Attributes["style"] = "page-break-before: always;";// 強制列印分頁

            label.Text = "公司名稱";

            label = e.Row.Cells[0].Controls[1] as Label;
            label.Text = "日期";

            label = e.Row.Cells[2].Controls[1] as Label;
            label.Text = "3號碼";

            label = e.Row.Cells[3].Controls[1] as Label;
            label.Text = "鞋型";

            label = e.Row.Cells[4].Controls[1] as Label;
            label.Text = "單價";

            label = e.Row.Cells[5].Controls[1] as Label;
            label.Text = "數量";

            label = e.Row.Cells[6].Controls[1] as Label;
            label.Text = "合計";

            label = e.Row.Cells[7].Controls[1] as Label;
            label.Text = "成本單價";

            label = e.Row.Cells[8].Controls[1] as Label;
            label.Text = "成本合計";

            label = e.Row.Cells[9].Controls[1] as Label;
            label.Text = "毛利";

            label = e.Row.Cells[10].Controls[1] as Label;
            label.Text = "應收";

            label = e.Row.Cells[11].Controls[1] as Label;
            label.Text = "已收";

            label = e.Row.Cells[12].Controls[1] as Label;
            label.Text = "未收";

            return;
        }

        //Int32 length = Encoding.GetEncoding("Big5").GetByteCount(label.Text);
        //if (166 == GridViewReport.Columns[1].HeaderStyle.Width)
        //{
        //    if (length > 24)
        //        label.Font.Size = 8;// 如果超過12個字，縮小字型
        //    else
        //        label.Font.Size = 10;
        //}
        //else
        //{
        //    if (length > 12)
        //        label.Font.Size = 8;// 如果超過6個字，縮小字型
        //    else
        //        label.Font.Size = 10;
        //}
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        ListBoxCompanies.Visible = false;
        ListBoxCompanies.DataBind();// 列表公司

        #region Session 紀錄
        Session["CompanyName"] = TextBoxSearch.Text;
        Session.Remove("CompanyId");
        #endregion
    }

    protected void ListBoxCompanies_DataBinding(object sender, EventArgs e)
    {
        ListBoxCompanies.DataSourceID = "SqlDataSourceCompaniesCondition";// 預設 - 條件搜尋

        if (String.IsNullOrWhiteSpace(TextBoxSearch.Text))// 沒有搜尋條件則更換 SqlDataSource
            ListBoxCompanies.DataSourceID = "SqlDataSourceCompanies";
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
        LabelMessage.Visible = false;

        PanelDate.Visible = false;
        CreateReport(selectReportSingle);

        #region Session 紀錄
        Session["CompanyId"] = ListBoxCompanies.SelectedValue;
        #endregion
    }
}