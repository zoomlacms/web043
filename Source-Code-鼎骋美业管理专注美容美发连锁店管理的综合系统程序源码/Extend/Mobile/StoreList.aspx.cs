using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL.Shop;

public partial class Extend_Mobile_StoreList : System.Web.UI.Page
{
    B_Store_Info storeBll = new B_Store_Info();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = storeBll.SelPage(1, 20, new F_StoreInfo()
            {

            }).dt;
            RPT.DataSource = dt;
            RPT.DataBind();
        }
    }
}