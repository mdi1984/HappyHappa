var app = angular.module("happyhappa");

app.controller("recipeDetailCtrl", ["$http", "$state", "$scope", "$stateParams", "hhRecipeSvc", "hhItemSvc", "hhUserSvc", function ($http, $state, $scope, $stateParams, hhRecipeSvc, hhItemSvc, hhUserSvc) {
  $scope.model = {}
  $scope.items = []
  $scope.enoughIngredients = true;
  $scope.id = $stateParams.id;
  $scope.hideLoadingIcon = false;

  var fridgeSecret;

  var init = function () {
    hhRecipeSvc.get({ id: $scope.id }, function (recipe) {
      $scope.model = recipe;
    });
    hhUserSvc.get(function (setting) {
      fridgeSecret = setting.FridgeSecret;
      hhItemSvc.absGet({ id: setting.FridgeSecret, abs: true }, function (items) {
        for (var i = 0; i < items.length; ++i) {
          var item = items[i];
          $scope.items[item.Name.toLowerCase()] = item.Products[0].Amount;
        }
        $scope.enoughIngredients = true;
        $scope.hideLoadingIcon = true;
      });
    })
    
  }

  $scope.isEnough = function (ingredient) {
    var itemAmount = $scope.items[ingredient.Name.toLowerCase()];
    if (itemAmount) {
      return itemAmount >= ingredient.Amount;
    }

    $scope.enoughIngredients = false;
    return false;
  }

  $scope.difference = function (ingredient) {
    var currentAmount = $scope.items[ingredient.Name.toLowerCase()];
    if (currentAmount) {
      return ingredient.Amount - currentAmount;
    }
    return ingredient.Amount;
  }

  $scope.cook = function () {
    var body = [];
    for (var i = 0;  i < $scope.model.Ingredients.length; ++i) {
      var ingredient = $scope.model.Ingredients[i];
      body[i] = {
        "FridgeId": fridgeSecret,
        "ItemName": ingredient.Name,
        "Amount": ingredient.Amount
      };
      var config = {
        method: "DELETE",
        url: "/api/item",
        data: body,
        headers: {"Content-Type": "application/json;charset=utf-8"}
      };
      $http(config).then(function () {
        $state.reload();
      });
    }
  }

  init();
}]);