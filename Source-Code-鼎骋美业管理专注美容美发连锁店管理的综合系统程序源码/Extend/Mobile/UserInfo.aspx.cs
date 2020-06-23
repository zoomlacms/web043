using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZoomLa.BLL;
using ZoomLa.Model;

public partial class Extend_Mobile_UserInfo : System.Web.UI.Page
{
    public M_UserInfo mu = null;
    B_User buser = new B_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        mu = buser.GetLogin();
    }
}