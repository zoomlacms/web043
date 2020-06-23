using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    public class M_Ex_PreOrder : M_Base
    {
        public int ID { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// 预约员工ID
        /// </summary>
        public int EmployID { get; set; }
        /// <summary>
        /// 预约员工名
        /// </summary>
        public string EmployName { get; set; }
        /// <summary>
        /// 客户相关信息
        /// </summary>
        public string ClientMobile { get; set; }
        public string ClientName { get; set; }
        public DateTime ClientDate { get; set; }
        /// <summary>
        /// 客户预约服务
        /// </summary>
        public string ClientNeed { get; set; }
        public string Remark { get; set; }
        public DateTime CDate { get; set; }
        public int ZStatus { get; set; }
        public int ZType { get; set; }
        /// <summary>
        /// 预约来源  (微信|收银)
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 如果和订单有关联,则填入(暂只用于预约订单Cloud)
        /// </summary>
        public int OrderID { get; set; }
        public override string TbName { get { return "ZL_Ex_PreOrder"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"StoreID","Int","4"},
                                {"EmployID","Int","4"},
                                {"EmployName","NVarChar","100"},
                                {"ClientMobile","NVarChar","100"},
                                {"ClientName","NVarChar","100"},
                                {"ClientDate","DateTime","8"},
                                {"ClientNeed","NText","20000"},
                                {"Remark","NVarChar","500"},
                                {"CDate","DateTime","8"},
                                {"ZStatus","Int","4"},
                                {"ZType","Int","4"},
                                {"Source","NVarChar","50"},
                                {"OrderID","Int","4"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_Ex_PreOrder model = this;
            if (model.CDate <= DateTime.Now) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.StoreID;
            sp[2].Value = model.EmployID;
            sp[3].Value = model.EmployName;
            sp[4].Value = model.ClientMobile;
            sp[5].Value = model.ClientName;
            sp[6].Value = model.ClientDate;
            sp[7].Value = model.ClientNeed;
            sp[8].Value = model.Remark;
            sp[9].Value = model.CDate;
            sp[10].Value = model.ZStatus;
            sp[11].Value = model.ZType;
            sp[12].Value = model.Source;
            sp[13].Value = model.OrderID;
            return sp;
        }
        public M_Ex_PreOrder GetModelFromReader(DbDataReader rdr)
        {
            M_Ex_PreOrder model = new M_Ex_PreOrder();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            model.EmployID = ConvertToInt(rdr["EmployID"]);
            model.EmployName = ConverToStr(rdr["EmployName"]);
            model.ClientMobile = ConverToStr(rdr["ClientMobile"]);
            model.ClientName = ConverToStr(rdr["ClientName"]);
            model.ClientDate = ConvertToDate(rdr["ClientDate"]);
            model.ClientNeed = ConverToStr(rdr["ClientNeed"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.ZType = ConvertToInt(rdr["ZType"]);
            model.Source = ConverToStr(rdr["Source"]);
            model.OrderID = ConvertToInt(rdr["OrderID"]);
            rdr.Close();
            return model;
        }
    }
}
