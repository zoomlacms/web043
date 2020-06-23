using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Common;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLa.Sns
{
    public class ExHelper
    {
        /// <summary>
        /// 获取当前登录用户的所属店铺
        /// </summary>
        public static M_Store_Info Store_User()
        {
            M_UserInfo mu = new B_User().GetLogin();
            return new B_Store_Info().SelModelByUser(mu.UserID);
        }
        public static M_CommonData Store2_User()
        {
            M_UserInfo mu = new B_User().GetLogin();
            B_Content conBll = new B_Content();
            M_CommonData storeMod = conBll.SelReturnModel(mu.SiteID);
            return storeMod;
        }
        public static M_WX_APPID WX_SelMyModel()
        {
            B_WX_APPID appBll = new B_WX_APPID();
            M_UserInfo mu = new B_User().GetLogin();
            int appid = DataConvert.CLng(DBCenter.ExecuteScala("ZL_CommonModel", "SpecialID", "GeneralID=" + mu.SiteID));
            if (appid < 1) { return null; }
            M_WX_APPID appMod = appBll.SelReturnModel(appid);
            return appMod;
        }
        /// <summary>
        /// 返回店铺可添加的商品节点
        /// </summary>
        /// <returns></returns>
        public static DataTable Store_NodeSel()
        {
            return DBCenter.Sel("ZL_Node","ParentID=12");
        }
        /// <summary>
        /// 按金额从大到小排序
        /// </summary>
        public static DataTable Store_MoneyRegular(int sid)
        {
            return DBCenter.Sel("ZL_User_MoneyRegular", "StoreID=" + sid, "Min ASC");
        }

        /// <summary>
        /// 员工筛选,带角色名称
        /// </summary>
        public static DataTable Employ_Sel(F_User filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "SiteID=" + filter.storeId + " ";
            where += " AND GroupID=" + ExConast.EmployGroup;
            if (!string.IsNullOrEmpty(filter.uname))
            {
                sp.Add(new SqlParameter("uname", "%" + filter.uname + "%"));
                where += " AND UserName LIKE @uname";
            }
            if (filter.status != -100)
            {
                where += " AND Status=" + filter.status;
            }
            PageSetting setting = PageSetting.Double(1, int.MaxValue, "ZL_User", "ZL_Ex_ERole", "A.UserID",
                "A.PageID=B.ID", where, "A.UserID DESC", sp);
            DBCenter.SelPage(setting);
            return setting.dt;
        }
        /// <summary>
        /// 获取指定店铺的员工|客户信息
        /// </summary>
        public static DataTable User_Sel(F_User filter)
        {
            return User_SelPage(1, int.MaxValue, filter).dt;
        }
        public static PageSetting User_SelPage(int cpage,int psize,F_User filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "SiteID=" + filter.storeId + " ";
            //where += " AND Status=0";
            switch (filter.utype)
            {
                case "employee":
                    where += " AND GroupID=" + ExConast.EmployGroup;
                    break;
                case "client":
                default:
                    where += " AND GroupID=" + ExConast.ClientGroup;
                    break;
            }
            if (!string.IsNullOrEmpty(filter.uname))
            {
                sp.Add(new SqlParameter("uname", "%" + filter.uname + "%"));
                where += " AND UserName LIKE @uname";
            }
            if (filter.status != -100)
            {
                where += " AND Status="+filter.status;
            }
            PageSetting setting = PageSetting.Single(cpage,psize,"ZL_User","UserID",where,"UserID DESC",sp);
            DBCenter.SelPage(setting);
            return setting;
        }
        public static string User_ShowStatus(object status)
        {
            string Status = DataConvert.CStr(status);
            switch (Status)
            {
                case "0":
                    return "<span style='color:green;'>正常</span>";
                case "1":
                    return "<span style='color:red;'>锁定</span>";
                case "2":
                    return "待认证";
                case "3":
                    return "双认证";
                case "4":
                    return "邮件认证";
                case "5":
                    return "待认证";
                default:
                    return Status.ToString();
            }
        }
        /// <summary>
        /// 返回用户等级(会员卡没有等级)
        /// </summary>
        public static string User_GetLevel(int level)
        {
            foreach (DataRow dr in ExConast.UserLevelDT.Rows)
            {
                if (Convert.ToInt32(dr["level"]) == level) { return dr["name"].ToString(); }
            }
            return "";
        }
        //----------------权限检测
        /// <summary>
        /// 检测是否有访问用户中心的权限
        /// 店长,收银员
        /// </summary>
        public static CommonReturn CheckUserLogin()
        {
            M_UserInfo mu = new B_User().GetLogin();
            if (mu.IsNull) { return CommonReturn.Failed("用户未登录"); }
            //检测用户是否拥有店铺,或为店铺收银员

            if (mu.SiteID < 1) { return CommonReturn.Failed("用户未绑定店铺"); }
            if (DBCenter.IsExist("ZL_CommonModel", "SuccessfulUserID=" + mu.UserID)) { return CommonReturn.Success(); }
            //是否绑定了店铺,是否为收银员
            if (mu.GroupID != ExConast.EmployGroup) { return CommonReturn.Failed("没有访问页面的权限"); }
            return CommonReturn.Success();
        }
        /// <summary>
        /// 校验当前登录用户的角色权限
        /// </summary>
        /// <param name="name"></param>
        public static CommonReturn CheckUserAuth(string auth)
        {
            M_UserInfo mu = new B_User().GetLogin();
            if (mu.SiteID < 1) { function.WriteErrMsg("禁止访问,用户未绑定店铺"); }
            //如果是店铺的所有者,则不需要检测
            bool flag = DBCenter.IsExist("ZL_CommonModel", "GeneralID=" + mu.SiteID + " AND SuccessfulUserID=" + mu.UserID);
            if (flag) { return CommonReturn.Success(); }
            //只有店长可访问该页面
            if (auth.Equals("manage")) { function.WriteErrMsg("你没有足够的访问权限"); }
            //-----------------------检测角色权限
            if (mu.PageID < 1) { function.WriteErrMsg("操作员未绑定角色"); }
            if (string.IsNullOrEmpty(auth)) { function.WriteErrMsg("未指定需要校验的权限"); }
            string auths = DataConvert.CStr(DBCenter.ExecuteScala("ZL_Ex_ERole", "RoleAuth", "ID=" + mu.PageID));
            if (!auths.Contains(auth)) { function.WriteErrMsg("你无权访问该页面"); }

            return CommonReturn.Success();
        }
        //---------------------Tools
        /// <summary>
        /// 从地址栏和Cookies中读取信息(店铺ID)
        /// </summary>
        public static string GetParam(string name)
        {
            if (string.IsNullOrEmpty(name)) { return ""; }
            HttpRequest req = HttpContext.Current.Request;
            name = name.ToLower();
            string value = req[name];
            if (string.IsNullOrEmpty(value) && req.Cookies[name] != null)
            {
                //部分键值并不对应
                if (name.Equals("id")) { name = "sid"; }
                value = req.Cookies[name].Value;
            }
            return value;
        }
    }
    //筛选员工或客户
    public class F_User
    {
        public string utype = "employee";
        public string uname = "";
        public int storeId = 0;
        /// <summary>
        /// 用户状态筛选
        /// </summary>
        public int status = -100;
    }
}
