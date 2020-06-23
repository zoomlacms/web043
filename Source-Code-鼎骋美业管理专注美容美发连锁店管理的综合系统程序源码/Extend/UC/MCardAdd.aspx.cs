using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_UC_MCardAdd : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_User_StoreUser suBll = new B_User_StoreUser();
    B_User_Card cdBll = new B_User_Card();
    //用户ID
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    public M_User_StoreUser suMod = new M_User_StoreUser();
    public M_UserInfo client = new M_UserInfo(true);
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("cash");
        if (!IsPostBack)
        {
            M_UserInfo mu = buser.GetLogin();
            Regular_RPT.DataSource = ExHelper.Store_MoneyRegular(mu.SiteID);
            Regular_RPT.DataBind();
            if (Mid > 0)
            {
                suMod = suBll.SelModelByUid(Mid, mu.SiteID);
                if (suMod == null) { function.WriteErrMsg("你无权操作该用户"); }
                client = buser.SelReturnModel(Mid);
                if (string.IsNullOrEmpty(suMod.CardNo))
                {
                    suMod.CardNo = cdBll.GetCardNo(mu.SiteID.ToString());
                    DBCenter.UpdateSQL(suMod.TbName, "CardNo='" + suMod.CardNo + "'", "ID=" + suMod.ID);
                }
                CardNo.Text = suMod.CardNo;
                CardPurse.Text = client.Purse.ToString("F2");
            }
            else
            {
                suMod.HoneyName = "未选择";
                op_tr.Visible = false;
                op_tr2.Visible = false;
            }
        }
    }
}