using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Model;
using ZoomLa.Model.Shop;

public partial class Extend_UA_StoreInfo : System.Web.UI.Page
{
    B_User buser = new B_User();
    M_Store_Info storeMod = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        if (!IsPostBack)
        {
            storeMod = new B_Store_Info().SelReturnModel(mu.SiteID);
            if()
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {

    }
}