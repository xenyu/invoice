using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Web.SessionState;

public class PrintHelper
{
    public PrintHelper()
    {
    }

    public static void PrintWebControl(Control ctrl)
    {
        PrintWebControl(ctrl, string.Empty);
    }

    public static void PrintWebControl(Control ctrl, String script)
    {
        StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);

        Page pg = new Page();
        pg.EnableEventValidation = false;
        
        if (!String.IsNullOrEmpty(script))
        {
            pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", script);
        }

        HtmlForm frm = new HtmlForm();
        pg.Controls.Add(frm);
        frm.Attributes.Add("runat", "server");
        frm.Controls.Add(ctrl);
        pg.DesignerInitialize();
        pg.RenderControl(htmlWrite);
        
        String strHTML = stringWrite.ToString();
        
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Write("<link href=\"../Styles/Print.css?20120207\" rel=\"stylesheet\" type=\"text/css\" />");
        HttpContext.Current.Response.Write("<script src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.4.3/jquery.min.js\" type=\"text/javascript\"></script>");
        HttpContext.Current.Response.Write("<script src=\"../Scripts/print.js\" type=\"text/javascript\"></script>");
        HttpContext.Current.Response.Write(strHTML);
        HttpContext.Current.Response.Write("<script>window.print();</script>");
        HttpContext.Current.Response.End();
    }
}