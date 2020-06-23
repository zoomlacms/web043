using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

public partial class Extend_Order_FastOrder : System.Web.UI.Page
{
    /*
     * 1,addUser为收银员,receiver为付款会员,散客则为 散客+日期
     */ 

    B_Product proBll = new B_Product();
    B_OrderList orderBll = new B_OrderList();
    B_User buser = new B_User();
    B_User_StoreUser suBll = new B_User_StoreUser();
    B_CartPro cpBll = new B_CartPro();
    public DataTable proDT, employDT;
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
 
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("cash");
        if (function.isAjax())
        {
            //string action = Request.Form["action"];
            string result = "";
            M_UserInfo mu = buser.GetLogin();
            //直接生成新单,无效的订单一定时间自动清理(未付款)
            int uid = DataConvert.CLng(Request.Form["uid"]);
            string cart = Request.Form["cart"];
            if (string.IsNullOrEmpty(cart)) { throw new Exception("未指定消费项目"); }
            if (string.IsNullOrEmpty(Request.Form["uid"])) { throw new Exception("未指定用户"); }
            //---------------------------根据信息生成订单
            DataTable cartDT = JsonConvert.DeserializeObject<DataTable>(cart);
            M_User_StoreUser suMod = new M_User_StoreUser();
            if (uid > 0)
            {
                suMod= suBll.SelModelByUid(uid, mu.SiteID);
            }
            List<M_CartPro> cpList = new List<M_CartPro>();
            //-----------------
            M_OrderList orderMod = null;
            if (Mid > 0)
            {
                orderMod = orderBll.SelReturnModel(Mid);
            }
            else
            {
                orderMod = orderBll.NewOrder(mu, M_OrderList.OrderEnum.Normal);
            }
            //每次均重新获取
            orderMod.Userid = mu.UserID;
            orderMod.AddUser = mu.UserName;//收银员名称
            orderMod.Receiver = suMod.UserID.ToString();
            orderMod.Rename = suMod.HoneyName;
            orderMod.StoreID = mu.SiteID;
            orderMod.Ordersamount = 0;
            foreach (DataRow dr in cartDT.Rows)
            {
                int cid = Convert.ToInt32(dr["ID"]);
                M_CartPro cpMod = new M_CartPro();
                if (cid > 0)
                {
                    cpMod = cpBll.SelReturnModel(cid);
                    if (cpMod.Orderlistid != Mid) { throw new Exception("购物车数据与订单不匹配"); }
                }
                else
                {
                    cpMod.StoreID = mu.SiteID;
                }
                //-------------------
                M_Product proMod = proBll.GetproductByid(Convert.ToInt32(dr["ProID"]));
                cpMod.ProID = proMod.ID;
                cpMod.Proname = proMod.Proname;
                cpMod.Danwei = proMod.ProUnit;
                cpMod.Shijia = proMod.LinPrice;
                cpMod.Pronum = Convert.ToInt32(dr["Pronum"]);
                cpMod.AllMoney = cpMod.Shijia * cpMod.Pronum;
                orderMod.Ordersamount += cpMod.AllMoney;
                //-------------------------
                cpMod.Username = suMod.HoneyName;
                cpMod.code = DataConvert.CStr(dr["code"]);//技师ID
                cpMod.Attribute = DataConvert.CStr(dr["Attribute"]);//技师姓名
                cpList.Add(cpMod);
                //city,bindpro,
            }
            if (orderMod.id > 0) { orderBll.UpdateByID(orderMod); }
            else { orderMod.id = orderBll.insert(orderMod); }
            string cpIds = "";
            foreach (M_CartPro cpMod in cpList)
            {
                if (cpMod.ID > 0) { cpBll.UpdateByID(cpMod); }
                else
                {
                    cpMod.Orderlistid = orderMod.id;
                    cpMod.ID = cpBll.GetInsert(cpMod);
                }
                cpIds += cpMod.ID + ",";
            }
            //删除未提交的购物车信息
            cpIds = cpIds.TrimEnd(',');
            DBCenter.DelByWhere("ZL_CartPro", "OrderListID=" + orderMod.id + " AND ID NOT IN (" + cpIds + ")");
            result = orderMod.id.ToString();
            
            //---------------------------------------------


            Response.Clear();
            Response.Write(result);
            Response.Flush();Response.End();return;
        }
        if (!IsPostBack)
        {
            M_UserInfo mu = buser.GetLogin();
            if (Mid > 0)
            {
                M_OrderList orderMod = orderBll.SelReturnModel(Mid);
                if (orderMod.StoreID != mu.SiteID) { function.WriteErrMsg("你无权操作该订单"); }
                if (orderMod.OrderStatus != (int)M_OrderList.StatusEnum.Normal) { function.WriteErrMsg("该订单不可修改"); }
                DataTable cpDT = cpBll.SelByOrderID(orderMod.id);
                Cart_Hid.Value = JsonConvert.SerializeObject(cpDT);
            }
        }
    }


    protected void Save_Btn_Click(object sender, EventArgs e)
    {
       
        //Response.Redirect("/Extend/Order/Cash.aspx?oid="+orderMod.id);
    }
}