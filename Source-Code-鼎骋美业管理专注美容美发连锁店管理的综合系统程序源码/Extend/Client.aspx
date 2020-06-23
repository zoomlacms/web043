<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client.aspx.cs" Inherits="Extend_Client" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>客户列表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">客户管理</a> [<a href="ClientAdd.aspx">添加客户</a>]</li>
    </ol>
<div class="input-group" style="margin-bottom:5px;">
    <asp:TextBox runat="server" ID="UName_T" class="form-control" placeholder="请输入用户名或手机号"/>
    <span class="input-group-btn">
        <asp:Button runat="server" ID="Search_Btn" Text="搜索" OnClick="Search_Btn_Click" class="btn btn-info"/>
    </span>
</div>
    <table class="table table-bordered table-striped">
        <thead>
          <tr>
            <th>会员信息</th>
            <th class="td_m">会员卡</th>
            <th class="td_m">累计消费(元)</th>
           <%-- <th class="td_m">获客方式</th>--%>
            <th class="td_m">上次消费时间</th>
            <th style="width:130px;">操作</th>
        </tr>
        </thead>
        <ZL:Repeater runat="server" ID="RPT" PageSize="20" BoxType="dp" PagePre="<tr><td colspan='13'><div class='text-center'>" PageEnd="</div></td></tr>">
                <ItemTemplate>
                    <tr ondblclick="location='ClientInfo.aspx?id=<%# Eval("UserID") %>';">
                        <td style="position:relative;">
                            <div style="position:absolute;">
                                <img src="<%#Eval("UserFace") %>" onerror="shownoface(this);" class="img50" style="border-radius:50%;"/>
                            </div>
                            <div style="margin-left: 60px;line-height:20px;padding-top:5px;">
                                <div><%#Eval("HoneyName") %></div>
                                <div><%#Eval("Mobile") %></div>
                            </div>
                        </td>
                        <td><%# Eval("Purse","{0:F2}") %></td>
           <%--             <td></td>--%>
                        <td><%#ZoomLa.Common.DataConverter.CDouble(Eval("AllMoney")).ToString("F2") %></td>
                        <td><%#ShowLastTime() %></td>
                        <td>
                            <a class="btn btn-info btn-sm" href="ClientInfo.aspx?id=<%#Eval("UserID") %>">详情</a>
                            <a class="btn btn-info btn-sm" href="/Extend/Order/FastOrder.aspx?UseID=<%#Eval("UserID") %>">开单</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></FooterTemplate>
            </ZL:Repeater>
    </table>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
   <script runat="server">
       public string ShowStatus()
       {
           string ok = "<span style='color:green;'>正常</span>";
           string err = "<span style='color:red;'>禁用</span>";
           return Convert.ToInt32(Eval("status")) == 0 ? ok : err;
       }
       public string ShowLastTime()
       {
           string time = Eval("UpdateTime","");
           if (string.IsNullOrEmpty(time)) { return "无记录"; }
           else{ return ZoomLa.Common.DataConverter.CDate(time).ToString("yyyy/MM/dd HH:mm"); }
       }
   </script>
</asp:Content>