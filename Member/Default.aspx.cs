using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Member_Default : System.Web.UI.Page
{
    #region SQL command
    protected static String accountChecked = "UPDATE aspnet_Account SET CheckDate = @UpdateDate WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";
    protected static String accountDelivered = "UPDATE aspnet_Account SET DeliverDate = @UpdateDate WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";
    protected static String accountFinished = "UPDATE aspnet_Account SET DeliverDate = @UpdateDate, CheckDate = @UpdateDate WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";

    protected static String selectAccountType = "SELECT AccountType FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";
    protected static String selectRecordProducts = "SELECT ProductId, Quantity FROM aspnet_Record WHERE (AccountId = @AccountId) AND (DeleteDate IS NULL)";
    protected static String inceaseProduct = "UPDATE aspnet_Product SET CurrentCount = CurrentCount + @Count WHERE (OwnerId = @OwnerId) AND (ProductId = @ProductId)";
    protected static String deceaseProduct = "UPDATE aspnet_Product SET CurrentCount = CurrentCount - @Count WHERE (OwnerId = @OwnerId) AND (ProductId = @ProductId)";

    protected static String selectCompanyFromAccount = "SELECT CompanyId FROM aspnet_Account WHERE (OwnerId = @OwnerId) AND (AccountId = @AccountId)";
    protected static String updateCompanyLastDate = "UPDATE aspnet_Company SET LastDate = @UpdateDate WHERE (OwnerId = @OwnerId) AND (CompanyId = @CompanyId)";

    protected static String selectCompany = "SELECT company.CompanyId, company.CompanyName FROM aspnet_Company company, aspnet_Account account WHERE (account.OwnerId = @OwnerId) AND (account.AccountId = @AccountId) AND (company.OwnerId = @OwnerId) AND (company.CompanyId = account.CompanyId)";
    #endregion

    #region 單據類型 GUID
    protected static String customSales = "FDD998A1-D477-484B-B0EE-6D0DE75EE166";
    protected static String customReturns = "5E5A5BE7-46C5-4D2B-ABD8-491330921243";
    protected static String manufactureSales = "A6FC3F19-C524-4BA1-A3C0-8F180D8093C5";
    protected static String manufactureReturns = "26239577-49DF-44F9-AEBD-729870F0744F";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Session["NavagatePage"] = String.Empty;
            return;
        }

        Title = Profile.PrincipalCompanyName + " - 管理首頁";

        #region 檢查是否為跳頁後返回
        String npage = (String)Session["NavagatePage"];
        if (!String.IsNullOrEmpty(npage))
        {
            String nmenu = (String)Session["NavagateMenu"];
            Int32 ngpindex = Convert.ToInt32(Session["NavagateGridPageIndex"]);
            Int32 ngrindex = Convert.ToInt32(Session["NavagateGridRowIndex"]);

            if (0 == npage.CompareTo("Default"))
            {
                if (0 == MenuDefault.Items.Count)
                    MenuDefault.DataBind();

                switch (nmenu)
                {
                    case "Waiting":
                        {
                            MenuDefault.Items[0].Selected = true;
                            MenuDefault_MenuItemClick(null, null);
                            UpdatePanelMenu.Update();

                            if (ngpindex > -1)
                                GridViewAccount.PageIndex = ngpindex;

                            if (ngrindex > -1)
                                GridViewAccount.SelectedIndex = ngrindex;

                            UpdatePanelMain.Update();

                            break;
                        }

                    case "Lost":
                        {
                            MenuDefault.Items[2].Selected = true;
                            MenuDefault_MenuItemClick(null, null);
                            UpdatePanelMenu.Update();

                            if (ngpindex > -1)
                                GridViewLost.PageIndex = ngpindex;

                            if (ngrindex > -1)
                                GridViewLost.SelectedIndex = ngrindex;

                            UpdatePanelMain.Update();

                            break;
                        }
                }
            }
        }
        #endregion
    }

    protected void MenuDefault_MenuItemClick(object sender, MenuEventArgs e)
    {
        LabelMessage.Visible = false;

        PanelAccount.Visible = false;
        PanelInventory.Visible = false;
        PanelLost.Visible = false;

        switch (MenuDefault.SelectedValue)
        {
            case "waiting": { PanelAccount.Visible = true; break; }
            case "inventory": { PanelInventory.Visible = true; break; }
            case "lost": { PanelLost.Visible = true; break; }
        }
    }

    protected void GridViewAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow != e.Row.RowType) return;

        #region CheckBox
        CheckBox deliver = (CheckBox)e.Row.Cells[4].Controls[1];
        if (!String.IsNullOrEmpty(deliver.Text))
        {
            deliver.Checked = true;
            deliver.Enabled = false;
        }
        deliver.Text = String.Empty;

        CheckBox check = (CheckBox)e.Row.Cells[5].Controls[1];
        if (!String.IsNullOrEmpty(check.Text))
        {
            check.Checked = true;
            check.Enabled = false;
        }
        check.Text = String.Empty;
        #endregion

        LinkButton lb = (LinkButton)e.Row.Cells[6].Controls[1];
        if (null != lb)// 寫入按鈕附加詢問訊息
        {
            lb.Attributes.Add("onclick", "return confirm('確定更新此列狀態？')");
        }
    }

    protected void GridViewAccount_PageIndexChanged(object sender, EventArgs e)
    {
        GridViewAccount.SelectedIndex = -1;
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

            using (adapter.SelectCommand = new SqlCommand(selectCompany, connection))
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

        #region Session 建立跳頁後上一頁歷程功能
        Session["NavagatePage"] = "Default";
        Session["NavagateMenu"] = "Waiting";
        Session["NavagateGridPageIndex"] = Convert.ToString(GridViewAccount.PageIndex);
        Session["NavagateGridRowIndex"] = GridViewAccount.SelectedIndex;
        #endregion

        Response.Redirect("Invoice.aspx");
    }

    protected void CheckBoxAccount_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;

        if (cb.Checked)
            GridViewAccount.SelectedIndex = cb.TabIndex;
    }

    protected void LinkButtonUpdateRow_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewAccount.SelectedIndex = lb.TabIndex;

        String commandString = String.Empty;
        CheckBox deliver = (CheckBox)GridViewAccount.SelectedRow.Cells[4].Controls[1];
        CheckBox check = (CheckBox)GridViewAccount.SelectedRow.Cells[5].Controls[1];

        #region 決定指令字串
        if (deliver.Enabled && check.Enabled)
        {
            if (deliver.Checked && check.Checked) commandString = accountFinished;
            else
            {
                if (deliver.Checked) commandString = accountDelivered;
                else if (check.Checked) commandString = accountChecked;
            }
        }
        else
        {
            if (deliver.Enabled && deliver.Checked) commandString = accountDelivered;
            else if (check.Enabled && check.Checked) commandString = accountChecked;
        }
        #endregion

        if (String.IsNullOrEmpty(commandString)) return;

        #region 寫入資料庫
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
                    command.Parameters.AddWithValue("AccountId", GridViewAccount.SelectedValue);
                    command.Parameters.AddWithValue("UpdateDate", DateTime.Now);

                    command.ExecuteNonQuery();
                }

                #region 出貨後更新庫存數量
                if (deliver.Enabled && deliver.Checked)
                {
                    String accountType = String.Empty;// 取得單據類型
                    using (SqlCommand command = new SqlCommand(selectAccountType, connection, transaction))
                    {
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("AccountId", GridViewAccount.SelectedValue);

                        Object o = command.ExecuteScalar();
                        if (null != o && DBNull.Value != o)
                            accountType = Convert.ToString(o);
                    }

                    if (!String.IsNullOrEmpty(accountType))
                    {
                        // 決定指令
                        if (0 == accountType.CompareTo(manufactureReturns.ToLower()) || 0 == accountType.CompareTo(customSales.ToLower()))// 客戶銷貨/廠商退貨，商品-
                            commandString = deceaseProduct;
                        else if (0 == accountType.CompareTo(manufactureSales.ToLower()) || 0 == accountType.CompareTo(customReturns.ToLower()))// 客戶退貨/廠商進貨，商品+
                            commandString = inceaseProduct;

                        DataTable table = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter();// 取得單據內商品 GUID 及進銷數量
                        using (adapter.SelectCommand = new SqlCommand(selectRecordProducts, connection, transaction))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("AccountId", GridViewAccount.SelectedValue);

                            adapter.Fill(table);
                        }

                        using (SqlCommand command = new SqlCommand(commandString, connection, transaction))
                        {
                            command.Parameters.AddWithValue("Count", "");
                            command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                            command.Parameters.AddWithValue("ProductId", "");

                            foreach (DataRow row in table.Rows)
                            {
                                command.Parameters["Count"].Value = row["Quantity"];
                                command.Parameters["ProductId"].Value = row["ProductId"];

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                #endregion

                #region 更新公司最後交易日期
                if (check.Checked && deliver.Checked)
                {
                    String companyId = String.Empty;
                    using (SqlCommand command = new SqlCommand(selectCompanyFromAccount, connection, transaction))
                    {
                        command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                        command.Parameters.AddWithValue("AccountId", GridViewAccount.SelectedValue);

                        Object o = command.ExecuteScalar();
                        if (null != o && DBNull.Value != o)
                            companyId = Convert.ToString(o);
                    }

                    if (!String.IsNullOrEmpty(companyId))
                    {
                        using (SqlCommand command = new SqlCommand(updateCompanyLastDate, connection, transaction))
                        {
                            command.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);
                            command.Parameters.AddWithValue("CompanyId", companyId);
                            command.Parameters.AddWithValue("UpdateDate", DateTime.Now);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                transaction.Commit();
                transaction = null;
            }
            catch
            {
                if (null != transaction)
                    transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        GridViewAccount.DataBind();// 更新列表
        GridViewAccount.SelectedIndex = -1;
    }

    protected void ButtonInventory_Click(object sender, EventArgs e)
    {
        GridViewInventory.DataBind();
    }

    protected void GridViewLost_PageIndexChanged(object sender, EventArgs e)
    {
        GridViewLost.SelectedIndex = -1;
    }

    protected void LinkButtonCompanyName_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewLost.SelectedIndex = lb.TabIndex % GridViewLost.PageSize;

        #region 紀錄公司資料
        Session["CompanyName"] = lb.Text;
        Session["CompanyId"] = lb.CommandArgument;
        #endregion

        #region Session 建立跳頁後上一頁歷程功能
        Session["NavagatePage"] = "Default";
        Session["NavagateMenu"] = "Lost";
        Session["NavagatePageIndex"] = Convert.ToString(GridViewLost.PageIndex);
        Session["NavagateGridRowIndex"] = GridViewLost.SelectedIndex;
        #endregion

        Response.Redirect("Company.aspx");
    }
}