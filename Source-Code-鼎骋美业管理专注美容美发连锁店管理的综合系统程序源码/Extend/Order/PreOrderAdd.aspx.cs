using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_Order_PreOrderAdd : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_Ex_PreOrder poBll = new B_Ex_PreOrder();
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("cash");
        if (!IsPostBack)
        {
            if (Mid > 0)
            {
                M_Ex_PreOrder poMod = poBll.SelReturnModel(Mid);
                ClientMobile_T.Text = poMod.ClientMobile;
                ClientName_T.Text = poMod.ClientName;
                ClientDate.Text = poMod.ClientDate.ToString("yyyy/MM/dd");
                ClientDate2.Text = poMod.ClientDate.ToString("HH:mm");
                ClientNeed_Hid.Value = poMod.ClientNeed;
                Remark.Text = poMod.Remark;
            }
            else
            {
                ClientDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_Ex_PreOrder poMod = new M_Ex_PreOrder();
        if (Mid > 0)
        {
            poMod = poBll.SelReturnModel(Mid);
        }
        poMod.ClientMobile = ClientMobile_T.Text;
        poMod.ClientName = ClientName_T.Text;
        poMod.ClientDate = Convert.ToDateTime(ClientDate.Text + " " + ClientDate2.Text+":00");
        poMod.ClientNeed = ClientNeed_Hid.Value;
        poMod.Remark = Remark.Text;
        if (poMod.ID > 0) { poBll.UpdateByID(poMod); }
        else
        {
            M_UserInfo mu = buser.GetLogin();
            poMod.EmployID = mu.UserID;
            poMod.EmployName = mu.UserName;
            poMod.StoreID = mu.SiteID;
            poBll.Insert(poMod);
        }
        function.WriteSuccessMsg("操作成功", "PreOrder.aspx");
    }
}