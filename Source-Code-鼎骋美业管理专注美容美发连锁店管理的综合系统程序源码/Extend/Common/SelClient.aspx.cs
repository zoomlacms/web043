using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_Common_SelClient : System.Web.UI.Page
{
    B_User_StoreUser suBll = new B_User_StoreUser();
    B_User buser = new B_User();
    public string addon { get { return DataConvert.CStr(Request.QueryString["addon"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        RPT.SPage = MyBind;
        switch (addon)
        {
            case "wechat":
                function.Script(this, "enableMulti();");
                break;
        }
        if (!IsPostBack)
        {
            RPT.DataBind();
        }
    }
    private DataTable MyBind(int pageSize, int pageIndex)
    {
        M_UserInfo mu = buser.GetLogin();
        Com_Filter filter = new Com_Filter()
        {
            storeId = mu.SiteID,
            uname = UName_T.Text.Trim(),
            skey = Label_T.Text.Trim()
        };
        filter.addon = addon;
        PageSetting setting = suBll.SelPage(pageIndex, pageSize, filter);
        RPT.ItemCount = setting.itemCount;
        return setting.dt;
    }

    protected void Skey_Btn_Click(object sender, EventArgs e)
    {
        RPT.DataBind();
    }
}