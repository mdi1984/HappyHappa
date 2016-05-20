var app = angular.module("happyhappa");

app.controller("settingsCtrl", ["$scope", "hhUserSvc", function ($scope, hhUserSvc) {
  $scope.model = {
    FridgeSecret: ""
  }

  var init = function () {
    hhUserSvc.get(function (setting) {
      $scope.model = setting;
    });
  }

  $scope.submit = function () {
    hhUserSvc.update($scope.model);
  }

  init();
}]);