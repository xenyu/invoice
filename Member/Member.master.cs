using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;

public partial class Member_Member : System.Web.UI.MasterPage
{
    #region SQL command
    protected static String selectCompanyData = "SELECT CompanyId, CompanyName, CompanyNumber, CompanyPhone, CompanyFax FROM [aspnet_Company] WHERE [OwnerId] = @OwnerId AND [CompanyType] = 'DE9DFF94-D08E-44DA-A21D-991C8D3133F9'";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        #region 確認載入使用者資料
        UserData userData = (UserData)Session["UserData"];
        if (null == userData || 0 != userData.UserName.CompareTo(Page.User.Identity.Name))
        {
            Session.Clear();

            userData = new UserData();
            if (!userData.Init(Request.UserHostAddress, Page.User.Identity.Name))
            {
                InsertError.Insert(Request.UserHostAddress, Request.FilePath, "Empty UserData.");
                Response.Redirect("~/Error.aspx", true);
                return;
            }
        }

        Session["UserData"] = userData;
        #endregion

        Object o = Session["Profile"];// 確認是否已載入 Profile
        if (null == o || true != Convert.ToBoolean(o))
        {
            #region 取得本身基本資料暫存到 Profile
            MembershipUser user = Membership.GetUser();
            if (null == user) throw new Exception("Profile initial exception!");

            Profile.PrincipalGuid = Convert.ToString(user.ProviderUserKey.ToString());
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    DataTable table = new DataTable();
                    using (adapter.SelectCommand = new SqlCommand(selectCompanyData, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("OwnerId", Profile.PrincipalGuid);

                        try
                        {
                            connection.Open();

                            adapter.Fill(table);
                            if (0 != table.Rows.Count)
                            {
                                Profile.PrincipalCompanyGuid = Convert.ToString(table.Rows[0]["CompanyId"]);
                                Profile.PrincipalCompanyName = Convert.ToString(table.Rows[0]["CompanyName"]);
                                Profile.PrincipalCompanyNumber = Convert.ToString(table.Rows[0]["CompanyNumber"]);
                                Profile.PrincipalCompanyPhone = Convert.ToString(table.Rows[0]["CompanyPhone"]);
                                Profile.PrincipalCompanyFax = Convert.ToString(table.Rows[0]["CompanyFax"]);

                                Session["Profile"] = true;
                            }
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            #endregion
        }

        if (!IsPostBack)
        {
            LabelNow.Text = DateTime.Today.ToShortDateString();
        }
    }

    protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
    {
        SiteMapNode node = (SiteMapNode)e.Item.DataItem;

        if (!String.IsNullOrEmpty(node["imageUrl"]))
            e.Item.ImageUrl = node["imageUrl"];// 不知為何變成要手動設定圖示

        if (0 == e.Item.Text.CompareTo(SiteMap.CurrentNode.Title))
            e.Item.Selected = true;
    }

    protected void TimerDate_Tick(object sender, EventArgs e)
    {
        LabelNow.Text = DateTime.Today.ToShortDateString();
    }
}
