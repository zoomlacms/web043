using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ZoomLa.BLL.API;
using ZoomLa.BLL;
using ZoomLa.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZoomLa.Model;
using ZoomLa.BLL.User.Addon;
using ZoomLa.Model.User;
using System.Web;
using ZoomLa.BLL.CreateJS;
using System.Data.SqlClient;
using System.Data;

namespace ZoomLa.Sns
{
    public class ZLPlugController : Controller
    {
        public ActionResult Test() { return View(); }
        public ActionResult Test2() { return View(); }
        private const string APIKey = "7fef6171469e80d32c0559f88b377245";
        M_APIResult retMod = new M_APIResult(M_APIResult.Failed);
        [HttpPost]
        public string wxqrcode(int refereeType, string refereeId, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key)) { retMod.retmsg = "未指定key"; }
                else if (!key.Equals(APIKey)) { retMod.retmsg = "key不正确"; }
                else
                {
                    int scenceid = 0;
                    B_CodeModel codeBll = new B_CodeModel("ZL_SSW_WXQRCode");
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("refereeId", refereeId) };
                    DataTable dt = codeBll.SelByWhere("refereeType=" + refereeType + " AND refereeId=" + refereeId, "ID DESC", sp);
                    if (dt.Rows.Count < 1)
                    {
                        DataRow dr = dt.NewRow();
                        dr["refereeType"] = refereeType;
                        dr["refereeId"] = refereeId;
                        scenceid = codeBll.Insert(dr);
                    }
                    else { scenceid = Convert.ToInt32(dt.Rows[0]["ID"]); }
                    //--------------------------------------------------------------------------
                    WxAPI wxapi = WxAPI.Code_Get();
                    string resultStr = APIHelper.GetWebResult("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + wxapi.AccessToken + "&type=jsapi", "POST", "{\"expire_seconds\":604800,\"action_name\":\"QR_LIMIT_SCENE\",\"action_info\":{\"scene\":{\"scene_id\":" + scenceid + "}}}");
                    JObject result = (JObject)JsonConvert.DeserializeObject(resultStr);
                    if (result["ticket"] == null) { retMod.retmsg = resultStr; ZLLog.L("wxqrcode failed1:" + resultStr); }
                    else
                    {
                        retMod.result = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(result["ticket"].ToString());
                        retMod.retcode = M_APIResult.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                retMod.retmsg = ex.Message;
                ZLLog.L("wxqrcode failed2:" + ex.Message);
            }
            return retMod.ToString();
        }
        /// <summary>
        /// 微信推送
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="content">请将内容UrlEncode编码</param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public string wxpush(string openid, string content, string key)
        {
            try
            {
                if (!key.Equals(APIKey)) { retMod.retmsg = "key不正确"; }
                else if (string.IsNullOrEmpty(openid)) { retMod.retmsg = "未指定openid"; }
                else if (string.IsNullOrEmpty(content)) { retMod.retmsg = "内容不能为空"; }
                else
                {
                    WxAPI api = WxAPI.Code_Get();
                    retMod.result = api.SendMsg(openid, HttpUtility.UrlDecode(content)); 
                    retMod.retcode = M_APIResult.Success;
                }
            }
            catch (Exception ex) { ZLLog.L("wxpush failed:[" + openid + "][" + content + "]" + ex.Message); retMod.retmsg = ex.Message; }
            return retMod.ToString();
        }
    }
}