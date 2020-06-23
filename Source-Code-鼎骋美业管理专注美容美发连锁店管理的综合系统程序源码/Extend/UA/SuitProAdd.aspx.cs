using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;

public partial class Extend_UA_SuitProAdd : System.Web.UI.Page
{
    B_Product proBll = new B_Product();
    B_Node nodeBll = new B_Node();
    B_Model modBll = new B_Model();
    B_Content conBll = new B_Content();
    B_User buser = new B_User();
    M_UserInfo mu = null;
    public int NodeID = 16;
    public int Mid { get { return DataConverter.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        mu = buser.GetLogin();
        if (!IsPostBack)
        {
            MyBind();
            Call.SetBreadCrumb(Master, "<li><a href='" + CustomerPageAction.customPath2 + "I/Main.aspx'>工作台</a></li><li><a href='../ProductManage.aspx'>商城管理</a></li><li><a href='SuitPro.aspx'>促销组合</a></li><li class='active'>编辑促销组合</li>");
        }
    }
    public void MyBind()
    {
        //DataTable dt = nodeBll.GetAllShopNode();
        //Node_L.Text = "<select id='node_dp' name='node_dp' class='form-control text_300'>" + nodeBll.CreateDP(dt) + "</select>";
        if (Mid > 0)
        {
            M_Product proMod = proBll.GetproductByid(Mid);
            if (proMod == null) { function.WriteErrMsg("商品[" + Mid + "]不存在"); }
            Name_T.Text = proMod.Proname;
            UProIDS_Hid.Value = proMod.Procontent;
            //ZStatus_Chk.Checked = proMod.Istrue == 1;
            Remind_T.Text = proMod.Proinfo;
            Price_T.Text = proMod.LinPrice.ToString("F2");
            //BookPrice_T.Text = proMod.BookPrice.ToString("F2");
            //Stock.Text = proMod.Stock.ToString();
            SFileUp.FileUrl = proMod.Thumbnails;
            //function.Script(this, "setnode(" + proMod.Nodeid + ");");
            //function.Script(this, "SetChkVal('GuessXML','" + proMod.GuessXML + "');");
        }
    }
    protected void Save_B_Click(object sender, EventArgs e)
    {
        //存商品表中,这样只有显示的逻辑需要扩展,其他逻辑可不变
        //商品增加筛选模型,便于扩展
        M_UserInfo mu = buser.GetLogin();
        M_Product proMod = proBll.GetproductByid(Mid);
        if (proMod == null) { proMod = new M_Product(); }
        proMod.Proname = Name_T.Text.Trim();
        //proMod.BindIDS = UProIDS_Hid.Value;
        proMod.Procontent = UProIDS_Hid.Value;
        if (SFileUp.HasFile)
        {
            SFileUp.SaveUrl = ZLHelper.GetUploadDir_System("shop", "product", "yyyyMMdd");
            proMod.Thumbnails = SFileUp.SaveFile();
        }
        proMod.LinPrice = DataConverter.CDouble(Price_T.Text);
        //proMod.BookPrice = DataConverter.CDouble(BookPrice_T.Text);
        proMod.ShiPrice = proMod.LinPrice;
        //proMod.Istrue = ZStatus_Chk.Checked ? 1 : 0;
        proMod.Proinfo = Remind_T.Text;
        proMod.Nodeid = 16;
        proMod.ModelID = 20;
        proMod.GuessXML = Request.Form["GuessXML"];
        proMod.UserShopID = mu.SiteID;
        if (proMod.ID > 0)
        {
            proBll.UpdateByID(proMod);
        }
        else
        {
            proMod.FirstNodeID = nodeBll.SelFirstNodeID(proMod.Nodeid);
            proMod.Class = 2;
            proMod.ProClass = 1;
            proMod.UserID = mu.UserID;
            proMod.AddUser = mu.UserName;
            proBll.Insert(proMod);
        }
        function.WriteSuccessMsg("操作成功", "SuitPro.aspx");
    }
}