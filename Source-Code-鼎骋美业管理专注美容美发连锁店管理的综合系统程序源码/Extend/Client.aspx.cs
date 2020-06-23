using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_Client : System.Web.UI.Page
{
    B_User_StoreUser ctBll = new B_User_StoreUser();
    B_User buser = new B_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("client");
        RPT.SPage = MyBind;
        if (!IsPostBack)
        {
            RPT.DataBind();
        }
    }
    private DataTable MyBind(int pageSize, int pageIndex)
    {
        M_UserInfo mu = buser.GetLogin();
        PageSetting setting = ctBll.SelPage(pageIndex, pageSize, new Com_Filter()
        {
            storeId = mu.SiteID,
            uname = UName_T.Text.Trim()
        });
        RPT.ItemCount = setting.itemCount;
        return setting.dt;
    }
    protected void Search_Btn_Click(object sender, EventArgs e)
    {
        RPT.DataBind();
    }
}