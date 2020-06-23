<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreSale.aspx.cs" Inherits="Extend_Report_StoreSale" MasterPageFile="~/Common/Master/User.Master"%>
<%@ Register Src="~/Extend/Common/Top_Report.ascx" TagPrefix="ZL" TagName="ReportTop" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
<title>报表罗盘</title>
<style>
.top_02 {height:100px; margin:auto; margin-top:10px; overflow:hidden;background-color:#f7f7f7}
.top_02_item {width:25%;padding:10px 0px;height:100%;float:left}
.top_02_item div {font:15px 微软雅黑 #626262;text-align:center;height:49%;line-height:39px;border-right:1px solid #eeeeee  }
.Order{width:90%;margin:auto;overflow:hidden}
.Order div:first-child {margin-top:20px;font:16px; }
.echart50 {display:inline-block;width:48%;height:200px;}
.echart100{display:block;width:100%;height:300px;margin-top:20px;}
</style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Content">
<ZL:ReportTop runat="server" ID="ReportTop" />
<ol class="breadcrumb">
    <li class="breadcrumb-item"><a title="会员中心" href="/User">会员中心</a></li>
    <li class="breadcrumb-item"><a href="<%=Request.RawUrl %>">销售统计</a></li>
</ol>
<div style="background-color:#ddd;height:50px;padding:5px 8px;">
    <div class="input-group">
        <span class="input-group-addon">筛选</span>
        <asp:TextBox runat="server" ID="STime_T" class="form-control text_md" placeholder="起始时间" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });"/>
        <asp:TextBox runat="server" ID="ETime_T" class="form-control text_md" placeholder="结束时间" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd' });"/>
        <span class="input-group-btn">
            <asp:Button runat="server" ID="Filter_Btn" class="btn btn-info" Text="重新统计" OnClick="Filter_Btn_Click"/>
        </span>
    </div>
</div>
<div>
    <div class="top_02">
        <div class="top_02_item">
            <div>品项收入<span></span></div>
            <div><%:product_sale.ToString("F2") %></div>
        </div>
        <div class="top_02_item">
            <div>充值金额<span></span></div>
            <div><%:mcard_recharge.ToString("F2") %></div>
        </div>
        <div class="top_02_item">
            <div>耗卡金额<span></span></div>
            <div><%:mcard_consume.ToString("F2") %></div>
        </div>
        <div class="top_02_item">
            <div>总计收入<span></span></div>
            <div><%:(product_sale+mcard_recharge).ToString("F2") %></div>
        </div>
    </div>
    <div class="top_02">
        <div class="top_02_item">
            <div>新增会员数<span></span></div>
            <div><%:user_new_count %></div>
        </div>
        <div class="top_02_item">
            <div>发卡张数<span></span></div>
            <div><%:user_new_count %></div>
        </div>
        <div class="top_02_item">
            <div>消费数(人次)<span></span></div>
            <div><%:order_result.count %></div>
        </div>
        <div class="top_02_item">
            <div>客单价<span></span></div>
            <div><%:order_result.ticketSale.ToString("F2") %></div>
        </div>
    </div>

    <%--环形图--%>
    <div class="Order">
        <div id="chart1" class="echart50"></div>
        <div id="chart2" class="echart50"></div>
        <div id="line_chart" class="echart100"></div>
        <div id="bar_chart" class="echart100"></div>
    </div>
</div>
 
    
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
<script src="/JS/DatePicker/WdatePicker.js"></script>
<script src="/Plugins/ECharts/build/source/echarts.js"></script>
<script type="text/javascript">
    $("#nav0").addClass("active");
        //var myChart2 = echarts.init(document.getElementById('main2'));
        //pie,多类型销售
        var chart1_option = {
            title: {
                text: '销售统计',
                subtext: '',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ["品项销售", '会员充值']
            },
            series: [
                {
                    name: '销售统计',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: "<%:product_sale%>", name: '品项销售' },
                        { value: "<%:mcard_recharge%>", name: '会员充值' },
                    ],
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };
        var chart2_option = {
            title: {
                text: '顾客来源',
                subtext: '',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ["客户预约", '微信下单',"到店开单"]
            },
            series: [
                {
                    name: '顾客来源',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: [
                        { value: "10", name: '客户预约' },
                        { value: "20", name: '微信下单' },
                        { value: "50", name: '到店开单' }
                    ],
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };


        var line_chart_option = {
            backgroundColor: "#fff",
            title: {
                text: '销售分析'
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['销售额']
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                feature: {
                    saveAsImage: {}
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                //'周一', '周二', '周三', '周四', '周五', '周六', '周日'
                data: [<%=line_chart_xais%>]
            },
            yAxis: { type: 'value' },
            series: [
                {
                    name: '销售额',
                    type: 'line',
                    stack: '总量',
                    //120, 132, 101, 134, 90, 230, 210
                    data: [<%=line_chart_data%>]
                },
            ]
        };
    var bar_option = {
        title: {
            text: '销售统计',
            subtext: ''
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['销售统计']//, '会员充值'
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        toolbox: {
            show: true,
            feature: {
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                data: [<%=line_chart_xais%>]
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '销售统计',
                type: 'bar',
                data: [<%=line_chart_data%>],
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            },
      <%--      {
                name: '会员充值',
                type: 'bar',
                data: [<%=line_chart_data%>],
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                },
                markLine: {
                    data: [
                        { type: 'average', name: '平均值' }
                    ]
                }
            }--%>
        ]
    };


    var chart1 = echarts.init(document.getElementById("chart1"));
    var chart2 = echarts.init(document.getElementById("chart2"));
    var line_chart = echarts.init(document.getElementById("line_chart"));
    var bar_chart = echarts.init(document.getElementById("bar_chart"));
    chart1.setOption(chart1_option);
    chart2.setOption(chart2_option);
    line_chart.setOption(line_chart_option);
    bar_chart.setOption(bar_option);

</script>
</asp:Content>
