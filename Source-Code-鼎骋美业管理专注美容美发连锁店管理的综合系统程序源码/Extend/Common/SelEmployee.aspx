<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelEmployee.aspx.cs" Inherits="Extend_Common_SelEmployee" %>
<div class="modal fade" id="employee_modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
    <div class="modal-dialog" role="document">
        <div class="modal-content" style="width: 700px;">
            <div class="modal-header">
                <div class="input-group">
                    <input type="text" class="form-control" placeholder="请输入员工姓名" />
                    <span class="input-group-btn">
                        <button type="button" class="btn btn-info">搜索</button>
                    </span>
                </div>
            </div>
            <div class="modal-body">
                <ul id="employ_ul">
                    <li ng-click="setEmployee(item);" ng-repeat="item in employee.list" class="btn btn-light">
                        <div class="text-center">
                            <img ng-src="{{item.UserFace}}" class="img50" onerror="shownoface(this);" />
                        </div>
                        <div style="font-size: 12px; margin-top: 5px;">
                            <span ng-bind="item.UserName"></span>
                            <span class="pull-right">员工</span>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-info" data-dismiss="modal">关闭窗口</button>
            </div>
        </div>
    </div>
</div>
