<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreList.aspx.cs" Inherits="Extend_Mobile_StoreList" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<link href="/dist/css/weui.min.css" rel="stylesheet" />
<title>店铺</title>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <div class="page">
        <div class="page__bd">
            <div class="weui-panel__bd">
                <div class="weui-panel weui-panel_access">
                    <asp:Repeater runat="server" ID="RPT">
                        <ItemTemplate>
                            <a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">
                                <div class="weui-media-box__hd">
                                    <img class="weui-media-box__thumb" src="<%#ZoomLa.Common.function.GetImgUrl(Eval("Logo")) %>" onerror="shownopic(this);">
                                </div>
                                <div class="weui-media-box__bd">
                                    <h4 class="weui-media-box__title"><%#Eval("Title") %></h4>
                                    <p class="weui-media-box__desc"><%#Eval("addr") %></p>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script"></asp:Content>