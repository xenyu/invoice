using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Data.SqlClient;

/// <summary>
/// 使用者及本公司資料
/// </summary>
public class UserData
{
    #region 資料庫命令
    protected static String checkExist = "SELECT COUNT(1) FROM [aspnet_Person] WHERE [PersonId] = @PersonId";
    protected static String checkOwnerId = "SELECT COUNT(1) FROM [aspnet_Person] WHERE [PersonId] = @PersonId AND [OwnerId] = [PersonId]";
    protected static String selectOwnerId = "SELECT [OwnerId] FROM [aspnet_Person] WHERE [PersonId] = @PersonId";
    protected static String selectPrincipal = "SELECT * FROM [aspnet_Principal] WHERE [OwnerId] = @OwnerId";
    protected static String selectAccountTypes = "SELECT [TypeId] FROM [aspnet_AccountType] ORDER BY [score]";
    protected static String selectCompany = "SELECT [CompanyId], [CompanyName] FROM [aspnet_Company] WHERE [OwnerId] = @OwnerId AND [CompanyType] = @CompanyType";
    #endregion

    /// <summary>
    /// 字串區間字元
    /// </summary>
    protected static Char[] spliter = new Char[] { '|' };

    /// <summary>
    /// 會計起始年度
    /// </summary>
    public Int32 Year = 0;
    /// <summary>
    /// 交易稅率5%
    /// </summary>
    public Int32 Tax = 5;
    /// <summary>
    /// 預設商品利潤，首次會自動以進價加上此利潤射為預設售價
    /// </summary>
    public Int32 Profit = 30;
    /// <summary>
    /// 建立商品時可以全部建立的類型數量
    /// </summary>
    public Int32 EffectTypes = 0;

    /// <summary>
    /// 連線IP
    /// </summary>
    public String IP = String.Empty;
    /// <summary>
    /// 本公司名稱
    /// </summary>
    public String CompanyName = String.Empty;
    /// <summary>
    /// 登入帳號 (判斷是否已變更帳號)
    /// </summary>
    public String UserName = String.Empty;

    /// <summary>
    /// 本公司類型ID
    /// </summary>
    public String CompanyType = String.Empty;
    /// <summary>
    /// 廠商類型ID
    /// </summary>
    public String ProviderType = String.Empty;
    /// <summary>
    /// 客戶類型ID
    /// </summary>
    public String CustomerType = String.Empty;

    /// <summary>
    /// 商品顯示欄位
    /// </summary>
    public List<String> ProductColumnList = new List<String>();
    /// <summary>
    /// 公司顯示欄位
    /// </summary>
    public List<String> CompanyColumnList = new List<String>();
    /// <summary>
    /// 人員顯示欄位
    /// </summary>
    public List<String> PersonColumnList = new List<String>();

    /// <summary>
    /// 首頁
    /// </summary>
    public String HomePage = String.Empty;
    /// <summary>
    /// 進銷頁
    /// </summary>
    public String AccountPage = String.Empty;
    /// <summary>
    /// 商品頁
    /// </summary>
    public String ProductPage = String.Empty;
    /// <summary>
    /// 報表頁
    /// </summary>
    public String ReportPage = String.Empty;
    /// <summary>
    /// 人員頁
    /// </summary>
    public String PersonPage = String.Empty;
    /// <summary>
    /// 公司頁
    /// </summary>
    public String CompanyPage = String.Empty;
    /// <summary>
    /// 設定頁
    /// </summary>
    public String OptionPage = String.Empty;
    /// <summary>
    /// 使用哪一個網頁導覽
    /// </summary>
    public String SiteMap = String.Empty;

    /// <summary>
    /// 使用者ID
    /// </summary>
    public Guid UserGuid = Guid.Empty;
    /// <summary>
    /// 負責人ID
    /// </summary>
    public Guid PrincipalGuid = Guid.Empty;
    /// <summary>
    /// 公司ID
    /// </summary>
    public Guid CompanyGuid = Guid.Empty;

    /// <summary>
    /// 廠商進貨類型
    /// </summary>
    public Guid StockType = Guid.Empty;
    /// <summary>
    /// 客戶銷貨類型
    /// </summary>
    public Guid ShipmentType = Guid.Empty;
    /// <summary>
    /// 廠商退貨類型
    /// </summary>
    public Guid StockReturnType = Guid.Empty;
    /// <summary>
    /// 客戶退貨類型
    /// </summary>
    public Guid ShipmentReturnType = Guid.Empty;

