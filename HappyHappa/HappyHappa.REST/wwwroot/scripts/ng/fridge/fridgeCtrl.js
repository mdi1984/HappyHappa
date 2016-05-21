var app = angular.module("happyhappa");

app.controller("fridgeCtrl", ["$scope", "hhUserSvc", "hhItemSvc", function ($scope, hhUserSvc, hhItemSvc) {
  $scope.model = [];
  var fridgeSecret = {};
  $scope.hideLoadingIcon = false;

  var init = function () {
    hhUserSvc.get(function(setting) {
      fridgeSecret = setting.FridgeSecret;
      hhItemSvc.query({ id: fridgeSecret }, function (items) {
        $scope.hideLoadingIcon = true;
        $scope.model = items;
      });
    });
  }
  
  init();
}]);