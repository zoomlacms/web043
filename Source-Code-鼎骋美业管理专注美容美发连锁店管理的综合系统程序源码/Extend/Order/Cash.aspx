<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cash.aspx.cs" Inherits="Extend_Order_Cash" MasterPageFile="~/Common/Master/UserEmpty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>收银台</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div style="height:600px;">
    <div id="summary">
        <div class="text-center">
            <h3><asp:Label runat="server" ID="StoreName_L"></asp:Label></h3>
        </div>
        <div class="ihead"><span>订单信息</span></div>
        <div class="item"><span>订单号</span><span class="item_r"><%:orderMod.OrderNo %></span></div>
        <div class="item"><span>下单时间</span><span class="item_r"><%:orderMod.AddTime %></span></div>
        <div class="item"><span>客户姓名</span><span class="item_r"><%:orderMod.Rename %></span></div>
        <div class="item"><span>收银员</span><span class="item_r"><%:orderMod.AddUser %></span></div>
        <div class="ihead"><span>消费明细</span></div>
        <table class="table">
            <asp:Repeater runat="server" ID="Cart_RPT" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("Proname")+"("+Eval("Attribute")+")"%></td>
                        <td><%#"x"+Eval("Pronum") %></td>
                        <td class="text-right" style="color:#000;"><i class="fa fa-rmb"></i> <%#Eval("AllMoney","{0:F2}") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <hr />
        <div class="item"><span>合计</span><span class="item_r"><%:orderMod.Ordersamount.ToString("F2") %></span></div>
        <div class="item"><span>应收金额</span><span class="item_r"><%:orderMod.Ordersamount.ToString("F2") %></span></div>
    </div>
    <div id="cashier" ng-app="app" ng-controller="appCtrl">
       <div id="method_wrap">
           <ul id="method_ul">
               <li ng-class="ui.current=='money'?'active':''" ng-click="ui.change('money');">现金</li>
               <%if (AllowMCard())
                       { %>
               <li ng-class="ui.current=='mcard'?'active':''" ng-click="ui.change('mcard');">会员余额</li>
               <%}
                       else
                       { %>
               <li class="disabled">会员余额</li>
               <%} %>
               <div class="clearfix"></div>
           </ul>             
       </div>
       <div id="method_body" ng-if="!finish">
           <div id="cash_money" class="method_panel" ng-if="ui.current=='money'">
           <div class="calculator">
               <div class="ihead">
                   <label style="font-size:12px;color:#999;">实收</label>
                   <i class="fa fa-rmb" style="font-size:28px;"></i>
                   <input id="receMoney_t" type="text" style="border:none;width:180px;height:50px;font-size:20px;padding:5px 8px" placeholder="输入收银金额" ng-model="payment.rece"/>
                   <label style="font-size:12px;" ng-bind-html="calc()|html"></label>
               </div>
               <div class="counter">
                   <div>
                       <span ng-click="addchar(7);">7</span>
                       <span ng-click="addchar(8);">8</span>
                       <span ng-click="addchar(9);">9</span>
                       <span ng-click="addchar('C');">C</span>
                   </div>
                   <div>
                       <span ng-click="addchar(4);">4</span>
                       <span ng-click="addchar(5);">5</span>
                       <span ng-click="addchar(6);">6</span>
                       <span ng-click="addchar('<');"><i class="fa fa-chevron-left"></i></span>
                   </div>
                   <div>
                       <span ng-click="addchar(1);">1</span>
                       <span ng-click="addchar(2);">2</span>
                       <span ng-click="addchar(3);">3</span>
                       <span class="finalBtn" ng-click="settle();">
                           结<br />算
                       </span>
                   </div>
                   <div>
                       <span ng-click="addchar('00');">00</span>
                       <span ng-click="addchar(0);">0</span>
                       <span ng-click="addchar('.');">.</span>
                   </div>
               </div>
           </div>
       </div>
           <div id="cash_mcard" class="method_panel" ng-if="ui.current=='mcard'" style="font-size:14px;">
           <div style="border:1px solid #ddd;background-color:#fafafa;padding:15px 20px;">
               使用会员余额支付 <span class="text-danger"><i class="fa fa-rmb"></i> <span ng-bind="payment.money"></span></span>
           </div>
           <div style="padding:15px 20px;">
               <span class="text-dark">可支付余额</span> <span class="text-danger"><i class="fa fa-rmb"></i> <%:client.Purse.ToString("F2") %></span>
           </div>
           <div>
               <input type="button" value="完成收款" ng-click="settleByMCard();" class="btn btn-danger btn-lg" style="width:100%;"/>
           </div>
       </div>
       </div>
       <div id="cash_result" class="method_panel text-center" ng-if="finish">
            <div class="item right-icon">
                <i class="fa fa-check" style="color: #fff; font-size: 50px;"></i>
            </div>
            <div class="item" style="color: #7ed321;">
                <i class="fa fa-rmb"></i><span><%:orderMod.Ordersamount.ToString("F2") %></span>
            </div>
            <div class="item" style="color: #999; font-size: 14px;">收款完成</div>
            <div class="item">
                <button type="button" class="btn btn-info btn-lg" onclick="parent.notify('cash_finish');">继续开单收款</button>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
