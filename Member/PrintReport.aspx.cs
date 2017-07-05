using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PrintReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Panel panel = Session["PrintObject"] as Panel;
        panel.Visible = true;
        
        PrintHelper.PrintWebControl(panel);
    }
}