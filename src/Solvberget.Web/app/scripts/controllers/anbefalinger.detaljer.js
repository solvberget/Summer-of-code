'use strict';

angular.module('Solvberget.WebApp')
    .controller('AnbefalingerDetaljerCtrl', function ($scope, $rootScope, $routeParams, lists) {

        $scope.list = lists.get({id : $routeParams.id}, function(){

            $rootScope.breadcrumb.push($scope.list.name, 'AnbefalingerDetaljerCtrl', {id: $scope.list.id});
        });

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Anbefalinger', 'AnbefalingerCtrl');

        $scope.pathFor = function(item){
            var title = encodeURIComponent(item.title.replace(' ','-').toLowerCase());
            return $rootScope.path(item.type + 'Ctrl', {id: item.id, title : title});
        };
    });
