using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.Common;
using ZoomLa.BLL.API;
using ZoomLa.Model.Shop;
using System.Data;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Manage.WeiXin
{
    public partial class EditWxMenu : System.Web.UI.Page
    {
        //返回：api unauthorized,微信公众号未认证,必须认证后才可使用菜单编辑功能
        WxAPI api = null;
        B_WX_APPID appbll = new B_WX_APPID();
        public int StoreId { get { return DataConverter.CLng(ViewState["StoreID"]); } set { ViewState["StoreID"] = value; } }
        public int AppId { get { return DataConverter.CLng(Request.QueryString["appid"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            try { api = WxAPI.Code_Get(AppId); } catch (Exception ex) { function.WriteErrMsg("微信公众号配置不正确," + ex.Message); }
            if (function.isAjax())
            {
                M_APIResult result = new M_APIResult();
                result.retcode = M_APIResult.Failed;
                WxAPI api = WxAPI.Code_Get(AppId);
                string action = Request["action"];
                //result.result = api.AccessToken;
                //RepToClient(result);
                try
                {
                    switch (action)
                    {
                        case "create":
                            string jsondata = "{\"button\":" + Request.Form["menus"] + "}";
                            result.result = api.CreateWxMenu(jsondata);
                            if (!result.result.Contains("errmsg")) { result.retcode = M_APIResult.Success; }
                            else { result.retmsg = result.result; }
                            break;
                        case "get":
                            result.result = api.GetWxMenu();
                            if (!result.result.Contains("errmsg")) { result.retcode = M_APIResult.Success; }
                            else { result.retmsg = result.result; }
                            break;
                        default:
                            result.retmsg = "接口[" + action + "]不存在";
                            break;
                    }
                }
                catch (Exception ex) { result.retmsg = ex.Message; }
                RepToClient(result);
            }

            if (!IsPostBack)
            {
                B_ARoleAuth.AuthCheckEx(ZLEnum.Auth.portable, "wechat");
                M_WX_APPID appmod = appbll.SelReturnModel(AppId);
                string alias = " [公众号:" + appmod.Alias + "]";
                string bread = "";
                //检测是否关联了店铺
                DataTable dt = DBCenter.Sel("ZL_CommonModel", "SpecialID IS NOT NULL AND SpecialID='" + appmod.ID + "'");
                if (dt.Rows.Count < 1)
                {
                    bread += "<span style='color:orange;'>(尚未绑定店铺)</span>";
                }
                else if (dt.Rows.Count == 1)
                {
                    StoreId = DataConvert.CLng(dt.Rows[0]["GeneralID"]);
                    bread += "<span><a href='javascript:;' onclick='wxmenu.initMenu();' class='btn btn-info btn-xs'>初始化菜单(" + dt.Rows[0]["Title"] + ")</a></span>";
                }
                else if (dt.Rows.Count > 1)
                {
                    StoreId = DataConvert.CLng(dt.Rows[0]["GeneralID"]);
                    bread += "<span><a href='javascript:;' onclick='wxmenu.initMenu();' class='btn btn-info btn-xs'>初始化菜单<span class='color:orange;'>(绑定了多个店铺)</span></a></span>";
                }

                Call.SetBreadCrumb(Master, "<li><a href='" + CustomerPageAction.customPath2 + "Main.aspx'>工作台</a></li><li><a href='" + CustomerPageAction.customPath2 + "WeiXin/WxAppManage.aspx'>公众号管理</a></li><li class='active'>自定义菜单" + alias + " "+bread+"</li>");
            }
        }
        private void RepToClient(M_APIResult result)
        {
            Response.Write(result.ToString()); Response.Flush(); Response.End();
        }
    }
}