using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
using ZoomLa.SQLDAL;

public partial class Extend_UA_ProductAdd : System.Web.UI.Page
{
    B_Store_Info storeBll = new B_Store_Info();
    B_User buser = new B_User();
    B_Model modBll = new B_Model();
    B_ModelField fieldBll = new B_ModelField();
    B_Node nodeBll = new B_Node();
    B_Product proBll = new B_Product();
    B_Group gpBll = new B_Group();
    B_Stock stockBll = new B_Stock();
    B_Shop_FareTlp fareBll = new B_Shop_FareTlp();
    B_KeyWord keyBll = new B_KeyWord();
    public M_Product proMod = new M_Product();
    public int ProID { get { return DataConverter.CLng(Request.QueryString["ID"]); } }
    public int NodeID = 13;
    public int ModelID = 20;
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("manage");
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    public void MyBind()
    {
        M_Product pinfo = null;
        if (ProID > 0) { proMod = pinfo = proBll.GetproductByid(ProID); NodeID = pinfo.Nodeid; }
        //-------------------------------
        M_Node nodeMod = nodeBll.SelReturnModel(NodeID);
        if (nodeMod.IsNull) { function.WriteErrMsg("节点[" + NodeID + "]不存在"); }
        Node_RPT.DataSource = ExHelper.Store_NodeSel();
        Node_RPT.DataBind();
        function.ScriptRad(this,"node_rad",NodeID.ToString());
        UpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        AddTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ProCode.Text = B_Product.GetProCode();
        if (ProID > 0)
        {
            #region 修改
            ModelID = pinfo.ModelID;
            AllClickNum_T.Text = pinfo.AllClickNum.ToString();
            ClickType.Value = "update";
            btnAdd.Visible = true;
            istrue_chk.Checked = pinfo.Istrue == 1 ? true : false;
            ProCode.Text = pinfo.ProCode;
            BarCode.Text = pinfo.BarCode;
            Proname.Text = pinfo.Proname;
            ProUnit.Text = pinfo.ProUnit;
            Recommend_T.Text = pinfo.Recommend.ToString();
           
            Proinfo.Text = pinfo.Proinfo;
            procontent.Value = pinfo.Procontent;
            //txt_Clearimg.Text = pinfo.Clearimg;
            //txt_Thumbnails.Text = pinfo.Thumbnails;
            //Quota.Text = pinfo.Quota.ToString();
            //DownQuota.Text = pinfo.DownQuota.ToString();

            ShiPrice.Text = pinfo.ShiPrice.ToString();
            LinPrice.Text = pinfo.LinPrice.ToString();
            UpdateTime.Text = pinfo.UpdateTime.ToString();
            AddTime.Text = pinfo.AddTime.ToString();
            isnew_chk.Checked = pinfo.Isnew == 1;//是否新品,热,等
            ishot_chk.Checked = pinfo.Ishot == 1;
            isbest_chk.Checked = pinfo.Isbest == 1;
            Sales_Chk.Checked = pinfo.Sales == 1;
            DataTable valueDT = proBll.GetContent(pinfo.TableName.ToString(), pinfo.ItemID);
            if (valueDT != null && valueDT.Rows.Count > 0)
            {
                ModelHtml.Text = fieldBll.InputallHtml(ModelID, NodeID, new ModelConfig()
                {
                    ValueDT = valueDT
                });
            }
            #endregion
        }
        else
        {
            isnew_chk.Checked = true;
            Sales_Chk.Checked = true;
            ModelHtml.Text = fieldBll.InputallHtml(ModelID, NodeID, new ModelConfig()
            {
                Source = ModelConfig.SType.Admin
            });
            btnAdd.Visible = false;
        }
    }
    //保存
    protected void EBtnSubmit_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        M_Store_Info storeMod = storeBll.SelModelByUser(mu.UserID);
        DataTable dt = fieldBll.GetModelFieldList(ModelID);
        DataTable gpdt = gpBll.GetGroupList();
        DataTable table = new Call().GetDTFromPage(dt, Page, ViewState);
        M_Product proMod = new M_Product();
        if (ProID > 0)
        {
            proMod = proBll.GetproductByid(ProID);
        }
        /*--------------proMod------------*/
        proMod.Nodeid = NodeID;
        proMod.ModelID = ModelID;
        proMod.ProCode = ProCode.Text;
        proMod.BarCode = BarCode.Text.Trim();
        proMod.Proname = Proname.Text.Trim();
        proMod.Kayword = Request.Form["tabinput"];
        keyBll.AddKeyWord(proMod.Kayword, 1);
        proMod.ProUnit = ProUnit.Text;
        proMod.AllClickNum = DataConverter.CLng(Request.Form["AllClickNum"]);
        proMod.ProClass = 1;
        proMod.Proinfo = Proinfo.Text;
        proMod.Procontent = procontent.Value;

        if (!string.IsNullOrEmpty(Request.Form["txt_pics"]))
        {
            try
            {
                DataTable imgdt = JsonConvert.DeserializeObject<DataTable>(Request.Form["txt_pics"]);
                if (imgdt.Rows.Count > 0)
                {
                    proMod.Thumbnails = proMod.Clearimg = DataConvert.CStr(imgdt.Rows[0]["url"]);
                }
            }
            catch (Exception) { }
        }
        //proMod.Quota = DataConverter.CLng(Quota.Text);
        //proMod.DownQuota = DataConverter.CLng(DownQuota.Text);
        proMod.ShiPrice = DataConverter.CDouble(ShiPrice.Text);
        proMod.LinPrice = DataConverter.CDouble(LinPrice.Text);
        proMod.Recommend = DataConverter.CLng(Recommend_T.Text);
        proMod.AllClickNum = DataConverter.CLng(AllClickNum_T.Text);
        //更新时间，若没有指定则为当前时间
        proMod.UpdateTime = DataConverter.CDate(UpdateTime.Text);
        proMod.AddTime = DataConverter.CDate(AddTime.Text);
        proMod.FirstNodeID = nodeBll.SelFirstNodeID(NodeID);
        proMod.UserShopID = storeMod.ID;
        proMod.UserType = DataConverter.CLng(Request.Form["UserPrice_Rad"]);
        proMod.Quota = DataConvert.CLng(Request.Form["Quota_Rad"]);
        proMod.DownQuota = DataConvert.CLng(Request.Form["DownQuota_Rad"]);
        
        proMod.TableName = modBll.SelReturnModel(ModelID).TableName;
        proMod.Sales = Sales_Chk.Checked ? 1 : 2;
        proMod.Istrue = istrue_chk.Checked ? 1 : 0;
        proMod.Ishot = ishot_chk.Checked ? 1 : 0;
        proMod.Isnew = isnew_chk.Checked ? 1 : 0;
        proMod.Isbest = isbest_chk.Checked ? 1 : 0;
        proMod.Allowed = 1;
        proMod.GuessXML = Request.Form["GuessXML"];
        proMod.BindIDS = "";
        string danju = proMod.UserShopID + DateTime.Now.ToString("yyyyMMddHHmmss");
        if (proMod.ID < 1 || ClickType.Value.Equals("addasnew"))
        {
            proMod.AddUser = mu.UserName;
            proMod.Nodeid = NodeID;
            proMod.AddTime = DateTime.Now;
            proMod.UpdateTime = DateTime.Now;
            proMod.ID = proBll.Add(table, proMod);
        }
        else
        {
            proBll.Update(table, proMod);
        }
        Response.Redirect("ProductShow.aspx?ID=" + proMod.ID);
    }
    //添加为新商品
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ClickType.Value = "addasnew";
        EBtnSubmit_Click(sender, e);
    }
}