using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

public partial class Extend_Report_EmployBonus : System.Web.UI.Page
{
    B_User buser = new B_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("sale");
        RPT.SPage = MyBind;
        if (!IsPostBack)
        {
            RPT.DataBind();
        }
    }
    private DataTable MyBind(int pageSize, int pageIndex)
    {
        M_UserInfo mu = buser.GetLogin();
        Com_Filter filter = new Com_Filter()
        {
            storeId = mu.SiteID,
            uname = JSName_T.Text,
            skey = Proname_T.Text,
            stime = STime_T.Text,
            etime = ETime_T.Text

        };
        PageSetting setting = SelPage(pageIndex, pageSize, filter);
        ByJS_RPT.DataSource = SelByJS(filter);
        ByJS_RPT.DataBind();
        RPT.ItemCount = setting.itemCount;
        return setting.dt;
    }
    protected void Search_Btn_Click(object sender, EventArgs e)
    {
        RPT.DataBind();
    }
    public PageSetting SelPage(int cpage,int psize,Com_Filter filter)
    {
        //code==技师ID,ProInfo==技师名
        List<SqlParameter> sp = new List<SqlParameter>();
        string where = GetWhere(filter, sp);
        //需要加上是否支付的筛选
        PageSetting setting = PageSetting.Double(cpage, psize, "ZL_CartPro","ZL_OrderInfo", "A.ID","A.OrderListID=B.ID", where, "A.ID DESC", sp,"A.*,B.PaymentStatus");
        DBCenter.SelPage(setting);
        return setting;
    }
    public DataTable SelByJS(Com_Filter filter)
    {
        List<SqlParameter> sp = new List<SqlParameter>();
        string where = GetWhere(filter, sp);
        return DBCenter.ExecuteTable("SELECT A.Attribute,SUM(CAST(A.AllMoney_Json AS MONEY)) AllMoney_Json "
            + " FROM ZL_CartPro A LEFT JOIN ZL_OrderInfo B ON A.OrderListID=B.ID "
            + " WHERE A.Attribute!='' AND " + where + " GROUP BY Attribute", sp);


    }
    private string GetWhere(Com_Filter filter, List<SqlParameter> sp)
    {
        string where = "B.PaymentStatus=1 AND A.AllMoney_Json IS NOT NULL";
        if (filter.storeId != -100)
        {
            where += " AND B.StoreID=" + filter.storeId;
        }
        //技师用户ID
        if (!string.IsNullOrEmpty(filter.uids))
        {
            SafeSC.CheckIDSEx(filter.uids);
            where += " AND (A.Code IS NOT NULL AND A.Code IN (" + filter.uids + "))";
        }
        if (!string.IsNullOrEmpty(filter.uname))
        {
            sp.Add(new SqlParameter("uname", "%" + filter.uname + "%"));
            where += " AND A.Attribute LIKE @uname";
        }
        if (!string.IsNullOrEmpty(filter.skey))
        {
            sp.Add(new SqlParameter("skey", "%" + filter.skey + "%"));
            where += " AND A.Proname LIKE @skey";
        }
        DateTime time = DateTime.Now;
        if (!string.IsNullOrEmpty(filter.stime) && DateTime.TryParse(filter.stime, out time))
        {
            where += " AND A.AddTime>='" + filter.stime + "'";
        }
        if (!string.IsNullOrEmpty(filter.etime) && DateTime.TryParse(filter.etime, out time))
        {
            where += " AND A.AddTime<='" + filter.etime + "'";
        }
        return where;
    }

    protected void Search_Btn_Click1(object sender, EventArgs e)
    {
        RPT.DataBind();
    }
}