<style type="text/css">
#method_wrap{border-bottom:1px solid #ddd;}
#method_ul{margin:0 auto;width:220px;}
#method_ul li{float:left;margin-right:10px;border:1px solid #ddd;background-color:#fafafa;border-bottom:none;text-align:center;width:100px;line-height:50px;cursor:pointer;}
#method_ul li.active{background-color:#e84c75;color:#fff;}
#method_ul li.disabled{color:#ddd;cursor:default;}

#summary {width:300px;font-size:12px;color:#999;border-right:1px solid #ddd;padding:0px 10px;position:fixed;height:100%;}
#summary .item{line-height:30px;}
#summary .item_r{color:#000;text-align:right;float:right;}
#summary .ihead{border-bottom:1px solid #ddd;text-align:center;}
#summary .ihead span{font-size:14px; font-weight:bolder; position:relative;bottom:-10px;padding:0 15px; background-color:#fff;}
#cashier{margin-left:320px;}
.method_panel{width:335px;padding-top:30px;margin:0 auto;}
.calculator .ihead{margin-bottom:5px;padding:5px 8px;
                   border:1px solid #e84c75;}
.counter div{margin-bottom:10px;}
.counter div span{background-color:#fafafa;cursor:pointer;
                   display:inline-block;width:80px;height:80px;text-align:center;padding-top:16px; border:1px solid #ddd;border-radius:4px;font-size:32px;}
.counter .finalBtn{position:absolute;height:170px;background-color:#e84c75;color:#fff;margin-left:5px;padding-top:40px;font-size:28px;}
#cash_result .item{margin-bottom:8px;}
#cash_result .right-icon{background-color:#7ed321;width:100px;height:100px;line-height:125px; margin:0 auto;text-align:center;border-radius:50%;}
/*禁止鼠标点击数字键盘造恩多选*/
span {-moz-user-select:none;-webkit-user-select:none;user-select:none;    }
</style>
<script src="/JS/Plugs/angular.min.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/ICMS/ZL_Common.js"></script>
<script src="/JS/ZL_Regex.js"></script>
<script>
    var diag = new ZL_Dialog();
    diag.maxbtn = false;
    diag.closebtn = false;
    angular.module("app", []).controller("appCtrl", function ($scope) {
        $scope.finish = false;
        $scope.cash_stop = false;
        $scope.ui = { current: "money" };//显示的ui界面
        $scope.ui.change = function (type) { this.current = type; }
        $scope.payment = { money: parseFloat("<%=orderMod.Ordersamount%>").toFixed(2), rece: parseFloat("<%=orderMod.Ordersamount%>").toFixed(2) };
        $scope.calc = function () {
            var balance = $scope.payment.rece - $scope.payment.money;
            if (balance == 0) { return "<span style='color:green;'>不用找零</span>"; }
            else if (balance > 0) { return "<span style='color:orange;'>找零:" + balance.toFixed(2); +"<span>" }
            else if (balance < 0) { return "<span style='color:red;'>还缺:" + balance.toFixed(2) + "</span>"; }
        }
        //结算,生成支付单并修改付款状态
        //会员扫码,现金支付
        var apiUrl = "/Extend/Order/Cash.aspx?oid=<%:OrderID%>&action=";
        $scope.settle = function () {
            if ($scope.cash_stop == true) { console.log("取消settle"); return; }
            var payment = $scope.payment;
            if (payment.rece < payment.money) { alert("实收金额不能小于应收金额"); return false; }
            if (payment.rece < 0 || payment.money < 0) { alert("金额不正确"); return false; }
            $scope.cash_stop = true;
            diag.ShowMask("正在处理支付");
            $.post(apiUrl + "settle", { type: "money", "rece": $scope.payment.rece }, function (data) {
                diag.CloseModal();
                if (data == "1") { $scope.finish = true; }
                else { $scope.cash_stop = false; alert(data); }
                $scope.$digest();
            })
        }
        //使用会员卡支付
        $scope.settleByMCard = function () {
            if ($scope.cash_stop == true) { console.log("取消settle"); return; }
            var payment = $scope.payment;
            payment.rece = payment.money;
            $scope.cash_stop = true;
            diag.ShowMask("正在处理会员卡支付");
            $.post(apiUrl + "settle", { type: "mcard" }, function (data) {
                diag.CloseModal();
                if (data == "1") { $scope.finish = true; }
                else { $scope.cash_stop = false; alert(data); }
                $scope.$digest();
            })
        }
        $scope.addchar = function (char) {
            var rece = $scope.payment.rece;
            rece += "";
            if (rece == "0") { rece = ""; }
            switch (char) {
                case "<":
                    if (rece.length <= 1) { rece = ""; }
                    else { rece = rece.substr(0, rece.length - 1); }
                    break;
                case "C":
                    rece = "";
                    break;
                case "."://只允许有一个小数点
                    if (rece.indexOf(".") > -1) { return; }
                    else { rece += "."; }
                    break;
                default:
                    rece += "" + char;
                    break;
            }
            //如果小数位过长,则截断
            if (!rece || rece == "") { rece = ""; }
            if (rece.indexOf(".") > -1) {
                var len = rece.indexOf(".") + 3;
                len = len > rece.length ? rece.length : len;
                rece = rece.substr(0, len);
            }
            if (Convert.ToDouble(rece) > 20000) { rece = "20000"; }
            $scope.payment.rece = rece;
        }   
    }).filter("html", ["$sce", function ($sce) {
        return function (text) { return $sce.trustAsHtml(text); }
    }]);
    $(function () {
        $("#receMoney_t")[0].focus();
    })
</script>
</asp:Content>