using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public class Message
{
    public Message()
    {
    }

    public static void LabelError(Label label, String text)
    {
        if (String.IsNullOrEmpty(text))
        {
            label.Visible = false;
        }
        else
        {
            label.ForeColor = Color.Red;
            label.Text = text;
            label.Visible = true;
        }
    }

    public static void LabelMessage(Label label, String text)
    {
        if (String.IsNullOrEmpty(text))
        {
            label.Visible = false;
        }
        else
        {
            label.ForeColor = Color.Blue;
            label.Text = text;
            label.Visible = true;
        }
    }
}
