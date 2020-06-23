<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreInfo.aspx.cs" Inherits="Extend_UA_StoreInfo" MasterPageFile="~/Common/Master/User.Master" %>
<%@ Register Src="~/Extend/Common/Top_StoreConfig.ascx" TagPrefix="ZL" TagName="Top_StoreConfig" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<title>站点信息</title>
<style>
.btn-info,.btn-primary { background:#EC6935; border-color:#EC6935;}
.btn-info:hover,.btn-primary:hover { background:#C7572A; border-color:#EC6935;}
</style>
<script type="text/javascript" charset="utf-8" src="/Plugins/Ueditor/ueditor.config.js"></script>
<script type="text/javascript" charset="utf-8" src="/Plugins/Ueditor/ueditor.all.min.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:Top_StoreConfig runat="server" ID="Top_StoreConfig" />
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User/Index">会员中心</a></li>
    <li class="breadcrumb-item">
        <a href="<%=Request.RawUrl %>">店铺主页</a>
    </li>
</ol>

    <table class="table table-striped table-bordered" style="margin-top: 10px;">
        <tr>
            <td class="text-right" style="width: 120px;">商铺名称：</td>
            <td>
                <input type="text" id="StoreName_T" name="StoreName_T" class="form-control text_md required" value="@Model.Title" />
                <span class="r_red">*</span>
            </td>
        </tr>
        <tr>
            <td class="text-right">商品风格样式：</td>
            <td>@Html.Partial("C_TemplateView", new C_TemplateView(styleDt) { IsFirstSelect = false })
            </td>
        </tr>
        @MvcHtmlString.Create(ViewBag.modelhtml)
                    <tr>
                        <td colspan="2" class="text-center">
                            <input type="submit" value="信息修改" class="btn btn-primary" />
                            <asp:Button runat="server" ID="Save_Btn" Text="信息修改" class="btn btn-info" OnClick="Save_Btn_Click" />
                            <input type="button" value="返回" class="btn btn-primary" onclick="javascript: history.go(-1)" />
                        </td>
                    </tr>
    </table>
         
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">
    <style type="text/css">
        .fd_tr_pictype .btn-group{display:none;}
    </style>
    <script src="/JS/DatePicker/WdatePicker.js"></script>
    <script src="/JS/Common.js"></script>
    <script src="/JS/Controls/ZL_Dialog.js"></script>
    <script src="/JS/ICMS/ZL_Common.js"></script>
    <script src="/JS/ZL_Content.js"></script>
    <script>
        function opentitle(url, title) {
            comdiag.reload = true;
            comdiag.maxbtn = false;
            ShowComDiag(url, title);
        }
        function closdlg() {
            CloseComDiag();
        }
        $(function () {
            $("#user8").addClass("active");
            $('#TempleID_Hid').val('@Model.DefaultSkins');
            initTlp();
            $("form").validate();
        })
    </script>
</asp:Content>