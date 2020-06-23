using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.Sns;

public partial class Extend_Report_Asset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("sale");
    }
}