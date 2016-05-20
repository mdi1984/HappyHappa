var app = angular.module("happyhappa", ["ui.router", "ngResource"]);

app.config(function ($stateProvider, $urlRouterProvider) {
  $urlRouterProvider.otherwise('/home');

  $stateProvider
      .state('home', {
        url: '/home',
        templateUrl: 'scripts/ng/home/home.html'
      })
      .state('recipes', {
        url: '/recipes',
        templateUrl: 'scripts/ng/recipes/recipes.html'
      })
    .state('intellicook', {
      url: '/intellicook',
      templateUrl: 'scripts/ng/intellicook/intellicook.html'
    })
      .state('fridge', {
        url: '/fridge',
        templateUrl: 'scripts/ng/fridge/fridge.html'
      })
      .state('settings', {
        url: '/settings',
        templateUrl: 'scripts/ng/settings/settings.html',
        controller: 'settingsCtrl',
      })

});