using Newtonsoft.Json;
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
using ZoomLa.Model.Shop;
using ZoomLa.Sns;

public partial class Extend_WX_WXUser : System.Web.UI.Page
{
    WxAPI api = null;
    B_WX_APPID appBll = new B_WX_APPID();
    B_WX_User wxuserBll = new B_WX_User();
    public int AppId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        M_CommonData storeMod = ExHelper.Store2_User();
        if (DataConverter.CLng(storeMod.SpecialID) < 1) { function.WriteErrMsg("尚未绑定微信号","/Extend/WX/WXConfig.aspx"); }
        AppId = Convert.ToInt32(storeMod.SpecialID);
        api = WxAPI.Code_Get(AppId);

        if (function.isAjax())
        {
            string action = Request.Form["action"]; string result = "";
            switch (action)
            {
                case "update":
                    M_WX_User oldmod = wxuserBll.SelForOpenid(Request.Form["openid"]);
                    if (oldmod != null && oldmod.ID > 0)
                    {
                        M_WX_User usermod = api.GetWxUserModel(Request.Form["openid"]);
                        usermod.ID = oldmod.ID;
                        usermod.CDate = DateTime.Now;
                        usermod.AppId = api.AppId.ID;
                        wxuserBll.UpdateByID(usermod);
                        result = JsonConvert.SerializeObject(usermod);
                    }
                    else
                        result = "-1";
                    break;
                default:
                    break;
            }
            Response.Write(result); Response.Flush(); Response.End();
        }
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    public void MyBind()
    {
        DataTable dt = wxuserBll.SelByAppId(AppId, Key_T.Text);
        EGV.DataSource = dt;
        EGV.DataBind();
        M_WX_APPID appmod = appBll.SelReturnModel(AppId);
    }
    public DataTable GetUserList()
    {
        List<M_WX_User> users = api.SelAllUser();
        foreach (var item in users)
        {
            item.CDate = DateTime.Now;
            item.AppId = AppId;
            wxuserBll.Insert(item);
        }
        return wxuserBll.SelByAppId(AppId);
    }
    protected void EGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EGV.PageIndex = e.NewPageIndex;
        MyBind();
    }
    public string GetSexIcon()
    {
        string classname = DataConverter.CLng(Eval("Sex")) == 1 ? "fa fa-male" : "fa fa-female";
        return "<span style='font-size:20px;' class='" + classname + " sex'></span>";
    }
    public string GetUserGroup()
    {
        switch (DataConverter.CLng(Eval("Groupid")))
        {
            case 0:
                return "未分组";
            case 1:
                return "黑名单";
            case 2:
                return "星标组";
            default:
                return "";
        }
    }
    protected void Search_B_Click(object sender, EventArgs e)
    {
        MyBind();
    }
}