using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns.BLL
{
    public class B_Shop_Bonus
    {
        private M_Shop_Bonus initMod = new M_Shop_Bonus();
        public string TbName = "", PK = "";
        public B_Shop_Bonus()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        public DataTable SelByParent(int pid)
        {
            string where = "(" + "A.ParentID=" + pid + " OR A.ID=" + pid + ")";
            DataTable dt = DBCenter.JoinQuery("A.*,B.Proname", "ZL_Shop_Bonus", "ZL_Commodities", "A.ProID=B.ID", where, "ParentID ASC");
            dt.Rows[0]["Proname"] = "统一设置";
            return dt;
        }
        public M_Shop_Bonus SelReturnModel(int ID)
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
        public M_Shop_Bonus SelModelByDefault()
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName,"IsDefault=1"))
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
        /// <summary>
        /// 根据父ID与商品ID,看有无匹配级更高的子项
        /// </summary>
        public M_Shop_Bonus SelModelByProID(int pid, int proid)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName, "ParentID="+pid+" AND ProID="+proid))
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
        public int Insert(M_Shop_Bonus model)
        {
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_Shop_Bonus model)
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
            string where = "ParentID=0 ";
            if (filter.storeId != -100)
            {
                where += " AND StoreId=" + filter.storeId;
            }
            return DBCenter.Sel(TbName, where, "ID DESC");
        }
    }
}
