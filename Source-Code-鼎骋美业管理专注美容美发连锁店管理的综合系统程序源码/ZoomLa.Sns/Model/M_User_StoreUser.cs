using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.Model;

namespace ZoomLa.Sns
{
    //分店铺的用户信息,每个店铺独有不共享
    public class M_User_StoreUser : M_Base
    {
        public M_User_StoreUser()
        {
            Sex = "未知";
            UserLevel = 0;
            ZStatus = 0;
            HoneyName = "散客";
            CardLevel = "0";
            
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        /// <summary>
        /// 所属店铺
        /// </summary>
        public int StoreID { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public int UserLevel { get; set; }
        public string HoneyName { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 性别  未知|男|女
        /// </summary>
        public string Sex { get; set; }
        public string BirthDay { get; set; }
        public string WXNo { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 用户标签,用于筛选用户
        /// </summary>
        public string UserLabel { get; set; }
        public DateTime CDate { get; set; }
        public string UserFace { get; set; }
        public int ZStatus { get; set; }
        public string CardNo { get; set; }
        public string CardLevel { get; set; }
        public string CardRemark { get; set; }
        public int CardStatus { get; set; }
        public override string TbName { get { return "ZL_User_StoreUser"; } }
        public override string PK { get { return "ID"; } }

        public override string[,] FieldList()
        {
            string[,] Tablelist = {
                                {"ID","Int","4"},
                                {"UserID","Int","4"},
                                {"StoreID","Int","4"},
                                {"UserLevel","Int","4"},
                                {"HoneyName","NVarChar","100"},
                                {"Mobile","VarChar","50"},
                                {"Sex","NVarChar","50"},
                                {"BirthDay","NVarChar","50"},
                                {"WXNo","NVarChar","100"},
                                {"Remark","NVarChar","500"},
                                {"CDate","DateTime","8"},
                                {"UserFace","NVarChar","100"},
                                {"ZStatus","Int","4"},
                                {"CardNo","NVarChar","100"},
                                {"CardLevel","NVarChar","100"},
                                {"CardRemark","NVarChar","500"},
                                {"CardStatus","Int","4"},
                                {"UserLabel","NVarChar","500"}
        };
            return Tablelist;
        }

        public override SqlParameter[] GetParameters()
        {
            M_User_StoreUser model = this;
            if (model.CDate <= DateTime.MinValue) { model.CDate = DateTime.Now; }
            SqlParameter[] sp = GetSP();
            sp[0].Value = model.ID;
            sp[1].Value = model.UserID;
            sp[2].Value = model.StoreID;
            sp[3].Value = model.UserLevel;
            sp[4].Value = model.HoneyName;
            sp[5].Value = model.Mobile;
            sp[6].Value = model.Sex;
            sp[7].Value = model.BirthDay;
            sp[8].Value = model.WXNo;
            sp[9].Value = model.Remark;
            sp[10].Value = model.CDate;
            sp[11].Value = model.UserFace;
            sp[12].Value = model.ZStatus;
            sp[13].Value = model.CardNo;
            sp[14].Value = model.CardLevel;
            sp[15].Value = model.CardRemark;
            sp[16].Value = model.CardStatus;
            sp[17].Value = model.UserLabel;
            return sp;
        }
        public M_User_StoreUser GetModelFromReader(DbDataReader rdr)
        {
            M_User_StoreUser model = new M_User_StoreUser();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.UserID = ConvertToInt(rdr["UserID"]);
            model.StoreID = ConvertToInt(rdr["StoreID"]);
            model.UserLevel = ConvertToInt(rdr["UserLevel"]);
            model.HoneyName = ConverToStr(rdr["HoneyName"]);
            model.Mobile = ConverToStr(rdr["Mobile"]);
            model.Sex = ConverToStr(rdr["Sex"]);
            model.BirthDay = ConverToStr(rdr["BirthDay"]);
            model.WXNo = ConverToStr(rdr["WXNo"]);
            model.Remark = ConverToStr(rdr["Remark"]);
            model.CDate = ConvertToDate(rdr["CDate"]);
            model.UserFace = ConverToStr(rdr["UserFace"]);
            model.ZStatus = ConvertToInt(rdr["ZStatus"]);
            model.CardNo = ConverToStr(rdr["CardNo"]);
            model.CardLevel = ConverToStr(rdr["CardLevel"]);
            model.CardRemark = ConverToStr(rdr["CardRemark"]);
            model.CardStatus = ConvertToInt(rdr["CardStatus"]);
            model.UserLabel = ConverToStr(rdr["UserLabel"]);
            rdr.Close();
            return model;
        }
    }
}
