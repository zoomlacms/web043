using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_Order_Cash : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_OrderList orderBll = new B_OrderList();
    B_CartPro cpBll = new B_CartPro();
    B_Payment payBll = new B_Payment();
    B_User_StoreUser suBll = new B_User_StoreUser();
    public M_OrderList orderMod = null;
    public M_UserInfo client = null;
    public M_UserInfo mu = null;
    public int OrderID { get { return DataConvert.CLng(Request.QueryString["oid"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("cash");
        mu = buser.GetLogin();
        M_Store_Info storeMod = ExHelper.Store_User();
        orderMod = orderBll.SelReturnModel(OrderID);
        client = buser.SelReturnModel(DataConvert.CLng(orderMod.Receiver));
        OrderHelper.CheckIsCanPay(orderMod);
        if (function.isAjax())
        {
            M_Payment payMod = payBll.CreateByOrder(orderMod);
            payMod.UserID = mu.UserID;
            payMod.CStatus = true;
            payMod.Status = (int)M_Payment.PayStatus.HasPayed;
            string result = "";
            try
            {
                string type = Request.Form["type"];
                switch (type)
                {
                    case "money":
                        #region 现金支付/扫码支付
                        {
                            double rece = DataConvert.CDouble(Request.Form["rece"]);
                            if (rece < payMod.MoneyPay) { throw new Exception("金额不正确,不能少于应收金额"); }
                            payMod.Remark = string.Format("应收{0},实收{1},找零{2}", payMod.MoneyPay, rece, rece - payMod.MoneyPay);
                            payMod.MoneyTrue = rece > payMod.MoneyPay ? payMod.MoneyPay : rece;
                            payMod.PayPlatID = (int)M_PayPlat.Plat.Offline;//现金扫码支付
                        }
                        #endregion
                        break;
                    case "mcard":
                        #region 会员卡支付
                        {
                            if (client.IsNull) { throw new Exception("未指定会员,无法使用会员卡支付"); }
                            else if (client.Purse < orderMod.Ordersamount) { throw new Exception("会员卡余额不足"); }
                            else
                            {
                                buser.MinusVMoney(client.UserID, orderMod.Ordersamount, M_UserExpHis.SType.Purse, "会员卡支付[" + orderMod.OrderNo + "]");
                                //检测会员卡余额是否充足
                                payMod.Remark = "会员卡支付";
                                payMod.MoneyTrue = orderMod.Ordersamount;
                                payMod.PayPlatID = (int)M_PayPlat.Plat.ECPSS;//会员卡支付
                            }
                        }
                        #endregion
                        break;
                    default:
                        throw new Exception("支付方式[" + type + "]不存在");
                }
                payMod.PaymentID = payBll.Add(payMod);
                OrderHelper.FinalStep(payMod, orderMod, null);
                result = "1";
            }
            catch (Exception ex)
            {
                ZLLog.L(ZLEnum.Log.pay, "订单:" + orderMod.id + ",原因:" + ex.Message);
                result = ex.Message;
            }
            if (client.UserID > 0)
            {
                DBCenter.UpdateSQL(client.TbName, "UpdateTime=getdate()", "UserID=" + client.UserID);
            }
            Response.Write(result); Response.Flush(); Response.End();

        }
        if (!IsPostBack)
        {
            StoreName_L.Text = storeMod.Title;
            Cart_RPT.DataSource = cpBll.SelByOrderID(orderMod.id);
            Cart_RPT.DataBind();
        }
    }
    //是否允许会员卡支付
    public bool AllowMCard()
    {
        //会员卡充值,余额不足,会员卡禁用
        M_User_StoreUser suMod = suBll.SelModelByUid(client.UserID,mu.SiteID);
        if (suMod.CardStatus < 0 || orderMod.Ordertype != 0) { return false; }
        else if (client.Purse < orderMod.Ordersamount) { return false; }
        else { return true; }
    }
}