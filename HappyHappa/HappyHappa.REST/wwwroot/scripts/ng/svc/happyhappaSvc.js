var app = angular.module("happyhappa");

app.factory("hhIntelliCookSvc", ["$resource", function ($resource) {
  return $resource("/api/intellicook/:id", { id: '@id' });
}]);

app.factory("hhItemSvc", ["$resource", function ($resource) {
  return $resource("/api/item/:id", { id: '@id'}, {
    'absGet': { method: 'GET', isArray: true, params: {abs: '@abs'}}
  });
}]);

app.factory("hhRecipeSvc", ["$resource", function ($resource) {
  return $resource("/api/recipe/:id", { id: '@id' });
}]);