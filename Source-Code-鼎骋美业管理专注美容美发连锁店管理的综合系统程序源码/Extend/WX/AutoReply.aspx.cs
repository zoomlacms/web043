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

public partial class Extend_WX_AutoReply : System.Web.UI.Page
{
    B_WX_ReplyMsg rpBll = new B_WX_ReplyMsg();
    B_WX_APPID appBll = new B_WX_APPID();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    public void MyBind()
    {
        M_WX_APPID appmod = ExHelper.WX_SelMyModel();
        DataTable dt = rpBll.SelByAppID(appmod.ID);
        EGV.DataSource = dt;
        EGV.DataBind();
    }
    protected void EGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EGV.PageIndex = e.NewPageIndex;
        MyBind();
    }
    protected void EGV_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "del2":
                break;
        }
        MyBind();
    }
    protected void BatDel_Btn_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.Form["idchk"]))
        {
            rpBll.DelByIDS(Request.Form["idchk"]);
            MyBind();
        }
    }
    public string GetIsDefault()
    {
        if (DataConverter.CLng(Eval("IsDefault")) == 1)
        {
            return "<i class='fa fa-check' style='color:green;'></i>";
        }
        else { return "<i class='fa fa-close' style='color:red;'></i>"; }
    }
    public string GetMsgType()
    {
        return WxAPI.GetMsgTypeStr(DataConverter.CLng(Eval("MsgType")));
    }

    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = e.Row.DataItem as DataRowView;
            e.Row.Attributes.Add("ondblclick", "location='AddReply.aspx?ID=" + dr["ID"]);
            e.Row.Attributes["style"] = "cursor:pointer;";
            e.Row.Attributes["title"] = Resources.L.双击修改;
        }
    }
}