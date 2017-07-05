using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

public partial class Members_PrintAccount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InvoiceData accountPrintData = (InvoiceData)Session["InvoiceData"];
            if (null == accountPrintData) return;

            AmountData amountData = accountPrintData.amountData;
            CompanyData companyData = accountPrintData.companyData;
            List<String[]> recordDataList = accountPrintData.recordDataList;

            Title = Profile.PrincipalCompanyName + " - 進銷單據列印";

            #region CompanyData
            LabelAccountNumber.Text = companyData.AccountNumber;
            LabelAccountType.Text = companyData.AccountType;
            LabelBillNumber.Text = companyData.BillNumber;

            LabelCompanyName.Text = companyData.CompanyName;
            //Int32 length = Encoding.GetEncoding("Big5").GetByteCount(LabelCompanyName.Text);
            //if (length > 30)
            //{
            //    LabelCompanyName.Font.Size = 8;// 如果超過15個字，縮小字型
            //    if (length > 38)
            //        LabelCompanyName.Font.Size = 6;// 如果超過19個字，縮小字型
            //}

            LabelCompanyNumber.Text = companyData.CompanyNumber;

            LabelCompanyPhone.Text = companyData.CompanyPhone;
            //length = Encoding.GetEncoding("Big5").GetByteCount(LabelCompanyPhone.Text);
            //if (length > 30)
            //{
            //    LabelCompanyPhone.Font.Size = 8;// 如果超過15個字，縮小字型
            //    if (length > 38)
            //        LabelCompanyPhone.Font.Size = 6;// 如果超過19個字，縮小字型
            //}

            LabelDate.Text = companyData.Date;
            LabelDeliverAddress.Text = companyData.DeliverAddress;
            LabelPayDate.Text = companyData.PayDate;
            LabelSelfFax.Text = companyData.SelfFax;
            LabelSelfName.Text = companyData.SelfName;
            LabelSelfNumber.Text = companyData.SelfNumber;
            LabelSelfPhone.Text = companyData.SelfPhone;
            LabelNote.Text = companyData.Note;
            #endregion

            #region AmountData
            LabelTotalNum.Text = amountData.Num;
            LabelSum.Text = amountData.Sum;
            LabelLeftPay.Text = amountData.LeftPay;
            LabelPrePay.Text = amountData.PrePay;
            LabelTax.Text = amountData.Tax;
            LabelTotal.Text = amountData.Total;
            #endregion

            #region RecordData
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Group", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Color", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size0", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size1", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size2", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size3", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size4", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size5", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size6", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size7", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size8", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size9", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Size10", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Price", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Quantity", typeof(String)));
            dataTable.Columns.Add(new DataColumn("Amount", typeof(String)));

            foreach (String[] recordData in recordDataList)
                dataTable.Rows.Add(recordData);
            #endregion

            GridViewAccount.DataSource = dataTable;
            GridViewAccount.DataBind();

            Response.Write("<script>window.print();</script>");
        }
    }
}
