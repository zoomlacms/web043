<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeAdd.aspx.cs" Inherits="Extend_EmployeeAdd" MasterPageFile="~/Common/Master/User.Master" %>

<%@ Register Src="~/Manage/I/ASCX/SFileUp.ascx" TagPrefix="ZL" TagName="SFileUp" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>员工管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="Employee.aspx">员工列表</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">员工管理</a></li>
    </ol>
    <table class="table table-bordered table-striped">
        <tr><td class="td_m">用户名</td><td><ZL:TextBox runat="server" ID="UserName_T" class="form-control text_300" placeholder="建议使用员工姓名" AllowEmpty="false"/></td></tr>
        <tr><td>密码</td><td><asp:TextBox runat="server" ID="UserPwd_T" TextMode="Password" class="form-control text_300" /></td></tr>
        <tr><td>确定密码</td><td><asp:TextBox runat="server" ID="UserPwd2_T" TextMode="Password" class="form-control text_300" /></td></tr>
        <tr><td>基础底薪</td><td><ZL:TextBox runat="server" ID="SiteRebateBalance_T" class="form-control text_300" ValidType="IntPostive"/></td></tr>
        <tr><td>提成方案</td><td>
            <label><input type="radio" name="bonus_rad" value="0"/>默认方案</label>
            <asp:Repeater runat="server" ID="Bonus_RPT">
                <ItemTemplate>
                    <label><input type="radio" name="bonus_rad" value="<%#Eval("ID") %>" /><%#Eval("Alias") %></label>
                </ItemTemplate>
            </asp:Repeater>
          </td></tr>
        <tr><td>员工角色</td><td>
            <asp:Repeater runat="server" ID="ERole_RPT">
                <ItemTemplate>
                    <label><input type="radio" name="role_rad" value="<%#Eval("ID") %>" /><%#Eval("RoleName") %></label>
                </ItemTemplate>
            </asp:Repeater>

                       </td></tr>
        <tr><td colspan="2" class="text-center">扩展信息</td></tr>
        <tr><td>手机号</td><td><ZL:TextBox runat="server" ID="Mobile_T" class="form-control text_300" ValidType="MobileNumber" /></td></tr>
        <tr><td>头像</td><td>
            <ZL:SFileUp runat="server" ID="SFileUp" />
        </td></tr>
        <tr><td></td><td>
            <asp:Button runat="server" ID="Save_Btn" class="btn btn-info" Text="保存信息" OnClick="Save_Btn_Click" />
            <a href="Employee.aspx" class="btn btn-light">返回列表</a>
                     </td></tr>
    </table>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/SelectCheckBox.js"></script>
</asp:Content>