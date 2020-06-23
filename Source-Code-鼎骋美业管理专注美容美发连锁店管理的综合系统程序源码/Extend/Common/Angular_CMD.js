app.service("$api", function ($http) {
    var self = this;
    self.post = function (action, postData, cb) {
        var config = {
            headers: {
                'X-Requested-With': "XMLHttpRequest",
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            transformRequest: function (data) { return $.param(data); }
        };
        $http.post("/Extend/Common/API.ashx?action=" + action, postData, config).success(cb);
    }
})
.directive("selEmployee", function ($api) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/Extend/Common/SelEmployee.aspx',
            link: function ($scope, $element, $attr) {
                $scope.employee = { list: [] };
                //选择了用户
                $scope.setEmployee = function (item) {
                    $scope.$emit("set_employee", item);
                }
                $api.post("sel_employee", {}, function (data) {
                    var retMod = APIResult.getModel(data);
                    if (!APIResult.isok(retMod)) { alert(retMod.retmsg); return; }
                    else
                    {
                        $scope.employee.list = retMod.result;
                    }
                });
            }
        }
    })
.directive("selProduct", function ($api) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/Extend/Common/SelProduct.aspx',
            link: function ($scope, $element, $attr) {
                $scope.product = {list:[]};
                $scope.setProduct = function (item) {
                    $scope.$emit("set_product", item);
                }
                $api.post("sel_product", {}, function (data) {
                    var retMod = APIResult.getModel(data);
                    if (!APIResult.isok(retMod)) { alert(retMod.retmsg); return; }
                    else
                    {
                        $scope.product.list = retMod.result;
                    }
                });
            }
        }
    })