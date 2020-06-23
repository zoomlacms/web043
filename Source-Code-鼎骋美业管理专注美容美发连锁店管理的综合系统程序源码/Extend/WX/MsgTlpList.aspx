﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MsgTlpList.aspx.cs" Inherits="Extend_WX_MsgTlpList" MasterPageFile="~/Common/Master/User.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>消息模板</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
    <li class="breadcrumb-item"><a href="Default.aspx">微信管理</a></li>
    <li class="breadcrumb-item">
        <a href="<%=Request.RawUrl %>">消息模板</a>
        [<a href='AddMsgTlp.aspx'>新建图文消息</a>]
<%--        [<a href='MsgMultiAdd.aspx'>新建多图文消息</a>]--%>
    </li>
</ol>
<ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" PageSize="10" IsHoldState="false" 
    OnPageIndexChanging="EGV_PageIndexChanging" AllowPaging="True" AllowSorting="True" OnRowCommand="EGV_RowCommand" OnRowDataBound="EGV_RowDataBound"
    CssClass="table table-striped table-bordered" EnableTheming="False" EnableModelValidation="True" EmptyDataText="数据为空">
    <Columns>
        <asp:TemplateField ItemStyle-CssClass="td_xs">
            <ItemTemplate>
                <input type="checkbox" name="idchk" value="<%#Eval("ID") %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="td_s" />
        <asp:TemplateField HeaderText="消息类型" ItemStyle-CssClass="td_m">
            <ItemTemplate>
                <%#GetMsgType() %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="消息名称" DataField="Alias"/>
    <%--    <asp:TemplateField HeaderText="消息内容">
            <ItemTemplate>
                <%#Eval("MsgContent") %>
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:BoundField DataField="CDate" DataFormatString="{0:yyyy年MM月dd日}" HeaderText="创建时间" ItemStyle-CssClass="td_l"/>
        <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_l">
            <ItemTemplate>
                <a class="option_style" href="<%#GetEditLink() %>"><i class="fa fa-pencil" title="修改"></i></a>
                <asp:LinkButton runat="server" CssClass="option_style" CommandName="del2" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要删除吗');"><i class="fa fa-trash-o" title="删除"></i></asp:LinkButton>
                <a class="option_style" href="MsgSend.aspx?ID=<%#Eval("ID") %>"><i class="fa fa-send"></i>发送</a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ZL:ExGridView>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
    function sendMsg(id) {
        comdiag.width = "width1024";
        ShowComDiag("MsgSend.aspx?ID=" + id, "消息群发");
    }
</script>
</asp:Content>