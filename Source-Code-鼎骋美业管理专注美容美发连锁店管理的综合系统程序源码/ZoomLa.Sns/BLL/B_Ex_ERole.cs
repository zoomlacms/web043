using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.Model;
using ZoomLa.Sns;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class B_Ex_ERole
    {
        private M_Ex_ERole initMod = new M_Ex_ERole();
        public string TbName = "", PK = "";
        public B_Ex_ERole()
        {
            TbName = initMod.TbName;
            PK = initMod.PK;
        }
        public DataTable Sel()
        {
            return DBCenter.Sel(TbName, "", PK + " DESC");
        }
        /// <summary>
        /// 给予前端使用
        /// </summary>
        public static DataTable SelRoles()
        {
            M_UserInfo mu = new B_User().GetLogin();
            return new B_Ex_ERole().Sel(new Com_Filter()
            {
                storeId = mu.SiteID
            });
        }
        public DataTable Sel(Com_Filter filter)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            string where = "1=1";
            if (filter.storeId != -100)
            {
                where += " AND StoreID=" + filter.storeId;
            }
            if (!string.IsNullOrEmpty(filter.skey))
            {
                sp.Add(new SqlParameter("skey", "%" + filter.skey + "%"));
                where += " AND RoleName LIKE @skey";
            }
            return DBCenter.Sel(TbName, where, "ID DESC");
        }
        public M_Ex_ERole SelReturnModel(int ID)
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
        public M_Ex_ERole SelModelByUid(int uid)
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
        public int Insert(M_Ex_ERole model)
        {
            return DBCenter.Insert(model);
        }
        public bool UpdateByID(M_Ex_ERole model)
        {
            return DBCenter.UpdateByID(model, model.ID);
        }
        public void Del(string ids)
        {
            if (string.IsNullOrEmpty(ids)) { return; }
            SafeSC.CheckIDSEx(ids);
            DBCenter.DelByIDS(TbName, PK, ids);
        }
        public static M_Ex_ERole GetERole(M_UserInfo mu)
        {
            M_Ex_ERole roleMod = new M_Ex_ERole();
            //检测是否为店长
            if (DBCenter.IsExist("ZL_CommonModel", "SuccessfulUserID=" + mu.UserID))
            {
                roleMod.RoleName = "店长";
                return roleMod;
            }
            if (mu.PageID < 1) { roleMod.RoleName = "未分配"; return roleMod; }
            roleMod = new B_Ex_ERole().SelReturnModel(mu.PageID);
            if (roleMod == null) { roleMod = new M_Ex_ERole();roleMod.RoleName = "不存在";return roleMod; }
            return roleMod;
            
        }
    }
}
