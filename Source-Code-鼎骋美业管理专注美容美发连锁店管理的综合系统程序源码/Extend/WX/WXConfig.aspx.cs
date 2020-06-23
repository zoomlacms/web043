using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_WXConfig : System.Web.UI.Page
{
    B_Store_Info storeBll = new B_Store_Info();
    B_User buser = new B_User();
    B_WX_APPID appBll = new B_WX_APPID();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("wechat");
        if (!IsPostBack)
        {
            M_UserInfo mu = buser.GetLogin();
            M_CommonData storeMod = ExHelper.Store2_User();
            M_WX_APPID appMod = new M_WX_APPID();
            if (DataConvert.CLng(storeMod.SpecialID) > 0)
            {
                appMod = appBll.SelReturnModel(DataConvert.CLng(storeMod.SpecialID));
            }
            if (appMod != null)
            {
                WXNo_T.Text = appMod.WxNo;
                AppID_T.Text = appMod.APPID;
                Secret_T.Text = appMod.Secret;
                QCode_UP.FileUrl = function.GetImgUrl(appMod.QRCode);
                OrginID.Text = appMod.OrginID;
                Pay_Account.Text = appMod.Pay_AccountID;
                Pay_Key.Text = appMod.Pay_Key;
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        M_CommonData storeMod = ExHelper.Store2_User();
        M_WX_APPID appMod = new M_WX_APPID();
        if (DataConvert.CLng(storeMod.SpecialID) > 0)
        {
            appMod = appBll.SelReturnModel(DataConvert.CLng(storeMod.SpecialID));
        }
        appMod.Alias = storeMod.Title;
        appMod.APPID = AppID_T.Text.Trim();
        if (QCode_UP.HasFile)
        {
            QCode_UP.SaveFile();
            appMod.QRCode = QCode_UP.FileUrl.Replace("/UploadFiles/", ""); ;
        }
        appMod.WxNo = WXNo_T.Text.Trim();
        appMod.Secret = Secret_T.Text.Trim();
        appMod.OrginID = OrginID.Text.Trim();
        appMod.Pay_AccountID = Pay_Account.Text.Trim();
        appMod.Pay_Key = Pay_Key.Text.Trim();
        if (appMod.ID > 0) { appBll.UpdateByID(appMod); }
        else
        {
            appMod.ID = appBll.Insert(appMod);
            storeMod.SpecialID = appMod.ID.ToString();
            DBCenter.UpdateSQL("ZL_Store_Reg", "StoreStyleID=" + appMod.ID, "ID=" + storeMod.ItemID);
        }
        function.WriteSuccessMsg("操作成功");
    }
}