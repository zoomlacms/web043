<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FastOrder.aspx.cs" Inherits="Extend_Order_FastOrder" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>快速开单</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">快速开单</a></li>
</ol>
<div ng-app="app" ng-controller="appCtrl">
    <div class="input-group" style="margin-bottom: 5px;">
        <input disabled="disabled" value="{{member.user.UserName}}" class="form-control" style="border-radius: unset;"  />
        <span class="input-group-btn">
            <button type="button" class="btn btn-info" ng-click="member.clear();"><i class="fa fa-recycle"></i> 清空选择</button>
        </span>
        <span class="input-group-btn">
            <button type="button" class="btn btn-info" ng-click="member.sel();"><i class="fa fa-user"></i> 选择会员</button></span>
    </div>
    <table class="table table-bordered table-striped">
    <tr>
        <td>消费项目</td>
        <td class="td_m">单价(元)</td>
        <td class="td_m">数量</td>
        <td class="td_m">技师</td>
        <td class="td_m">优惠/抵扣</td>
        <td class="td_m">小计(元)</td>
        <td class="td_m">操作</td>
    </tr>
    <tr ng-repeat="item in cart.list">
        <td ng-bind="item.Proname"></td>
        <td ng-bind="item.Shijia"></td>
        <td><input type="text" ng-model="item.Pronum" class="form-control-sm"/></td>
        <td>
            <a ng-if="item.code==0" href="javascript:;" ng-click="cart.selEmployee(item);">选择技师 <i class="fa fa-chevron-down"></i></a>
            <a ng-if="item.code>0" href="javascript:;" ng-click="cart.selEmployee(item);" title="重新选择 ">{{item.Attribute}}</a>
        </td>
        <td></td>
        <td ng-bind="cart.calcItem(item);"></td>
        <td><a href="javascript:;" ng-click="cart.del(item);" class="btn btn-danger btn-sm"><i class="fa fa-trash-o"></i></a></td>
    </tr>
</table>
    <div>
        <input type="text" class="form-control" placeholder="订单备注,200字以内" maxlength="200" />
    </div>
    <div class="margin_t5">
    <button type="button" class="btn btn-info" onclick="$('#product_modal').modal('show');">添加消费</button>
        <div class="pull-right">
            应收金额：<span class="r_red"><i class="fa fa-rmb"></i> <span ng-bind="cart.total();"></span></span>
            <button type="button" class="btn btn-info" ng-click="saveToSuspend();">挂单</button>
            <button type="button" class="btn btn-info" ng-click="saveToCash();" ng-disabled="cash_stop">{{cash_text}}</button>
        </div>
    </div>
    <sel-employee></sel-employee>
    <sel-product></sel-product>
</div>
<asp:HiddenField runat="server" id="Cart_Hid" Value=""/>
<link href="/Extend/Common/sel.css" rel="stylesheet" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Plugs/angular.min.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
    var diag = new ZL_Dialog();
    diag.maxbtn = false;
    diag.backdrop = true;
    diag.width = "width1024";
    var scope = null;
    function selMember(user) {
        console.log(user);
        scope.member.user = user;
        scope.$digest();
        diag.CloseModal();
    }
    function notify(cmd) {
        switch (cmd) {
            case "cash_finish":
                diag.CloseModal();
                location = "FastOrder.aspx";
                break;
        }
    }
    //-------------------------------
    var app=angular.module("app", []).controller("appCtrl", function ($scope) {
        scope = $scope;
        $scope.orderId = "<%:Mid%>";
        $scope.cash_stop = false;
        $scope.cash_text = "收款";
        $scope.member = { user: { UserID: 0, UserName: "散客" } };//选择的会员
        $scope.member.clear = function ()
        {
            this.user.UserID = 0;
            this.user.UserName = "散客";
        }
        $scope.member.sel = function () {
            diag.ShowModal("/Extend/Common/SelClient.aspx", "-");
        }
        $scope.cart = { list: [], active: null };
        $scope.cart.newMod = function () {
            return { "ID": 0, CartID: "", "PClass": "0", "ProClass": 0, "Orderlistid": 0, "ProID": 0, "Pronum": 0, "Username": "", "Proname": "", "Danwei": "", "Shijia": 0.0, "AllMoney": 0.0, "Addtime": "2017-12-15T12:03:39.7970694+08:00", "EndTime": "9999-12-31T23:59:59.9999999", "Additional": "", "StoreID": 0, "AllMoney_Json": "", "AddStatus": "", "cartStatus": "", "code": 0, "ProInfo": "", "ArriveMoney": 0.0, "Usepoint": 0, "UsePointArrive": 0.0, "Attribute": "", "FarePrice": "", "ArriveRemind": "", "Remark": "", "PK": "ID" };
        }
        //单项小计
        $scope.cart.calcItem = function (item) {
            return (item.Pronum * item.Shijia).toFixed(2);
        }
        $scope.cart.total = function () {
            var ref = this;
            var money = 0;
            for (var i = 0; i < ref.list.length; i++) {
                money += parseFloat(ref.calcItem(ref.list[i]));
            }
            return money.toFixed(2);
        }
        $scope.cart.del = function (item) {
            if (!confirm("确定要移除吗")) { return false; }
            var ref = this;
            for (var i = 0; i < ref.list.length; i++) {
                if (ref.list[i].ProID == item.ProID) { ref.list.splice(i, 1); break; }
            }
        }
        //添加新商品至购物车
        $scope.$on("set_product", function (v, product) {
            var ref = $scope.cart;
            for (var i = 0; i < ref.list.length; i++) {
                if (ref.list[i].ProID == product.ID) { return; }
            }
            var cartMod = ref.newMod();
            cartMod.ProID = product.ID;
            cartMod.Proname = product.Proname;
            cartMod.Shijia = product.LinPrice;
            cartMod.Pronum = 1;
            ref.list.push(cartMod);
        })
        //选择职员
        $scope.cart.selEmployee = function (cart) {
            var ref = this;
            ref.active = cart;
            $("#employee_modal").modal("show");
            console.log(ref.active)
        }
        $scope.$on("set_employee", function (e, employee) {
            var ref = $scope.cart;
            ref.active.code = employee.UserID;
            ref.active.Attribute = employee.UserName;
            $("#employee_modal").modal("hide");
        })
        //生成订单,并保存,支持取单
        $scope.saveAsOrder = function (cb) {
            if ($scope.cart.list.length < 1) { alert("尚未选定消费项目"); return false; }
            $scope.cash_stop = true;
            $scope.cash_text = "生成订单中....";
            var postData = { cart: angular.toJson($scope.cart.list) };
            postData.uid = $scope.member.user.UserID;
            postData.orderId = $scope.orderId;
            $.post("FastOrder.aspx?id=<%:Mid%>", postData, function (data) {
                $scope.orderId = data;
                $scope.cash_stop = false;
                $scope.cash_text = "收款";
                if (cb) { cb(); }
            })
        }
        //保存并收银
        $scope.saveToCash = function () {
            $scope.saveAsOrder(function () {
                diag.closebtn = false;
                diag.ShowModal("/Extend/Order/Cash.aspx?oid=" + $scope.orderId, "-");
            });
        }
        //保存并挂单
        $scope.saveToSuspend = function () {
            $scope.saveAsOrder(function () {
                location = "FastOrder.aspx";
            });
        }
        var json = $("#Cart_Hid").val();
        if (json != "") { $scope.cart.list = JSON.parse(json); }
    })
</script>
<script src="/Extend/Common/Angular_CMD.js"></script>
</asp:Content>