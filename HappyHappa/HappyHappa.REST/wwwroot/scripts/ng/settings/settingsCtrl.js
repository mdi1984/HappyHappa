var app = angular.module("happyhappa");

app.controller("settingsCtrl", ["$scope", "$localStorage", function ($scope, $localStorage) {
  $scope.model = {
    FridgeSecret: ""
  }

  var init = function () {
    $scope.model.FridgeSecret = $localStorage.FridgeSecret;
    //hhUserSvc.get(function (setting) {
    //  $scope.model = setting;
    //});
  }

  $scope.submit = function () {
    //hhUserSvc.update($scope.model);
    $localStorage.FridgeSecret = $scope.model.FridgeSecret;
  }

  init();
}]);