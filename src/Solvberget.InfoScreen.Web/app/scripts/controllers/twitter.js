'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('TwitterCtrl', function ($scope, $rootScope, tweets) {
    $scope.items = tweets.query();
    $scope.interval = 10; // seconds
    
    $rootScope.title = "Tweets om #sølvberget";
  });
