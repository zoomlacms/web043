using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Components;
using System.Text;
using System.Xml;
using ZoomLa.BLL.Helper;
using ZoomLa.BLL.Shop;
using Newtonsoft.Json;

namespace ZoomLaCMS.Manage.Shop
{
    public partial class ShowProduct : CustomerPageAction
    {
        B_Node bnode = new B_Node();
        B_ModelField bfield = new B_ModelField();
        B_Product bll = new B_Product();
        B_Trademark Tradk = new B_Trademark();
        B_Stock Sll = new B_Stock();
        M_Product CData = new M_Product();
        B_Order_IDC idcBll = new B_Order_IDC();
        B_Group gpBll = new B_Group();
        B_Content conBll = new B_Content();
        public M_Product pinfo = null;
        public int Mid { get { return Convert.ToInt32(Request.QueryString["id"]); } }
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pinfo = bll.GetproductByid(Mid);
                M_Node nodeMod = bnode.SelReturnModel(pinfo.Nodeid);
                M_Product preMod = bll.GetNearID(pinfo.Nodeid, pinfo.ID, 0);
                M_Product nextMod = bll.GetNearID(pinfo.Nodeid, pinfo.ID, 1);
                M_CommonData storeMod = conBll.Store_SelModel(pinfo.UserShopID);
                if (preMod != null) { PrePro_Btn.Attributes.Remove("disabled"); PrePro_Btn.CommandArgument = preMod.ID.ToString(); }
                if (nextMod != null) { NextPro_Btn.Attributes.Remove("disabled"); NextPro_Btn.CommandArgument = nextMod.ID.ToString(); }

                lblCountHits.Text = pinfo.AllClickNum.ToString();
                this.nodename.Text = "<a href=\"ProductManage.aspx?NodeID=" + nodeMod.NodeID + "\">" + nodeMod.NodeName + "</a>";
                AddUser_L.Text = pinfo.AddUser;
                StoreName.Text = storeMod == null ? "" : storeMod.Title;
                ProCode.Text = pinfo.ProCode;
                BarCode.Text = pinfo.BarCode;
                Proname.Text = pinfo.Proname;
                Kayword.Text = pinfo.Kayword;
                ProUnit.Text = pinfo.ProUnit;
                Rateset1.Text = pinfo.Rateset.ToString();
                lblpoint.Text = pinfo.PointVal.ToString();
                Weight.Text = pinfo.Weight.ToString();
                this.Largess1.Text = pinfo.Largess == 1 ? "是" : "否";
                this.txtRecommend.Text = pinfo.Recommend.ToString();
                if (!string.IsNullOrEmpty(pinfo.IDCPrice))
                {
                    ProExtend_L.Text = JsonConvert.SerializeObject(idcBll.P_GetValid(pinfo.IDCPrice));
                }
                ProClass1.Text = pinfo.ProClass == 1 ? "正常销售" : "特价处理";
                Proinfo.Text = pinfo.Proinfo.ToString();
                Procontent.Text = pinfo.Procontent.ToString();
                Clearimg.Text = ComRE.Img_NoPic(function.GetImgUrl(pinfo.Clearimg));
                Thumbnails.Text = ComRE.Img_NoPic(function.GetImgUrl(pinfo.Thumbnails));
                DownQuota.Text = pinfo.DownQuota.ToString();
                Quota.Text = pinfo.Quota.ToString();
                Stock.Text = pinfo.Stock.ToString();
                StockDown.Text = pinfo.StockDown.ToString();
                Rate.Text = pinfo.Rate.ToString();
                if (pinfo.Dengji == 1)
                {
                    Dengji1.Text = "★";
                }
                if (pinfo.Dengji == 2)
                {
                    Dengji1.Text = "★★";
                }
                if (pinfo.Dengji == 3)
                {
                    Dengji1.Text = "★★★";
                }
                if (pinfo.Dengji == 4)
                {
                    Dengji1.Text = "★★★★";
                }
                if (pinfo.Dengji == 5)
                {
                    Dengji1.Text = "★★★★★";
                }

