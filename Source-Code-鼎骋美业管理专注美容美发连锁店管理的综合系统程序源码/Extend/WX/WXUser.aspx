<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WXUser.aspx.cs" Inherits="Extend_WX_WXUser" MasterPageFile="~/Common/Master/User.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>微信用户</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
       <ol class="breadcrumb">
           <li class="breadcrumb-item"><a title="会员中心" href="/User/Index/Default">会员中心</a></li>
           <li class="breadcrumb-item"><a href="Default.aspx">微信管理</a></li>
           <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">微信用户</a></li>
       </ol>
<div class="input-group">
    <asp:TextBox runat="server" ID="Key_T" class="form-control" placeholder="请输入用户名称" />
    <span class="input-group-btn">
        <asp:Button ID="Search_B" CssClass="btn btn-default" runat="server" Text="搜索" OnClick="Search_B_Click" />
    </span>
</div>
<div style="margin-top:5px;">
<ZL:ExGridView ID="EGV" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" EnableTheming="False" IsHoldState="false"
                class="table table-striped table-bordered" EmptyDataText="当前没有信息!!"  OnPageIndexChanging="EGV_PageIndexChanging" >
        <Columns>
            <asp:TemplateField HeaderText="用户头像">
                <ItemTemplate>
                    <img class="img50" src="<%#Eval("HeadImgUrl") %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="用户名">
                <ItemTemplate><span class="name"><%#Eval("Name") %></span></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CMS用户信息">
                <ItemTemplate>
                    <a href="javascript:;" onclick="user.showuinfo('<%#Eval("UserID") %>');"><%#Eval("UserName") %> </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="OpenID">
                <ItemTemplate><%#Eval("OpenID") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="性别">
                <ItemTemplate>
                    <%#GetSexIcon() %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="更新时间">
                <ItemTemplate>
                    <span class="cdate"><%#Eval("CDate") %></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <a href="javascript:;" title="更新用户信息" data-oid="<%#Eval("OpenID") %>" class="option_style wxoption"><span class="fa fa-refresh"></span>更新用户信息</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ZL:ExGridView>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/Controls/ZL_Dialog.js"></script>
<script>
        $().ready(function () {
            $(".wxoption").click(function () {
                UpdateUser($(this).data('oid'));
            });
        });
        function UpdateUser(openid) {
            $(".wxoption[data-oid='" + openid + "'] span").addClass("fa-spin");
            var $tr = $(".wxoption[data-oid='" + openid + "']").closest('tr');
            $.ajax({
                type: 'POST',
                data: { action: 'update', openid: openid, appid: '<%=AppId %>' },
                success: function (data) {
                    if (data == '-1') {
                        $tr.remove();
                        return;
                    }
                    var obj = JSON.parse(data);
                    $tr.find('.imgurl').attr('src', obj.HeadImgUrl);
                    $tr.find('.name').text(obj.Name);
                    $tr.find('.sex').attr('class', obj.Sex == 1 ? 'fa fa-male' : 'fa fa-female');
                    $tr.find('.groupid').text(GetGroupName(obj.Groupid));
                    $(".wxoption[data-oid='" + openid + "'] span").removeClass("fa-spin");
                }
            });
        }
        //用户组名(暂时静态处理)
        function GetGroupName(groupid) {
            switch (groupid) {
                case 0:
                    return "未分组";
                case 1:
                    return "黑名单";
                case 2:
                    return "星标组";
                default:
                    return "";
            }
        }
    </script>
</asp:Content>