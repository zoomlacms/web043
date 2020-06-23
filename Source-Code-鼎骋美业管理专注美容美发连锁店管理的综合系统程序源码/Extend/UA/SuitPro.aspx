<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuitPro.aspx.cs" Inherits="Extend_UA_SuitCard" MasterPageFile="~/Common/Master/User.Master"%>
<%@ Register Src="/Extend/Common/TopMenu.ascx" TagPrefix="ZL" TagName="TopMenu" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>套卡列表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:TopMenu runat="server" id="TopMenu" Active="2" />
<ol class="breadcrumb">
<li class="breadcrumb-item"><a title="会员中心" href="/User/">会员中心</a></li>
<li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">套卡列表</a> [<a href="/Extend/UA/SuitProAdd.aspx">添加商品</a>]</li>
</ol>
<ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="20" 
        OnRowDataBound="EGV_RowDataBound" OnPageIndexChanging="EGV_PageIndexChanging" IsHoldState="false" 
        OnRowCommand="EGV_RowCommand" AllowPaging="True" AllowSorting="True" CssClass="table table-striped table-bordered table-hover" EnableTheming="False" EnableModelValidation="True" EmptyDataText="<%$Resources:L,无相关数据 %>">
        <Columns>
            <asp:TemplateField ItemStyle-CssClass="td_xs">
                <ItemTemplate><input type="checkbox" name="idchk" value='<%#Eval("id") %>' /></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="ID" HeaderStyle-Width="3%" DataField="ID" />
            <asp:TemplateField HeaderText="<%$Resources:L,商品图片 %>">
                <HeaderStyle CssClass="td_m" />
                <ItemTemplate>
                    <img src="<%#ZoomLa.Common.function.GetImgUrl(Eval("Thumbnails")) %>" class="img50" onerror="shownopic(this);" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:L,商品名称 %>">
                <ItemTemplate>
                    [<%#Eval("NodeName") %>]
                        <a href="SuitProAdd.aspx?id=<%#Eval("id")%>"><%#Eval("ProName") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="<%$Resources:L,录入者 %>" DataField="AddUser" HeaderStyle-Width="5%" />
            <asp:BoundField HeaderText="<%$Resources:L,单位 %>" DataField="ProUnit" HeaderStyle-Width="5%" />
            <asp:TemplateField HeaderText="<%$Resources:L,价格 %>">
                <ItemTemplate>
                   <%#Eval("LinPrice","F2") %>
                </ItemTemplate>
            </asp:TemplateField>
<%--            <asp:TemplateField HeaderText="<%$Resources:L,类型 %>">
                <ItemTemplate>
                    <%#formatnewstype(Eval("ProClass",""))%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="<%$Resources:L,状态 %>">
                <ItemTemplate>
                    <%#proBll.ShowStatus(GetDataItem()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:L,操作 %>" ItemStyle-CssClass="codetr">
                <ItemTemplate>
                    <a class="option_style" href="SuitProAdd.aspx?id=<%#Eval("id")%>"><i class="fa fa-pencil" title="<%=Resources.L.编辑 %>"></i></a>
                    <asp:LinkButton runat="server" CssClass="option_style" CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="return confirm('确定要将商品移入回收站吗');"><i class="fa fa-trash-o" title="<%=Resources.L.删除 %>"></i><%=Resources.L.删除 %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ZL:ExGridView>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script"></asp:Content>