using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Other;
using ZoomLa.Common;
using ZoomLa.Components;
using ZoomLa.Model.Other;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLaCMS.Manage.WeiXin.Msg
{
    public partial class AddMsgTlp : System.Web.UI.Page
    {
        B_WX_APPID appBll = new B_WX_APPID();
        B_WX_MsgTlp msgBll = new B_WX_MsgTlp();
        public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
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
            if (Mid > 0)
            {
                M_WX_MsgTlp msgMod = msgBll.SelReturnModel(Mid);
                Alias_T.Text = msgMod.Alias;
                switch (msgMod.MsgType)
                {
                    case "text":
                        Content_T.Text = msgMod.MsgContent;
                        break;
                    case "image":
                        {
                            M_WXImgItem itemMod = JsonConvert.DeserializeObject<M_WXImgItem>(msgMod.MsgContent);
                            Image_Title_T.Text = itemMod.Title;
                            Image_Desc_T.Text = itemMod.Description;
                            Img_UP.FileUrl = itemMod.PicUrl;
                            Image_Url_T.Text = itemMod.Url;
                        }
                        break;
                }
                function.ScriptRad(this, "msgtype_rad", msgMod.MsgType);
            }
        }

        protected void Save_Btn_Click(object sender, EventArgs e)
        {
            M_WX_MsgTlp msgMod = new M_WX_MsgTlp();
            if (Mid > 0) { msgMod = msgBll.SelReturnModel(Mid); }
            msgMod.MsgType = Request.Form["msgtype_rad"];
            msgMod.MsgContent = GetMsgContent(msgMod.MsgType);
            msgMod.Alias = Alias_T.Text;
            if (msgMod.ID > 0) { msgBll.UpdateByID(msgMod); }
            else { msgBll.Insert(msgMod); }
            function.WriteSuccessMsg("操作成功", "MsgTlpList.aspx");
        }
        private string GetMsgContent(string msgtype)
        {
            switch (msgtype)
            {
                case "text":
                    {
                        //M_WxTextMsg textMsg = new M_WxTextMsg();
                        //textMsg.Content = Content_T.Text;
                        return Content_T.Text;
                    }
                case "image":
                    {
                        //M_WxImgMsg imgMsg = new M_WxImgMsg();
                        M_WXImgItem itemMod = new M_WXImgItem() { Title = Image_Title_T.Text, Description = Image_Desc_T.Text };
                        if (Img_UP.HasFile)
                        {
                            Img_UP.SaveFile();
                            itemMod.PicUrl = SiteConfig.SiteInfo.SiteUrl + Img_UP.FileUrl;
                        }
                        itemMod.Url = StrHelper.UrlDeal(Image_Url_T.Text);
                        //imgMsg.Articles.Add(itemMod);
                        return JsonConvert.SerializeObject(itemMod);
                    }
                default:
                    throw new Exception("类型错误");
            }
        }
    }
}