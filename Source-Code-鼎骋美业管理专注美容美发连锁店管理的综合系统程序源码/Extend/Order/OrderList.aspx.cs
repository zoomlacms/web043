using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;
using ZoomLa.Sns;
public partial class Extend_Order_OrderList : System.Web.UI.Page
{
    B_OrderList orderBll = new B_OrderList();
    B_Product proBll = new B_Product();
    B_CartPro cpBll = new B_CartPro();
    B_User buser = new B_User();
    B_Payment payBll = new B_Payment();
    B_PayPlat platBll = new B_PayPlat();
    OrderCommon orderCom = new OrderCommon();
    private DataTable OrderDT
    {
        get { return ViewState["CartDT"] as DataTable; }
        set { ViewState["CartDT"] = value; }
    }
    private DataTable StoreDT
    {
        get { return ViewState["StoreDT"] as DataTable; }
        set { ViewState["StoreDT"] = value; }
    }
    //-------------------------------------
    public string OrderType
    {
        get
        {
            return DataConvert.CStr(Request.QueryString["OrderType"]);
        }
    }
    public string Skey
    {
        get { return Request.QueryString["Skey"]; }
    }
    //all,receive,needpay,comment
    public string Filter
    {
        get
        {
            return DataConvert.CStr(Request.QueryString["Filter"]);
        }
    }
    //--------------------------------
    //客人的用户ID
    public int ClientUid { get { return DataConvert.CLng(Request.QueryString["cuid"]); } }      
    protected void Page_Load(object sender, EventArgs e)
    {
        Order_RPT.SPage = SelPage;
        if (!IsPostBack)
        {
            Order_RPT.DataBind();
        }
    }
    private DataTable SelPage(int pageSize, int pageIndex)
    {
        M_UserInfo mu = buser.GetLogin();
        Filter_Order filter = new Filter_Order();
        filter.cpage = pageIndex;
        filter.psize = pageSize;
        filter.orderType = OrderType;
        filter.uids = ClientUid.ToString();
        filter.storeType = mu.SiteID.ToString();

        PageSetting setting = ExOrder.Order_Sel(filter);
        OrderDT = setting.dt;
        StoreDT = orderCom.SelStoreDT(OrderDT);
        //Skey_T.Text = Skey;

        DataTable dt = new DataTableHelper().DistinctByField(OrderDT, "ID");
        if (dt != null && dt.Rows.Count > 0)
        {
            TotalSum_Hid.Value = DataConvert.CDouble(OrderDT.Compute("SUM(ordersamount)", "")).ToString("f2");
            //function.Script(this, "CheckOrderType('" + Filter + "')");
            Order_RPT.ItemCount = setting.itemCount;
            empty_div.Visible = false;
        }
        else { empty_div.Visible = true; TotalSum_Hid.Value = "0"; }
        return dt;

    }
    //获取订单操作按钮,分为已下单(未付款),已下单(已付款),已完结(显示)
    public string Getoperator(DataRowView dr)
    {
        M_OrderList orderMod = new M_OrderList();
        string result = "";
        int orderStatus = DataConverter.CLng(dr["OrderStatus"]);
        string payNo = DataConvert.CStr(dr["PaymentNo"]);
        int oid = Convert.ToInt32(dr["ID"]);
        int aside = Convert.ToInt32(dr["Aside"]);
        string orderNo = dr["OrderNo"].ToString();
        //----------如果已经删除,则不执行其余的判断
        //if (aside != 0)//如果已删除,则不进行其余的判断
        //{
        //    result += "<div><a href='javascript:;' data-oid='" + oid + "' onclick='ConfirmAction(this,\"reconver\");'>还原订单</a></div>";
        //    result += "<div><a href='javascript:;' data-oid='" + oid + "' onclick='ConfirmAction(this,\"realdel\");'>彻底删除</a></div>";
        //    return result;
        //}
        if (string.IsNullOrEmpty(payNo))//未付款,显示倒计时和付款链接
        {
            //bool isnormal = true;
            ////订单未完成,且为正常状态,显示付款
            //if (isnormal && OrderHelper.IsCanPay(dr.Row))
            //{
            //    result += "<div><a href='/Payonline/OrderPay.aspx?OrderCode=" + orderNo + "' target='_blank' class='btn btn-primary'>前往付款</a></div>";
            //}
            //result += "<div><a href='javascript:;' data-oid='" + oid + "' onclick='ConfirmAction(this,\"del\");'>移除订单</a></div>";
           // result += "<a href='/Extend/Order/FastOrder.aspx?ID=" + dr["ID"] + "' class='btn btn-info btn-sm'>快速开单</a>";
        }
        else
        {
            //已付款,但处理申请退款等状态
            if (orderStatus < (int)M_OrderList.StatusEnum.Normal)
            {
                //result += "<a href='AddShare.aspx?CartID=" + dr["CartID"] + "'>取消退款</a><br />";
            }
        }
        return result;
    }
    public string GetImgUrl()
    {
        return function.GetImgUrl(Eval("Thumbnails"));
    }
    public string GetShopUrl()
    {
        return OrderHelper.GetShopUrl(DataConvert.CLng(Eval("StoreID")), Convert.ToInt32(Eval("ProID")));
    }
    public string GetMessage()
    {
        if (Convert.ToInt32(Eval("OrderType")) == 7)
        {
            return "<tr class='idm_tr'><td colspan='6'><p class='idcmessage'>详记：" + Eval("Ordermessage", "") + "</p></td></tr>";
        }
        else return "";
    }
    public string GetPayInfo()
    {
        if (!string.IsNullOrEmpty(Eval("PaymentNo", "")))
        {
            return Eval("PaymentNo", "");
        }
        else { return OrderHelper.GetPayStatus(Convert.ToInt32(Eval("PaymentStatus"))); }
    }
    //-----------------------------------------------------
    protected void Order_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rpt = e.Item.FindControl("Pro_RPT") as Repeater;
            if (rpt == null)
            {
                return;
            }
            DataRowView drv = e.Item.DataItem as DataRowView;

