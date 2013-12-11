'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('NewsCtrl', function ($scope, $rootScope, news) {
    $scope.items = news;
    $rootScope.title = "Nyheter fra Sølvberget";
  });
