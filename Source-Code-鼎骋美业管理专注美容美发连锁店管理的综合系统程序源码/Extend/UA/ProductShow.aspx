<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductShow.aspx.cs" Inherits="Extend_UA_ProductShow" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>商品展示</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
        <li class="breadcrumb-item"><a href="/User/UserShop/ProductList">商品列表</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">商品信息</a></li>
    </ol>
<table class="table table-striped table-bordered">
    <tbody>
        <tr>
            <td class="text-right" style="width:120px;">商品类型：</td>
            <td><%:nodeMod.NodeName %></td>
        </tr>
        <tr><td class="text-right">商品ID：</td>
            <td><%:proMod.ID %></td>
        </tr>
        <tr><td class="text-right">商品编号：</td>
            <td><%:proMod.ProCode %></td>
        </tr>
        <tr>
            <td class="text-right">商品名称：</td>
            <td><%:proMod.Proname %></td>
        </tr>
        <tr>
            <td class="text-right">零售价：</td>
            <td><%:proMod.LinPrice.ToString("F2") %></td>
        </tr>
        <asp:Label runat="server" ID="ModelHtml" EnableViewState="false"></asp:Label>
        <tr>
            <td></td>
			<td>
				<a href="ProductAdd.aspx?id=<%=proMod.ID %>" class="btn btn-primary">重新修改</a>
                <a href="ProductAdd.aspx" class="btn btn-primary">继续添加</a>
                <a href="/User/UserShop/ProductList" class="btn btn-primary">商品列表</a>
			</td>
		</tr>
    </tbody>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent"></asp:Content>