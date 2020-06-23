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

public partial class Extend_WXConfig : System.Web.UI.Page
{
    B_Store_Info storeBll = new B_Store_Info();
    B_User buser = new B_User();
    B_Ex_WXAPPID appBll = new B_Ex_WXAPPID();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            M_UserInfo mu = buser.GetLogin();
            M_WX_APPID appMod = appBll.SelModelByStoreId(mu.SiteID);
            if (appMod != null)
            {
                WXNo_T.Text = appMod.WxNo;
                AppID_T.Text = appMod.APPID;
                Secret_T.Text = appMod.Secret;
                QCode_UP.FileUrl = function.GetImgUrl(appMod.QRCode);
                OrginID.Text = appMod.OrginID;
                Pay_Account.Text = appMod.Pay_AccountID;
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        M_Store_Info storeMod = storeBll.SelReturnModel(mu.SiteID);
        M_WX_APPID appMod = appBll.SelModelByStoreId(storeMod.ID);
        if (appMod == null) { appMod = new M_WX_APPID(); }
        appMod.IsDefault = storeMod.ID;
        appMod.Alias = storeMod.Title;
        appMod.APPID = AppID_T.Text.Trim();
        if (QCode_UP.HasFile)
        {
            QCode_UP.SaveFile();
            appMod.QRCode = QCode_UP.FileUrl.Replace("/UploadFiles/",""); ;
        }
        appMod.WxNo = WXNo_T.Text.Trim();
        appMod.Secret = Secret_T.Text.Trim();
        appMod.OrginID = OrginID.Text.Trim();
        appMod.Pay_AccountID = Pay_Account.Text.Trim();
        if (appMod.ID > 0) { appBll.UpdateByID(appMod); }
        else
        {
            appMod.ID = appBll.Insert(appMod);
            M_ModelInfo modInfo = new B_Model().SelReturnModel(50);
            B_Content conBll = new B_Content();
            M_CommonData conMod = new M_CommonData();
            conMod.NodeID = 5;
            conMod.ModelID = modInfo.ModelID;
            conMod.Title = appMod.Alias;
            conMod.Inputer = mu.UserName;
            conMod.ItemID = appMod.ID;
            conMod.TableName = modInfo.TableName;
            conBll.insert(conMod);
        }
        function.WriteSuccessMsg("操作成功");
    }
}