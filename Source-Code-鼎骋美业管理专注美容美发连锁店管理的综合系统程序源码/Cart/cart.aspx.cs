using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.BLL.User;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Model.User;
using ZoomLa.SQLDAL;

/*
 * 购物车页面根据所传来的ItemID,SkuID,PCount将指定商品加入购物车,按店铺分栏显示
 * 自营商品的店铺StoreID为0
 */

namespace ZoomLaCMS.Cart
{
    public partial class cart : System.Web.UI.Page
    {

        B_OrderBaseField fieldBll = new B_OrderBaseField();
        B_Product proBll = new B_Product();
        B_Cart cartBll = new B_Cart();
        B_Shop_SuitPro suitBll = new B_Shop_SuitPro();
        B_User buser = new B_User();
        //B_Shop_RegionPrice regionBll = new B_Shop_RegionPrice();
        OrderCommon orderCom = new OrderCommon();
        public int StoreID { get { return DataConvert.CLng(Request.Cookies["sid"].Value); } }
        //仅用于标识显示积分商品,或普通商品,不参与其他逻辑
        public int ProClass { get { return DataConvert.CLng(Request.QueryString["Proclass"]); } }
        //Cookies中的购物车ID
        public string CartCookID
        {
            get
            {
                return B_Cart.GetCartID(Context);
            }
        }
        private DataTable CartDT
        {
            get
            {
                return (DataTable)ViewState["CartDT"];
            }
            set { ViewState["CartDT"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                M_UserInfo mu = buser.GetLogin();
                if (!mu.IsNull && mu.Status != 0) { function.WriteErrMsg("你的帐户未通过验证或被锁定"); }
                int proid = DataConvert.CLng(Request["id"]);
                //int suitid = DataConvert.CLng(Request["suitid"]);
                int pronum = DataConvert.CLng(Request["pronum"], 1);
                int pclass = -1;//非-1则为添加了商品,需要跳转
                if (proid > 0)
                {
                    M_Product proMod = proBll.GetproductByid(proid);
                    if (proMod == null) { function.WriteErrMsg("商品不存在"); }
                    AddToCart(mu, proMod, pronum);
                    pclass = proMod.ProClass;
                }
                if (function.isAjax()) { return; }//ajax下不需要数据绑定与跳转
                if (pclass > -1) { Response.Redirect(Request.Url.AbsolutePath + "?ProClass=" + pclass); return; }
                MyBind();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            CartDT = null;
        }
        private void MyBind()
        {
            M_UserInfo mu = buser.GetLogin();
            B_Cart.UpdateUidByCartID(CartCookID, mu);
            CartDT = cartBll.SelByCartID(CartCookID, mu.UserID, ProClass);//从数据库中获取
            CartDT.DefaultView.RowFilter = "StoreID=" + StoreID;
            CartDT = CartDT.DefaultView.ToTable();
            RPT.DataSource = orderCom.SelStoreDT(CartDT);
            RPT.DataBind();
            totalmoney.InnerText = TotalPrice.ToString("f2");
        }
        protected void RPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                Repeater rpt = e.Item.FindControl("ProRPT") as Repeater;
                CartDT.DefaultView.RowFilter = "StoreID=" + dr["ID"];
                DataTable dt = CartDT.DefaultView.ToTable();
                if (dt.Rows.Count < 1) { e.Item.Visible = false; }
                rpt.DataSource = dt;
                rpt.DataBind();
            }
        }
        protected void ProRPT_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = e.Item.DataItem as DataRowView;
                switch (DataConvert.CLng(dr["Class"]))
                {
                    case 2://套装促销
                        Repeater rpt = e.Item.FindControl("SuitPro_RPT") as Repeater;
                        rpt.DataSource = proBll.Suit_GetProduct(DataConvert.CStr(dr["procontent"]), DataConvert.CLng(dr["Pronum"]));
                        rpt.DataBind();
                        break;
                    default://普通商品,支持赠品等促销逻辑
                        {
                            Label ptLabel = e.Item.FindControl("Present_HTML") as Label;
                            ptLabel.Text = PageHelper.Aspx_GetHtml("/cart/comp/cart_present.aspx", dr);
                        }
                        break;
                }
            }
        }
        //结算,到订单页再生成AllMoney
        protected void NextStep_Click(object sender, EventArgs e)
        {
            //AJAX就先检测一遍,未登录则弹窗
            B_User.CheckIsLogged(Request.RawUrl);
            M_UserInfo mu = buser.GetLogin();//提交前同步一次,避免即时登录造成的cart中id未与用户关联
            B_Cart.UpdateUidByCartID(CartCookID, mu);
            string ids = Request.Form["prochk"];
            Response.Redirect("/Extend/Mobile/GetOrderInfo.aspx?ids=" + ids + "&ProClass=" + ProClass);//"#none"
        }
        //-----前端View
        //将计算出的单价缓存,用于避免重复计算
        private double TempPrice = 0;
        private double TotalPrice = 0;
        //单项商品小计
        public string GetPrice()
        {
            int pronum = Convert.ToInt32(Eval("Pronum"));
            double total = TempPrice * pronum;
            TotalPrice += total;
            return total.ToString("0.00");
        }
        //单价
        public string GetMyPrice()
        {
            int proID = Convert.ToInt32(Eval("ProID"));
            double linPrice = TempPrice = Convert.ToDouble(Eval("LinPrice"));
            M_UserInfo mu = buser.GetLogin();
            M_Product proMod = proBll.GetproductByid(proID);
            //多区域价格
            //if (string.IsNullOrEmpty(Region))
            //{
            //    Region = buser.GetRegion(mu.UserID);
            //}
            //TempPrice = regionBll.GetRegionPrice(proID, linPrice, Region, mu.GroupID);
            //如果多区域价格未匹配,则匹配会员价
            if (TempPrice == linPrice || TempPrice <= 0) { TempPrice = proBll.P_GetByUserType(proMod, mu); }
            string html = "<i class='fa fa-rmb'></i><span id='price_" + Eval("ID") + "'>" + TempPrice.ToString("f2") + "</span>";
            return html;
        }
        //------------------------------Tools
        // 生成购物车编号("Shopby OrderNo"的value) 
        //返回当前登录用户,如未登录,则返回0
        private M_UserInfo GetLogin()
        {
            M_UserInfo mu = buser.GetLogin(false);
            if (mu == null)
            {
                mu = new M_UserInfo();
                mu.UserID = 0;
                mu.UserName = "未登录";
            }
            return mu;
        }
        //根据传参将商品加入购物车后跳转(支持按商品ID,套装ID购买)
        private void AddToCart(M_UserInfo mu, M_Product proMod, int pronum)
        {
            if (pronum < 1) { pronum = 1; }
            if (proMod == null || proMod.ID < 1) { return; }//商品不存在
            if (proMod.ProClass == (int)M_Product.ClassType.IDC) { Response.Redirect("/Cart/FillIDCInfo.aspx?proid=" + proMod.ID); }
            OrderCommon.ProductCheck(proMod);
            #region 特选商品逻辑 [delete]
            if (proMod.Largess == 1)
            {
                B_User_BindPro ubpBll = new B_User_BindPro();
                M_User_BindPro ubpMod = null;
                if (mu.IsNull) { return; }
                ubpMod = ubpBll.SelModelByUid(mu.UserID);
                if (ubpMod == null || !ubpMod.ProIDS.Contains("," + proMod.ID + ",")) { Response.Redirect(Request.Url.AbsolutePath + "?ProClass=" + proMod.ProClass); return; }
            }
            #endregion
            //-----------------检测完成加入购物车
            M_Cart cartMod = new M_Cart();
            cartMod.Cartid = CartCookID;
            cartMod.StoreID = proMod.UserShopID;
            cartMod.ProID = proMod.ID;
            cartMod.Pronum = pronum;
            cartMod.userid = mu.UserID;
            cartMod.Username = mu.UserName;
            cartMod.FarePrice = proMod.LinPrice.ToString();
            cartMod.ProAttr = DataConvert.CStr(Request["attr"]);
            cartMod.Proname = proMod.Proname;
            int id = cartBll.AddModel(cartMod);
        }
}
}