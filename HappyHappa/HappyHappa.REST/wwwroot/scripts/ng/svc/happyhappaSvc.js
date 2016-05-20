var app = angular.module("happyhappa");

app.factory("hhUserSvc", ["$resource", function ($resource) {
  return $resource("/api/user", null, {
    'update': { method: 'PUT' }
  });
}]);