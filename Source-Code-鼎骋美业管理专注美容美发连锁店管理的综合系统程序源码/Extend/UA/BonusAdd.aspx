<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BonusAdd.aspx.cs" Inherits="Extend_UA_BonusAdd" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/Top_StoreConfig.ascx" TagPrefix="ZL" TagName="Top_StoreConfig" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>提成信息</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ZL:Top_StoreConfig runat="server" ID="Top_StoreConfig" />
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="Bonus.aspx">提成管理</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">提成信息</a></li>
    </ol>
     <table class="table table-bordered">
        <tr><td class="td_m">方案名称</td>
            <td><ZL:TextBox runat="server" ID="Alias_T" class="form-control text_300" AllowEmpty="false"/></td></tr>
        <tr><td>是否默认</td><td>
            <input type="checkbox" runat="server" id="IsDefault_Chk" class="switchChk" />
            <div class="r_green">
                该店铺员工是否默认应用该提成方案
            </div>
          </td></tr>
         <tr><td>备注</td><td>
             <asp:TextBox runat="server" ID="Remark_T" TextMode="MultiLine" class="form-control text_500" Rows="3"/>
                        </td></tr>
        <tr><td>提成设置</td>
            <td ng-app="app" ng-controller="appCtrl">
                <table class="table table-bordered table-condensed">
                    <thead><tr>
                        <th>服务名称</th>
                        <th class="td_m">提成方式</th>
                        <th class="td_l">提成</th>
                        <th class="td_m">操作</th>
                           </tr></thead>
                    <tr ng-repeat="item in list">
                        <td ng-bind="item.Proname"></td>
                        <td>
                            <select class="form-control text_150" ng-model="item.BonusType">
                                <option value="0">比例提成</option>
                                <option value="1">固定提成</option>
                            </select>
                        </td>
                        <td>
                            <div class="input-group">
                                <input type="text" class="form-control" ng-model="item.BonusValue1"/>
                                <span class="input-group-addon" ng-if="item.BonusType==0">%</span>
                                <span class="input-group-addon" ng-if="item.BonusType==1">元</span>
                            </div>
                        </td>
                        <td>
                            <button type="button" ng-click="del(item);" class="btn btn-danger btn-sm" ng-if="item.ProID>0"><i class="fa fa-trash-o"></i></button>
                        </td>
                    </tr>
                </table>
                <div style="margin-top:5px;text-align:right;">
                    <input type="button" value="选择商品" onclick="$('#product_modal').modal('show');" class="btn btn-info"/>
                </div>
               <sel-product></sel-product>
            </td>
        </tr>
         <tr>
             <td></td>
             <td>
                 <input type="button" value="保存信息" onclick="scope.subCheck();" class="btn btn-info"/>
                 <asp:Button runat="server" ID="Save_Btn" style="display:none;" OnClick="Save_Btn_Click" />
             </td>
         </tr>
    </table>
    <asp:HiddenField runat="server" ID="Data_Hid" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<link href="/dist/css/bootstrap-switch.min.css" rel="stylesheet" />
<script src="/dist/js/bootstrap-switch.js"></script>
<script src="/JS/Plugs/angular.min.js"></script>
<script src="/JS/ZL_Regex.js"></script>
<link href="/Extend/Common/sel.css" rel="stylesheet" />
<script>
    $("#nav1").addClass("active");
</script>
<script>
    var scope = null;
    var app=angular.module("app", []).controller("appCtrl", function ($scope) {
        scope = $scope;
        $scope.list = [];
        $scope.newMod = function () {
            return { ID: 0, ParentID: 0, ProID: 0, Proname: "", BonusType: 0, BonusValue1: "" };
        }
        $scope.subCheck = function () {
            for (var i = 0; i < $scope.list.length; i++) {
                var model = $scope.list[i];
                if (Convert.ToDouble(model.BonusValue1)<=0) { alert("未指定提成"); return false; }
            }
            $("#Data_Hid").val(angular.toJson($scope.list));
            $("#Save_Btn").click();
            return true;
        }
        $scope.del = function (item) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i] == item) { $scope.list.splice(i, 1); }
            }
        }
        //添加新商品至购物车
        $scope.$on("set_product", function (v, product) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i].ProID == product.ID) { return; }
            }
            var model = $scope.newMod();
            model.ProID = product.ID;
            model.Proname = product.Proname;
            $scope.list.push(model);
        })
        {
            if ($("#Data_Hid").val() == "") {
                var model = $scope.newMod();
                model.Proname = "统一配置";
                $scope.list.push(model);
            }
            else {
                $scope.list = JSON.parse($("#Data_Hid").val());
            }
        }
    })
</script>
<script src="/Extend/Common/Angular_CMD.js"></script>
</asp:Content>