<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductAdd.aspx.cs" Inherits="Extend_UA_ProductAdd" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
.fd_td_l {width:120px; }
.proclass_tab {display:none;}
#cbind_btn{display:none;}
</style>
<script charset="utf-8" src="/Plugins/Ueditor/ueditor.config.js"></script>
<script charset="utf-8" src="/Plugins/Ueditor/ueditor.all.js"></script>
<link type="text/css" href="/dist/css/bootstrap-switch.min.css" rel="stylesheet" />
<title>编辑商品</title>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <ol class="breadcrumb">
<li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
<li class="breadcrumb-item"><a href="/User/UserShop/ProductList">商品列表</a></li>
<li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">商品管理</a></li>
</ol>
<ul class="nav nav-tabs">
    <li class="nav-item">
        <a class="nav-link active" href="#Tabs0" data-toggle="tab">基本信息</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="#Tabs1" data-toggle="tab">商品说明</a>
    </li>
</ul>
<div class="tab-content panel-body padding0">
    <div id="Tabs0" class="tab-pane active manage_content">
        <table class="table table-striped table-bordered">
                <tbody>
                    <tr>
                        <td class="td_m"><strong>商品类型：</strong></td>
                        <td>
                            <asp:Repeater runat="server" ID="Node_RPT">
                               <ItemTemplate>
                                   <label><input type="radio" name="node_rad" value="<%#Eval("NodeID") %>"/><%#Eval("NodeName") %></label>
                               </ItemTemplate>
                            </asp:Repeater>
                             <asp:HiddenField runat="server" ID="ClickType" />
                        </td>
                    </tr>
                    <tr>
                        <td><strong>商品编号：</strong></td>
                        <td>
                            <asp:TextBox ID="ProCode" runat="server" class="form-control text_300" /></td>
                    </tr>
                                        <tr>
                      <td><strong>条形码：</strong></td>
                        <td>
                            <asp:TextBox ID="BarCode" runat="server" class="form-control text_300 num nofocus" /></td>
                    </tr>
                    <tr>
                        <td><strong>商品名称：<span class="r_red">*</span></strong></td>
                        <td>
                            <asp:TextBox ID="Proname" runat="server" class="form-control text_500" onkeyup="isgoEmpty('Proname','span_Proname');"/>
                            <span class="rd_red">*<asp:RequiredFieldValidator ID="RV1" runat="server" ControlToValidate="Proname" Display="Dynamic" ErrorMessage="商品名称不能为空!" SetFocusOnError="True" /></span>
                            <span id="span_Proname"></span><span><span class="vaild_tip"></span></span>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>商品单位：</strong></td>
                        <td>
                            <asp:TextBox ID="ProUnit" runat="server" class="form-control text_md" Text="件" />
                            <span class="rd_red">
                                <asp:RequiredFieldValidator ID="RV2" runat="server" ControlToValidate="ProUnit" Display="Dynamic" ErrorMessage="商品单位不能为空!" SetFocusOnError="True" /></span>
                            <div id="Unitd" class="btn btn-group">
                                <button type="button" class="btn btn-default">件</button>
                                <button type="button" class="btn btn-default">个</button>
                                <button type="button" class="btn btn-default">只</button>
                                <button type="button" class="btn btn-default">组</button>
                                <button type="button" class="btn btn-default">套</button>
                                <button type="button" class="btn btn-default">把</button>
                                <button type="button" class="btn btn-default">双</button>
                                <button type="button" class="btn btn-default">台</button>
                                <button type="button" class="btn btn-default">年</button>
                                <button type="button" class="btn btn-default">月</button>
                                <button type="button" class="btn btn-default">日</button>
                                <button type="button" class="btn btn-default">季</button>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_l"><strong>标签价格：<span class="rd_red">*</span></strong></td>
                        <td>
                            <div class="input-group text_s">
                                <asp:TextBox ID="ShiPrice" runat="server" Text="0" class="form-control text_s" autocomplete="off" /><span class="input-group-addon">元</span>
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="ShiPrice" ErrorMessage="价格格式不对!" ValidationExpression="\d+[.]?\d*" Display="Dynamic" SetFocusOnError="True" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ShiPrice" ErrorMessage="价格不能为空!" SetFocusOnError="True" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td><strong>零售价：<span class="rd_red">*</span></strong></td>
                        <td>
                            <div class="input-group text_s">
                                <asp:TextBox ID="LinPrice" runat="server" class="form-control text_s" autocomplete="off" /><span class="input-group-addon">元</span>
                            </div>
                            <asp:RegularExpressionValidator ID="RV14" runat="server" ControlToValidate="LinPrice" ErrorMessage="零售价格式不对!" ValidationExpression="\d+[.]?\d*" Display="Dynamic" SetFocusOnError="True" />
                            <asp:RequiredFieldValidator ID="RV4" runat="server" ControlToValidate="LinPrice" Display="Dynamic" ErrorMessage="零售价不能为空!" SetFocusOnError="True" />
                        </td>
                    </tr>
                    <tr>
                        <td><strong>折扣优惠:</strong></td>
                        <td>
                            <div class="input-group text_s">
                                <asp:TextBox ID="Recommend_T" runat="server" Text="0" class="form-control text_s num" /><span class="input-group-addon">%</span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>销售状态：</strong></td>
                        <td>
                            <asp:CheckBox ID="Sales_Chk" Text="打勾表示销售中，否则为停售状态" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><strong>属性设置：</strong></td>
                        <td>
                            <asp:CheckBox ID="istrue_chk" runat="server" Text="审核通过" Checked="true" />
                            <asp:CheckBox ID="isnew_chk" runat="server" Text="新品" />
                            <asp:CheckBox ID="ishot_chk" runat="server" Text="热销" />
                            <asp:CheckBox ID="isbest_chk" runat="server" Text="精品" />
                        </td>
                    </tr>
                     <asp:Literal ID="ModelHtml" runat="server"></asp:Literal>
                </tbody>
        </table>
    </div>
    <div id="Tabs1" class="tab-pane">
        <table class="table table-striped table-bordered">
            <tbody>
                    <tr>
                        <td class="td_l"><strong>商品简介：</strong></td>
                        <td>
                            <asp:TextBox ID="Proinfo" runat="server" TextMode="MultiLine"  class="form-control m715-50" style="height:70px;" placeholder="用于首页及栏目页显示，最多255个字符" />
                        </td>
                    </tr>
                    <tr>
                        <td><strong>详细介绍：</strong></td>
                        <td>
                            <textarea id="procontent" style="width: 715px; height: 300px;" runat="server"></textarea>
                            <%=Call.GetUEditor("procontent",3) %>
                        </td>
                    </tr>
                <%--    <tr id="addsmallimg">
                        <td><strong>商品缩略图：</strong></td>
                        <td>
                            <asp:TextBox ID="txt_Thumbnails" runat="server" class="form-control text_300" />
                            <iframe id="smallimgs" style="top: 2px; width: 100%; height: 25px;" src="/Common/fileupload.aspx?FieldName=Thumbnails&ModelID=<%:ModelID %>&NodeID=<%:NodeID %>" frameborder="0" marginheight="0" marginwidth="0" scrolling="no"></iframe>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>商品清晰图：</strong></td>
                        <td>
                            <asp:TextBox ID="txt_Clearimg" runat="server" class="form-control text_300" />
                            <iframe id="bigimgs" style="top: 2px; width: 100%; height: 25px;" src="/Common/fileupload.aspx?FieldName=Clearimg&ModelID=<%:ModelID %>&NodeID=<%:NodeID %>" frameborder="0" marginheight="0" marginwidth="0" scrolling="no"></iframe>
                        </td>
                    </tr>--%>
                    <tr>
                        <td><strong>点击数：</strong></td>
                        <td>
                            <asp:TextBox ID="AllClickNum_T" runat="server" class="form-control  text_300" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td><strong>发布时间：</strong></td>
                        <td>
                            <asp:TextBox ID="AddTime" runat="server" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd HH:mm' });" class="form-control  text_300" /></td>
                    </tr>
                    <tr>
                        <td><strong>更新时间：</strong></td>
                        <td>
                            <asp:TextBox ID="UpdateTime" runat="server" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd HH:mm' });" class="form-control  text_300" /></td>
                    </tr>
                </tbody>
        </table>
    </div>
