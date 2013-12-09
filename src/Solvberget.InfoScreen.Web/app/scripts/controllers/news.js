'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('NewsCtrl', function ($scope, news) {
    $scope.items = news;
  });
