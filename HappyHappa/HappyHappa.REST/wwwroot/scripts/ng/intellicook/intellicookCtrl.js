var app = angular.module("happyhappa");

app.controller("intellicookCtrl", ["$scope", "hhIntelliCookSvc", "hhUserSvc", function ($scope, hhIntelliCookSvc, hhUserSvc) {
  $scope.model = [];
  $scope.hideLoadingIcon = false;
  var init = function () {
    hhUserSvc.get(function (setting) {
      fridgeSecret = setting.FridgeSecret;
      hhIntelliCookSvc.query({id: fridgeSecret}, function (recipes) {
        $scope.model = recipes;
        $scope.hideLoadingIcon = true;
      });
    });
  }

  init();
}]);