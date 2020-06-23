<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MCardAdd.aspx.cs" Inherits="Extend_UC_MCardAdd" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>会员卡</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
    <li class="breadcrumb-item"><a href="MCard.aspx">会员卡列表</a></li>
    <li class="breadcrumb-item">
        <a href="<%=Request.RawUrl %>">会员卡</a>
        [<a href="/Extend/ClientAdd.aspx">添加会员</a>]
    </li>
</ol>
<table class="table table-bordered">
    <tr>
        <td class="td_m">会员</td>
        <td>
           <div class="input-group">
               <input type="text" class="form-control text_300" disabled="disabled" value="<%:suMod.HoneyName %>"/>
               <span class="input-group-btn">
                   <input type="button" value="选择会员" class="btn btn-info" onclick="showMember();"/>
               </span>
           </div>
        </td>
    </tr>
    <tr><td>会员卡号</td><td><asp:Label runat="server" ID="CardNo"></asp:Label></td></tr>
    <tr>
        <td>现有余额</td>
        <td><asp:Label runat="server" ID="CardPurse"></asp:Label></td>
    </tr>
    <tr runat="server" id="op_tr2"><td>充值金额</td>
        <td>
            <asp:Repeater runat="server" ID="Regular_RPT">
                <ItemTemplate>
                    <div>
                        <label><input type="radio" name="regular_rad" value="<%#Eval("ID") %>"/> <%#Eval("Alias") %>(<span class="r_red"><%#Eval("Min","{0:f2}") %></span>)</label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div>
               <label><input type="radio" name="regular_rad" value="0"/> <input type="text" placeholder="手动输入充值金额,必须为整数" class="form-control text_300" id="money_t"/></label>
            </div>
        </td>
    </tr>
    <tr runat="server" id="op_tr"><td></td>
        <td><input type="button" class="btn btn-info" value="收款" onclick="showCash();"/></td>
    </tr>
</table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/Modal/APIResult.js"></script>
<script>
    var diag = new ZL_Dialog();
    diag.maxbtn = false;
    diag.backdrop = true;
    diag.width = "width1024";
    function showMember() { diag.ShowModal("/Extend/Common/SelClient.aspx", "选择会员"); }
    function selMember(user) {
        location = "/Extend/UC/MCardAdd.aspx?ID=" + user.UserID;
    }
    function showCash() {
        //生成充值订单,并进入支付界面
        var postData = { uid: "<%:client.UserID%>" };
        postData.regular = Convert.ToInt($("input[name='regular_rad']:checked").val());
        postData.money = Convert.ToInt($("#money_t").val());
        if (postData.regular == 0 && postData.money < 1) { alert("未选择充值金额"); return false; }
        $.post("/Extend/Common/API.ashx?action=order_recharge", postData, function (data) {
            var model = APIResult.getModel(data);
            if (APIResult.isok(model)) { diag.ShowModal("/Extend/Order/Cash.aspx?oid=" + model.result, "-"); }
            else { console.log(model.retmsg); }
           
        })
    }
    function notify(cmd) {
        switch (cmd) {
            case "cash_finish":
                diag.CloseModal();
                location = location;
                break;
        }
    }
</script>
</asp:Content>
