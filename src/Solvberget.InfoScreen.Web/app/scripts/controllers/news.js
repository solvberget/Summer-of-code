'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('NewsCtrl', function ($scope, $rootScope, news) {
    $scope.items = news.query();
    $scope.interval = 10; // seconds
    
    $rootScope.title = "Nyheter fra Sølvberget";
  });
