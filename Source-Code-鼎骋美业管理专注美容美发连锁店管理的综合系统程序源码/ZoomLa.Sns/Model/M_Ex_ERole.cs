using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    public class M_Ex_ERole : M_Base
    {
        public M_Ex_ERole()
        {
            ZType = 0;
            ZStatus = 0;
        }
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string RoleAuth { get; set; }
        public string Remark { get; set; }
        public int ZType { get; set; }
        public int ZStatus { get; set; }
        public DateTime CDate { get; set; }
        public int StoreID { get; set; }
        public override string TbName { get { return "ZL_Ex_ERole"; } }
        public override string PK { get { return "ID"; } }
        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"RoleName","NVarChar","50"},
                                {"RoleAuth","NVarChar","4000"},
                                {"Remark","NVarChar","500"},
                                {"ZType","Int","4"},
                                {"ZStatus","Int","4"},
                                {"CDate","DateTime","8"},
                                {"StoreID","Int","4"}
        };
            return Tablelist;
        }
        public override SqlParameter[] GetParameters()
        {
            M_Ex_ERole model = this;
            if (model.CDate <= DateTime.MinValue) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.RoleName;
            sp[2].Value = model.RoleAuth;
            sp[3].Value = model.Remark;
            sp[4].Value = model.ZType;
            sp[5].Value = model.ZStatus;
            sp[6].Value = model.CDate;
            sp[7].Value = model.StoreID;
            return sp;
        }
        public M_Ex_ERole GetModelFromReader(DbDataReader rdr)
        {
            M_Ex_ERole model = new M_Ex_ERole();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.RoleName = ConverToStr(rdr["RoleName"]);
            model.RoleAuth = ConverToStr(rdr["RoleAuth"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            model.ZType = ConvertToInt(rdr["ZType"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            rdr.Close();
            return model;
        }
    }
}
