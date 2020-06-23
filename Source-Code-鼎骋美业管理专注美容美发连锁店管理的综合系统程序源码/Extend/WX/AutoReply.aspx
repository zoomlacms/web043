<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoReply.aspx.cs" Inherits="Extend_WX_AutoReply" MasterPageFile="~/Common/Master/User.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>欢迎语配置</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
    <li class="breadcrumb-item"><a href="Default.aspx">微信管理</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">自动回复</a> <a href="AddReply.aspx">[添加回复]</a></li>
</ol>
     <ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="False" PageSize="10" OnRowDataBound="EGV_RowDataBound" 
        OnPageIndexChanging="EGV_PageIndexChanging" IsHoldState="false" AllowPaging="True" AllowSorting="True" OnRowCommand="EGV_RowCommand"
        CssClass="table table-striped table-bordered table-hover" EnableTheming="False" EnableModelValidation="True" EmptyDataText="没有内容">
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="td_s">
                <ItemTemplate>
                    <input type="checkbox" name="idchk" value="<%#Eval("ID") %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="关键词" DataField="Filter" ItemStyle-CssClass="td_m" />
            <asp:TemplateField HeaderText="类型" ItemStyle-CssClass="td_m" >
                <ItemTemplate>
                    <%#GetMsgType() %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="默认">
                <ItemTemplate>
                    <%#GetIsDefault() %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="回复信息" DataField="Content"/>
            <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_l">
                <ItemTemplate>
                    <a href="AddReply.aspx?ID=<%#Eval("ID") %>" class="option_style"><i class="fa fa-pencil" title="修改"></i></a>
                    <asp:LinkButton runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="del2" OnClientClick="return confirm('确定要删除吗');" CssClass="option_style"><i class="fa fa-trash-o" title="删除"></i>删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ZL:ExGridView>
<asp:Button runat="server" ID="BatDel_Btn" OnClick="BatDel_Btn_Click" Text="批量删除" CssClass="btn btn-primary" OnClientClick="return confirm('确定要删除吗');" />
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent"></asp:Content>