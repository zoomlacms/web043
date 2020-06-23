<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployBonus.aspx.cs" Inherits="Extend_Report_EmployBonus" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/Top_Report.ascx" TagPrefix="ZL" TagName="ReportTop" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>技师报表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:ReportTop runat="server" ID="ReportTop" />
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">技师报表</a></li>
</ol>   
<div class="input-group" style="margin-bottom:5px;">
    <asp:TextBox runat="server" ID="JSName_T" placeholder="技师姓名" class="form-control text_150" />
    <asp:TextBox runat="server" ID="Proname_T" placeholder="服务名称" class="form-control text_150"/>
    <asp:TextBox runat="server" ID="STime_T" placeholder="开始时间" class="form-control text_150" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });"/>
    <asp:TextBox runat="server" ID="ETime_T" placeholder="结束时间" class="form-control text_150" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });"/>
    <span class="input-group-btn">
        <asp:Button runat="server" ID="Search_Btn" Text="筛选" OnClick="Search_Btn_Click" class="btn btn-info"/>
    </span>
</div>   
<ul class="nav nav-tabs">
     <li class="nav-item">
        <a class="nav-link active" href="#tab0" data-toggle="tab">销售详情</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="#tab1" data-toggle="tab">按技师汇总</a>
    </li>
</ul>
<div class="tab-content">
<div class="tab-pane active" id="tab0">
<table class="table table-bordered table-striped">
        <thead>
            <tr>
                <th>ID</th>
                <th>服务名称</th>
                <th>金额</th>
                <th>技师</th>
                <th>提成</th>
                <th>时间</th>
            </tr>
        </thead>
    <ZL:Repeater runat="server" ID="RPT"  PageSize="20" BoxType="dp" PagePre="<tr><td colspan='13'><div class='text-center'>" PageEnd="</div></td></tr>">
        <ItemTemplate>
            <tr>
                <td><%#Eval("ID") %></td>
                <td><%#Eval("Proname") %></td>
                <td><%#Eval("AllMoney","{0:F2}") %></td>
                <td><a href="javascript:;" data-id="<%#Eval("Code") %>"><%#Eval("Attribute") %></a></td>
                <td><%#Eval("AllMoney_Json","{0:f2}") %></td>
                <td><%#Eval("AddTime") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </ZL:Repeater>
    </table>
</div>
<div class="tab-pane" id="tab1">
   <table class="table table-bordered table-striped">
       <thead><tr><th>技师名称</th><td>提成汇总</td></tr></thead>
        <asp:Repeater runat="server" ID="ByJS_RPT">
        <ItemTemplate>
            <tr><td><%#Eval("Attribute") %></td><td><%#Eval("AllMoney_Json","{0:f2}") %></td></tr>
        </ItemTemplate>
    </asp:Repeater>
   </table>
</div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script>
    $("#nav1").addClass("active");
</script>
</asp:Content>