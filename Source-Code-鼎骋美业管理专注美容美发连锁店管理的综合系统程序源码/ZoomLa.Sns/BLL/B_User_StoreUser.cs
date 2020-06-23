using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.SQLDAL;
using ZoomLa.SQLDAL.SQL;

namespace ZoomLa.Sns
{
    public class B_User_StoreUser
    {
        private M_User_StoreUser initMod = new M_User_StoreUser();
        public string TbName = "", PK = "";
        public string TbView = "ZL_Ex_StoreUserView";
        public B_User_StoreUser()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbView, "", PK + " DESC");
        }
        public PageSetting SelPage(int cpage, int psize, Com_Filter filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "1=1 ";
            if (!string.IsNullOrEmpty(filter.uname))
            {
                sp.Add(new SqlParameter("uname", "%" + filter.uname + "%"));
                where += " AND (HoneyName LIKE @uname OR Mobile LIKE @uname)";
            }
            //用户标签,支持空格
            if (!string.IsNullOrEmpty(filter.skey))
            {
                string sql = "";
                string[] labels = filter.skey.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < labels.Length; i++)
                {
                    string label = labels[i];
                    sp.Add(new SqlParameter("label" + i, "%" + label + "%"));
                    if (i == 0) { sql += " UserLabel LIKE @label" + i + " "; }
                    else { sql += " OR UserLabel LIKE @label" + i; }
                }
                where += " AND (" + sql + ")";
            }
            if (filter.storeId != -100)
            {
                where += " AND StoreID=" + filter.storeId;
            }
            switch (filter.addon)
            {
                case "wechat"://绑定了微信的用户
                    where += " AND OpenID IS NOT NULL";
                    break;
                default:
                    break;
            }
            PageSetting setting = PageSetting.Single(cpage, psize, TbView, "UserID", where, "UserID DESC", sp);
            DBCenter.SelPage(setting);
            return setting;
        }
        public M_User_StoreUser SelReturnModel(int ID)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName, PK, ID))
            {
                if (reader.Read())
                {
                    return initMod.GetModelFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        public M_User_StoreUser SelModelByUid(int uid,int sid)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName,"UserID="+uid+" AND StoreId="+sid))
            {
                if (reader.Read())
                {
                    return initMod.GetModelFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        public int Insert(M_User_StoreUser model)
        {
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_User_StoreUser model)
        {
            return DBCenter.UpdateByID(model, model.ID);
        }
        public void Del(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }
            SafeSC.CheckIDSEx(ids);
            DBCenter.DelByIDS(TbName, PK, ids);
        }
        public DataTable Sel(Com_Filter filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "1=1 ";
            if (filter.storeId != -100)
            {
                where += " AND StoreId=" + filter.storeId;
            }
            return null;
        }
    }
}
