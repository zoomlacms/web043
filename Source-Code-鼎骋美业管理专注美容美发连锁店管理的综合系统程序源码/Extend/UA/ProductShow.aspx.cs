using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

public partial class Extend_UA_ProductShow : System.Web.UI.Page
{
    B_Product proBll = new B_Product();
    B_User buser = new B_User();
    B_ModelField bfield = new B_ModelField();
    B_Content conBll = new B_Content();
    B_Node nodeBll = new B_Node();
    public M_Product proMod = new M_Product();
    public M_Node nodeMod = new M_Node();
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("manage");
        M_UserInfo mu = buser.GetLogin();
        proMod = proBll.GetproductByid(Mid);
        nodeMod = nodeBll.SelReturnModel(proMod.Nodeid);
        if (mu.SiteID != proMod.UserShopID) { function.WriteErrMsg("你无权管理该商品"); }
        DataTable dr = proBll.GetContent(proMod.TableName.ToString(), proMod.ItemID);
        this.ModelHtml.Text = this.bfield.InputallHtml(proMod.ModelID, proMod.Nodeid, new ModelConfig()
        {
            ValueDT = dr,
            Mode = ModelConfig.SMode.PreView
        });
    }
}