using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.Sns;

public partial class Extend_Report_StoreSale : System.Web.UI.Page
{
    public double product_sale, mcard_recharge, mcard_consume, order_preTicketSale;
    public int user_new_count;
    public Sale_Result order_result = new Sale_Result();

    public string line_chart_xais = "", line_chart_data = "";
    public string stime
    {
        get
        {
            DateTime time = DateTime.Now;
            if (!DateTime.TryParse(STime_T.Text,out time)) { STime_T.Text = DateTime.Now.ToString("yyyy/MM/01"); }
            return STime_T.Text;
        }
    }
    public string etime
    {

        get
        {
            DateTime time = DateTime.Now;
            if (!DateTime.TryParse(ETime_T.Text, out time)) { ETime_T.Text = DateTime.Now.ToString("yyyy/MM/dd"); }
            return ETime_T.Text;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("sale");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    public string GetXAxis(DataTable dt)
    {
        string result = "";
        foreach (DataRow dr in dt.Rows)
        {
            result += string.Format("'{0}',", Convert.ToDateTime(dr["date"]).ToString("dd"));
        }
        return result.TrimEnd(',');
    }
    public string GetData(DataTable dt)
    {
        string result = "";
        foreach (DataRow dr in dt.Rows)
        {
            result += string.Format("{0},", Convert.ToDouble(dr["total"]).ToString("F2"));
        }
        return result.TrimEnd(',');
    }
    private void MyBind()
    {
        M_UserInfo mu = new B_User().GetLogin();
        Sale_Filter defFilter = new Sale_Filter()
        {
            stime = stime,
            etime = etime,
            sid = mu.SiteID
        };
        //品项收入(仅商品,不含充卡)
        product_sale = ExSale.Sale_Total(new Sale_Filter()
        {
            sid = mu.SiteID,
            stime = stime,
            etime = etime,
            orderType = "0"//微信支付的是否过滤掉???????
        });
        //充值金额
        mcard_recharge = ExSale.Sale_Total(new Sale_Filter()
        {
            sid = mu.SiteID,
            stime = stime,
            etime = etime,
            orderType = "5"
        });
        //会员卡消费金额(订单支付方式为会员卡)
        mcard_consume = ExSale.Sale_Total(new Sale_Filter()
        {
            sid = mu.SiteID,
            stime = stime,
            etime = etime,
            payPlat = (int)M_PayPlat.Plat.ECPSS,
        });
        //----------------
        user_new_count = ExSale.User_New_Count(defFilter);
        order_result = ExSale.Sale_Order_Count(defFilter);
        //-----折线图
        //商品销售
        DataTable line_chart_dt = ExSale.Report_SelByDay(defFilter);
        line_chart_xais = GetXAxis(line_chart_dt);
        line_chart_data = GetData(line_chart_dt);
    }
    protected void Filter_Btn_Click(object sender, EventArgs e)
    {
        MyBind();
    }
}