'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('EventsCtrl', function ($scope, $rootScope, events) {
    $scope.items = events.query();

    $scope.interval = 10; // seconds

    $rootScope.title = "Arrangement hos Sølvberget";
  });

