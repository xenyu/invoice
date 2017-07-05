using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = Request.UserHostAddress;

        if (null == Request.UrlReferrer)
            HyperLink1.NavigateUrl = "http://invoice.governsoft.com";
        else
            HyperLink1.NavigateUrl = Request.UrlReferrer.ToString();
    }
}