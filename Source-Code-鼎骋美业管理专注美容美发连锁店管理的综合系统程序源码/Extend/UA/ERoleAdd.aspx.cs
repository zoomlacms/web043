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

public partial class Extend_UA_ERoleAdd : System.Web.UI.Page
{
    B_Ex_ERole erBll = new B_Ex_ERole();
    B_User buser = new B_User();
    M_UserInfo mu = new M_UserInfo();
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("manage");
        mu = buser.GetLogin();
        if (!IsPostBack)
        {
            if (Mid > 0)
            {
                M_Ex_ERole erMod = erBll.SelReturnModel(Mid);
                if (erMod == null) { function.WriteErrMsg("角色信息不存在"); }
                if (erMod.StoreID != mu.SiteID) { function.WriteErrMsg("你无权操作该信息"); }
                RoleName_T.Text = erMod.RoleName;
                Remark_T.Text = erMod.Remark;
                function.ScriptRad(this,"ztype_rad",erMod.ZType.ToString());
                function.ScriptRad(this, "auth_chk", erMod.RoleAuth);
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_Ex_ERole erMod = new M_Ex_ERole();
        if (Mid > 0) { erMod = erBll.SelReturnModel(Mid); }
        erMod.RoleName = RoleName_T.Text.Trim();
        erMod.RoleAuth = Request.Form["auth_chk"];
        erMod.Remark = Remark_T.Text.Trim();
        erMod.ZType = DataConvert.CLng(Request.Form["ztype_rad"]);
        if (erMod.ID > 0) { erBll.UpdateByID(erMod); }
        else
        {
            erMod.StoreID = mu.SiteID;
            erMod.ID = erBll.Insert(erMod);
        }
        function.WriteSuccessMsg("操作成功","/Extend/UA/ERole.aspx");
    }
}