<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoneyRegular.aspx.cs" Inherits="Extend_UA_MoneyRegular" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/TopMenu.ascx" TagPrefix="ZL" TagName="TopMenu" %>

<asp:Content runat="server" ContentPlaceHolderID="head"><title>充值规则列表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:TopMenu runat="server" id="TopMenu" Active="1" />
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/">会员中心</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">充值规则列表</a> [<a href="/Extend/UA/MoneyRegularAdd.aspx">添加规则</a>]</li>
</ol>
<ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" PageSize="10" IsHoldState="false" 
        OnPageIndexChanging="EGV_PageIndexChanging" AllowPaging="True" AllowSorting="True" OnRowCommand="EGV_RowCommand" OnRowDataBound="EGV_RowDataBound"
        CssClass="table table-striped table-bordered" EnableTheming="False" EnableModelValidation="True" EmptyDataText="数据为空">
        <Columns>
            <asp:TemplateField ItemStyle-CssClass="td_s">
                <ItemTemplate>
                    <input type="checkbox" name="idchk" value="<%#Eval("ID") %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="别名" DataField="Alias" />
            <asp:BoundField HeaderText="充值金额" DataField="Min" DataFormatString="{0:f2}" />
            <asp:BoundField HeaderText="金额" DataField="Purse" DataFormatString="{0:f2}" />
            <asp:BoundField HeaderText="积分" DataField="Point" DataFormatString="{0:f2}" />
            <asp:BoundField HeaderText="备注" DataField="AdminRemind"/>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <a class="option_style" href="MoneyRegularAdd.aspx?id=<%#Eval("ID") %>"><i class="fa fa-pencil" title="修改"></i></a>
                    <asp:LinkButton runat="server" CssClass="option_style" CommandName="del2" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要删除吗');"><i class="fa fa-trash-o" title="删除"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ZL:ExGridView>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
</asp:Content>