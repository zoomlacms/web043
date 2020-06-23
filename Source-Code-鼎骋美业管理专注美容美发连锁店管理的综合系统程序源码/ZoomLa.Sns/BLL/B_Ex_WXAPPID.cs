using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.BLL.CreateJS;
using ZoomLa.Model;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public class B_Ex_WXAPPID
    {
        private B_CodeModel codeBll = null;
        private string TbName, PK;
        //private M_WX_APPID initMod = new M_WX_APPID();
        public B_Ex_WXAPPID()
        {
            TbName = "ZL_C_wechatpay";
            PK = "ID";
            codeBll = new B_CodeModel(TbName);
        }
        public int Insert(M_WX_APPID model)
        {
            return codeBll.Insert(GetReaderFromModel(model));
            //return Sql.insertID(TbName, model.GetParameters(model), BLLCommon.GetParas(model), BLLCommon.GetFields(model));
        }
        public bool UpdateByID(M_WX_APPID model)
        {
            DataRow dr = GetReaderFromModel(model);
            //return Sql.UpdateByIDs(TbName, PK, model.ID.ToString(), BLLCommon.GetFieldAndPara(model), model.GetParameters(model));
            codeBll.UpdateByID(GetReaderFromModel(model));
            return true;
        }
        public M_WX_APPID SelModelByStoreId(int sid)
        {
            DataTable dt = DBCenter.Sel(TbName, "StoreID=" + sid, "ID DESC");
            if (dt.Rows.Count < 1) { return null; }
            return GetModelFromReader(dt.Rows[0]);
        }
        public DataTable Sel()
        {
            return Sql.Sel(TbName, "", "CDate Desc");
        }
        public DataRow GetReaderFromModel(M_WX_APPID model)
        {
            DataTable dt = codeBll.SelStruct();
            DataRow dr = dt.NewRow();
            dr["Alias"] = model.Alias;
            dr["APPID"] = model.APPID;
            dr["Secret"] = model.Secret;
            dr["StoreId"] = model.IsDefault;
            dr["WxNo"] = model.WxNo;
            dr["OrginID"] = model.OrginID;
            dr["Pay_AccountID"] = model.Pay_AccountID;
            dr["QRCode"] = model.QRCode;
            return dr;
        }
        public M_WX_APPID GetModelFromReader(DataRow rdr)
        {
            M_WX_APPID model = new M_WX_APPID();
            model.ID = Convert.ToInt32(rdr["ID"]);
            model.Alias = DataConvert.CStr(rdr["Alias"]);
            model.APPID = DataConvert.CStr(rdr["APPID"]);
            model.Secret = DataConvert.CStr(rdr["Secret"]);
            model.Token = "";
            //model.CDate = ConvertToDate(rdr["CDate"]);
            //model.Status = ConvertToInt(rdr["Status"]);
            model.IsDefault = DataConvert.CLng(rdr["StoreId"]);
            /* model.TokenDate = ConvertToDate(rdr["TokenDate"]);*/
            model.WxNo = DataConvert.CStr(rdr["WxNo"]);
            //model.WelStr = ConverToStr(rdr["WelStr"]);
            model.OrginID = DataConvert.CStr(rdr["OrginID"]);
            model.Pay_AccountID = DataConvert.CStr(rdr["Pay_AccountID"]);
            //model.Pay_Key = ConverToStr(rdr["Pay_Key"]);
            //model.Pay_SSLPath = ConverToStr(rdr["Pay_SSLPath"]);
            //model.Pay_SSLPassword = ConverToStr(rdr["Pay_SSLPassword"]);
            model.QRCode = DataConvert.CStr(rdr["QRCode"]);
            return model;
        }
    }
}
