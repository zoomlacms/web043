<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientInfo.aspx.cs" Inherits="Extend_ClientInfo" MasterPageFile="~/Common/Master/User.Master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>客户信息</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<div>
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
             <li class="breadcrumb-item"><a href="Client.aspx">客户列表</a></li>
            <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">客户信息</a></li>
        </ol>
<table class="table table-bordered table-striped">
    <tr>
        <td class="td_m">姓名</td><td><ZL:TextBox runat="server" ID="HoneyName_T" class="form-control" AllowEmpty="false"/></td>
        <td class="td_m">生日</td><td><ZL:TextBox runat="server" ID="BirthDay_T" class="form-control" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });" /></td>
        <td class="td_m">注册时间</td><td><%:suMod.CDate.ToString() %></td>
    </tr>
    <tr>
        <td>手机号</td><td><ZL:TextBox runat="server" ID="Mobile_T" ValidType="MobileNumber"  class="form-control"/></td>
        <td>微信号</td><td><ZL:TextBox runat="server" ID="WXNo_T"  class="form-control"/></td>
        <td>最后消费</td><td><%:userMod.UpdateTime %></td>
    </tr>
    <tr><td>会员卡余额</td><td><%:userMod.Purse.ToString("F2") %></td>
        <td>会员等级</td><td><%:ZoomLa.Sns.ExHelper.User_GetLevel(suMod.UserLevel) %></td>
        <td>会员卡状态</td>
        <td>
            <label><input type="radio" name="CardStatus_rad" value="0" />启用</label>
            <label><input type="radio" name="CardStatus_rad" value="-1" />禁用</label>
        </td>
    </tr>
    <tr><td>客户标签</td><td colspan="5">
         <div id="OAkeyword"></div>
        <asp:TextBox ID="Keywords" runat="server" CssClass="form-control" />
        <asp:HiddenField runat="server" ID="IgnoreKey_Hid" />
        <div>(空格或回车键分隔，长度不超过10字符或5汉字)</div>

                     </td></tr>
    <tr><td>客户等级</td><td colspan="5">
        <label><input type="radio" name="UserLevel_rad" value="0" />银卡</label>
        <label><input type="radio" name="UserLevel_rad" value="1" />金卡</label>
        <label><input type="radio" name="UserLevel_rad" value="2" />白金卡</label>
        <label><input type="radio" name="UserLevel_rad" value="3" />钻石卡</label>
    </td>
    </tr>
    <tr>
        <td></td>
        <td colspan="5">
            <asp:Button runat="server" ID="Save_Btn" Text="保存信息" OnClick="Save_Btn_Click" class="btn btn-info btn-sm"/>
        </td>
    </tr>
    </table>
<ul class="nav nav-tabs">
    <li class="nav-item">
        <a class="nav-link active" href="#Tabs0" data-toggle="tab">订单</a>
    </li>
  <%--  <li class="nav-item">
        <a class="nav-link" href="#Tabs1" data-toggle="tab">会员卡</a>
    </li>--%>
</ul>
<div class="tab-content panel-body padding0">
    <div id="Tabs0" class="tab-pane active">
        <iframe src="/Extend/Order/OrderList.aspx?cuid=<%:suMod.UserID %>" style="border:none;width:100%;height:700px;"></iframe>
    </div>
</div>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/SelectCheckBox.js"></script>
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script src="/JS/OAKeyWord.js"></script>
<style type="text/css">
 #OAkeyword{ min-height:36px; border:1px solid #ccc; display:inline-block;min-width:300px;padding-top:3px;}
 #Keywords {display: none;}
</style>
<script>
    if ($("#OAkeyword").length > 0) { $("#OAkeyword").tabControl({ maxTabCount: 20, tabW: 80 }, $("#Keywords").val()); }//关键词
</script>
</asp:Content>