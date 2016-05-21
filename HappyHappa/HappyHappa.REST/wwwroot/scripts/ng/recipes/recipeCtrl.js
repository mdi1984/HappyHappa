var app = angular.module("happyhappa");

app.controller("recipeCtrl", ["$scope", "hhRecipeSvc",  function ($scope, hhRecipeSvc) {
  $scope.model = [];

  var init = function () {
    hhRecipeSvc.query(function (recipes) {
      $scope.model = recipes;
    });
  }

  init();
}]);