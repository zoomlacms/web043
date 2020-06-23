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
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.Sns.BLL;
using ZoomLa.SQLDAL;

public partial class Extend_UA_BonusAdd : System.Web.UI.Page
{
    B_User buser = new B_User();
    B_Shop_Bonus bnBll = new B_Shop_Bonus();
    public int Mid { get { return DataConvert.CLng(Request.QueryString["ID"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        ExHelper.CheckUserAuth("manage");
        if (!IsPostBack)
        {
            if (Mid > 0)
            {
                M_UserInfo mu = buser.GetLogin();
                M_Shop_Bonus bnMod = bnBll.SelReturnModel(Mid);
                if (bnMod.StoreID != mu.SiteID) { function.WriteErrMsg("无权管理该商品"); }
                if (bnMod.ParentID > 0) { function.WriteErrMsg("不可访问子条目"); }
                Alias_T.Text = bnMod.Alias;
                IsDefault_Chk.Checked = bnMod.IsDefault == 1;
                DataTable infoDT = bnBll.SelByParent(bnMod.ID);
                Data_Hid.Value = JsonConvert.SerializeObject(infoDT);
            }
        }
    }

    protected void Save_Btn_Click(object sender, EventArgs e)
    {
        M_UserInfo mu = buser.GetLogin();
        M_Shop_Bonus bnMod = new M_Shop_Bonus();
        DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data_Hid.Value);
        if (Mid > 0) { bnMod = bnBll.SelReturnModel(Mid); }
        bnMod.Alias = Alias_T.Text.Trim();
        bnMod.IsDefault = IsDefault_Chk.Checked ? 1 : 0;
        bnMod.ParentID = 0;
        bnMod.Remark = Remark_T.Text;
        bnMod.BonusType = Convert.ToInt32(dt.Rows[0]["BonusType"]);
        bnMod.BonusValue1 = DataConvert.CDouble(dt.Rows[0]["BonusValue1"]).ToString("F2");
  
        if (bnMod.IsDefault == 1)
        {
            DBCenter.UpdateSQL(bnMod.TbName, "IsDefault=0", "IsDefault=1");
        }
        if (bnMod.ID > 0)
        {
            bnBll.UpdateByID(bnMod);
        }
        else
        {
            bnMod.StoreID = mu.SiteID;
            bnMod.ID = bnBll.Insert(bnMod);
        }
        //更新子类目信息
        if (dt.Rows.Count > 1)
        {
            string ids = StrHelper.GetIDSFromDT(dt,"ID");
            DBCenter.DelByWhere(bnMod.TbName, "ID NOT IN (" + ids + ") AND ParentID=" + bnMod.ID);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                //为防止信息修改,增加店铺验证
                DataRow dr = dt.Rows[i];
                M_Shop_Bonus model = new M_Shop_Bonus().GetModelFromReader(dr);
                model.ParentID = bnMod.ID;
                model.StoreID = bnMod.StoreID;
                if (model.ID > 0) { bnBll.UpdateByID(model); }
                else
                {
                    model.ID = bnBll.Insert(model);
                }
            }
        }

        function.WriteSuccessMsg("操作成功","Bonus.aspx");
    }
}