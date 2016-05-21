var app = angular.module("happyhappa");

app.controller("recipeDetailCtrl", ["$scope", "$stateParams", "hhRecipeSvc", function ($scope, $stateParams, hhRecipeSvc) {
  $scope.model = {}

  var id = $stateParams.id;

  var init = function () {
    hhRecipeSvc.get({id: id}, function (recipe) {
      $scope.model = recipe;
    });
  }

  init();
}]);