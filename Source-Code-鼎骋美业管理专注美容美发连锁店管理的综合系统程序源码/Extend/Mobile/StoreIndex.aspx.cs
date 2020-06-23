using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.SQLDAL;

public partial class Extend_Mobile_StoreIndex : System.Web.UI.Page
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
        Response.Redirect("/Store/StoreIndex?id="+Mid);
    }
}