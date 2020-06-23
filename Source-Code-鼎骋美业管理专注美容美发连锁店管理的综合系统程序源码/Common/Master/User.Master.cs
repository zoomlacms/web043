using System;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;

namespace ZoomLaCMS.Common.Master
{
    public partial class User : System.Web.UI.MasterPage
    {
        M_UserInfo info = new M_UserInfo();
        B_User buser = new B_User();
        B_Admin badmin = new B_Admin();
        public M_UserInfo mu = new M_UserInfo();
        protected void Page_Init(object sender, EventArgs e)
        {
            mu = buser.GetLogin();
            if (buser.CheckLogin())
            {
                if (mu == null || mu.IsNull || mu.UserID < 1)
                {
                    Response.Redirect("/User/Login");
                }
                else if (mu.Status != 0) { function.WriteErrMsg("你的帐户未通过验证或被锁定，请与网站管理员联系", "/User/Login"); }
            }
            else
            {
                B_User.CheckIsLogged(Request.RawUrl);
            }
            CommonReturn retMod = ExHelper.CheckUserLogin();
            if (!retMod.isok) { function.WriteErrMsg(retMod.err); }
        }
        public M_Store_Info storeMod = null;
        B_Store_Info storeBll = new B_Store_Info();
        protected void Page_Load(object sender, EventArgs e)
        {
            storeMod = storeBll.SelReturnModel(mu.SiteID);
            //M_UserInfo mu = buser.GetLogin();
            //uName.Text = mu.UserName;
        }
    }
}