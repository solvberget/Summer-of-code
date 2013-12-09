'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('EventsCtrl', function ($scope, events) {
    $scope.items = events;
  });

