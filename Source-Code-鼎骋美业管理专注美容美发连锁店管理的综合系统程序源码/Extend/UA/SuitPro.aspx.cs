using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Common;

public partial class Extend_UA_SuitCard : System.Web.UI.Page
{
    public B_Product proBll = new B_Product();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MyBind();
        }
    }
    public void MyBind()
    {
        EGV.DataSource = proBll.GetProductAll(new Filter_Product()
        {
            pclass = 2,
            stype = "ProName",
        });
        EGV.DataBind();
    }
    protected void Search_B_Click(object sender, EventArgs e)
    {
        MyBind();
    }
    protected void EGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EGV.PageIndex = e.NewPageIndex;
        MyBind();
    }
    protected void EGV_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = DataConverter.CLng(e.CommandArgument);
        switch (e.CommandName.ToLower())
        {
            case "del":
                {
                    proBll.RealDelByIDS(id.ToString());
                }
                break;
        }
        MyBind();
    }
    protected void EGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void batBtn_Click(object sender, EventArgs e)
    {
        string ids = Request.Form["idchk"];
        if (!string.IsNullOrEmpty(ids))
        {
            proBll.RealDelByIDS(ids);
        }
        function.WriteSuccessMsg("操作成功");
    }
}