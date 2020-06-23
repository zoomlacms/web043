<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelClient.aspx.cs" Inherits="Extend_Common_SelClient" MasterPageFile="~/Common/Master/UserEmpty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>选择会员</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <div class="input-group">
        <asp:TextBox runat="server" ID="UName_T"  class="form-control" placeholder="请输入会员名或手机号"/>
        <asp:TextBox runat="server" ID="Label_T" class="form-control" placeholder="多标签用空隔分割" style="border-left:none;"/>
        <span class="input-group-btn">
            <asp:Button runat="server" ID="Skey_Btn" Text="搜索" OnClick="Skey_Btn_Click" class="btn btn-info" style="border-radius:unset;"/>
        </span>
    </div>
 <%--   <div style="margin-top:5px;">
      <label class="btn btn-light"><input type="checkbox" />余额超过1万</label>
      <label class="btn btn-light"><input type="checkbox" />半年未消费</label>
    </div>--%>
    <table id="EGV" class="table table-striped table-bordered" style="margin-top:5px;">
            <tr>
                <td>姓名</td>
                <td>手机号</td>
                <td>微信名称</td>
                <td>会员标签</td>
                <td>操作</td>
            </tr>
            <ZL:Repeater runat="server" ID="RPT" PageSize="20" BoxType="dp" PagePre="<tr><td><label><input type='checkbox' id='chkAll'/></label></td><td colspan='13'><div class='text-center'>" PageEnd="</div></td></tr>">
                <ItemTemplate>
                    <tr>
                        <td>
                           <label>
                               <input type="checkbox" name="idchk" value="<%#Eval("UserID") %>" data-model='{"UserID":"<%#Eval("UserID") %>","UserName":"<%#Eval("HoneyName") %>","Mobile":"<%#Eval("Mobile") %>"}'/>
                               <%#Eval("HoneyName") %>
                           </label>
                        </td>
                        <td><%#Eval("Mobile") %></td>
                        <td><%#Eval("WXName","") %></td>
                        <td><%#Eval("UserLabel") %></td>
                        <td><a href="javascript:;" class="btn btn-info single_btn"
                            onclick="sure({UserID:'<%#Eval("UserID") %>',UserName:'<%#Eval("HoneyName") %>',Mobile:'<%#Eval("Mobile") %>'});">选择</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></FooterTemplate>
            </ZL:Repeater>
        </table>
    <div style="margin-top:5px;display:none;" id="multi_div">
       <button type="button" class="btn btn-info" onclick="surelist();">确定选择</button>
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
<script src="/JS/SelectCheckBox.js"></script>
<script>
    function sure(user) {
        if (parent) { parent.selMember(user); }
    }
    function surelist()
    {
        var $chks = $("input[name='idchk']:checked");
        var list = [];
        for (var i = 0; i < $chks.length; i++) {
            var model = $($chks[i]).data("model");
            list.push(model);
        }
        parent.selMember(list);
    }
    //启用多选,关闭单选
    function enableMulti() {
        $("#multi_div").show();
        $(".single_btn").hide();
    }
    $(function () {
        $("#chkAll").click(function () {
            selectAllByName(this, "idchk");
        });
    })
</script>
</asp:Content>