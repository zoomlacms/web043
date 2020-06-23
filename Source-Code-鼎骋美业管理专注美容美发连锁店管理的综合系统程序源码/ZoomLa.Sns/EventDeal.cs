using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZoomLa.BLL;
using ZoomLa.BLL.Shop;
using ZoomLa.Model;
using ZoomLa.Model.Shop;
using ZoomLa.Sns.BLL;
using ZoomLa.SQLDAL;

namespace ZoomLa.Sns
{
    public static class EventDeal
    {
        public static void SubscribeEvent()
        {
            OrderHelper.OrderFinish_DE func = OrderFinish;
            OrderHelper.OrderFinish_Event += func;
        }
        public static void OrderFinish(M_OrderList mod, M_Payment pinfo)
        {
            B_OrderList orderBll = new B_OrderList();
            B_CartPro cpBll = new B_CartPro();
            B_User buser = new B_User();
            //B_User_Card cdBll = new B_User_Card();
            //B_User_StoreUser suBll = new B_User_StoreUser();
            switch (mod.Ordertype)
            {
                case (int)M_OrderList.OrderEnum.Domain:
                    {
                        B_Shop_MoneyRegular regBll = new B_Shop_MoneyRegular();
                        //M_User_StoreUser suMod = suBll.SelReturnModel(Convert.ToInt32(mod.Receiver));
                        //M_User_Card cdMod = cdBll.SelModelByUid(suMod.ID);
                        //cdMod.CardPurse += mod.Ordersamount;
                        //cdBll.UpdateByID(cdMod);
                        int regular = DataConvert.CLng(mod.Money_code);
                        M_Shop_MoneyRegular regMod = null;
                        if (regular > 0)
                        {
                            regMod = regBll.SelReturnModel(regular);
                        }
                        else
                        {
                            regMod = new M_Shop_MoneyRegular();
                            regMod.Min = mod.Ordersamount;
                        }
                        buser.AddMoney(DataConvert.CLng(mod.Receiver), regMod.Min + regMod.Purse, M_UserExpHis.SType.Purse, "充值,订单号:" + mod.OrderNo + ",充值:" + regMod.Min + ",赠送:" + regMod.Purse);
                        if (regMod.Point > 0){ buser.AddMoney(DataConvert.CLng(mod.Receiver), regMod.Point, M_UserExpHis.SType.Point, "充值赠送积分,订单号:" + mod.OrderNo);}
                    }
                    break;
                case (int)M_OrderList.OrderEnum.Cloud://通过微信下的预约订单
                    {
                        //付款后增加预约记录,由收银员完成开单
                        //{ id: 0, proid: 0, proname: "", empid: 0, empname: "" };
                        DataTable cpDT = cpBll.SelByOrderID(mod.id);
                        List<M_Ex_PreOrder_Item> itemList = new List<M_Ex_PreOrder_Item>();
                        foreach (DataRow dr in cpDT.Rows)
                        {
                            M_Ex_PreOrder_Item model = new M_Ex_PreOrder_Item();
                            model.proid = Convert.ToInt32(dr["ProID"]);
                            model.proname = DataConvert.CStr(dr["Proname"]);
                            model.empid = DataConvert.CLng(dr["code"]);
                            model.empname = DataConvert.CStr(dr["Proinfo"]);
                            itemList.Add(model);
                        }
                        B_Ex_PreOrder poBll = new B_Ex_PreOrder();
                        M_Ex_PreOrder poMod = new M_Ex_PreOrder();
                        poMod.ClientMobile = mod.Mobile.ToString();
                        poMod.ClientName = mod.Rename;
                        poMod.ClientDate = DataConvert.CDate(mod.ExpTime);
                        poMod.ClientNeed = JsonConvert.SerializeObject(itemList);
                        poMod.Remark = mod.Ordermessage;
                        poMod.Source = "微信";
                        poMod.OrderID = mod.id;
                        poMod.ID = poBll.Insert(poMod);
                    }
                    break;
                default://普通订单,计算提成,赠送积分
                    {
                        B_Shop_Bonus bnBll = new B_Shop_Bonus();
                        DataTable cpDT = cpBll.SelByOrderID(mod.id);
                        foreach (DataRow dr in cpDT.Rows)
                        {
                            int code = DataConvert.CLng(dr["code"]);
                            int proid = DataConvert.CLng(dr["Proid"]);
                            double allMoney = DataConvert.CDouble(dr["AllMoney"]);
                            if (code < 1) { continue; }
                            #region 技师计算提成
                            //技师绑定了何种分成方案,
                            M_UserInfo jsMod = buser.SelReturnModel(code);
                            M_Shop_Bonus bnMod = null;
                            if (jsMod.VIP < 1)
                            {
                                bnMod = bnBll.SelModelByDefault();
                            }
                            else { bnMod = bnBll.SelReturnModel(jsMod.VIP); }
                            //无提成方案则不计算
                            if (bnMod != null)
                            {
                                //根据商品ID,看有无匹配的子数据类型
                                M_Shop_Bonus bnMod2 = bnBll.SelModelByProID(bnMod.ID, proid);
                                if (bnMod2 != null) { bnMod = bnMod2; }
                                //-------计算提成,写入CartPro中
                                double bonus = 0;
                                switch (bnMod.BonusType)
                                {
                                    case 0:
                                        bonus = allMoney * (DataConvert.CDouble(bnMod.BonusValue1) * 0.01);
                                        break;
                                    case 1:
                                        bonus = DataConvert.CDouble(bnMod.BonusValue1);
                                        break;
                                }
                                DBCenter.UpdateSQL("ZL_CartPro", "AllMoney_Json='" + bonus.ToString("F2") + "'", "ID=" + dr["ID"]);
                            }
                            #endregion
                        }
                        //-------赠送积分,默认每一元送一分
                        buser.AddMoney(DataConvert.CLng(mod.Receiver), mod.Receivablesamount, M_UserExpHis.SType.Point, "订单[" + mod.id + "]赠送积分");
                    }
                    break;
            }

        }
    }
    public class M_Ex_PreOrder_Item
    {
        //预留
        public int id = 0;
        public int proid = 0;
        public string proname = "";
        public int empid = 0;
        public string empname = "";
    }
}
