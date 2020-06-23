using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLa.Sns
{
    public class ExOrder
    {
        public static string ShowPayPlatName(int PayPlatID)
        {
            switch (PayPlatID)
            {
                case (int)M_PayPlat.Plat.ECPSS:
                    return "会员卡";
                case (int)M_PayPlat.Plat.Offline:
                    return "现金支付";
                case (int)M_PayPlat.Plat.WXPay:
                    return "微信支付";
                default:
                    return PayPlatID.ToString();
            }
        }
        public static PageSetting Order_Sel(Filter_Order filter)
        {
            string where = "OrderType!=" + (int)M_OrderList.OrderEnum.Hide;
            List<SqlParameter> sp = new List<SqlParameter>();
            //是否包含回收站订单
            if (filter.aside != -100) { where += " AND Aside=" + filter.aside; }
            #region 用户中心快速筛选
            switch (filter.fast)
            {
                case "all"://全部(不含回收站)
                    where += " AND Aside=0";
                    break;
                case "unpaid"://待付款==状态为未付款的
                    where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.NoPay;
                    break;
                case "prepay"://已预付款(尚未支付尾款的订单)
                    where += " AND Aside=0 AND Delivery=1 AND IsCount=0";
                    break;
                case "paid"://已支付(只支付了预付款的不在此列)
                    where += " AND Aside=0 AND ((PaymentStatus=" + (int)M_OrderList.PayEnum.HasPayed + " AND Delivery=0) OR (Delivery=1 AND IsCount=1 AND Settle=1)) ";
                    break;
                case "needpay"://需付款
                    where += " AND Aside=0 AND PaymentStatus=0";
                    break;
                case "receive"://需确认收货
                    where += " AND Aside=0 AND StateLogistics=1";
                    break;
                case "comment"://已付款未评价
                               //where += " AND (OrderStatus=" + (int)M_OrderList.StatusEnum.OrderFinish + " AND StateLogistics=" + (int)M_OrderList.ExpEnum.HasReceived + ")";
                               //where += " AND (SELECT COUNT(*) FROM ZL_CartPro WHERE Orderlistid=ID AND (AddStatus IS NULL OR AddStatus=''))>0";//AddStatus中会有退货记录,所以筛选为必须为空
                               //break;
                case "finish"://客户已付款收货 ||客户已完成退货
                    {
                        where += string.Format(" AND ({0} OR {1})",
                            "(OrderStatus=" + (int)M_OrderList.StatusEnum.OrderFinish + " AND StateLogistics=" + (int)M_OrderList.ExpEnum.HasReceived + ")",
                            "(PaymentStatus=" + (int)M_OrderList.PayEnum.Refunded + ")");
                    }
                    break;
                case "issure":
                    where += " AND IsSure=0 ";
                    break;
                case "recycle"://订单回收站
                    where = "OrderType!=" + (int)M_OrderList.OrderEnum.Hide + " AND Aside=1";
                    break;
            }
            #endregion
            #region 后台快速筛选
            switch (filter.addon)
            {
                case "unpaid"://待付款==状态为未付款的
                    where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.NoPay;
                    break;
                case "prepay"://已预付款(尚未支付尾款的订单)
                    where += " AND Delivery=1 AND IsCount=0";
                    break;
                case "paid":
                    where += " AND ((PaymentStatus=" + (int)M_OrderList.PayEnum.HasPayed + " AND Delivery=0) OR (Delivery=1 AND IsCount=1 AND Settle=1)) ";
                    //where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.HasPayed;
                    break;
                case "unexp"://待发货==已付款+未发货
                    where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.HasPayed + " AND StateLogistics=" + (int)M_OrderList.ExpEnum.NoSend;
                    break;
                case "exped"://已发货==大于未发货状态的订单
                    where += " AND StateLogistics>" + (int)M_OrderList.ExpEnum.NoSend;
                    break;
                case "finished":
                    //where += " AND OrderStatus=" + (int)M_OrderList.StatusEnum.OrderFinish + " AND StateLogistics=" + (int)M_OrderList.ExpEnum.HasReceived;
                    //后期移除,暂时支持预付
                    where += " AND (OrderStatus=" + (int)M_OrderList.StatusEnum.OrderFinish + " AND StateLogistics=" + (int)M_OrderList.ExpEnum.HasReceived + ")";
                    break;
                case "unrefund":
                    where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.RequestRefund;
                    break;
                case "refunded":
                    where += " AND PaymentStatus=" + (int)M_OrderList.PayEnum.Refunded;
                    break;
                case "recycle"://订单回收站==已关闭
                    where = " Aside=1 ";
                    break;
                case "all"://全部(不含回收站)
                default:
                    break;
            }
            #endregion
            //店铺类型
            switch (filter.storeType)
            {
                case "all":
                    break;
                case "store":
                    where += " AND StoreID>0";
                    break;
                case "shop":
                    where += " AND StoreID=0 ";
                    break;
                default:
                    if (DataConvert.CLng(filter.storeType) > 0) { where += " AND StoreID=" + DataConvert.CLng(filter.storeType) + " "; }
                    break;
            }
            if (filter.isSure != -100) { where += " AND IsSure=" + filter.isSure; }
            //订单类型，未指定则抽出常规订单
            if (string.IsNullOrEmpty(filter.orderType)) { where += " AND OrderType IN (0,1,4)"; }
            else if (filter.orderType.Equals("-100") || filter.orderType.Equals("-1")) { }
            else { SafeSC.CheckIDSEx(filter.orderType); where += " AND OrderType IN (" + filter.orderType + ")"; }
            //商品名,订单号,用户名,手机号,用户ids
            if (!string.IsNullOrEmpty(filter.proname)) { where += " AND ProName LIKE @proname"; sp.Add(new SqlParameter("proname", "%" + filter.proname + "%")); }
            if (!string.IsNullOrEmpty(filter.orderno)) { where += " AND OrderNo LIKE @orderno"; sp.Add(new SqlParameter("orderno", "%" + filter.orderno + "%")); }
            if (!string.IsNullOrEmpty(filter.reuser)) { where += " AND (Rename LIKE @reuser OR Receiver LIKE @reuser)"; sp.Add(new SqlParameter("reuser", "%" + filter.reuser + "%")); }
            if (!string.IsNullOrEmpty(filter.mobile)) { where += " AND MobileNum LIKE @mobile"; sp.Add(new SqlParameter("mobile", "%" + filter.mobile + "%")); }
            if (!string.IsNullOrEmpty(filter.uids) && SafeSC.CheckIDS(filter.uids))
            {
                where += " AND Receiver IN (" + filter.uids + ")";
            }
            //下单日期
            if (!string.IsNullOrEmpty(filter.stime))
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(filter.stime, out result)) { where += " AND AddTime>=@stime"; sp.Add(new SqlParameter("stime", result.ToString("yyyy/MM/dd 00:00:00"))); }
            }
            if (!string.IsNullOrEmpty(filter.etime))
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(filter.etime, out result)) { where += " AND AddTime<=@etime"; sp.Add(new SqlParameter("etime", result.ToString("yyyy/MM/dd 23:59:59"))); }
            }
            //发货时间
            if (!string.IsNullOrEmpty(filter.expstime) || !string.IsNullOrEmpty(filter.expetime)) { where += " AND ExpSTime IS NOT NULL "; }
            if (!string.IsNullOrEmpty(filter.expstime))//按发货日期筛选
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(filter.expstime, out result)) { where += " AND ExpSTime>=@expstime"; sp.Add(new SqlParameter("expstime", result.ToString("yyyy/MM/dd 00:00:00"))); }
            }
            if (!string.IsNullOrEmpty(filter.expetime))
            {
                DateTime result = DateTime.Now;
                if (DateTime.TryParse(filter.expetime, out result)) { where += " AND ExpSTime<=@expetime"; sp.Add(new SqlParameter("expetime", result.ToString("yyyy/MM/dd 23:59:59"))); }
            }
            //搜索,支持指定条件
            if (!string.IsNullOrEmpty(filter.skey))
            {
                sp.Add(new SqlParameter("skey", "%" + filter.skey + "%"));
                switch (filter.stype)
                {
                    case "exp":
                        where += " AND ExpressDelivery LIKE @skey";
                        break;
                    case "oid":
                        where += " AND ID= " + DataConvert.CLng(filter.skey);
                        break;
                }
            }
            if (!String.IsNullOrEmpty(filter.oids))
            {
                SafeSC.CheckIDSEx(filter.oids);
                where += " AND ID IN (" + filter.oids + ")";
            }
            if (filter.payType != -100)
            {
                where += " AND PayType=" + filter.payType;
            }

            string view = "ZL_CartProView";
            //只取订单的ID
            PageSetting setting = PageSetting.Single(filter.cpage, filter.psize, view, "ID", where, " GROUP BY ID ORDER BY ID DESC", sp, "ID");
            DBCenter.SelPage(setting);
            string ids = "";
            foreach (DataRow dr in setting.dt.Rows)
            {
                ids += dr["id"] + ",";
            }
            ids = ids.TrimEnd(',');
            setting.itemCount = DataConvert.CLng(DBCenter.Count("(SELECT ID FROM ZL_CartProView WHERE " + where + " GROUP BY ID) A", "", sp));
            setting.pageCount = SqlBase.GetPageCount(setting.itemCount, setting.psize);
            //根据订单ID取出购物车中的数据,需要进行名称等筛选
            if (!string.IsNullOrEmpty(ids))
            {
                sp.Clear();
                string cartWhere = "ID IN (" + ids + ") ";
                if (!string.IsNullOrEmpty(filter.proname)) { cartWhere += " AND ProName LIKE @proname"; sp.Add(new SqlParameter("proname", "%" + filter.proname + "%")); }
                setting.dt = DBCenter.Sel(view, cartWhere, "ID DESC", sp);
            }
            return setting;
        }

    }
}
