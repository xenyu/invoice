using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

/// <summary>
/// 將錯誤訊息寫入資料庫
/// </summary>
public class InsertError
{
    protected static String insertError = "INSERT INTO [aspnet_Error] ( [IP], [Page], [Exception], [Date] ) VALUES ( @IP, @Page, @Exception, @Date )";

    public InsertError()
    {
    }

    public static void Insert(String ip, String page, String exception)
    {
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
        {
            using (SqlCommand command = new SqlCommand(insertError, connection))
            {
                try
                {
                    connection.Open();

                    command.Parameters.AddWithValue("IP", ip);
                    command.Parameters.AddWithValue("Page", page);
                    command.Parameters.AddWithValue("Exception", exception);
                    command.Parameters.AddWithValue("Date", DateTime.Now);

                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
