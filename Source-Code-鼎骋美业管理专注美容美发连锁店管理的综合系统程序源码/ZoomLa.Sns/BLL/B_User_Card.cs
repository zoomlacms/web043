using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.Common;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class B_User_Card
    {
        private M_User_Card initMod = new M_User_Card();
        public string TbName = "", PK = "";
        public B_User_Card()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        public M_User_Card SelReturnModel(int ID)
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
        public M_User_Card SelModelByUid(int uid)
        {
            using (DbDataReader reader = DBCenter.SelReturnReader(TbName, "UserID=" + uid))
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
        public int Insert(M_User_Card model)
        {
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_User_Card model)
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
        /// <summary>
        ///  3011712211620165128
        ///  3+店ID+日期(yyMMddHHmmss)(12位)+6位随机数
        /// </summary>
        /// <param name="storeId">店铺ID或独有标识</param>
        /// <returns></returns>
        public string GetCardNo(string storeId = "0")
        {
            if (storeId.Length < 2)
            {
                storeId = "0" + storeId;
            }
            string cardNo = "3" + storeId +
                DateTime.Now.ToString("yyMMddHHmmss") + function.GetRandomString(6, 2);
            return cardNo;
        }
    }
}