                ShiPrice.Text = pinfo.ShiPrice.ToString();
                Brand.Text = pinfo.Brand.ToString();
                Producer.Text = pinfo.Producer.ToString();
                LinPrice.Text = pinfo.LinPrice.ToString();
                BookPrice_L.Text = pinfo.BookPrice==0?"未开启" : pinfo.BookPrice.ToString("f2");
                Wholesaleone1.Text = pinfo.Wholesaleone == 1 ? "是" : "否";
                if (pinfo.Istrue == 1)
                {
                    this.istrue1.Text = "审核通过";
                }
                else
                {
                    this.istrue1.Text = "审核未通过";
                }
                Stock.Enabled = false;
                UpdateTime.Text = pinfo.UpdateTime.ToString();
                ModeTemplate.Text = pinfo.ModeTemplate.ToString();
                if (pinfo.Isnew == 1) { this.istrue1.Text += "  新品"; }
                if (pinfo.Ishot == 1) { this.istrue1.Text += "  热销"; }
                if (pinfo.Isbest == 1) { this.istrue1.Text += "  精品"; }
                if (pinfo.Sales == 1) { Sales1.Text = "销售中"; }
                if (pinfo.Sales != 1) { Sales1.Text = "停销状态"; }
                if (pinfo.Allowed == 1) { Allowed.Text = "缺货时允许购买"; }
                if (pinfo.Allowed != 1) { Allowed.Text = "缺货时不允许购买"; }

                DataTable dr = bll.GetContent(pinfo.TableName.ToString(), DataConverter.CLng(pinfo.ItemID));
                this.ModelHtml.Text = this.bfield.InputallHtml(pinfo.ModelID, pinfo.Nodeid, new ModelConfig()
                {
                    ValueDT = dr,
                    Mode = ModelConfig.SMode.PreView
                });
                PointVal_L.Text = pinfo.PointVal.ToString();
                BindUserPrice(pinfo);

                Call.SetBreadCrumb(Master, "<li><a href='" + CustomerPageAction.customPath2 + "Main.aspx'>工作台</a></li><li><a href='ProductManage.aspx'>商城管理</a></li><li><a href='ProductManage.aspx'>商品管理</a></li><li><a href=\"ProductManage.aspx?NodeID="
                  + nodeMod.NodeID + "\">" + nodeMod.NodeName + "</a></li><li class='active'>预览商品</li>" + "<div class='pull-right hidden-xs'><span onclick=\"opentitle('../Content/Node/EditNode.aspx?NodeID=" + pinfo.Nodeid + "','配置本节点');\" class='fa fa-cog' title='配置本节点' style='cursor:pointer;margin-left:5px;'></span></div>");
            }
        }
        private void BindUserPrice(M_Product pinfo)
        {
            switch (pinfo.UserType)
            {
                case 1:
                    Price_Member_T.Text = DataConverter.CDouble(pinfo.UserPrice).ToString("f2");
                    price_member_div.Attributes["style"] = "";
                    break;
                case 2:
                    price_group_div.Attributes["style"] = "";
                    break;
                default:
                    UserPri_L.Text = "未设置";
                    break;
            }

            DataTable gpdt = gpBll.GetGroupList();
            //附加会员价,限购数,最低购买数等限制
            gpdt.Columns.Add(new DataColumn("price", typeof(string)));
            if (pinfo != null && pinfo.ID > 0)
            {
                if (pinfo.UserPrice.Contains("["))
                {
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(pinfo.UserPrice);
                    if (dt.Columns.Contains("price")) { dt.Columns["price"].ColumnName = "value"; }
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow[] gps = gpdt.Select("GroupID='" + dr["gid"] + "'");
                        if (gps.Length > 0) { gps[0]["price"] = DataConverter.CDouble(dr["value"]).ToString("F2"); }
                    }
                }
            }
            Price_Group_RPT.DataSource = gpdt;
            Price_Group_RPT.DataBind();
        }
        public string getproimg(string type)
        {
            string restring;
            restring = "";
            if (!string.IsNullOrEmpty(type)) { type = type.ToLower(); }
            if (!string.IsNullOrEmpty(type) && (type.IndexOf(".gif") > -1 || type.IndexOf(".jpg") > -1 || type.IndexOf(".png") > -1))
            {
                restring = "<img src=../../" + type + " border=0 width=60 height=45>";
            }
            else
            {
                restring = "<img src=../../UploadFiles/nopic.gif border=0 width=60 height=45>";
            }
            return restring;
        }
        protected void NextPro_Btn_Click(object sender, EventArgs e)
        {
            string url = "ShowProduct.aspx?ID=" + (sender as Button).CommandArgument;
            Response.Redirect(url);
        }
    }
}