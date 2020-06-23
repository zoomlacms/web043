<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MsgSend.aspx.cs" Inherits="ZoomLaCMS.Manage.WeiXin.Msg.MsgSend" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>消息群发</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<table class="table table-bordered table-striped">
    <tr><td class="td_m">群发模式</td><td>
        <label><input type="radio" name="mode_rad" value="loop" checked="checked"/>轮循群发</label>
        <label><input type="radio" name="mode_rad" value="api"/>接口群发</label>
        <div class="rd_green">轮循群发:不消耗群发次数,多图文必须设定好内容链接,缩略图无法显示</div>
        <div class="rd_green">接口群发:通过微信接口群发,服务号每月四次,订阅号每日一次</div>
    </td></tr>
    <tr><td>消息标题</td><td><asp:Label runat="server" ID="Title_L" /></td></tr>
    <tr><td>消息类型</td><td><asp:Label runat="server" ID="MsgType_L" /></td></tr>
    <tr runat="server" visible="false"><td>消息内容</td><td><asp:Label runat="server" ID="MsgContent_L" /></td></tr>
    <tr><td>推送用户</td>
        <td>
            <div ng-app="app" ng-controller="appCtrl">
                <table class="table table-bordered">
                    <thead><tr><th>用户名</th><th>微信名</th><th class="td_m">操作</th></tr></thead>
                    <tr ng-repeat="item in list">
                        <td ng-bind="item.name"></td>
                        <td ng-bind="item.wxname"></td>
                        <td>
                            <button type="button" class="btn btn-danger btn-sm" ng-click="del(item);"><i class="fa fa-trash-o"></i></button>
                        </td>
                    </tr>
                </table>
            </div>
            <input type="button" value="选择用户(默认推送给所有用户)" class="btn btn-info btn-sm" onclick="showMember();"/>
        </td>
    </tr>
    <tr><td>发送结果</td><td>
        <table class="table table-bordered">
            <tr>
                <td>ID</td>
                <td>公众号</td>
                <td>结果</td>
                <td>备注</td>
            </tr>
            <asp:Repeater runat="server" ID="Result_RPT" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td class="td_s"><%#Eval("appid") %></td>
                        <td class="td_l"><%#Eval("Alias") %></td>
                        <td class="td_s"><%#Convert.ToBoolean(Eval("isok"))?ComRE.Icon_OK:ComRE.Icon_Error %></td>
                        <td style="color: red;"><%# Convert.ToBoolean(Eval("isok"))?"":Eval("result","") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
                     </td></tr>
    <tr><td></td><td>
        <asp:Button runat="server" ID="Send_Btn" Text="开始发送" OnClick="Send_Btn_Click" class="btn btn-info" OnClientClick="return showWait();" />
                 </td></tr>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<style type="text/css">
.wxapp_wrap{font-weight:normal;display:inline-block;width:150px;}
</style>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/Plugs/angular.min.js"></script>
<script>
    function showWait() {
        comdiag.maxbtn = false;
        comdiag.ShowMask("正在处理中,请等待.");
        return true;
    }
</script>
<script>
    var app = angular.module("app", []).controller("appCtrl", function ($scope) {
        $scope.list = [];
        $scope.del = function (item) {
            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i] == item) { $scope.list.splice(i, 1); break; }
            }
        }
        
    })
    function showMember() {
        comdiag.width = "width1024";
        ShowComDiag("/Extend/Common/SelClient.aspx?addon=wechat", "");
    }
    function selMember(user) {
        scope.member.user = user;
        scope.$digest();
        diag.CloseModal();
    }
</script>
</asp:Content>