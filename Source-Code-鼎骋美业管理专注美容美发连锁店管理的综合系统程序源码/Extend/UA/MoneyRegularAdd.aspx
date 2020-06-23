<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoneyRegularAdd.aspx.cs" Inherits="Extend_UA_MoneyRegularAdd" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/TopMenu.ascx" TagPrefix="ZL" TagName="TopMenu" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>充值管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:TopMenu runat="server" id="TopMenu" Active="1" />
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/">会员中心</a></li>
    <li class="breadcrumb-item"><a href="MoneyRegular.aspx">充值规则列表</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">规则管理</a></li>
</ol>
<table class="table table-bordered table-striped">
<tr><td class="td_l">别名</td><td><ZL:TextBox runat="server" ID="Alias_T" class="form-control text_300" AllowEmpty="false"/></td></tr>
<tr><td>充值金额</td><td>
    <ZL:TextBox runat="server" ID="Min_T" CssClass="form-control text_300" AllowEmpty="false" ValidType="FloatZeroPostive" />
                              </td></tr>
<tr><td>赠送金额</td><td><ZL:TextBox runat="server" ID="Purse_T" CssClass="form-control text_300" AllowEmpty="false" ValidType="FloatZeroPostive" /></td></tr>
<tr><td>赠送积分</td><td><ZL:TextBox runat="server" ID="Point_T" CssClass="form-control text_300" AllowEmpty="false" ValidType="FloatZeroPostive" /></td></tr>
<tr><td>备注</td><td><asp:TextBox runat="server" ID="AdminRemind_T" CssClass="form-control text_500" MaxLength="50" /></td></tr>
<tr><td></td><td>
    <asp:Button runat="server" ID="Save_Btn" Text="保存信息" OnClick="Save_Btn_Click" CssClass="btn btn-primary" />
    <a href="RegularList.aspx" class="btn btn-default">返回列表</a>
</td></tr>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
 
</asp:Content>