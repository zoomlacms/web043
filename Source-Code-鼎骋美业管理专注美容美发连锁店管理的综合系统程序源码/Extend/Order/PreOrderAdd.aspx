<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreOrderAdd.aspx.cs" Inherits="Extend_Order_PreOrderAdd" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>预约管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
    <li class="breadcrumb-item"><a href="PreOrder.aspx">预约列表</a></li>
<li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">预约管理</a></li>
</ol>
<table class="table table-bordered table-striped">
    <tr><td class="td_m">手机号<span class="r_red">*</span></td>
        <td><ZL:TextBox class="form-control text_300" runat="server" ID="ClientMobile_T" AllowEmpty="false" ValidType="MobileNumber"/></td></tr>
    <tr><td>预约人<span class="r_red">*</span></td>
        <td>
            <ZL:TextBox class="form-control text_300" runat="server" ID="ClientName_T" AllowEmpty="false"/>
            <button type="button" class="btn btn-info" onclick="showMember();">选择用户</button>
        </td>
            
    </tr>
    <tr><td>到店时间<span class="r_red">*</span></td>
        <td>
           <ZL:TextBox onclick="WdatePicker(dateRegion);" CssClass="form-control  text_150" runat="server" ID="ClientDate" AllowEmpty="false"/>
           <ZL:TextBox onclick="WdatePicker({ dateFmt: 'HH:mm' });" CssClass="form-control text_150" runat="server" ID="ClientDate2" AllowEmpty="false"/>
        </td></tr>
    <tr>
        <td>服务和技师<span class="r_red">*</span>
        </td>
        <td ng-app="app" ng-controller="appCtrl">
            <input type="button" value="选择服务" class="btn btn-info" ng-click="selProduct();" />
            <sel-product></sel-product>
            <sel-employee></sel-employee>

            <table class="table table-bordered table-striped margin_t5">
                <thead><tr><th>品名</th><th>技师</th><th>操作</th></tr></thead>
                <tr ng-repeat="item in list">
                    <td ng-bind="item.proname"></td>
                    <td>
                        <a ng-if="item.empid==0" href="javascript:;" ng-click="selEmployee(item);">选择技师 <i class="fa fa-chevron-down"></i></a>
                        <a ng-if="item.empid>0" href="javascript:;" ng-click="selEmployee(item);" title="重新选择 ">{{item.empname}}</a>
                    </td>
                    <td><button type="button" class="btn btn-info btn-sm" ng-click="del(item);"><i class="fa fa-trash-o"></i></button></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr><td>备注</td>
        <td><asp:TextBox runat="server" TextMode="MultiLine" class="form-control" Rows="5" ID="Remark"/></td></tr>
    <tr>
        <td></td>
        <td>
            <asp:Button runat="server" ID="Save_Btn" Text="保存信息" class="btn btn-info" OnClick="Save_Btn_Click" OnClientClick="return scope.presave();"/>
            <a href="PreOrder.aspx" class="btn btn-light">返回列表</a>
        </td>
    </tr>
</table>
</div>
<asp:HiddenField runat="server" ID="ClientNeed_Hid" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<link href="/Extend/Common/sel.css" rel="stylesheet" />
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script src="/JS/Plugs/angular.min.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
    var dateRegion = {
        dateFmt: 'yyyy/MM/dd',
        minDate: '<%=DateTime.Now.ToString("yyyy/MM/dd")%>',
        maxDate: '<%=DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd")%>'
    };
    var scope = null;
    var app = angular.module("app", []).controller("appCtrl", function ($scope) {
        scope = $scope;
        $scope.list = [];
        $scope.active = null;
        $scope.newMod = function () {
            return { id: 0, proid: 0, proname: "", empid: 0, empname: "" };
        }
        $scope.del = function (item) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i] == item) { $scope.list.splice(i, 1); return; }
            }
        }
        $scope.presave = function () {
            $("#ClientNeed_Hid").val(angular.toJson($scope.list));
            return true;
        }
        $scope.selProduct = function () {
            $("#product_modal").modal("show");
        }
        $scope.selEmployee = function (item) {
            $scope.active = item;
            $("#employee_modal").modal("show");
        }
        $scope.$on("set_product", function (e, m) {
            var model = $scope.newMod();
            model.proid = m.ID;
            model.proname = m.Proname;
            for (var i = 0; i < $scope.list.length; i++) {
                //已存在则不再添加
                if ($scope.list[i].proid == model.proid) { return; }
            }
            $scope.list.push(model);
        })
        $scope.$on("set_employee", function (e, m) {
            if ($scope.active == null) { return; }
            $scope.active.empid = m.UserID;
            $scope.active.empname = m.UserName;
            $("#employee_modal").modal("hide");
        })
        var clientNeed = $("#ClientNeed_Hid").val();
        if (clientNeed != "") {
            try { $scope.list = JSON.parse(clientNeed); } catch (ex) { }
        }
    })
    function showMember() {
        comdiag.width = "width1024";
        ShowComDiag("/Extend/Common/SelClient.aspx", "-");
    }
    function selMember(user) {
        $("#ClientMobile_T").val(user.Mobile);
        $("#ClientName_T").val(user.UserName);
        CloseComDiag();
    }
</script>
<script src="/Extend/Common/Angular_CMD.js"></script>
</asp:Content>