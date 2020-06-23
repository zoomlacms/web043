using Newtonsoft.Json;
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
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_Order_PreOrder : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_Product proBll = new B_Product();
    B_OrderList orderBll = new B_OrderList();
    B_CartPro cpBll = new B_CartPro();
    B_Ex_PreOrder poBll = new B_Ex_PreOrder();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("cash");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    private void MyBind()
    {
        M_UserInfo mu = buser.GetLogin();
        EGV.DataSource = poBll.Sel(new Com_Filter()
        {
            storeId = mu.SiteID,
            status = "0",
            uname = "",//顾客姓名
            addon = ""//技师姓名
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
                poBll.Del(e.CommandArgument.ToString());
                break;
            case "fast":
                //生成订单后,跳转至指定界面
                M_UserInfo mu = buser.GetLogin();
                M_UserInfo client = new M_UserInfo();
                M_Ex_PreOrder poMod = poBll.SelReturnModel(Convert.ToInt32(e.CommandArgument));
                if (poMod == null) { function.WriteErrMsg("预约单不存在"); }
                if (poMod.StoreID != mu.SiteID) { function.WriteErrMsg("不可操作该订单"); }
                if (poMod.ZStatus != 0) { function.WriteErrMsg("预约单状态不正确"); }
                DataTable infoDT = JsonConvert.DeserializeObject<DataTable>(poMod.ClientNeed);
                //----------------------------------
                List<M_CartPro> cpList = new List<M_CartPro>();
                M_OrderList orderMod = orderBll.NewOrder(mu, M_OrderList.OrderEnum.Normal);
                orderMod.Userid = mu.UserID;
                orderMod.AddUser = mu.UserName;//收银员名称
                orderMod.Receiver = client.UserID.ToString();
                orderMod.Rename = client.UserName;
                orderMod.StoreID = mu.SiteID;
                orderMod.Ordersamount = 0;
                foreach (DataRow dr in infoDT.Rows)
                {
                    M_CartPro cpMod = new M_CartPro();
                    M_Product proMod = proBll.GetproductByid(Convert.ToInt32(dr["ProID"]));
                    cpMod.ProID = proMod.ID;
                    cpMod.Proname = proMod.Proname;
                    cpMod.Danwei = proMod.ProUnit;
                    cpMod.Shijia = proMod.LinPrice;
                    cpMod.Pronum = 1;
                    cpMod.AllMoney = cpMod.Shijia * cpMod.Pronum;
                    orderMod.Ordersamount += cpMod.AllMoney;
                    //-------------------------
                    cpMod.Username = client.UserName;
                    cpMod.StoreID = mu.SiteID;
                    cpMod.Addtime = DateTime.Now;
                    cpMod.code = DataConvert.CLng(dr["empid"]).ToString();//技师ID
                    cpMod.ProInfo = DataConvert.CStr(dr["empname"]);//技师姓名
                    cpList.Add(cpMod);
                }
                orderMod.id = orderBll.insert(orderMod);
                foreach (M_CartPro cpMod in cpList)
                {
                    cpMod.Orderlistid = orderMod.id;
                    cpMod.ID = cpBll.GetInsert(cpMod);
                }
                poMod.ZStatus = 99;
                poBll.UpdateByID(poMod);
                Response.Redirect("/Extend/Order/FastOrder.aspx?ID=" + orderMod.id);
                break;
        }
        MyBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = e.Row.DataItem as DataRowView;
            Repeater rpt = e.Row.FindControl("Product_RPT") as Repeater;
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(DataConvert.CStr(dr["ClientNeed"]));
            rpt.DataSource = dt;
            rpt.DataBind();
        }
    }
}