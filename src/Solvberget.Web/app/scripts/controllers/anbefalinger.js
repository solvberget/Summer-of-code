'use strict';

angular.module('Solvberget.WebApp')
  .controller('AnbefalingerCtrl', function ($scope, $rootScope, lists) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Anbefalinger');

        $scope.items = lists.query();

        $scope.pathFor = function(item){

            var title = encodeURIComponent(item.Name.replace(' ','-').toLowerCase());
            return $rootScope.path('AnbefalingerDetaljerCtrl', {id: item.Id, title: title});
        };
  });
