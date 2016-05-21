var app = angular.module("happyhappa");

app.controller("recipeCtrl", ["$scope", "hhRecipeSvc",  function ($scope, hhRecipeSvc) {
  $scope.model = [];
  $scope.hideLoadingIcon = false;

  var init = function () {
    hhRecipeSvc.query(function (recipes) {
      $scope.hideLoadingIcon = true;
      $scope.model = recipes;
    });
  }

  init();
}]);