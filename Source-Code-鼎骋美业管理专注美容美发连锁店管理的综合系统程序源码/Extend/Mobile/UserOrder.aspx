<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserOrder.aspx.cs" Inherits="Extend_Mobile_UserOrder" MasterPageFile="~/Extend/Common/Wechat.master"%>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>用户订单</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<asp:HiddenField runat="server" ID="TotalSum_Hid" />
    <div class="orderlist" id="orderlist">
            <div runat="server" id="empty_div" visible="false" class="alert alert-info margin_t5">没有匹配的订单信息</div>
            <ZL:Repeater ID="Order_RPT" runat="server"  PagePre="<div class='clearfix'></div><div class='text-center foot_page'><label class='pull-left'><input type='checkbox' id='chkAll'/>全选</label>" PageEnd="</div>"
                 OnItemDataBound="Order_RPT_ItemDataBound" OnItemCommand="Order_RPT_ItemCommand" PageSize="10" BoxType="dp">
                <ItemTemplate>
                    <div class="order-item">
                        <table class="table prolist">
                            <thead>
                            <tr class="tips_div">
                                <th class="orderinfo" colspan="8">
                                    <div style="display: inline-block; width: 1100px;font-size:12px;">
                                        <span><strong><label><input type="checkbox" name="idchk" value="<%#Eval("ID") %>" style="position:relative;top:2px;"/>编号：</strong><%#Eval("ID") %></label></span>
                                        <span><strong>订单号：</strong><a href="OrderListInfo.aspx?ID=<%#Eval("ID") %>" title="订单详情"><%#Eval("OrderNo") %></a></span>
                                        <span><strong>下单时间：</strong><%#Eval("AddTime") %></span>
                                        <span><strong>付款单号：</strong><%#GetPayInfo() %></span>
                                        <span><strong>发货时间：</strong><%#GetExpSTime() %></span>
                                        <span><strong>推荐人：</strong><%#GetPUserInfo() %></span>
                                    </div>
                                     <span style="font-size:16px;font-weight:bolder;">店铺：<asp:Label ID="Store_L" runat="server"/></span>
                                    <a href="javascript:;" title="收缩/展开" onclick="order.slideup(this);"><i class="fa fa-chevron-circle-down" style="font-size: 20px;"></i></a>
                                    <asp:LinkButton runat="server" CommandArgument='<%#Eval("ID") %>' CommandName="del2" Visible='<%#Filter.Equals("recycle")?false:true %>'
                                        class="pull-right btn btn-xs btn-danger margin_l5" OnClientClick="return confirm('确定要移入回收站吗');"><i class="fa fa-trash"></i> 删除订单</asp:LinkButton>
                                    <a href="Addon/ExpPrint.aspx?ID=<%#Eval("ID") %>" class="pull-right btn btn-xs btn-info <%#ZoomLa.Common.DataConverter.CLng(Eval("ExpressNum"))>0?"":"hidden" %>"><i class="fa fa-print"></i> 打印快递单</a>
                                </th>
                            </tr></thead>
                            <tbody class="prowrap">
                                <asp:Repeater ID="Pro_RPT" runat="server" EnableViewState="false" OnItemDataBound="Pro_RPT_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class="<%#Container.ItemIndex>4?"pro_more hidden":"" %>">
                                            <td style="text-align: left; border-right: none; border-left: none;">
                                                <span>
                                                    <a href="<%#GetShopUrl() %>" target="_blank">
                                                        <img src="<%#GetImgUrl() %>" onerror="shownopic(this);" class="img50" /></a>
                                                    <span><%#Eval("Proname") %></span>
                                                </span>
                                                <%#Eval("PClass","").Equals("2")?"<input type='button' class='btn btn-info' value='促销组合' onclick=\"order.showSuit('"+Eval("CartID")+"');\">":"" %>
                                            </td>
                                            <td class="td_md goodservice" style="border-left: none;"><%#GetRepair() %></td>
                                            <td class="td_md"><%#Eval("Shijia","{0:f2}") %></td>
                                            <td class="td_md gray9">x<%#Eval("Pronum") %></td>
                                            <asp:Literal runat="server" ID="Order_Lit" EnableViewState="false"></asp:Literal>
                                        </tr>
                                        <asp:Panel runat="server" Visible='<%#Container.ItemIndex==5?true:false %>'>
                                            <tr><td colspan="4" class="text-left" style="line-height:30px;height:30px;"><a href="javascript:;" onclick="order.showMore(this);" class="btn btn-info">查看更多商品 <i class="fa fa-chevron-right"></i></a></td></tr>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%#GetMessage() %>
                            </tbody>
                        </table>
                    </div>
                </ItemTemplate>
                <FooterTemplate></FooterTemplate>
            </ZL:Repeater>
     </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script">

</asp:Content>