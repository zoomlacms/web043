<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelProduct.aspx.cs" Inherits="Extend_Common_SelProduct" %>
<div class="modal fade" id="product_modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
         <div class="modal-dialog" role="document">
             <div class="modal-content" style="width:700px;">
                <div class="modal-header">
                    <div class="input-group">
                         <input type="text" class="form-control" placeholder="请输入商品名"/>
                         <span class="input-group-btn">
                             <button type="button" class="btn btn-info">搜索</button>
                         </span>
                     </div>
                </div>
                  <div class="modal-body">
                     <ul id="pro_ul">
                         <li ng-click="setProduct(item);" ng-repeat="item in product.list" class="btn btn-light">
                             <span ng-bind="item.Proname" class="pull-left"></span>
                             <span class="pull-right r_red"><i class="fa fa-rmb"></i><span ng-bind="item.LinPrice|number:2"></span></span>
                         </li>
                     </ul>
                 </div>
                 <div class="modal-footer">
                     <button type="button" class="btn btn-info" data-dismiss="modal">关闭窗口</button>
                 </div>
             </div>
         </div>
     </div>