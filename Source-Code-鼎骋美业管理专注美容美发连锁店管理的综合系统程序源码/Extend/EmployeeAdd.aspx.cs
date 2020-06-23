using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns;
using ZoomLa.Sns.BLL;
using ZoomLa.SQLDAL;

public partial class Extend_EmployeeAdd : System.Web.UI.Page
{
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    B_User buser = new B_User();
    B_Store_Info storeBll = new B_Store_Info();
    B_Ex_ERole erBll = new B_Ex_ERole();
    B_Shop_Bonus bnBll = new B_Shop_Bonus();
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("employee");
        if (!IsPostBack)
        {
            M_UserInfo mu = buser.GetLogin();
            Bonus_RPT.DataSource = bnBll.Sel(
                new Com_Filter() { storeId = mu.SiteID });
            Bonus_RPT.DataBind();
            ERole_RPT.DataSource = B_Ex_ERole.SelRoles();
            ERole_RPT.DataBind();
            if (Mid > 0)
            {
                M_UserInfo userMod = buser.SelReturnModel(Mid);
                if (userMod.IsNull) { function.WriteErrMsg("用户不存在"); }
                UserName_T.Text = userMod.UserName;
                Mobile_T.Text = userMod.ZnPassword;
                SFileUp.FileUrl = userMod.UserFace;
                UserName_T.Attributes["disabled"] = "disabled";
                SiteRebateBalance_T.Text = userMod.SiteRebateBalance.ToString("F0");
                function.ScriptRad(this,"bonus_rad",userMod.VIP.ToString());
                function.ScriptRad(this, "role_rad", userMod.PageID.ToString());
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo loginUser = buser.GetLogin();
        M_Store_Info storeMod = storeBll.SelModelByUser(loginUser.UserID);
        M_UserInfo mu = null;
        string uname = UserName_T.Text.Trim();
        string upwd = UserPwd_T.Text.Trim();
        string upwd2 = UserPwd2_T.Text.Trim();
        if (Mid > 0)
        {
            mu = buser.SelReturnModel(Mid);
            //修改密码
            if (!string.IsNullOrEmpty(upwd))
            {
                if (!upwd.Equals(upwd2)) { function.WriteErrMsg("密码与确认密码不匹配"); }
                mu.UserPwd = StringHelper.MD5(upwd);
            }
        }
        else
        {
            if (StrHelper.StrNullCheck(uname, upwd, upwd2)) { function.WriteErrMsg("用户名或密码不能为空"); }
            if (!upwd.Equals(upwd2)) { function.WriteErrMsg("密码与确认密码不匹配"); }
            if (buser.IsExistUName(uname)) { function.WriteErrMsg("用户名[" + uname + "]已存在"); }
            mu = buser.NewUser(uname, upwd);
            mu.SiteID = storeMod.ID;
            mu.GroupID = ExConast.EmployGroup;
        }
        mu.SiteRebateBalance = DataConvert.CDouble(SiteRebateBalance_T.Text);
        mu.ZnPassword = Mobile_T.Text;
        mu.VIP = DataConvert.CLng(Request.Form["bonus_rad"]);
        mu.PageID = DataConvert.CLng(Request.Form["role_rad"]);
        if (SFileUp.HasFile)
        {
            SFileUp.SaveUrl = ZLHelper.GetUploadDir_System("shop", "user");
            mu.UserFace = SFileUp.SaveFile();
        }
        if (mu.UserID > 0)
        {
            buser.UpdateByID(mu);
        }
        else
        {
            buser.AddModel(mu);
        }
        function.WriteSuccessMsg("操作成功","Employee.aspx");
    }
}