            OrderDT.DefaultView.RowFilter = "OrderNo='" + drv["OrderNo"] + "'";
            rpt.DataSource = OrderDT.DefaultView.ToTable();
            rpt.DataBind();
        }
    }
    protected void Order_RPT_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        switch (e.CommandName)
        {
            case "del2":
                {
                    orderBll.ChangeStatus(id.ToString(), "recycle");
                }
                break;
        }
        Order_RPT.DataBind();
    }
    protected void Order_RPT_PreRender(object sender, EventArgs e)
    {
        ViewState["StoreDT"] = null;
        ViewState["OrderDT"] = null;
    }
    protected void Pro_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //如果变复杂,将其分离为局部页
            if (e.Item.ItemIndex == 0)//首行判断
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                M_Payment payMod = OrderHelper.GetPaymentByOrderNo(dr.Row);
                int count = OrderDT.Select("id=" + dr["ID"]).Length;
                string html = "";
                //收货人
                html += "<td class='td_l rowtd' rowspan='" + count + "'>";
                html += "<div>会员:<a href='javascript:;'>"+dr["rename"]+"</a></div>";
                html += "<div>收银:<a href='javascript:;'>"+dr["AddUser"]+"</a></div>";
                //html += "<i class='fa fa-user'></i> <a href='OrderList.aspx?UserID=" + dr["UserID"] + "' title='按用户筛选'>" + B_User.GetUserName(dr["HoneyName"], dr["AddUser"]) + "</a>";
                //html += "(<a href='javascript:;' onclick='user.showuinfo(" + dr["UserID"] + ");' title='查看用户信息'><span style='color:green;'>" + dr["UserID"] + "</span></a>)";
                html += "</td>";
                //金额栏
                html += "<td class='td_l rowtd' rowspan='" + count + "' style='font-size:12px;'>总计:<i class='fa fa-rmb'></i> " + Convert.ToDouble(dr["OrdersAmount"]).ToString("f2") + "<br />";
                string _paytypeHtml = OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.PayType);
                if (!string.IsNullOrEmpty(_paytypeHtml)) { _paytypeHtml = _paytypeHtml + "<br />"; }
                html += _paytypeHtml;
                html += "<div>商品:" + (DataConvert.CDouble(dr["OrdersAmount"]) - DataConvert.CDouble(dr["Freight"])).ToString("F2") + "</div>";
                //html += "<div>运费:" + DataConvert.CDouble(dr["Freight"]).ToString("F2") + "</div>";
                html += "<div title='优惠券'>优惠:" + payMod.ArriveMoney.ToString("F2") + "</div>";
                //html += "<div>积分:" + payMod.UsePointArrive.ToString("f2") + "(" + payMod.UsePoint.ToString("F0") + ")</div>";
                html += "<div style='color:#d9534f;'>需付:" + payMod.MoneyReal.ToString("F2") + "</div>";
                if (!string.IsNullOrEmpty(DataConvert.CStr(dr["PaymentNo"])))
                {
                    string plat = ExOrder.ShowPayPlatName(DataConvert.CLng(dr["PayPlatID"]));
                    html += "<span style='color:#337ab7;'>" + plat + "</span>"
                        + "(" + OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.Pay) + ")</a>";
                }
                else
                {
                    html += "(" + OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.Pay) + ")";
                }
                html += "</td>";
                //订单状态
                //html += "<td class='td_md rowtd' rowspan='" + count + "'>";
                //int ordertype = DataConvert.CLng(dr["OrderType"]);
                //html += "</td>";
                //操作栏
                html += "<td class='td_md rowtd' rowspan='" + count + "'></td>";
                (e.Item.FindControl("Order_Lit") as Literal).Text = html;
            }
        }
    }
}