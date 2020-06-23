<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="Tools_test" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ul class="type_ul">
        <li class="btn btn-info">美食</li>
        <li class="btn btn-info">超市</li>
        <li class="btn btn-info">鲜果购</li>
        <li class="btn btn-info">甜点饮点</li>
        <li class="btn btn-info">土豪馆</li>
        <li class="btn btn-info">美团专送</li>
        <li class="btn btn-info">鲜花蛋糕</li>
        <li class="btn btn-info">送药上门</li>
    </ul>
<style type="text/css">
    .type_ul li { float: left; width: 110px; margin-right:5px;margin-bottom:5px;width:80px;height:80px;border-radius:50%;padding-top:30px; }
</style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script"></asp:Content>