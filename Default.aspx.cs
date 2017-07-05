using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void Login_LoggedIn(object sender, EventArgs e)
    {
        Session.Clear();// 重置 Session 紀錄
    }
}