    /// <summary>
    /// 建構式
    /// </summary>
    public UserData()
    {
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="ip">連線IP</param>
    /// <param name="username">使用者名稱</param>
    /// <returns>是否成功建立</returns>
    public Boolean Init(String ip, String username)
    {
        Boolean result = false;

        IP = ip;
        UserName = username;
        UserGuid = new Guid(Membership.GetUser(username).ProviderUserKey.ToString());

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();

                        #region 確認這個使用者aspnet_Person是否已經建立，這筆資料應在First.aspx中建立，未建立一律回傳false
                        command.CommandText = checkExist;
                        command.Parameters.AddWithValue("PersonId", UserGuid);
                        if (0 == Convert.ToInt32(command.ExecuteScalar()))
                            return false;
                        #endregion

                        #region 確認目前使用者ID是否為負責人ID
                        command.CommandText = checkOwnerId;
                        if (0 == Convert.ToInt32(command.ExecuteScalar()))//不是負責人，取得負責人ID
                        {
                            command.CommandText = selectOwnerId;
                            PrincipalGuid = new Guid(command.ExecuteScalar().ToString());
                        }
                        else//使用者就是負責人
                        {
                            PrincipalGuid = UserGuid;
                        }
                        #endregion

                        #region 取得aspnet_Principle資料
                        table.Clear();

                        using (adapter.SelectCommand = new SqlCommand(selectPrincipal, connection))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("OwnerId", PrincipalGuid);
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                Year = Convert.ToInt32(table.Rows[0]["StartYear"]);
                                Tax = Convert.ToInt32(table.Rows[0]["Tax"]);
                                Profit = Convert.ToInt32(table.Rows[0]["BasicProfit"]);
                                EffectTypes = Convert.ToInt32(table.Rows[0]["EffectTypes"]);

                                HomePage = table.Rows[0]["HomePage"].ToString();
                                AccountPage = table.Rows[0]["AccountPage"].ToString();
                                ProductPage = table.Rows[0]["ProductPage"].ToString();
                                ReportPage = table.Rows[0]["ReportPage"].ToString();
                                PersonPage = table.Rows[0]["PersonPage"].ToString();
                                CompanyPage = table.Rows[0]["CompanyPage"].ToString();
                                OptionPage = table.Rows[0]["OptionPage"].ToString();
                                SiteMap = table.Rows[0]["SiteMap"].ToString();

                                CompanyType = table.Rows[0]["CompanyType"].ToString();
                                ProviderType = table.Rows[0]["ProviderType"].ToString();
                                CustomerType = table.Rows[0]["CustomerType"].ToString();

                                String[] productColumn = table.Rows[0]["ProductProperty"].ToString().Split(spliter);
                                ProductColumnList.Clear();
                                foreach (String s in productColumn)
                                    ProductColumnList.Add(s);

                                String[] companyColumn = table.Rows[0]["CompanyProperty"].ToString().Split(spliter);
                                CompanyColumnList.Clear();
                                foreach (String s in companyColumn)
                                    CompanyColumnList.Add(s);

                                String[] personColumn = table.Rows[0]["PersonProperty"].ToString().Split(spliter);
                                PersonColumnList.Clear();
                                foreach (String s in personColumn)
                                    PersonColumnList.Add(s);
                            }
                        }
                        #endregion

                        #region 取得各種單據類型
                        table.Clear();

                        using (adapter.SelectCommand = new SqlCommand(selectAccountTypes, connection))
                        {
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                StockType = new Guid(table.Rows[0]["TypeId"].ToString());
                                ShipmentType = new Guid(table.Rows[1]["TypeId"].ToString());
                                StockReturnType = new Guid(table.Rows[2]["TypeId"].ToString());
                                ShipmentReturnType = new Guid(table.Rows[3]["TypeId"].ToString());
                            }
                        }
                        #endregion

                        #region 取得公司ID及公司名稱
                        table.Clear();

                        using (adapter.SelectCommand = new SqlCommand(selectCompany, connection))
                        {
                            adapter.SelectCommand.Parameters.AddWithValue("OwnerId", UserGuid);
                            adapter.SelectCommand.Parameters.AddWithValue("CompanyType", new Guid(CompanyType));
                            adapter.Fill(table);

                            if (table.Rows.Count > 0)
                            {
                                CompanyName = table.Rows[0]["CompanyName"].ToString();
                                CompanyGuid = new Guid(table.Rows[0]["CompanyId"].ToString());
                            }
                        }
                        #endregion

                        result = true;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        return result;
    }
}