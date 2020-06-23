using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class B_Ex_PreOrder
    {
        private M_Ex_PreOrder initMod = new M_Ex_PreOrder();
        public string TbName = "", PK = "";
        public B_Ex_PreOrder()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        public M_Ex_PreOrder SelReturnModel(int ID)
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
        public int Insert(M_Ex_PreOrder model)
        {
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_Ex_PreOrder model)
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
            if (!string.IsNullOrEmpty(filter.status))
            {
                where += " AND ZStatus IN (" + filter.status + ")";
            }
            //根据姓名或号码搜索
            if (!string.IsNullOrEmpty(filter.uname))
            {
                sp.Add(new SqlParameter("uname", "%" + filter.uname + "%"));
                where += " AND (ClientName LIKE @uname  OR ClientMobile LIKE @uname)";
            }
            if (!string.IsNullOrEmpty(filter.addon))
            {
                sp.Add(new SqlParameter("addon", "%" + filter.addon + "%"));
                where += " AND EmployName LIKE @addon";
            }
            return DBCenter.Sel(TbName, where, PK + " DESC", sp);
        }
    }
    public class Com_Filter
    {
        public string uids = "";
        public string uname = "";
        public string ids = "";
        public string status = "";
        public string type = "-100";
        public string skey = "";
        public string addon = "";
        public int storeId = -100;
        public string stime = "";
        public string etime = "";
    }
}
