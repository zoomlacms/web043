<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopMenu.ascx.cs" Inherits="Extend_Mobile_TopMenu" %>
<ul class="topnav">
    <li id="nav0"><a href="/User/UserShop/ProductList">商品管理</a></li>
    <li id="nav1"><a href="/Extend/UA/MoneyRegular.aspx">充值管理</a></li>
    <li id="nav2"><a href="/Extend/UA/SuitPro.aspx">套卡管理</a></li>
    <div style="clear:both;"></div>
</ul>
<script>
    $("#nav<%=Active%>").addClass("active");
</script>