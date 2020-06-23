<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bonus.aspx.cs" Inherits="Extend_UA_Bonus"  MasterPageFile="~/Common/Master/User.Master"%>
<%@ Register Src="~/Extend/Common/Top_StoreConfig.ascx" TagPrefix="ZL" TagName="Top_StoreConfig" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>提成管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ZL:Top_StoreConfig runat="server" ID="Top_StoreConfig" />
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">提成管理</a> [<a href="BonusAdd.aspx">添加提成方案</a>]</li>
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
        <asp:TemplateField HeaderText="名称">
            <ItemTemplate>
                <a href="BonusAdd.aspx?ID=<%#Eval("ID") %>"><%#Eval("Alias") %></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CDate" DataFormatString="{0:yyyy年MM月dd日}" HeaderText="创建时间" ItemStyle-CssClass="td_l"/>
        <asp:BoundField DataField="Remark" HeaderText="备注" />
        <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_l">
            <ItemTemplate>
                <a class="option_style" href="BonusAdd.aspx?id=<%#Eval("ID") %>"><i class="fa fa-pencil" title="修改"></i></a>
                <asp:LinkButton runat="server" class="option_style" CommandName="del2" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要删除吗');"><i class="fa fa-trash-o" title="删除"></i></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ZL:ExGridView>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
    <script>
        $("#nav1").addClass("active");
    </script>
</asp:Content>