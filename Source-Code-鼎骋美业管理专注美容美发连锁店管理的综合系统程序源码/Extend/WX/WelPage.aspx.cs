using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;

public partial class Extend_WX_WelPage : System.Web.UI.Page
{

    B_WX_APPID appBll = new B_WX_APPID();
    M_WxImgMsg msgMod = new M_WxImgMsg();
    public int AppID { get { return DataConverter.CLng(Request.QueryString["appid"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        if (!IsPostBack)
        {
            M_WX_APPID appmod = ExHelper.WX_SelMyModel();
            if (!string.IsNullOrEmpty(appmod.WelStr))
            {
                try
                {
                    msgMod = JsonConvert.DeserializeObject<M_WxImgMsg>(appmod.WelStr);
                    Title_T.Text = msgMod.Articles[0].Title;
                    Content_T.Text = msgMod.Articles[0].Description;
                    PicUrl_T.Text = msgMod.Articles[0].PicUrl;
                    Url_T.Text = msgMod.Articles[0].Url;
                }
                catch { Content_T.Text = "数据格式错误:" + appmod.WelStr; }
            }
        }
    }
    //<li><a href="/Admin/Main.aspx">工作台</a></li><li><a href="/Admin/WeiXin/Default.aspx">微信管理</a></li><li class="active">微信发送</li>
    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_WXImgItem itemMod = new M_WXImgItem() { Title = Title_T.Text, Description = Content_T.Text };
        itemMod.PicUrl = StrHelper.UrlDeal(PicUrl_T.Text);
        itemMod.Url = StrHelper.UrlDeal(Url_T.Text);
        msgMod.Articles.Add(itemMod);
        M_WX_APPID appmod = ExHelper.WX_SelMyModel();
        appmod.WelStr = JsonConvert.SerializeObject(msgMod);
        appBll.UpdateByID(appmod);
        WxAPI.Code_Get(appmod).AppId.WelStr = appmod.WelStr;
        function.WriteSuccessMsg("修改成功");
    }
}