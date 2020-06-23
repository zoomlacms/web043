<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="Extend_Mobile_UserInfo" MasterPageFile="~/Extend/Common/Wechat.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<title>会员</title>
<style type="text/css">
.weui-cell__bd p { margin-bottom:0;}
.weui-cells a  { text-decoration:none;}
.user_top { height:14em; background:linear-gradient(240deg, #26242F 0%, #4F4C5B 100%) !important;}
.user_top_circle { width: 100%; height: 10em; position: relative; overflow: hidden;}
.user_top_circle_i { position: absolute; bottom: 0; width: 1000px; height: 1000px; background-repeat: no-repeat; background-position: bottom center; background-size: 50% 168px; left: 50%; -webkit-transform-origin: center center; -ms-transform-origin: center center; transform-origin: center center; -webkit-transform: translateX(-50%); -ms-transform: translateX(-50%); transform: translateX(-50%); background:url(/Template/DCMY/style/images/usertop.png); border-radius:100%;}
.user_top_i { position:absolute; padding-top:2em; left:1em; right:1em; bottom:1em; top:2em; height:12em; box-shadow:0 2px 4px 0 rgba(0, 0, 0, 0.1); border-radius:0.5em; background:#FFF;}
.user_top_i img { width:6em; border-radius:50%; border:1px solid #999;}
.user_top_i p { margin-bottom:0; font-size:1.5em; height:2em; line-height:2em;}
.nav-item span{font-weight:bolder;font-size:16px;}
body { background:#f4f4f4;}
</style>
<%--<link href="/dist/css/bootstrap4.min.css" rel="stylesheet" />
<script src="/dist/js/popper.min.js"></script>
<script src="/dist/js/bootstrap4.min.js"></script>--%>
<link href="/dist/css/bootstrap.min.css" rel="stylesheet" />
<script src="/dist/js/bootstrap.min.js"></script>
<link type="text/css" rel="stylesheet" href="/App_Themes/Guest.css" />  
<link href="/Template/DCMY/style/global.css" rel="stylesheet"/>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">

<div class="d-block d-sm-none">
        <div class="user_top">
            <div class="user_top_i">
                <img src="<%=mu.UserFace %>" onerror="shownoface(this);" />
                <p><%:mu.HoneyName %></p>
            </div>
        </div>
        <div class="weui-cells">
            <a class="weui-cell weui-cell_access" href="UserOrder.aspx">
                <div class="weui-cell__bd">
                    <p>我的订单</p>
                </div>
                <div class="weui-cell__ft">查看全部订单</div>
            </a>
        </div>
        <div class="user_c">
            <ul>
                <li><a href="#"><i class="fa fa-cart-plus"></i><p>待付款</p></a></li>
                <li><a href="#"><i class="fa fa-truck"></i><p>待收货</p></a></li>
                <li><a href="#"><i class="fa fa-check"></i><p>已收货</p></a></li>
                <li><a href="#"><i class="fa fa-comments-o"></i><p>售后</p></a></li>
                <div class="clearfix"></div>
            </ul>
            <ul>
                <li><a href="javascript:;"><span><%:mu.Purse.ToString("F2") %></span><p>账户余额</p></a></li>
                <li><a href="#"><span>0</span><p>可用积分</p></a></li>
                <li><a href="#"><span>0</span><p>待定积分</p></a></li>
                <li><a href="#"><span>0</span><p>小金库</p></a></li>
                <div class="clearfix"></div>
            </ul>
        </div>
        <%--        <div class="weui-cells">
                    <a class="weui-cell weui-cell_access" href="/User/Info/UserRecei">
                        <div class="weui-cell__bd"><p><i class="fa fa-map-marker"></i> 收货地址</p></div>
                        <div class="weui-cell__ft"></div>
                    </a>
                </div>--%>
        <div class="weui-cells">
            <a class="weui-cell weui-cell_access" href="tel:021-88888888">
                <div class="weui-cell__bd"><p><i class="fa fa-phone"></i> 联系我们：021-88888888</p></div>
                <div class="weui-cell__ft"></div>
            </a>
            <a class="weui-cell weui-cell_access" href="javascript:;">
                <div class="weui-cell__bd"><p><i class="fa fa-user"></i> 关于鼎骋美业</p></div>
                <div class="weui-cell__ft"></div>
            </a>
        </div>
        
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script"></asp:Content>