using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;

public partial class Extend_Employee : System.Web.UI.Page
{
    /*
     * 1,直接以员工姓名作为用户名
     * 2,员工SiteID为所属店铺,PageID==角色身份
     * 3,员工组=4,顾客组=5
     */ 
    B_User buser = new B_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("employee");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    private void MyBind()
    {
        M_UserInfo mu = buser.GetLogin();
        EGV.DataSource = ExHelper.Employ_Sel(new F_User() {
            storeId = mu.SiteID,
        });
        EGV.DataBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView dr = e.Row.DataItem as DataRowView;
        //    e.Row.Attributes.Add("ondblclick", "location='DeliveryManAdd.aspx?ID=" + dr["ID"] + "'");
        //}
    }
    protected void EGV_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToLower())
        {
            case "del"://只锁定员工,不能删除
                string ids = e.CommandArgument.ToString();
                buser.BatAudit(ids, 1);
                break;
        }
        MyBind();
    }
    protected void EGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EGV.PageIndex = e.NewPageIndex;
        MyBind();
    }
}