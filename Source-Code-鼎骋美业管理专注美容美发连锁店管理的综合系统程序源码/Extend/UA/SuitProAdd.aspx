<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuitProAdd.aspx.cs" Inherits="Extend_UA_SuitProAdd" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Manage/I/ASCX/SFileUp.ascx" TagPrefix="ZL" TagName="SFileUp" %>
<%@ Register Src="~/Extend/Common/TopMenu.ascx" TagPrefix="ZL" TagName="TopMenu" %>

<asp:Content runat="server" ContentPlaceHolderID="head"><title>套卡管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:TopMenu runat="server" ID="TopMenu" Active="2" />
<ol class="breadcrumb">
<li class="breadcrumb-item"><a title="会员中心" href="/User/">会员中心</a></li>
<li class="breadcrumb-item"><a href="SuitPro.aspx">套卡列表</a></li>
<li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">套卡管理</a></li>
</ol>     
<table class="table table-striped table-bordered table-hover">
        <tr>
            <td class="td_m">套卡名称：</td>
            <td>
                <ZL:TextBox ID="Name_T" runat="server" AllowEmpty="false" CssClass="form-control text_300" />
            </td>
        </tr>
        <tr><td>商品图片</td><td><ZL:SFileUp runat="server" ID="SFileUp" /></td></tr>
        <tr><td>组合价格</td><td>
            <ZL:TextBox runat="server" ID="Price_T" class="form-control text_300" AllowEmpty="false" ValidType="FloatZeroPostive" />
                         </td></tr>
<%--        <tr><td>库存</td>
            <td>
                <asp:TextBox runat="server" ID="Stock" class="form-control text_s" disabled="disabled"  Text="0"/>
                <input type="button" value="库存管理" class="btn btn-info" onclick="SetStock();" id="stock_btn" />
            </td>
        </tr>--%>
        <tr>
            <td>组合商品：</td>
            <td  ng-app="app" ng-controller="appCtrl">
                <input type="button" value="选择商品" onclick="$('#product_modal').modal('show');" class="btn btn-info" runat="server" id="selpro_btn" />
                <asp:HiddenField runat="server" ID="UProIDS_Hid" Value=""/>
 <%--               <script>
                    var upro = {};
                    upro.showdiag = function () { ShowComDiag("Shop/ProductsSelect.aspx?callback=selupro&action=suitpro", "选择商品"); }
                    function selupro(list) {
                        list = JSON.parse(list);
                        scope.merge(list);
                    }
                    //存ids与另一个字符存具体的信息
                </script>--%>
               <table class="table table-bordered table-striped margin_t5" style="width:600px;">
                   <tr><td>ID</td><td>商品名</td><td>数量</td><td>操作</td></tr>
                   <tbody>
                       <tr ng-repeat="item in list">
                           <td ng-bind="item.id" class="td_s"></td>
                           <td ng-bind="item.name"></td>
                           <td class="td_m"><input type="text" class="form-control" ng-model="item.suitnum" /></td>
                           <td class="td_m"><a href="javascript:;" ng-click="del(item);" /><i class="fa fa-trash-o"></i> 删除</td>
                       </tr>
                   </tbody>
               </table>
                <sel-product></sel-product>
            </td>
        </tr>
        <tr>
            <td>备注信息：</td>
            <td>
                <asp:TextBox ID="Remind_T" runat="server" TextMode="MultiLine" Width="500px" Height="80px" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <input type="button" value="保存信息" onclick="scope.presave();" class="btn btn-info" />
                <asp:Button ID="Save_B" runat="server" OnClick="Save_B_Click" style="display:none;" />
                <a href="SuitPro.aspx" class="btn btn-info">返回列表</a>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/SelectCheckBox.js"></script>
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/Controls/ZL_Array.js"></script>
<script src="/JS/ZL_Regex.js"></script>
<script src="/JS/Plugs/angular.min.js"></script>
<script src="/dist/js/bootstrap-switch.js"></script>
<script>
    var scope = null;
    var app=angular.module("app", []).controller("appCtrl", function ($scope) {
        //id,name,price,suitnum,prounit
        scope = $scope;
        $scope.list = [];
        $scope.newMod = function () {
            return { id: 0, name: "", suitnum: 1, price: 0 };
        }
        try { if (!ZL_Regex.isEmpty($("#UProIDS_Hid").val())) { $scope.list = JSON.parse($("#UProIDS_Hid").val()); } } catch (e) { $scope.list = []; }
        $scope.del = function (item) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i] == item) { $scope.list.splice(i, 1); return; }
            }
        }
        $scope.presave = function () {
            var json = angular.toJson($scope.list);
            $("#UProIDS_Hid").val(json);
            $("#Save_B").click();
        }
        $scope.$on("set_product", function (v, product) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i].id == product.ID) { return; }
            }
            var model = $scope.newMod();
            model.id = product.ID;
            model.name = product.Proname;
            $scope.list.push(model);
        })
    })
    function CloseDiag() {
        comdiag.CloseModal();
    }
    function closeDiag() { CloseComDiag(); }
    function setnode(nid) {
        $("#node_dp").val(nid);
    }
</script>
<script src="/Extend/Common/Angular_CMD.js"></script>
</asp:Content>
