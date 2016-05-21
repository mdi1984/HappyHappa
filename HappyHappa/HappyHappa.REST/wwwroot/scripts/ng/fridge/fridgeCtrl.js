var app = angular.module("happyhappa");

app.controller("fridgeCtrl", ["$scope", "$localStorage", "hhItemSvc", function ($scope, $localStorage, hhItemSvc) {
  $scope.model = [];
  var fridgeSecret = {};
  $scope.hideLoadingIcon = false;

  var init = function () {
    fridgeSecret = $localStorage.FridgeSecret;
    hhItemSvc.query({ id: fridgeSecret }, function (items) {
      $scope.hideLoadingIcon = true;
      $scope.model = items;
    });
  }
  
  init();
}]);