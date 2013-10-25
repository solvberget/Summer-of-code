'use strict';

angular.module('Solvberget.WebApp')
  .controller('AnbefalingerCtrl', function ($scope, $rootScope) {

        $rootScope.pageTitle = 'ANBEFALINGER';

        $scope.items = [{id: 1,
            title : "Fredrik's anbefalinger",
            titleUri: 'fredriks-anbefalinger',
            subtext: "Lagt til 24. oktober 2013",
            imageUrl : 'http://placehold.it/60x80'},
            { id:2,
                title : "Bestselgere 2013",
                titleUri:'bestselgere-2013',
                subtext: "Lagt til 23. oktober 2013",
                imageUrl: 'http://placehold.it/60x80' }];

  });
