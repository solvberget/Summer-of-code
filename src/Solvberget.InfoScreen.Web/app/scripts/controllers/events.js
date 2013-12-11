'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('EventsCtrl', function ($scope, $rootScope, events) {
    $scope.items = events;
    $rootScope.title = "Arrangement hos Sølvberget";
  });

