<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Extend_WX_Default" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>微信管理</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ol class="breadcrumb">
        <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
        <li class="breadcrumb-item"><a href="/Extend/WX/Default.aspx">微信管理</a> [<a href="ClientAdd.aspx">添加客户</a>]</li>
    </ol>
    <div id="MainApp" class="wxapp_box">
        <div class="wxapp">
            <a href="WXConfig.aspx"><i class="fa fa-cog center-block"></i>微信配置</a>
        </div>
        <div class="wxapp">
            <a href="WelPage.aspx"><i class="fa fa-comment center-block"></i>欢迎语</a>
        </div>
        <div class="wxapp">
            <a href="WXUser.aspx"><i class="fa fa-user center-block" style="background: #73B9FF;"></i>微信用户</a>
        </div>
        <div class="wxapp">
            <a href="MsgTlpList.aspx"><i class="fa fa-newspaper-o center-block" style="background: #FFC926;"></i>消息推送</a>
        </div>
        <div class="wxapp">
            <a href="AutoReply.aspx"><i class="fa fa-comment center-block" style="background: #0085B2;"></i>自动回复</a>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<style type="text/css">
#MainApp{margin-top:5px;}
.wxapp{ float:left; width:160px; height:150px;text-align:center;padding:10px;} 
.wxapp a{ display:inline-block; text-align:center; color:#333;}
.wxapp a:hover{ text-decoration:none;}
.wxapp a i{ display:block; margin-bottom:5px; padding:10px; width:80px; height:80px; max-width:100%; background:#31b0d5; border-radius:10px; font-size:4em; color:#fff; box-shadow:0 0 3px 1px rgba(49,163,273,0.4);} 
.wxapp a i:hover{ font-size:4.2em; transition-duration:0.7s; box-shadow:0 0 5px 2px rgba(49,163,273,0.8); color:#ddd;}
.wxapp .wxapp_a::after{ content:"!"; margin-left:5px; font-weight:bold; color:#f00;}
</style>
</asp:Content>