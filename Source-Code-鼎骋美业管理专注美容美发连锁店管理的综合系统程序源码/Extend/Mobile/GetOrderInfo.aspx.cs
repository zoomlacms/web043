using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;
public partial class Extend_Mobile_GetOrderInfo : System.Web.UI.Page
{
    //用户到店消费,不需要地址等
    B_Arrive avBll = new B_Arrive();
    B_Cart cartBll = new B_Cart();
    B_CartPro cartProBll = new B_CartPro();
    B_User buser = new B_User();
    B_UserRecei receBll = new B_UserRecei();
    B_OrderList orderBll = new B_OrderList();
    B_OrderBaseField fieldBll = new B_OrderBaseField();
    B_Shop_FareTlp fareBll = new B_Shop_FareTlp();
    B_Product proBll = new B_Product();
    B_Payment payBll = new B_Payment();
    B_Shop_RegionPrice regionBll = new B_Shop_RegionPrice();
    B_Shop_Present ptBll = new B_Shop_Present();
    B_Order_Invoice invBll = new B_Order_Invoice();
    OrderCommon orderCom = new OrderCommon();
    private double allmoney = 0;//购物车中商品金额统计
    public int ProClass = 1;
    public int StoreID { get { return DataConvert.CLng(Request.Cookies["sid"].Value); } }
    public string ids { get { return Request.QueryString["ids"]; } }
    private DataTable CartDT
    {
        get
        {
            return (DataTable)ViewState["CartDT"];
        }
        set { ViewState["CartDT"] = value; }
    }
    /*----------------------------------------------------------------------------------------------------*/
    protected void Page_Load(object sender, EventArgs e)
    {
        B_User.CheckIsLogged(Request.RawUrl);
        M_UserInfo mu = buser.GetLogin();
        if (mu.Status != 0) { function.WriteErrMsg("你的帐户未通过验证或被锁定"); }

        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(ids)) { function.WriteErrMsg("请先选定需要购买的商品"); }
            ReUrl_A1.HRef = "/Cart/Cart.aspx?Proclass=" + ProClass;
            ReUrl_A1.Visible = true;
            MyBind();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        CartDT = null;
    }
    public void MyBind()
    {
        StringWriter sw = new StringWriter();
        M_UserInfo mu = buser.GetLogin(false);
        CartDT = cartBll.SelByCartID(B_Cart.GetCartID(), mu.UserID, ProClass, ids);
        CartDT.DefaultView.RowFilter = "StoreID=" + StoreID;
        CartDT = CartDT.DefaultView.ToTable();
        if (CartDT.Rows.Count < 1)
        {
            function.WriteErrMsg("你尚未选择商品,<a href='/Extend/Mobile/UserOrder.aspx'>查看我的订单</a>");
        }
        //------核算费用
        allmoney = UpdateCartAllMoney(CartDT);
        //------费用统计
        itemnum_span.InnerText = CartDT.Rows.Count.ToString();
        totalmoney_span1.InnerText = allmoney.ToString("f2");
        //------店铺
        Store_RPT.DataSource = orderCom.SelStoreDT(CartDT);
        Store_RPT.DataBind();
        //------发票绑定
        Server.Execute("/Cart/Comp/Invoice.aspx", sw);
        sw = new StringWriter();
        //------积分抵扣
        //{
        //    int usepoint = Point_CanBeUse(allmoney);
        //    if (usepoint > 0)
        //    {
        //        point_body.Visible = true;
        //        Point_L.Text = mu.UserExp.ToString();
        //        //int usepoint = (int)(allmoney * (SiteConfig.ShopConfig.PointRatiot * 0.01));
        //        function.Script(this, "SumByPoint(" + usepoint + ");");
        //        PointRate_Hid.Value = SiteConfig.ShopConfig.PointRate.ToString("F2");
        //    }
        //    else
        //    {
        //        point_tips.Visible = true;
        //    }
        //}
        //------优惠券
        Server.Execute("/Cart/Comp/ArriveList.aspx?allmoney=" + allmoney + "&ids=" + ids, sw);
        Arrive_Lit.Text = sw.ToString();
    }
    protected void Store_RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;
            //按店铺展示商品列表
            Repeater rpt = e.Item.FindControl("ProRPT") as Repeater;
            CartDT.DefaultView.RowFilter = "StoreID=" + drv["ID"];
            DataTable dt = CartDT.DefaultView.ToTable();
            if (dt.Rows.Count < 1) { e.Item.Visible = false; }
            else
            {
                rpt.DataSource = dt;
                rpt.DataBind();
                //运费计算";
                //Literal html_lit = e.Item.FindControl("FareType_L") as Literal;
                //DataTable fareDT = GetFareDT(dt);
                //html_lit.Text = CreateFareHtml(fareDT);
            }
        }
    }
    protected void ProRPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dr = e.Item.DataItem as DataRowView;
            Label ptLabel = e.Item.FindControl("Present_HTML") as Label;
            ptLabel.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_present.aspx", dr);
        }
    }
    protected void AddOrder_Btn_Click(object sender, EventArgs e)
    {
        //1,生成订单,2,关联购物车中商品为已绑定订单
        M_UserInfo mu = buser.GetLogin(false);
        CartDT = cartBll.SelByCartID(B_Cart.GetCartID(), mu.UserID, ProClass, ids);//需要购买的商品
        if (CartDT.Rows.Count < 1) function.WriteErrMsg("你尚未选择商品,<a href='/Extend/Mobile/UserOrder.aspx'>查看我的订单</a>");
        CartDT.DefaultView.RowFilter = "StoreID=" + StoreID;
        CartDT = CartDT.DefaultView.ToTable();
        //------生成订单前检测区
        foreach (DataRow dr in CartDT.Rows)
        {
            if (!HasStock(dr["Allowed"], DataConvert.CLng(dr["stock"]), Convert.ToInt32(dr["Pronum"])))
                function.WriteErrMsg("购买失败," + dr["proname"] + "的库存数量不足");
        }
        //------检测End
        //按店铺生成订单
        DataTable storeDT = CartDT.DefaultView.ToTable(true, "StoreID");
        List<M_OrderList> orderList = new List<M_OrderList>();//用于生成临时订单,统计计算(Disuse)
        foreach (DataRow dr in storeDT.Rows)
        {
            #region 暂不使用字段
            //Odata.province = this.DropDownList1.SelectedValue;
            //Odata.city = this.DropDownList2.SelectedValue;//将地址省份与市ID存入,XML数据源
            //Odata.Guojia = "";//国家
            //Odata.Chengshi = DropDownList2.SelectedItem.Text;//城市
            //Odata.Diqu = DropDownList3.SelectedItem.Text;//地区
            //Odata.Delivery = DataConverter.CLng(Request.Form["Delivery"]);
            //Odata.Deliverytime = DataConverter.CLng(this.Deliverytime.Text);
            //Odata.Mobile = receMod.MobileNum;
            #endregion
            M_OrderList Odata = new M_OrderList();
            //通过微信支付的预购订单[special]
            Odata.Ordertype = (int)M_OrderList.OrderEnum.Cloud;
            Odata.OrderNo = B_OrderList.CreateOrderNo((M_OrderList.OrderEnum)Odata.Ordertype);
            Odata.StoreID = Convert.ToInt32(dr["StoreID"]);
            CartDT.DefaultView.RowFilter = "StoreID=" + Odata.StoreID;
            DataTable storeCartDT = CartDT.DefaultView.ToTable();
            //M_UserRecei receMod = receBll.GetSelect(arsID, mu.UserID);
            //if (receMod == null) { function.WriteErrMsg("用户尚未选择送货地址,或地址不存在"); }

            //直接使用微信获取的用户信息填充
            Odata.Receiver = mu.UserID.ToString();
            Odata.Rename = mu.HoneyName;
            Odata.AddUser = mu.UserName; ;
            Odata.Userid = mu.UserID;
            //Odata.Phone = receMod.phone;
            //Odata.MobileNum = receMod.MobileNum;
            //Odata.Email = receMod.Email;
            //Odata.Shengfen = receMod.Provinces;
            //Odata.Jiedao = receMod.Street;
            //Odata.ZipCode = receMod.Zipcode;


            Odata.Invoiceneeds = DataConverter.CLng(Request.Form["invoice_rad"]);//是否需开发票

            Odata.Ordermessage = ORemind_T.Text;//订货留言
                                                //-----金额计算
            Odata.Balance_price = GetTotalMoney(storeCartDT);
            Odata.Freight = 0;
            Odata.Ordersamount = Odata.Balance_price + Odata.Freight;//订单金额
            Odata.AllMoney_Json = orderCom.GetTotalJson(storeCartDT);//附加需要的虚拟币
            Odata.Specifiedprice = Odata.Ordersamount; //订单金额;
            Odata.OrderStatus = (int)M_OrderList.StatusEnum.Normal;//订单状态
            Odata.Paymentstatus = (int)M_OrderList.PayEnum.NoPay;//付款状态
                                                                 //Odata.Integral = DataConverter.CLng(Request.QueryString["jifen"]);
            //Odata.ExpTime = exptime_hid.Value;
            Odata.id = orderBll.insert(Odata);
            cartProBll.CopyToCartPro(mu, storeCartDT, Odata.id);
            orderList.Add(Odata);
            orderCom.SendMessage(Odata, null, "ordered");
        }
        cartBll.DelByids(ids);
        //-----------------订单生成后处理
        //进行减库存等操作
        foreach (DataRow dr in CartDT.Rows)
        {
            M_Product model = proBll.GetproductByid(Convert.ToInt32(dr["Proid"]));
            model.Stock = model.Stock - DataConvert.CLng(dr["Pronum"]);
            SqlHelper.ExecuteSql("Update ZL_Commodities Set Stock=" + model.Stock + " Where ID=" + model.ID);
        }
        //生成支付单,处理优惠券,并进入付款步骤
        M_Payment payMod = payBll.CreateByOrder(orderList);
        //优惠券
        //if (!string.IsNullOrEmpty(Request.Form["Arrive_Hid"]))
        //{
        //    M_Arrive avMod = avBll.SelModelByFlow(Request.Form["Arrive_Hid"], mu.UserID);
        //    double money = payMod.MoneyPay;
        //    string remind = "支付单抵扣[" + payMod.PayNo + "]";
        //    M_Arrive_Result retMod = avBll.U_UseArrive(avMod, mu.UserID, cartDT, money, remind);
        //    if (retMod.enabled)
        //    {
        //        payMod.MoneyPay = retMod.money;
        //        payMod.ArriveMoney = retMod.amount;
        //        payMod.ArriveDetail = avMod.ID.ToString();
        //    }
        //    else { payMod.ArriveDetail = "优惠券[" + avMod.ID + "]异常 :" + retMod.err; }
        //}
        //积分处理
        //if (point_body.Visible && DataConvert.CLng(Point_T.Text) > 0)
        //{
        //    int point = DataConvert.CLng(Point_T.Text);
        //    //此处需咨询,上限额度是否要扣减掉优惠券

        //    int maxPoint = Point_CanBeUse(payMod.MoneyPay + payMod.ArriveMoney);
        //    //if (point <= 0) { function.WriteErrMsg("积分数值不正确"); }
        //    if (point > mu.UserExp) { function.WriteErrMsg("您的积分不足!"); }
        //    if (point > maxPoint) { function.WriteErrMsg("积分不能大于可兑换金额[" + maxPoint + "]!"); }
        //    //生成支付单时扣除用户积分
        //    buser.ChangeVirtualMoney(mu.UserID, new M_UserExpHis() { ScoreType = (int)M_UserExpHis.SType.Point, score = -point, detail = "积分抵扣,支付单号:" + payMod.PayNo });
        //    payMod.UsePoint = point;
        //    payMod.UsePointArrive = Point_ToMoney(point);
        //    payMod.MoneyPay = payMod.MoneyPay - payMod.UsePointArrive;
        //}
        payMod.MoneyReal = payMod.MoneyPay;
        payMod.Remark = CartDT.Rows.Count > 1 ? "[" + CartDT.Rows[0]["ProName"] as string + "]等" : CartDT.Rows[0]["ProName"] as string;
        payMod.PaymentID = payBll.Add(payMod);
        Response.Redirect("/PayOnline/wxpay_mp.aspx?PayNo=" + payMod.PayNo);
        //Response.Redirect("/PayOnline/Orderpay.aspx?PayNo=" + payMod.PayNo);
    }
    /*----------------------------------------------------------------------------------------------------*/
    #region 重算商品金额
    //更新购物车中的AllMoney(实际购买总价),便于后期查看详情
    private double UpdateCartAllMoney(DataTable dt)
    {
        M_UserInfo mu = buser.GetLogin();
        double allmoney = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];
            M_Cart cartMod = new M_Cart().GetModelFromReader(dr);
            M_Product proMod = proBll.GetproductByid(Convert.ToInt32(dr["Proid"]));

            //if (price == proMod.LinPrice) { price = proBll.P_GetByUserType(proMod, mu); }
            //double price = proBll.P_GetByUserType(proMod, mu);
            double price = proMod.LinPrice;
            //--多价格编号,则使用多价格编号的价钱,ProName(已在购物车页面更新)
            //double price =proBll.GetPriceByCode(dr["code"], proMod.Wholesalesinfo, ref price);
            cartMod.AllMoney = price * cartMod.Pronum;
            cartMod.AllIntegral = cartMod.AllMoney;
            cartMod.FarePrice = price.ToString("F2");
            //----检查有无价格方面的促销活动,如果有,检免多少金额
            {
                W_Filter filter = new W_Filter();
                filter.cartMod = cartMod;
                filter.TypeFilter = "money";
                ptBll.WhereLogical(filter);
                cartMod.AllMoney = cartMod.AllMoney - filter.DiscountMoney;
            }
            //----计算折扣
            dr["AllMoney"] = cartMod.AllMoney;
            dr["AllIntegral"] = cartMod.AllIntegral;
            //if (proMod.Recommend > 0)
            //{
            //    dr["AllMoney"] = (cartMod.AllIntegral - (cartMod.AllIntegral * ((double)proMod.Recommend / 100)));
            //    cartMod.AllMoney = Convert.ToDouble(dr["AllMoney"]);
            //}
            cartBll.UpdateByID(cartMod);
            allmoney += cartMod.AllMoney;
        }
        return allmoney;
    }
    //获取总金额
    private double GetTotalMoney(DataTable dt)
    {
        //不需要再重新计算,因为每次进入页面都会重算
        return Convert.ToDouble(dt.Compute("SUM(AllMoney)", ""));
    }
    #endregion
    #region Common
    // True有库存
    public bool HasStock(object allowed, int stock, int pronum)
    {
        bool flag = true;
        if (allowed.ToString().Equals("0") && stock < pronum)
        {
            flag = false;
        }
        return flag;
    }
    #endregion
    #region 积分抵扣
    //用户最大能使用多少带你分
    private int Point_CanBeUse(double orderMoney)
    {
        int usepoint = 0;
        if (SiteConfig.ShopConfig.PointRatiot <= 0 || SiteConfig.ShopConfig.PointRatiot > 100 || SiteConfig.ShopConfig.PointRate <= 0) { return usepoint; }
        //可使用多少积分
        usepoint = (int)((orderMoney * (SiteConfig.ShopConfig.PointRatiot * 0.01)) / SiteConfig.ShopConfig.PointRate);
        usepoint = usepoint < 1 ? 0 : usepoint;
        return usepoint;
    }
    //积分兑换为资金
    private double Point_ToMoney(int points)
    {
        if (points <= 0) { return 0; }
        return points * SiteConfig.ShopConfig.PointRate;
    }
    #endregion
}