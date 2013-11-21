'use strict';

angular.module('Solvberget.WebApp')
  .controller('ListsCtrl', function ($scope, $rootScope, lists) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Anbefalinger');

        $scope.items = lists.query();

        $scope.pathFor = function(item){
            return $rootScope.path('ListCtrl', {id: item.id});
        };
  });
