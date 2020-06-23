<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WXConfig.aspx.cs" Inherits="Extend_WXConfig" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Manage/I/ASCX/SFileUp.ascx" TagPrefix="ZL" TagName="SFileUp" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>微信配置</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
   <div>
                <ol class="breadcrumb">
            <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
            <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">微信配置</a></li>
        </ol>
    <table class="table table-bordered table-striped">
        <tr><td class="td_m">微信号</td>
            <td><ZL:TextBox runat="server" ID="WXNo_T" AllowEmpty="false" class="form-control text_300"/></td></tr>
        <tr><td >APPID</td>
            <td><ZL:TextBox runat="server" ID="AppID_T" AllowEmpty="false"  class="form-control text_300"/></td>
        </tr>
        <tr><td >Secret</td>
            <td><ZL:TextBox runat="server" ID="Secret_T" AllowEmpty="false"  class="form-control text_300"/></td>
        </tr>
          <tr><td >原始ID</td>
            <td><ZL:TextBox runat="server" ID="OrginID" AllowEmpty="false"  class="form-control text_300"/></td>
        </tr>
          <tr><td >二维码图片</td>
            <td>
                <ZL:SFileUp runat="server" ID="QCode_UP" FType="Img" />
            </td>
        </tr>
          <tr><td >商户编号</td>
            <td><ZL:TextBox runat="server" ID="Pay_Account" AllowEmpty="false"  class="form-control text_300"/></td>
        </tr>
        <tr><td></td><td><asp:Button runat="server" ID="Save_Btn" Text="保存信息" class="btn btn-info" OnClick="Save_Btn_Click"/></td></tr>
    </table>
   </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent"></asp:Content>