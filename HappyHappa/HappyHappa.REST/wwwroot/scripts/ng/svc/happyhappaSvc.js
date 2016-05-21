var app = angular.module("happyhappa");

app.factory("hhItemSvc", ["$resource", function ($resource) {
  return $resource("/api/item/:id", { id: '@id' });
}]);

app.factory("hhUserSvc", ["$resource", function ($resource) {
  return $resource("/api/user", null, {
    'update': { method: 'PUT' }
  });
}]);

app.factory("hhRecipeSvc", ["$resource", function ($resource) {
  return $resource("/api/recipe/:id", { id: '@id' });
}]);