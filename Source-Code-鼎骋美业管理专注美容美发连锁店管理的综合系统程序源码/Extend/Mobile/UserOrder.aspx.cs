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
public partial class Extend_Mobile_UserOrder : System.Web.UI.Page
{
    B_OrderList orderBll = new B_OrderList();
    B_Product proBll = new B_Product();
    B_CartPro cpBll = new B_CartPro();
    B_User buser = new B_User();
    B_Payment payBll = new B_Payment();
    B_PayPlat platBll = new B_PayPlat();
    OrderCommon orderCom = new OrderCommon();
    public string BaseUrl = "OrderList.aspx?OrderType=" + SafeSC.GetRequest("OrderType");
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
    protected void Page_Load(object sender, EventArgs e)
    {
        Order_RPT.SPage = SelPage;
        if (!IsPostBack)
        {
            //if (!string.IsNullOrEmpty(Request["UserID"])) { UserID_T.Text = Request["UserID"]; }
            //if (!string.IsNullOrEmpty(Request["storeType"])) { StoreType_DP.SelectedValue = Request["storeType"]; }
            //DataRow countdr = cpBll.SelOrderCount(StoreType_DP.SelectedValue, OrderType).Rows[0];
            //order_all_l.Text += "(" + countdr["all"] + ")";
            //order_unpaid_l.Text += "(" + countdr["unpaid"] + ")";
            //order_prepay_l.Text += "(" + countdr["prepay"] + ")";
            //order_paid_l.Text += "(" + countdr["paid"] + ")";
            //order_unexp_l.Text += "(" + countdr["unexp"] + ")";
            //order_exped_l.Text += "(" + countdr["exped"] + ")";
            //order_finished_l.Text += "(" + countdr["finished"] + ")";
            //order_unrefund_l.Text += "(" + countdr["unrefund"] + ")";
            //order_refunded_l.Text += "(" + countdr["refunded"] + ")";
            //order_recycle_l.Text += "(" + countdr["recycle"] + ")";
            Order_RPT.DataBind();
            //if (Filter.Equals("recycle")) { op_recycle.Visible = true; }
            //else { op_normal.Visible = true; }
        }
    }
    private DataTable SelPage(int pageSize, int pageIndex)
    {
        M_UserInfo mu = buser.GetLogin();
        Filter_Order filter = new Filter_Order();
        filter.cpage = pageIndex;
        filter.psize = pageSize;
        //filter.storeType = StoreType_DP.SelectedValue;
        filter.orderType = "-100";
        //filter.addon = Filter;
        filter.uids = mu.UserID.ToString();
        //filter.proname = ProName_T.Text;
        //filter.orderno = OrderNo_T.Text;
        //filter.reuser = ReUser_T.Text;
        //filter.mobile = Mobile_T.Text;
        //filter.uids = UserID_T.Text;
        //filter.stype = SkeyType_DP.SelectedValue;
        //filter.skey = Skey_T.Text;
        //filter.stime = STime_T.Text;
        //filter.etime = ETime_T.Text;
        //filter.expstime = ExpSTime_T.Text;
        //filter.expetime = ExpETime_T.Text;
        PageSetting setting = ExHelper.Order_Sel(filter);
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
    protected void Export_Btn_Click(object sender, EventArgs e)
    {

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
            //订单过期判断
            if (SiteConfig.ShopConfig.OrderExpired > 0)
            {
                int seconds = GetOrderUnix(dr);
                if (seconds <= 0)
                { result += "<div class='gray9'>订单关闭</div>"; }//isnormal = false;
                else
                { result += "<div class='ordertime' data-time='" + seconds + "'></div>"; }
            }
            ////订单未完成,且为正常状态,显示付款
            //if (isnormal && OrderHelper.IsCanPay(dr.Row))
            //{
            //    result += "<div><a href='/Payonline/OrderPay.aspx?OrderCode=" + orderNo + "' target='_blank' class='btn btn-primary'>前往付款</a></div>";
            //}
            //result += "<div><a href='javascript:;' data-oid='" + oid + "' onclick='ConfirmAction(this,\"del\");'>移除订单</a></div>";
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
    //还差多少分钟到期
    public int GetOrderUnix(DataRowView dr)
    {
        DateTime addTime = Convert.ToDateTime(dr["AddTime"]);
        int seconds = (SiteConfig.ShopConfig.OrderExpired * 60 * 60) - (int)(DateTime.Now - addTime).TotalSeconds;
        return seconds;
    }
    //是否允许返修,退货等售后服务
    public string GetRepair()
    {
        //已完结状态才能返修,退款等售后,其他情况下申请订单退款
        string guess = DataConvert.CStr(Eval("GuessXML"));
        string result = "";
        int orderStatus = Convert.ToInt32(Eval("OrderStatus"));
        if (Eval("AddStatus").ToString().Contains("exchange"))
        {
            result += "<a href='javascript:;' class='gray9'>已申请换货</a>";
        }
        else if (Eval("AddStatus").ToString().Contains("repair"))
        {
            result += "<a href='javascript:;' class='gray9'>已申请返修</a>";
        }
        else if (Eval("AddStatus").ToString().Contains("drawback"))
        {
            result += "<a href='javascript:;' class='gray9'>已申请退货</a>";
        }
        //else if (!string.IsNullOrEmpty(guess) && orderStatus >= (int)M_OrderList.StatusEnum.OrderFinish)
        //{
        //    result += "<a href='ReqRepair.aspx?cid=" + Eval("CartID") + "' class='gray9'>返修/退换货</a>";
        //}
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
    public string GetStoreName(int storeid)
    {
        DataRow[] dr = StoreDT.Select("ID=" + storeid);
        if (dr.Length > 0) { return dr[0]["StoreName"].ToString(); }
        else { return ""; }
    }
    public string GetPayInfo()
    {
        if (!string.IsNullOrEmpty(Eval("PaymentNo", "")))
        {
            return Eval("PaymentNo", "");
        }
        else { return OrderHelper.GetPayStatus(Convert.ToInt32(Eval("PaymentStatus"))); }
    }
    public string GetExpSTime()
    {
        if (DataConvert.CLng(Eval("ExpressNum")) < 1) { return "<span style='color:red;'>未发货</span>"; }
        else
        {
            return Eval("ExpSTime").ToString();
        }
    }
    public string GetPUserInfo()
    {
        int puid = DataConvert.CLng(Eval("ParentUserID"));
        if (puid < 1) { return "无推荐人"; }
        else
        {
            string puname = buser.GetUserNameByIDS(puid.ToString());
            return "<a href=\"javascript:;\" onclick=\"user.showuinfo('" + puid + "')\">" + puname + "(" + puid + ")</a>";
        }
    }
    //-----------------------------------------------------
    protected void Order_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rpt = e.Item.FindControl("Pro_RPT") as Repeater;
            Label storeName = e.Item.FindControl("Store_L") as Label;
            if (rpt == null || storeName == null)
            {
                return;
            }
            DataRowView drv = e.Item.DataItem as DataRowView;
            storeName.Text = GetStoreName(Convert.ToInt32(drv["StoreID"]));

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
                html += "<i class='fa fa-user'></i> <a href='OrderList.aspx?UserID=" + dr["UserID"] + "' title='按用户筛选'>" + B_User.GetUserName(dr["HoneyName"], dr["AddUser"]) + "</a>";
                html += "(<a href='javascript:;' onclick='user.showuinfo(" + dr["UserID"] + ");' title='查看用户信息'><span style='color:green;'>" + dr["UserID"] + "</span></a>)";
                html += "</td>";
                //金额栏
                html += "<td class='td_l rowtd' rowspan='" + count + "' style='font-size:12px;'>总计:<i class='fa fa-rmb'></i> " + Convert.ToDouble(dr["OrdersAmount"]).ToString("f2") + "<br />";
                string _paytypeHtml = OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.PayType);
                if (!string.IsNullOrEmpty(_paytypeHtml)) { _paytypeHtml = _paytypeHtml + "<br />"; }
                html += _paytypeHtml;
                html += "<div>商品:" + (DataConvert.CDouble(dr["OrdersAmount"]) - DataConvert.CDouble(dr["Freight"])).ToString("F2") + "</div>";
                html += "<div>运费:" + DataConvert.CDouble(dr["Freight"]).ToString("F2") + "</div>";
                html += "<div title='优惠券'>优惠:" + payMod.ArriveMoney.ToString("F2") + "</div>";
                html += "<div>积分:" + payMod.UsePointArrive.ToString("f2") + "(" + payMod.UsePoint.ToString("F0") + ")</div>";
                switch (payMod.PayType)
                {
                    case (int)M_OrderList.PayTypeEnum.PrePay:
                        try
                        {
                            M_PrePayinfo preInfo = new M_PrePayinfo(payMod.PrePayInfo);
                            string payedTlp = "<div>(<span style='color:green;'>已付款</span>:{0},{1})</div>";
                            string nopayTlp = "<div>(<span style='color:red;'>未付款</span>)</div>";
                            html += "<div style='color:#d9534f;'>定金:" + preInfo.money_pre.ToString("F2") + "</div>";
                            html += preInfo.money_pre_payed > 0 ? string.Format(payedTlp, preInfo.money_pre_payed.ToString("F2"), preInfo.pre_payMethod) : nopayTlp;
                            html += "<div style='color:#d9534f'>尾款:" + preInfo.money_after.ToString("F2") + "</div>";
                            html += preInfo.money_after_payed > 0 ? string.Format(payedTlp, preInfo.money_after_payed.ToString("F2"), preInfo.after_payMethod) : nopayTlp;
                            html += "<div style='color:#d9534f'>总计:" + preInfo.money_total.ToString("F2") + "</div>";
                        }
                        catch
                        {
                            html += "预付信息错误";
                        }
                        break;
                    default:
                        html += "<div style='color:#d9534f;'>需付:" + payMod.MoneyReal.ToString("F2") + "</div>";
                        break;
                }
                if (!string.IsNullOrEmpty(DataConvert.CStr(dr["PaymentNo"])))
                {
                    string plat = platBll.GetPayPlatName(DataConvert.CStr(dr["PaymentNo"]));
                    html += "<a href='" + StrHelper.Url_AddParam(Request.RawUrl, "filter=paid") + "' title='筛选已付款订单'>" + "<span style='color:#337ab7;'>" + plat + "</span>"
                        + "(" + OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.Pay) + ")</a>";
                }
                else
                {
                    html += "(" + OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.Pay) + ")";
                }
                html += "</td>";
                //订单状态
                html += "<td class='td_md rowtd' rowspan='" + count + "'>";
                html += "<span class='gray9'>" + (DataConvert.CLng(dr["IsSure"]) == 1 ? "已确认" : "未确认") + "</span><br />";
                html += "<span class='gray9'>" + OrderHelper.GetStatus(dr.Row, OrderHelper.TypeEnum.Order) + "</span> <br />";
                int ordertype = DataConvert.CLng(dr["OrderType"]);
                //非旅游与酒店订单,则显示快递信息
                int expid = DataConvert.CLng(dr["ExpressNum"]);
                if (ordertype != 7 && ordertype != 9)
                {
                    html += "<a href='javascript:;' class='expview_a' data-expid='" + expid + "' id='expview_" + dr["ID"] + "_a' onclick=\"exp.apilink(this,'" + expid + "');\">" + OrderHelper.GetExpStatus(Convert.ToInt32(dr["StateLogistics"])) + "</span> <br/>";
                }
                html += "</td>";
                //操作栏
                html += "<td class='td_md rowtd' rowspan='" + count + "'><a href='OrderListInfo.aspx?ID=" + dr["ID"] + "' class='order_bspan'>订单详情</a><br/>" + Getoperator(dr) + "</td>";
                (e.Item.FindControl("Order_Lit") as Literal).Text = html;
            }
        }
    }
}