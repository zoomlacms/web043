<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERoleAdd.aspx.cs" Inherits="Extend_UA_ERoleAdd" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>员工角色</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item">
            <a href="/Extend/Employee.aspx">员工管理</a>
        </li>
                <li class="breadcrumb-item">
            <a href="/Extend/UA/ERole.aspx">员工角色</a>
        </li>
         <li class="breadcrumb-item">
            <a href="<%=Request.RawUrl %>">角色管理</a>
        </li>
    </ol>
<table class="table table-bordered table-striped">
    <tr><td class="td_m">角色名</td>
        <td><ZL:TextBox runat="server" ID="RoleName_T" class="form-control m715-50" AllowEmpty="false"/></td></tr>
    <tr><td>备注</td>
        <td><asp:TextBox runat="server" ID="Remark_T" class="form-control m715-50" Rows="5" TextMode="MultiLine"/></td></tr>
    <tr><td colspan="2" class="text-center">权限</td></tr>
    <tr>
        <td>职能</td>
        <td>
            <label><input type="radio" name="ztype_rad" value="0" checked="checked" />无</label>
            <label><input type="radio" name="ztype_rad" value="1"/>技师</label>
        </td>
    </tr>
    <tr><td>权限</td><td class="auth_body">
        <label><input type="checkbox" name="auth_chk" value="cash"/>收银(预约|开单|订单|充值等)</label>
        <label><input type="checkbox" name="auth_chk" value="employ"/>员工管理</label>
        <label><input type="checkbox" name="auth_chk" value="client"/>会员管理</label>
        <label><input type="checkbox" name="auth_chk" value="sale" />销售报表</label>
        <label><input type="checkbox" name="auth_chk" value="wechat"/>微信管理</label>
     </td></tr>
    <tr><td></td><td>
        <asp:Button runat="server" ID="Save_Btn" OnClick="Save_Btn_Click" Text="保存信息" class="btn btn-info"/>
                 </td></tr>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/dist/js/bootstrap-switch.js"></script>
<script src="/JS/SelectCheckBox.js"></script>
<style type="text/css">
.auth_body label{margin-right:10px;border:1px solid #ddd;padding:5px 8px;border-radius:4px;}
.auth_body label input[name='auth_chk']{position:relative;top:3px;margin-right:3px;}
</style>
</asp:Content>