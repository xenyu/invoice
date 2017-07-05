using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public enum SizeCount
{
    Size0,
    Size1,
    Size2,
    Size3,
    Size4,
    Size5,
    Size6,
    Size7,
    Size8,
    Size9,
    Size10,
    SizeEnd
}

/// <summary>
/// AccountData 的摘要描述
/// </summary>
public class AccountData
{
    #region Members
    protected String m_Group = String.Empty;
    protected String m_Color = String.Empty;
    protected Int32[] m_Size = new Int32[(Int32)SizeCount.SizeEnd];//每一個尺寸的數量
    protected Guid[] m_Product = new Guid[(Int32)SizeCount.SizeEnd];//每一個尺寸的商品ProductId
    protected Int32 m_Quantity = 0;
    protected Double m_Amount = 0;
    protected Double m_Price = 0;
    #endregion

    #region Properties
    public String Group { get { return m_Group; } set { m_Group = value; } }
    public String Color { get { return m_Color; } set { m_Color = value; } }
    
    public Int32 Size0 { get { return m_Size[0]; } set { m_Size[0] = value; } }
    public Int32 Size1 { get { return m_Size[1]; } set { m_Size[1] = value; } }
    public Int32 Size2 { get { return m_Size[2]; } set { m_Size[2] = value; } }
    public Int32 Size3 { get { return m_Size[3]; } set { m_Size[3] = value; } }
    public Int32 Size4 { get { return m_Size[4]; } set { m_Size[4] = value; } }
    public Int32 Size5 { get { return m_Size[5]; } set { m_Size[5] = value; } }
    public Int32 Size6 { get { return m_Size[6]; } set { m_Size[6] = value; } }
    public Int32 Size7 { get { return m_Size[7]; } set { m_Size[7] = value; } }
    public Int32 Size8 { get { return m_Size[8]; } set { m_Size[8] = value; } }
    public Int32 Size9 { get { return m_Size[9]; } set { m_Size[9] = value; } }
    public Int32 Size10 { get { return m_Size[10]; } set { m_Size[10] = value; } }

    public Guid Product0 { get { return m_Product[0]; } set { m_Product[0] = value; } }
    public Guid Product1 { get { return m_Product[1]; } set { m_Product[1] = value; } }
    public Guid Product2 { get { return m_Product[2]; } set { m_Product[2] = value; } }
    public Guid Product3 { get { return m_Product[3]; } set { m_Product[3] = value; } }
    public Guid Product4 { get { return m_Product[4]; } set { m_Product[4] = value; } }
    public Guid Product5 { get { return m_Product[5]; } set { m_Product[5] = value; } }
    public Guid Product6 { get { return m_Product[6]; } set { m_Product[6] = value; } }
    public Guid Product7 { get { return m_Product[7]; } set { m_Product[7] = value; } }
    public Guid Product8 { get { return m_Product[8]; } set { m_Product[8] = value; } }
    public Guid Product9 { get { return m_Product[9]; } set { m_Product[9] = value; } }
    public Guid Product10 { get { return m_Product[10]; } set { m_Product[10] = value; } }

    public Int32 Quantity { get { return m_Quantity; } set { m_Quantity = value; } }
    public Double Amount { get { return m_Amount; } set { m_Amount = value; } }
    public Double Price { get { return m_Price; } set { m_Price = value; } }
    #endregion

    public AccountData()
	{
	}

    public object[] ToArray()
    {
        object[] data = { m_Group, m_Color, Size0, Size1, Size2, Size3, Size4, Size5, Size6, Size7, Size8, Size9, Size10, m_Price, m_Quantity, m_Amount };
        return data;
    }
}
