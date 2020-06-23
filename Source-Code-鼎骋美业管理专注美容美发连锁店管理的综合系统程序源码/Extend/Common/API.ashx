<%@ WebHandler Language="C#" Class="API" %>

using System;
using System.Web;
using System.Data;
using ZoomLa.BLL;
using ZoomLa.BLL.User;
using ZoomLa.BLL.API;
using ZoomLa.Model.Shop;
using ZoomLa.BLL.Shop;
using ZoomLa.Model;
using ZoomLa.SQLDAL;
using Newtonsoft.Json;
using ZoomLa.Sns;


public class API : API_Base, IHttpHandler
{
    B_Product proBll = new B_Product();
    B_User buser = new B_User();
    B_OrderList orderBll = new B_OrderList();
    B_User_StoreUser suBll = new B_User_StoreUser();
    public void ProcessRequest(HttpContext context)
    {
        //用户登录用户中心后调用
        retMod.retcode = M_APIResult.Failed;
        M_UserInfo mu = buser.GetLogin();
        if (mu.IsNull) { retMod.retmsg = "用户未登录"; RepToClient(retMod);return; }
        ExHelper.CheckUserAuth("cash");
        try
        {
            switch (Action)
            {
                case "sel_employee"://技师,员工等选择
                    {
                        DataTable         employDT = ExHelper.User_Sel(new F_User()
                        {
                            storeId = mu.SiteID
                        });
                        employDT.Columns["salt"].ColumnName = "UserFace";
                        employDT = employDT.DefaultView.ToTable(true, "UserID,UserName,HoneyName,UserFace".Split(','));
                        retMod.result = JsonConvert.SerializeObject(employDT);
                        retMod.retcode = M_APIResult.Success;
                    }
                    break;
                case "sel_product":
                    {
                        DataTable proDT = proBll.GetProductAll(new Filter_Product()
                        {
                            storeid = mu.SiteID
                        });
                        proDT = proDT.DefaultView.ToTable(true, "ID,Proname,LinPrice".Split(','));
                        retMod.result = JsonConvert.SerializeObject(proDT);
                        retMod.retcode = M_APIResult.Success;
                    }
                    break;
                case "order_recharge":
                    {
                        B_Shop_MoneyRegular regBll = new B_Shop_MoneyRegular();
                        double money = DataConvert.CLng(Req("money"));
                        int regular = DataConvert.CLng(Req("regular"));//要应用的充值规则
                        int uid = DataConvert.CLng(Req("uid"));//目标用户
                        M_UserInfo client = buser.SelReturnModel(uid);
                        M_Shop_MoneyRegular regMod = null;
                        if (regular > 0)
                        {
                            regMod = regBll.SelReturnModel(regular);
                        }
                        else
                        {
                            regMod = new M_Shop_MoneyRegular();
                            regMod.Alias = "手动输入金额";
                            regMod.Min = money;
                            regMod.StoreID = mu.SiteID;
                        }
                        if (regMod.StoreID != mu.SiteID || regMod.Min < 1) { retMod.retmsg = "充值金额错误";RepToClient(retMod); }

                        M_User_StoreUser suMod = suBll.SelModelByUid(uid, mu.SiteID);
                        if (client.IsNull || suMod == null) { retMod.retmsg = "会员不存在"; }
                        else
                        {
                            M_OrderList orderMod = orderBll.NewOrder(mu, M_OrderList.OrderEnum.Domain);
                            orderMod.Ordersamount = regMod.Min;
                            orderMod.Specifiedprice = regMod.Min;
                            orderMod.Balance_price = regMod.Min;
                            orderMod.Money_code = regMod.ID.ToString();
                            orderMod.StoreID = mu.SiteID;
                            orderMod.Receiver = uid.ToString();
                            orderMod.Rename = suMod.HoneyName;
                            orderMod.Ordermessage = regMod.Alias;
                            orderMod.id = orderBll.insert(orderMod);
                            retMod.result = orderMod.id.ToString();
                            retMod.retcode = M_APIResult.Success;
                        }
                    }
                    break;
                default:
                    {
                        retMod.retmsg = "[" + Action + "]接口不存在";
                    }
                    break;
            }
        }
        catch (Exception ex) { retMod.retmsg = ex.Message; }
        RepToClient(retMod);
    }

    public bool IsReusable { get { return false; } }

}