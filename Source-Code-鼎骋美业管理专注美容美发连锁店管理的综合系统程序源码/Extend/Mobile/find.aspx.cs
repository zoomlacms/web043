using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Model.Shop;
using ZoomLa.SQLDAL;

public partial class Extend_Mobile_find : System.Web.UI.Page
{
    public int Mid
    {
        get
        {
            if (!string.IsNullOrEmpty(Request["ID"])) { return DataConvert.CLng(Request["ID"]); }
            else if (Request.Cookies["sid"] != null)
            {
                return DataConvert.CLng(Request.Cookies["sid"].Value);
            }
            else { return 0; }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        M_Store_Info storeMod = new B_Store_Info().SelReturnModel(Mid);
        Response.Redirect("/Class_15/Default.aspx?id=" + Mid + "&isd=" + storeMod.UserName);
    }
}