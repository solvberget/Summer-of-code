'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('InstagramCtrl', function ($scope, $rootScope) {

    $rootScope.title = "Instagram feed";
    
    if($scope.template.slideOptions) $scope.tagName = $scope.template.slideOptions.tagName;
    if (!$scope.tagName) $scope.tagName = 'sølvberget';
});

