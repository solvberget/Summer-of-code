'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('NewsCtrl', function ($scope, $rootScope, news) {
    $scope.items = news.query();
    $rootScope.title = "Nyheter fra Sølvberget";
  });
