using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.SQLDAL;

public partial class Extend_Wechat : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        WxAPI.AutoSync(Request.Url.AbsoluteUri);
        int sid = DataConvert.CLng(Request.QueryString["ID"]);
        if (sid > 1) { Response.Cookies["sid"].Value = sid.ToString(); }
        else if (Request.Cookies["sid"] != null)//看Cookie中是否有信息
        {
            sid = DataConvert.CLng(Request.Cookies["sid"].Value);
        }
        if (sid < 1) { function.WriteErrMsg("未指定店铺ID");return; }
        if (!DBCenter.IsExist("ZL_CommonModel", "GeneralID=" + sid + " AND ModelID=24"))
        {
            function.WriteErrMsg("指定店铺的信息不存在");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
