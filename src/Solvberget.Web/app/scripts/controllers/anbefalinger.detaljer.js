'use strict';

angular.module('Solvberget.WebApp')
    .controller('AnbefalingerDetaljerCtrl', function ($scope, $rootScope, $routeParams, mediaItemListRepository) {

        $rootScope.pageTitle = 'ANBEFALINGER';

        $scope.list = mediaItemListRepository.get($routeParams.id);

        $scope.pathFor = function(item){

            var title = encodeURIComponent(item.title.replace(' ','-').toLowerCase());

            return $rootScope.path(item.type + 'Ctrl', {id: item.id, title : title});
        };
    });
