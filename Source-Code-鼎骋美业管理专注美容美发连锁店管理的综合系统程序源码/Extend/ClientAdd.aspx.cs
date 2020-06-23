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

public partial class Extend_ClientAdd : System.Web.UI.Page
{
    B_User_StoreUser suBll = new B_User_StoreUser();
    B_User buser = new B_User();
    public int Mid { get { return DataConverter.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("client");
        if (!IsPostBack)
        {
            if (Mid > 0)
            {
                M_UserInfo mu = buser.GetLogin();
                M_User_StoreUser suMod = suBll.SelReturnModel(Mid);
                if (suMod.StoreID != mu.SiteID) { function.WriteErrMsg("该客户不可访问"); }
                HoneyName.Text = suMod.HoneyName;
                Mobile.Text = suMod.Mobile;
                Sex_DP.SelectedValue = suMod.Sex;
                BirthDay.Text = suMod.BirthDay;
                WXNo.Text = suMod.WXNo;
                Remark.Text = suMod.Remark;
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        M_User_StoreUser suMod = new M_User_StoreUser();
        if (Mid > 0) { suMod = suBll.SelReturnModel(Mid); }
        suMod.HoneyName = HoneyName.Text.Trim();
        suMod.Mobile = Mobile.Text.Trim();
        suMod.Sex = Sex_DP.SelectedValue;
        suMod.BirthDay = BirthDay.Text.Trim();
        suMod.WXNo = WXNo.Text.Trim();
        suMod.Remark = Remark.Text.Trim();
        if (suMod.ID < 1)
        {
            //同步新建用户并关联
            M_UserInfo newmu = buser.NewUser(function.GetRandomString(8),function.GetRandomString(6));
            newmu.HoneyName = suMod.HoneyName;
            newmu.ZnPassword = suMod.Mobile;
            newmu.WorkNum = "前台添加";
            newmu.UserID = buser.Add(newmu);
            suMod.StoreID = mu.SiteID;
            suMod.UserID = newmu.UserID;
            suMod.ID = suBll.Insert(suMod);
        }
        else
        {
            M_UserInfo newmu = buser.SelReturnModel(suMod.UserID);
            newmu.HoneyName = suMod.HoneyName;
            newmu.ZnPassword = suMod.Mobile;
            buser.UpdateByID(newmu);
            suBll.UpdateByID(suMod);
        }
        function.WriteSuccessMsg("操作成功","Client.aspx");
    }
}