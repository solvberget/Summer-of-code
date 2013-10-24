'use strict';

angular.module('Solvberget.WebApp')
  .controller('AnbefalingerCtrl', function ($scope, $rootScope) {

        $rootScope.pageTitle = 'ANBEFALINGER';

        $scope.items = [{id: 1, title : "Item #1", imageUrl : 'http://placehold.it/60x80'},
            { id:2, title : "Item #2", imageUrl: 'http://placehold.it/60x80' }];

  });
