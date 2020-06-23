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
using ZoomLa.SQLDAL;

public partial class Extend_UA_MoneyRegularAdd : System.Web.UI.Page
{
    B_Shop_MoneyRegular regularBll = new B_Shop_MoneyRegular();
    M_Shop_MoneyRegular regularMod = new M_Shop_MoneyRegular();
    B_User buser = new B_User();
    private int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    private void MyBind()
    {
        M_UserInfo mu = buser.GetLogin();
        if (Mid > 0)
        {
            regularMod = regularBll.SelReturnModel(Mid);
            if (regularMod.StoreID != mu.SiteID) { function.WriteErrMsg("你无权访问该信息"); }
            Alias_T.Text = regularMod.Alias;
            Min_T.Text = regularMod.Min.ToString("f2");
            Purse_T.Text = regularMod.Purse.ToString("f2");
            Point_T.Text = regularMod.Point.ToString("f2");
            AdminRemind_T.Text = regularMod.AdminRemind;
        }
    }
    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        if (Mid > 0) { regularMod = regularBll.SelReturnModel(Mid); }
        regularMod.Alias = Alias_T.Text.Trim();
        regularMod.Min = DataConvert.CDouble(Min_T.Text);
        if (regularMod.Min <= 0) { function.WriteErrMsg("充值金额不能为0"); }
        regularMod.Purse = DataConvert.CDouble(Purse_T.Text);
        regularMod.Point = DataConvert.CDouble(Point_T.Text);
        regularMod.AdminRemind = AdminRemind_T.Text.Trim();
        regularMod.AdminID = regularMod.AdminID;
        if (Mid > 0)
        {
            regularMod.EditDate = DateTime.Now;
            regularBll.UpdateByID(regularMod);
        }
        else
        {
            regularMod.StoreID = mu.SiteID;
            //if (regularBll.IsExist(regularMod.Min)) { function.WriteErrMsg("充值金额[" + regularMod.Min.ToString("f2") + "]的规则已存在,不可重复添加"); }
            regularBll.Insert(regularMod);
        }
        function.WriteSuccessMsg("保存成功", "MoneyRegular.aspx");
    }
}