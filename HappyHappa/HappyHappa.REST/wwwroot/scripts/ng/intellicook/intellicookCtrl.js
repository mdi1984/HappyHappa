var app = angular.module("happyhappa");

app.controller("intellicookCtrl", ["$scope", "hhIntelliCookSvc", "$localStorage", function ($scope, hhIntelliCookSvc, $localStorage) {
  $scope.model = [];
  $scope.hideLoadingIcon = false;
  var init = function () {
    fridgeSecret = $localStorage.FridgeSecret;
    hhIntelliCookSvc.query({id: fridgeSecret}, function (recipes) {
      $scope.model = recipes;
      $scope.hideLoadingIcon = true;
    });
  }

  init();
}]);