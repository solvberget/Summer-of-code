'use strict';

angular.module('Solvberget.WebApp')
    .controller('ListCtrl', function ($scope, $rootScope, $routeParams, lists) {

        $scope.list = lists.get({id : $routeParams.id}, function(){

            $rootScope.breadcrumb.push($scope.list.name, 'ListCtrl', {id: $scope.list.id});
        });

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Anbefalinger', 'ListsCtrl');
    });
