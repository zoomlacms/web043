using Newtonsoft.Json;
using System;
using System.Data;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;

namespace ZoomLaCMS.Manage.UserShopMannger
{
    public partial class StoreUpdate : CustomerPageAction
    {
        B_WX_APPID wxBll = new B_WX_APPID();
        B_ModelField mfbll = new B_ModelField();
        B_Content conBll = new B_Content();
        B_Model modBll = new B_Model();
        B_Store_Info stBll = new B_Store_Info();
        B_Store_Style styleBll = new B_Store_Style();
        B_User buser = new B_User();
        public int Mid { get { return DataConverter.CLng(Request.QueryString["ID"]); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                BindWX_DP.DataSource = wxBll.Sel();
                BindWX_DP.DataBind();
                BindWX_DP.Items.Insert(0,"未绑定微信");
                TlpView_Tlp.DataSource = styleBll.Sel();
                TlpView_Tlp.DataBind();
                if (Mid > 0)
                {
                    M_CommonData storeMod = conBll.SelReturnModel(Mid);
                    UserShopName_T.Text = storeMod.Title;
                    DataTable condt = conBll.GetContent(Mid);
                    ModelHtml.Text = mfbll.InputallHtml(storeMod.ModelID, 0, new ModelConfig()
                    {
                        ValueDT = condt
                    });
                    TlpView_Tlp.SelectedID = storeMod.DefaultSkins.ToString();
                    UserName_T.Text = storeMod.Inputer;
                    UserName_H.Value = storeMod.SuccessfulUserID.ToString();
                    BindWX_DP.SelectedValue = storeMod.SpecialID;
                }
                else
                {
                    DataTable dt = modBll.SelByType("6");
                    if (dt.Rows.Count < 1) { function.WriteErrMsg("请先创建店铺申请模型"); }
                    ModelHtml.Text = mfbll.InputallHtml(Convert.ToInt32(dt.Rows[0]["ModelID"]), 0, new ModelConfig());
                }
                Call.SetBreadCrumb(Master, "<li><a href='" + customPath2 + "Main.aspx'>工作台</a></li><li><a href='../Shop/ProductManage.aspx'>商城管理</a></li><li><a href='StoreManage.aspx'>店铺管理</a></li><li class='active'>店铺信息</li>");
            }
        }
        protected void Esubmit_Click(object sender, EventArgs e)
        {
            M_CommonData storeMod = new M_CommonData();
            M_UserInfo mu = buser.SelReturnModel(DataConverter.CLng(UserName_H.Value));
            if (mu.IsNull) { function.WriteErrMsg("未为店铺指定用户"); }
            if (Mid > 0) { storeMod = conBll.SelReturnModel(Mid); }
            else
            {
                storeMod.ModelID = Convert.ToInt32(modBll.SelByType("6").Rows[0]["ModelID"]);
                M_ModelInfo modMod = modBll.SelReturnModel(storeMod.ModelID);
                storeMod.TableName = modMod.TableName;
            }
            storeMod.SuccessfulUserID = mu.UserID;
            storeMod.Inputer = mu.UserName;
            storeMod.Title = UserShopName_T.Text;
            storeMod.DefaultSkins = DataConverter.CLng(TlpView_Tlp.SelectedID);
            storeMod.SpecialID = BindWX_DP.SelectedValue;
            DataTable dt = this.mfbll.GetModelFieldList(storeMod.ModelID);
            DataTable table = new Call().GetDTFromPage(dt, this, ViewState);
            table = stBll.FillDT(storeMod, table);
            if (storeMod.GeneralID > 0)
            {
                conBll.UpdateContent(table, storeMod);
            }
            else
            {
                storeMod.GeneralID= conBll.AddContent(table, storeMod);
            }
            mu.SiteID = storeMod.GeneralID;
            buser.UpdateByID(mu);
            function.WriteSuccessMsg("操作成功", "StoreManage.aspx");
        }
    }
}