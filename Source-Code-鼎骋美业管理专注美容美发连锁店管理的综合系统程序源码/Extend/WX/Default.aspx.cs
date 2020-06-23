using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_WX_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        string url = "";
        M_CommonData storeMod = ExHelper.Store2_User();
        if (DataConvert.CLng(storeMod.SpecialID) < 1)
        { url = "/Extend/WX/WXConfig.aspx"; Response.Redirect(url); }
       
    }
}