using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Other;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_WX_MsgTlpList : System.Web.UI.Page
{
    B_WX_MsgTlp tlpBll = new B_WX_MsgTlp();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    private void MyBind()
    {
        EGV.DataSource = tlpBll.Sel();
        EGV.DataBind();
    }
    public string GetMsgType()
    {
        return tlpBll.GetMsgType(Eval("MsgType", ""));
    }
    public string GetEditLink()
    {
        int id = Convert.ToInt32(Eval("ID"));
        return GetEditLink(id, Eval("MsgType", ""));

    }
    public string GetEditLink(int id, string msgType)
    {
        string url = "";
        switch (msgType)
        {
            case "text":
            case "image":
                url = "AddMsgTlp.aspx?id=" + id;
                break;
            case "multi":
                url = "MsgMultiAdd.aspx?ID=" + id;
                break;
            default:
                break;
        }
        return url;
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
                int id = Convert.ToInt32(e.CommandArgument);
                tlpBll.Del(id);
                break;
        }
        MyBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = e.Row.DataItem as DataRowView;
            e.Row.Attributes.Add("ondblclick", "location='" + GetEditLink(Convert.ToInt32(dr["ID"]), DataConvert.CStr(dr["msgType"])) + "'");
        }
    }
}