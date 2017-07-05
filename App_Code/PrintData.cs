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

public class CompanyData
{
    public String AccountType = String.Empty;
    public String AccountNumber = String.Empty;
    public String CompanyNumber = String.Empty;
    public String CompanyName = String.Empty;
    public String CompanyPhone = String.Empty;
    public String DeliverAddress = String.Empty;

    public String BillNumber = String.Empty;
    public String PayDays = String.Empty;
    public String Date = String.Empty;
    public String PayDate = String.Empty;

    public String SelfName = String.Empty;
    public String SelfNumber = String.Empty;
    public String SelfPhone = String.Empty;
    public String SelfFax = String.Empty;

    public String Note = String.Empty;
}

public class AmountData
{
    public String Num = String.Empty;
    public String Tax = String.Empty;
    public String Sum = String.Empty;
    public String Total = String.Empty;
    public String PrePay = String.Empty;
    public String LeftPay = String.Empty;
}

public class InvoiceData
{
    public AmountData amountData = null;
    public CompanyData companyData = null;
    public List<String[]> recordDataList = null;
}
