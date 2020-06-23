<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreOrder.aspx.cs" Inherits="Extend_Order_PreOrder" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>预约</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">预约</a></li>
</ol>
    <div class="input-group">
    <span class="input-group-btn">
        <a href="PreOrderAdd.aspx" class="btn btn-info">添加预约</a>
    </span>
    <asp:TextBox runat="server" class="form-control" placeholer="预订人姓名或手机号"/>
    <span class="input-group-btn">
        <asp:Button runat="server" class="btn btn-info" Text="搜索"></asp:Button>
    </span>
</div>
    <div class="margin_t5">
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
        <asp:TemplateField HeaderText="预约人">
            <ItemTemplate>
                <div><%#Eval("ClientName") %></div>
                <div><%#Eval("ClientMobile") %></div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="预约服务|技师">
            <ItemTemplate>
                <asp:Repeater runat="server" ID="Product_RPT"  EnableViewState="false">
                    <ItemTemplate>
                       <div><span><%#Eval("ProName") %></span><strong>|</strong><%#Eval("EmpName") %></div>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CDate" DataFormatString="{0:yyyy年MM月dd日}" HeaderText="提交时间" ItemStyle-CssClass="td_l"/>
        <asp:BoundField DataField="ClientDate" DataFormatString="{0:yyyy年MM月dd日 HH:mm}" HeaderText="预约到店时间" />
        <asp:BoundField DataField="Remark" HeaderText="备注" />
        <asp:TemplateField HeaderText="操作" ItemStyle-CssClass="td_l">
            <ItemTemplate>
               <asp:LinkButton runat="server" class="btn btn-info btn-sm" CommandName="del2" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要取消该预约吗');">取消</asp:LinkButton>
               <a href="PreOrderAdd.aspx?ID=<%#Eval("ID") %>" class="btn btn-info btn-sm">更改</a>
               <asp:LinkButton runat="server" class="btn btn-info btn-sm" CommandName="fast" CommandArgument='<%#Eval("ID") %>' OnClientClick="return confirm('确定要将预约开单吗');">开单</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ZL:ExGridView>
</div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">

</asp:Content>