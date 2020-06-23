<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="Extend_Order_OrderList" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>订单列表</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
<asp:HiddenField runat="server" ID="TotalSum_Hid" />
    <div class="orderlist" id="orderlist">
            <div runat="server" id="empty_div" visible="false" class="alert alert-info margin_t5">没有匹配的订单信息</div>
            <ZL:Repeater ID="Order_RPT" runat="server"  PagePre="<div class='clearfix'></div><div class='text-center foot_page'>" PageEnd="</div>"
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
                                    </div>
                                </th>
                            </tr></thead>
                            <tbody class="prowrap">
                                <asp:Repeater ID="Pro_RPT" runat="server" EnableViewState="false" OnItemDataBound="Pro_RPT_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class="<%#Container.ItemIndex>4?"pro_more hidden":"" %>"  style="font-size:12px;">
                                            <td style="text-align: left; border-right: none; border-left: none;">
                                                <span>
                                                    <a href="<%#GetShopUrl() %>" target="_blank">
                                                        <img src="<%#GetImgUrl() %>" onerror="shownopic(this);" class="img50" /></a>
                                                    <span>
                                                            <%#Eval("Proname") %>
                                                        (技师:<span class="r_red"><%#Eval("Attribute") %></span>)
                                                    </span>
                                                </span>
                                            </td>
<%--                                            <td class="td_md goodservice" style="border-left: none;"><%#GetRepair() %></td>--%>
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
<style type="text/css">
.table {margin-bottom:0px;}
.rd_red_l{color:red;font-size:1.5em;}
.orderlist td,.orderlist th{font-size:14px;font-family:'Microsoft YaHei';}
.orderlist .order-item{border:1px solid #ddd;border-top:none;font-size:14px;font-family:'Microsoft YaHei';}
.orderlist .gray9{color:#999;}
.orderlist .orderinfo { line-height:20px;}
/*.orderlist .shopinfo{line-height:20px;}*/
.orderlist .opinfo {line-height:20px;text-align:right;padding-right:15px;color:gray;}
.orderlist .tips_div{background-color:#f5f5f5;font-weight:normal;}
.orderlist .tips_div span{margin-right:5px;font-weight:normal;}
.orderlist .top_div{line-height:30px; background-color:#f5f5f5;margin-top:10px;}
.orderlist .prolist td{ line-height:70px; border-left:1px solid #ddd;height:70px;text-align:center;}
.orderlist .prolist td:last-child{border-right:none;}
.orderlist .proname div{padding:5px;}
.orderlist .goodservice {text-align:right;padding-right:20px;}
.orderlist .prolist .rowtd {line-height:20px;padding-top:30px}
.orderlist .order_navs{position:relative;}
.orderlist .search_div{position:absolute;right:0px;top:3px;}
.orderlist .ordertime{color:#999;}
.orderlist .order_bspan { font-size:1em;}
.orderlist .idcmessage{color:#999;line-height:normal;margin:0;}
.orderlist .idm_tr td{line-height:normal;height:auto;text-align:left;}
</style>
</asp:Content>