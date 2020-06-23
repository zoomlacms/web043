using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Other;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model;
using ZoomLa.Model.Other;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Manage.WeiXin.Msg
{
    public partial class MsgSend : System.Web.UI.Page
    {
        B_WX_APPID appbll = new B_WX_APPID();
        B_WX_MsgTlp tlpBll = new B_WX_MsgTlp();
        WxAPI api = null;
        public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            ExHelper.CheckUserAuth("wechat");
            if (!IsPostBack)
            {
                MyBind();
                Call.HideBread(Master);
            }
        }
        private void MyBind()
        {
            M_WX_MsgTlp tlpMod = tlpBll.SelReturnModel(Mid);
            Title_L.Text = tlpMod.Alias;
            MsgType_L.Text = tlpBll.GetMsgType(tlpMod.MsgType);
            MsgContent_L.Text = tlpMod.MsgContent;
        }

        protected void Send_Btn_Click(object sender, EventArgs e)
        {
            //string appids = Request.Form["app_chk"];
            //if (string.IsNullOrEmpty(appids)) { function.WriteErrMsg("未指定需要发送的微信号"); }
            string appids = ExHelper.WX_SelMyModel().ID.ToString();
            //根据不同的消息类型调用不同的接口发送
            M_WX_MsgTlp tlpMod = tlpBll.SelReturnModel(Mid);

            DataTable dt = null;
            switch (tlpMod.MsgType)
            {
                case "text":
                    dt = SendText(tlpMod, appids);
                    break;
                case "image":
                    dt = SendImage(tlpMod, appids);
                    break;
                case "multi":
                    dt = SendMulti(tlpMod, appids);
                    break;
            }
            Result_RPT.DataSource = dt;
            Result_RPT.DataBind();
        }
        private DataTable GetResultStruct()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("appid");
            dt.Columns.Add("alias");
            dt.Columns.Add("media");
            dt.Columns.Add("isok");
            dt.Columns.Add("result");
            return dt;
        }
        private DataTable SendText(M_WX_MsgTlp tlpMod, string appids)
        {
            DataTable dt = GetResultStruct();
            foreach (string id in appids.Split(','))
            {
                if (DataConvert.CLng(id) < 1) { continue; }
                api = WxAPI.Code_Get(Convert.ToInt32(id));
                DataRow dr = dt.NewRow();
                dr["isok"] = true;
                dr["appid"] = api.AppId.APPID.ToString();
                dr["alias"] = api.AppId.Alias;
                switch (Request["mode_rad"])
                {
                    case "loop":
                        api.SendAllBySingle(tlpMod.MsgContent);
                        break;
                }
            }
            return dt;
        }
        private DataTable SendImage(M_WX_MsgTlp tlpMod, string appids)
        {
            DataTable dt = GetResultStruct();
            M_WXImgItem itemMod = JsonConvert.DeserializeObject<M_WXImgItem>(tlpMod.MsgContent);
            foreach (string id in appids.Split(','))
            {
                if (DataConvert.CLng(id) < 1) { continue; }
                api = WxAPI.Code_Get(Convert.ToInt32(id));
                DataRow dr = dt.NewRow();
                dr["isok"] = true;
                dr["appid"] = api.AppId.APPID.ToString();
                dr["alias"] = api.AppId.Alias;
                switch (Request["mode_rad"])
                {
                    case "loop":
                        M_WxImgMsg imgMod = new M_WxImgMsg();
                        imgMod.Articles.Add(itemMod);
                        api.SendAllBySingle(imgMod);
                        break;
                }
            }
            return dt;
        }
        private DataTable SendMulti(M_WX_MsgTlp tlpMod, string appids)
        {
            DataTable dt = GetResultStruct();
            List<M_WXImgItem> itemList = JsonConvert.DeserializeObject<List<M_WXImgItem>>(tlpMod.MsgContent);
            foreach (string id in appids.Split(','))
            {
                if (DataConvert.CLng(id) < 1) { continue; }
                api = WxAPI.Code_Get(Convert.ToInt32(id));
                DataRow dr = dt.NewRow();
                dr["isok"] = true;
                dr["appid"] = api.AppId.APPID.ToString();
                dr["alias"] = api.AppId.Alias;
                //每个APPID都需要独立上传一次
                //----------------------------------------------
                try
                {
                    string media = "";
                    UploadMultiNews(itemList, ref media);
                    dr["media"] = media;
                    switch (Request["mode_rad"])
                    {
                        case "loop":
                            {
                                M_WxImgMsg msg = new M_WxImgMsg();
                                msg.Articles = itemList;
                                api.SendAllBySingle(msg);
                            }
                            break;
                        case "api":
                        default:
                            {
                                M_WXAllMsg model = new M_WXAllMsg() { filter = new M_WXFiter() { group_id = "", is_to_all = true }, msgtype = "mpnews", mpnews = new M_WXMsgMedia() { media_id = media } };
                                api.SendAll(model);
                            }
                            break;
                    }
                }
                catch (Exception ex) { dr["result"] = ex.Message; dr["isok"] = false; }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        // 微信上传图文素材,然后再群发
        private bool UploadMultiNews(List<M_WXImgItem> imgList, ref string artmedia)
        {
            List<M_WXNewsItem> itemList = new List<M_WXNewsItem>();
            foreach (var item in imgList)
            {
                //上传封面图片
                string media = "";
                if (!api.UploadImg(item.PicUrl, ref media)) { throw new Exception("封面上传失败,原因:" + media); }
                //添加多图文信息
                itemList.Add(new M_WXNewsItem()
                {
                    title = item.Title,
                    digest = "digest",
                    thumb_media_id = media,
                    author = SiteConfig.SiteInfo.SiteName,
                    content = item.Description,
                    content_source_url = item.Url
                });
            }
            string result= api.UploadMPNews(itemList);
            JObject jobj = JsonConvert.DeserializeObject<JObject>(result);
            artmedia = jobj["media_id"].ToString();
            return true;
        }
    }
}