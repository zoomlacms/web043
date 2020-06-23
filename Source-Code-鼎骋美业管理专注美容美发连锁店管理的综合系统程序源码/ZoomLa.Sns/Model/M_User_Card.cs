using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;
namespace ZoomLa.Sns
{
    public class M_User_Card : M_Base
    {
        public M_User_Card()
        {
            CardNo = "";
            CardType = 0;
            CardStatus = 1;
            CardLevel = 0;
            CardPurse = 0;
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        /// <summary>
        /// 开卡店铺ID(未允许多店铺共享下,也只能用于该店铺)
        /// </summary>
        public int StoreID { get; set; }
        public string CardNo { get; set; }
        /// <summary>
        /// 卡级别,也可关联卡类商品ID
        /// </summary>
        public int CardLevel { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public int CardType { get; set; }
        /// <summary>
        /// 卡状态 0:未激活,1:已激活,-1:停用
        /// </summary>
        public int CardStatus { get; set; }
        /// <summary>
        /// 卡余额
        /// </summary>
        public double CardPurse { get; set; }
        /// <summary>
        /// 卡备注(用户可见)
        /// </summary>
        public string CardRemark { get; set; }
        /// <summary>
        /// 使用范围(如支持多店使用)
        /// </summary>
        public string CardScope { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string CardAddon1 { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string CardAddon2 { get; set; }
        /// <summary>
        /// 创建的收银员|管理员
        /// </summary>
        public int OPUserID { get; set; }
        public DateTime CDate { get; set; }
        /// <summary>
        /// 系统或管理员备注
        /// </summary>
        public string Remark { get; set; }

        public override string TbName { get { return "ZL_User_Card"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"UserID","Int","4"},
                                {"StoreID","Int","4"},
                                {"CardNo","NVarChar","50"},
                                {"CardLevel","Int","4"},
                                {"CardType","Int","4"},
                                {"CardStatus","Int","4"},
                                {"CardPurse","Money","8"},
                                {"CardRemark","NVarChar","500"},
                                {"CardScope","NVarChar","4000"},
                                {"CardAddon1","NVarChar","4000"},
                                {"CardAddon2","NVarChar","4000"},
                                {"OPUserID","Int","4"},
                                {"CDate","DateTime","8"},
                                {"Remark","NVarChar","500"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_User_Card model = this;
            if (model.CDate <= DateTime.Now) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.UserID;
            sp[2].Value = model.StoreID;
            sp[3].Value = model.CardNo;
            sp[4].Value = model.CardLevel;
            sp[5].Value = model.CardType;
            sp[6].Value = model.CardStatus;
            sp[7].Value = model.CardPurse;
            sp[8].Value = model.CardRemark;
            sp[9].Value = model.CardScope;
            sp[10].Value = model.CardAddon1;
            sp[11].Value = model.CardAddon2;
            sp[12].Value = model.OPUserID;
            sp[13].Value = model.CDate;
            sp[14].Value = model.Remark;
            return sp;
        }
        public M_User_Card GetModelFromReader(DbDataReader rdr)
        {
            M_User_Card model = new M_User_Card();
            model.ID = ConvertToInt(rdr["ID"]);
            model.UserID = ConvertToInt(rdr["UserID"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            model.CardNo = ConverToStr(rdr["CardNo"]);
            model.CardLevel = ConvertToInt(rdr["CardLevel"]);
            model.CardType = ConvertToInt(rdr["CardType"]);
            model.CardStatus = ConvertToInt(rdr["CardStatus"]);
            model.CardPurse = ConverToDouble(rdr["CardPurse"]);
            model.CardRemark = ConverToStr(rdr["CardRemark"]);
            model.CardScope = ConverToStr(rdr["CardScope"]);
            model.CardAddon1 = ConverToStr(rdr["CardAddon1"]);
            model.CardAddon2 = ConverToStr(rdr["CardAddon2"]);
            model.OPUserID = ConvertToInt(rdr["OPUserID"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            rdr.Close();
            return model;
        }
    }
}
