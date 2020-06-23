using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    public class M_Shop_Bonus : M_Base
    {
        public M_Shop_Bonus()
        {
            BonusType = 0;
            ParentID = 0;
            ZType = 0;
            ZStatus = 0;
        }
        public int ID { get; set; }
        /// <summary>
        /// 支持子项(对商品,其优先于全局配置)
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 默认为0==全部
        /// </summary>
        public int ProID { get; set; }
        public int StoreID { get; set; }
        /// <summary>
        /// 是否默认分红类型 1==默认
        /// </summary>
        public int IsDefault { get; set; }
        public string Alias { get; set; }
        /// <summary>
        /// 分红类型 0==按比率，1==固定金额
        /// </summary>
        public int BonusType { get; set; }
        public string BonusValue1 { get; set; }
        public string BonusValue2 { get; set; }
        public string BonusValue3 { get; set; }
        public string BonusValue4 { get; set; }
        /// <summary>
        /// 分红方案类型 0:技师方案,1:销售方案
        /// </summary>
        public int ZType { get; set; }
        public int ZStatus { get; set; }
        public DateTime CDate { get; set; }
        public string Remark { get; set; }

        public override string TbName { get { return "ZL_Shop_Bonus"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"ParentID","Int","4"},
                                {"ProID","Int","4"},
                                {"StoreID","Int","4"},
                                {"IsDefault","Int","4"},
                                {"Alias","NVarChar","100"},
                                {"BonusType","Int","4"},
                                {"BonusValue1","NVarChar","100"},
                                {"BonusValue2","NVarChar","100"},
                                {"BonusValue3","NVarChar","100"},
                                {"BonusValue4","NVarChar","100"},
                                {"ZType","Int","4"},
                                {"ZStatus","Int","4"},
                                {"CDate","DateTime","8"},
                                {"Remark","NVarChar","500"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_Shop_Bonus model = this;
            if (model.CDate <= DateTime.MinValue) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.ParentID;
            sp[2].Value = model.ProID;
            sp[3].Value = model.StoreID;
            sp[4].Value = model.IsDefault;
            sp[5].Value = model.Alias;
            sp[6].Value = model.BonusType;
            sp[7].Value = model.BonusValue1;
            sp[8].Value = model.BonusValue2;
            sp[9].Value = model.BonusValue3;
            sp[10].Value = model.BonusValue4;
            sp[11].Value = model.ZType;
            sp[12].Value = model.ZStatus;
            sp[13].Value = model.CDate;
            sp[14].Value = model.Remark;
            return sp;
        }
        public M_Shop_Bonus GetModelFromReader(DataRow rdr)
        {
            M_Shop_Bonus model = new M_Shop_Bonus();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.ParentID = ConvertToInt(rdr["ParentID"]);
            model.ProID = ConvertToInt(rdr["ProID"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            model.IsDefault = ConvertToInt(rdr["IsDefault"]);
            model.Alias = ConverToStr(rdr["Alias"]);
            model.BonusType = ConvertToInt(rdr["BonusType"]);
            model.BonusValue1 = ConverToStr(rdr["BonusValue1"]);
            model.BonusValue2 = ConverToStr(rdr["BonusValue2"]);
            model.BonusValue3 = ConverToStr(rdr["BonusValue3"]);
            model.BonusValue4 = ConverToStr(rdr["BonusValue4"]);
            model.ZType = ConvertToInt(rdr["ZType"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            return model;
        }
        public M_Shop_Bonus GetModelFromReader(DbDataReader rdr)
        {
            M_Shop_Bonus model = new M_Shop_Bonus();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.ParentID = ConvertToInt(rdr["ParentID"]);
            model.ProID = ConvertToInt(rdr["ProID"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            model.IsDefault = ConvertToInt(rdr["IsDefault"]);
            model.Alias = ConverToStr(rdr["Alias"]);
            model.BonusType = ConvertToInt(rdr["BonusType"]);
            model.BonusValue1 = ConverToStr(rdr["BonusValue1"]);
            model.BonusValue2 = ConverToStr(rdr["BonusValue2"]);
            model.BonusValue3 = ConverToStr(rdr["BonusValue3"]);
            model.BonusValue4 = ConverToStr(rdr["BonusValue4"]);
            model.ZType = ConvertToInt(rdr["ZType"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            rdr.Close();
            return model;
        }
    }
}
