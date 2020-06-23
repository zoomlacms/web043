<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="Extend_Employee" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/Top_StoreConfig.ascx" TagPrefix="ZL" TagName="Top_StoreConfig" %>

<asp:Content runat="server" ContentPlaceHolderID="head"><title>员工列表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:Top_StoreConfig runat="server" ID="Top_StoreConfig" />
<div>
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item">
            <a href="<%=Request.RawUrl %>">员工管理</a>
             [<a href="EmployeeAdd.aspx">添加员工</a>]
        </li>
    </ol>
    <ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" PageSize="10" IsHoldState="false" 
    OnPageIndexChanging="EGV_PageIndexChanging" AllowPaging="True" AllowSorting="True" OnRowCommand="EGV_RowCommand" OnRowDataBound="EGV_RowDataBound"
    CssClass="table table-striped table-bordered" EnableTheming="False" EnableModelValidation="True" EmptyDataText="数据为空">
    <Columns>
        <asp:TemplateField ItemStyle-CssClass="td_xs">
            <ItemTemplate>
                <input type="checkbox" name="idchk" value="<%#Eval("UserID") %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="ID" DataField="UserID" ItemStyle-CssClass="td_s" />
        <asp:TemplateField HeaderText="账号">
            <ItemTemplate>
                <%#Eval("UserName") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="头像">
            <ItemTemplate>
                <img class="img50" src="<%#Eval("salt") %>" onerror="shownoface(this);" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="角色">
            <ItemTemplate>
                <a href="/Extend/UA/ERoleAdd.aspx?ID=<%#Eval("PageID") %>"><%#Eval("RoleName") %></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="RegTime" DataFormatString="{0:yyyy年MM月dd日}" HeaderText="注册时间" ItemStyle-CssClass="td_l"/>
        <asp:BoundField DataField="LastLoginTime" DataFormatString="{0:yyyy年MM月dd日 HH:mm}" HeaderText="最近登录"/>
        <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_l">
            <ItemTemplate>
                <a class="option_style" href="EmployeeAdd.aspx?id=<%#Eval("UserID") %>"><i class="fa fa-pencil" title="修改"></i></a>
                <asp:LinkButton runat="server" class="option_style" CommandName="del2" CommandArgument='<%#Eval("UserID") %>' OnClientClick="return confirm('确定要删除吗');"><i class="fa fa-trash-o" title="删除"></i></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ZL:ExGridView>
    </div>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script>$("#nav2").addClass("active");</script>
</asp:Content>