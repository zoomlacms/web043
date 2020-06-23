<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientAdd.aspx.cs" Inherits="Extend_ClientAdd" MasterPageFile="~/Common/Master/User.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>客户管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="Client.aspx">客户列表</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">客户管理</a></li>
    </ol>
<table class="table table-bordered table-striped">
    <tr>
        <td class="td_m">姓名<span class="r_red">*</span></td>
        <td><ZL:TextBox runat="server" ID="HoneyName" class="form-control text_300" AllowEmpty="false"/></td>
    </tr>
    <tr><td>手机号<span class="r_red">*</span></td>
        <td><ZL:TextBox runat="server" ID="Mobile" class="form-control text_300" AllowEmpty="false" ValidType="MobileNumber"/></td></tr>
    <tr><td>性别</td><td>
        <asp:DropDownList runat="server" ID="Sex_DP" class="form-control text_300">
            <asp:ListItem Value="未知">未知</asp:ListItem>
            <asp:ListItem Value="男">男</asp:ListItem>
            <asp:ListItem Value="女">女</asp:ListItem>
        </asp:DropDownList>
                   </td></tr>
    <tr><td>生日</td><td><ZL:TextBox runat="server" ID="BirthDay" class="form-control text_300" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });"/></td></tr>
    <tr><td>微信号</td><td><ZL:TextBox runat="server" ID="WXNo" class="form-control text_300"/></td></tr>
    <tr><td>备注</td><td><ZL:TextBox runat="server" ID="Remark" class="form-control m715-50" Rows="5" TextMode="MultiLine"/></td></tr>
    <tr><td></td><td><asp:Button runat="server" ID="Save_Btn" class="btn btn-info" OnClick="Save_Btn_Click" Text="保存信息"/></td></tr>
</table>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/DatePicker/WdatePicker.js"></script>
</asp:Content>