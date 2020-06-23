using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class ExSale
    { 
        //店铺销售汇总
        public static double Sale_Total(Sale_Filter filter)
        {
            //查询该店铺订单的销售金额
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "OrderStatus=99 AND Receivablesamount>0 " + Sale_GetWhere(filter, sp);
            return DataConvert.CDouble(DBCenter.ExecuteScala("ZL_Order_PayedView", "SUM(Receivablesamount)", where, "", sp));
        }
        //到店人数统计
        public static int Sale_User_Count(Sale_Filter filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "OrderStatus=99 AND Receivablesamount>0 " + Sale_GetWhere(filter, sp);
            //DataTable dt= DBCenter.ExecuteTable("SELECT ID FROM ZL_Order_PayedView "+where+" GROUP BY Receiver", sp);
            return DBCenter.Count("ZL_Order_PayedView", where, sp);
        }
        //订单数,客单价,消费总额
        public static Sale_Result Sale_Order_Count(Sale_Filter filter)
        {
            Sale_Result result = new Sale_Result();
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "1=1 " + Sale_GetWhere(filter, sp);
            result.count = DBCenter.Count("ZL_Order_PayedView", where, sp);
            result.sales = DataConvert.CDouble(DBCenter.ExecuteScala("ZL_Order_PayedView", "SUM(Receivablesamount)", where, "", sp));
            return result;
        }
        //预约单数(以顾客到店时间为准)
        public static int PreOrder_Count(Sale_Filter filter)
        {
            string where = "1=1 ";
            if (filter.sid != -100)
            {
                where += " AND StoreId=" + filter.sid;
            }
            if (!string.IsNullOrEmpty(filter.stime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.stime, out date))
                {
                    string stime = date.ToString("yyyy/MM/dd 00:00:00");
                    where += " AND ClientDate>='" + stime + "'";
                }
            }
            if (!string.IsNullOrEmpty(filter.etime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.etime, out date))
                {
                    string etime = date.ToString("yyyy/MM/dd 23:59:59");
                    where += " AND ClientDate<='" + etime + "'";
                }
            }
            return DBCenter.Count("ZL_Ex_PreOrder", where);
        }
        //指定时间内的新增会员数
        public static int User_New_Count(Sale_Filter filter)
        {
            string where = "1=1 ";
            if (filter.sid != -100)
            {
                where += " AND StoreId=" + filter.sid;
            }
            if (!string.IsNullOrEmpty(filter.stime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.stime, out date))
                {
                    string stime = date.ToString("yyyy/MM/dd 00:00:00");
                    where += " AND CDate>='" + stime + "'";
                }
            }
            if (!string.IsNullOrEmpty(filter.etime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.etime, out date))
                {
                    string etime = date.ToString("yyyy/MM/dd 23:59:59");
                    where += " AND CDate<='" + etime + "'";
                }
            }
            return DBCenter.Count("ZL_Ex_StoreUserView", where);
        }
        //生成通用where语句
        private static string Sale_GetWhere(Sale_Filter filter, List<SqlParameter> sp)
        {
            string where = " ";
            if (filter.sid > 0) { where += " AND StoreID=" + filter.sid; }
            if (!string.IsNullOrEmpty(filter.stime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.stime, out date))
                {
                    //sp.Add(new SqlParameter("stime", Convert.ToDateTime(stime).ToString("yyyy/MM/dd 00:00:00")));
                    string stime = date.ToString("yyyy/MM/dd 00:00:00");
                    where += " AND AddTime>='" + stime + "'";
                }
            }
            if (!string.IsNullOrEmpty(filter.etime))
            {
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(filter.etime, out date))
                {
                    //sp.Add(new SqlParameter("etime", Convert.ToDateTime(etime).ToString("yyyy/MM/dd 23:59:59")));
                    string etime = date.ToString("yyyy/MM/dd 23:59:59");
                    where += " AND AddTime<='" + etime + "'";
                }
            }
            //根据订单用途类型筛选
            if (!string.IsNullOrEmpty(filter.orderType))
            {
                SafeSC.CheckIDSEx(filter.orderType);
                where += " AND OrderType IN (" + filter.orderType + ")";
            }
            if (filter.payPlat != -100)
            {
                where += " AND PayPlatID=" + filter.payPlat;
            }
            return where;
        }

        //--------------TO CMS
        public static DataTable Report_SelByProduct(Sale_Filter filter)
        {
            string SDate = filter.stime;
            string EDate = filter.etime;
            int StoreID = filter.sid;
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "(PaymentNO IS NOT NULL AND PaymentNO!='') AND OrderStatus=99 ";
            if (!string.IsNullOrEmpty(SDate))
            {
                where += " AND PayTime>=@stime";
                sp.Add(new SqlParameter("stime", Convert.ToDateTime(SDate).ToString("yyyy/MM/dd 00:00:00")));
            }
            if (!string.IsNullOrEmpty(EDate))
            {
                where += " AND PayTime<=@etime";
                sp.Add(new SqlParameter("etime", Convert.ToDateTime(EDate).ToString("yyyy/MM/dd 23:59:59")));
            }
            if (StoreID != -100) { where += " AND StoreID=" + StoreID; }
            //if (NodeID > 0) { where += " AND NodeID=" + NodeID; }
            string mtable = "(SELECT SUM(AllMoney)AS AllMoney,SUM(Pronum)AS Pronum,ProID,ProName,Nodeid FROM ZL_Order_ProSaleView  WHERE " + where + " GROUP BY ProID,ProName,Nodeid)";
            return DBCenter.JoinQuery("A.*,B.NodeName", mtable, "ZL_Node", "A.NodeID=B.NodeID", "", "AllMoney DESC", sp.ToArray());
        }
        public static DataTable Report_SelByDay(Sale_Filter filter)
        {
            DateTime STime = Convert.ToDateTime(filter.stime);
            DateTime ETime = Convert.ToDateTime(filter.etime);
            B_OrderList orderBll = new B_OrderList();
            DataTable saleDT = orderBll.Report_SelByDate(STime, ETime);
            DataTable dayDT = new DataTable();
            dayDT.Columns.Add("date", typeof(string));
            dayDT.Columns.Add("total", typeof(double));
            for (DateTime s = STime; s <= ETime; s = s.AddDays(1))
            {
                DataRow day = dayDT.NewRow();
                //DateTime sdate = Convert.ToDateTime("{0}/{1}/{2} 00:00:00");
                string sdate = s.ToString("#yyyy/MM/dd 00:00:00#"), edate = s.ToString("#yyyy/MM/dd 23:59:59#");
                saleDT.DefaultView.RowFilter = "PayTime>= " + sdate + " AND PayTime<= " + edate;
                day["date"] = s.ToString("yyyy-MM-dd");
                day["total"] = 0;
                foreach (DataRow dr in saleDT.DefaultView.ToTable().Rows)
                {
                    day["Total"] = DataConvert.CDouble(day["Total"]) + DataConvert.CDouble(dr["OrdersAmount"]);
                }
                dayDT.Rows.Add(day);
            }
            return dayDT;
        }

    }
    //订单销售筛选
    public class Sale_Filter
    {
        public int sid = -100;
        public string stime = "";
        public string etime = "";
        public string orderType = "";
        //支付方式筛选
        public int payPlat = -100;
        //需要筛选的节点IDS
        public string NodeIDS = "";
    }
    public class Sale_Result
    {
        public int count = 0;
        public double sales = 0;
        public double ticketSale { get { return sales / count; } }
    }
}
