'use strict';

angular.module('Solvberget.WebApp')
    .controller('AnbefalingerDetaljerCtrl', function ($scope, $rootScope, $routeParams, lists) {

        $rootScope.pageTitle = 'ANBEFALINGER';

        $scope.list = lists.get({id : $routeParams.id});

        $scope.pathFor = function(item){

            var title = encodeURIComponent(item.Title.replace(' ','-').toLowerCase());
            return $rootScope.path(item.Type + 'Ctrl', {id: item.Id, title : title});
        };
    });