</div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
<div class="text-center">
    <input type="button" class="btn btn-primary" id="submit_btn" value="保存商品信息" onclick="PreSubmit();" />
    <asp:Button ID="EBtnSubmit" runat="server" OnClick="EBtnSubmit_Click" Style="display: none;" />
    <asp:Button ID="btnAdd" class="btn btn-primary" Text="添加为新商品" runat="server" OnClick="btnAdd_Click" />
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<style type="text/css">
.rd_red{color:red;}
</style>
<script src="/dist/js/bootstrap-switch.js"></script>
<script src="/JS/Common.js"></script>
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script src="/JS/Controls/ZL_Array.js"></script>
<script src="/JS/ZL_Regex.js"></script>
<script src="/JS/ZL_Content.js"></script>
<script src="/JS/Modal/shop.js?v=3"></script>
<script src="/JS/Modal/APIResult.js"></script>
<script src="/JS/OAKeyWord.js"></script>
<script src="/JS/SelectCheckBox.js"></script>
<script>
    $(function () {
        ZL_Regex.B_Num(".num");
        $("#Unitd").find("button").click(function () { $("#ProUnit").val($(this).text()); });
        //----模板信息
        Tlp_initTemp();
    });
    var diag = new ZL_Dialog();
//生产商
var producer = {};
producer.sel = function () {
    comdiag.width = "width960";
    ShowComDiag("Producerlist.aspx", "选择厂商");
}
producer.selback = function ()
{

}
//品牌商标
var trademark = {};
trademark.sel = function () { ShowComDiag("Brandlist.aspx?producer=",""); }
trademark.selback = function () { }
function CloseDiag() {
    diag.CloseModal();
    comdiag.CloseModal();
}

function SetStock(pid) {
    diag.url = "Stock/Stock.aspx?action=addpro&pid=<%=ProID %>";
    diag.title = "库存管理";
    diag.maxbtn = false;
    diag.reload = true;
    diag.backdrop = true;
    diag.ShowModal();
}
function addStock(pronum) {
    CloseDiag();
    var num = parseInt($("#Stock").val());
    $("#Stock").val(pronum + num);
}
var present = { scope: { presave: function () { }}, $hid: $("#present_hid") };
UserFunc = null;
</script>
</asp:Content>