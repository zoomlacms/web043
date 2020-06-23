using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.Sns;

public partial class Extend_UA_ERole : System.Web.UI.Page
{
    B_Ex_ERole erBll = new B_Ex_ERole();
    B_User buser = new B_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("manage");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    private void MyBind()
    {
        M_UserInfo mu = buser.GetLogin();
        EGV.DataSource = erBll.Sel(new Com_Filter()
        {
            storeId = mu.SiteID
        });
        EGV.DataBind();
    }
    protected void EGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EGV.PageIndex = e.NewPageIndex;
        MyBind();
    }
    protected void EGV_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del2":
                erBll.Del(e.CommandArgument.ToString());
                break;
        }
        MyBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}