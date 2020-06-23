using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_ClientInfo : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_User_StoreUser suBll = new B_User_StoreUser();
    public M_User_StoreUser suMod = null;
    public M_UserInfo userMod = null;
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("client");
        M_UserInfo mu = buser.GetLogin();
        userMod = buser.SelReturnModel(Mid);
        suMod = suBll.SelModelByUid(userMod.UserID, mu.SiteID);
        if (!IsPostBack)
        {
            HoneyName_T.Text = suMod.HoneyName;
            BirthDay_T.Text = suMod.BirthDay;
            WXNo_T.Text = suMod.WXNo;
            Mobile_T.Text = suMod.Mobile;
            Keywords.Text = suMod.UserLabel;
            function.ScriptRad(this, "CardStatus_rad",suMod.CardStatus.ToString());
            function.ScriptRad(this,"UserLevel_rad",suMod.UserLevel.ToString());
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        suMod = suBll.SelModelByUid(Mid, mu.SiteID);
        suMod.HoneyName = HoneyName_T.Text;
        suMod.Mobile = Mobile_T.Text;
        suMod.BirthDay = DataConvert.CDate(BirthDay_T.Text).ToString("yyyy/MM/dd");
        suMod.WXNo = WXNo_T.Text;
        suMod.UserLabel = Request.Form["tabinput"];
        suMod.UserLevel = DataConvert.CLng(Request.Form["UserLevel_rad"]);
        suMod.CardStatus = DataConvert.CLng(Request.Form["CardStatus_rad"]);
        suBll.UpdateByID(suMod);
        function.WriteSuccessMsg("操作成功");
    }

}