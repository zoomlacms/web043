using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Sns.BLL;
using ZoomLa.Sns;
using ZoomLa.Model;

public partial class Extend_UA_Bonus : System.Web.UI.Page
{
    B_Shop_Bonus bnBll = new B_Shop_Bonus();
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
        EGV.DataSource = bnBll.Sel(new Com_Filter()
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
                bnBll.Del(e.CommandArgument.ToString());
                break;
        }
        MyBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView dr = e.Row.DataItem as DataRowView;
        //    e.Row.Attributes.Add("ondblclick", "location='AddEnglishQuestion.aspx?ID=" + dr["ID"] + "'");
        //}
    }
}