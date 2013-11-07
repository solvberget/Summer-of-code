'use strict';

angular.module('Solvberget.WebApp')
  .controller('MinSideCtrl', function ($scope, $rootScope) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side', 'MinSideCtrl');

  }).controller('MyFavoritesCtrl', function ($scope, $rootScope, favorites) {

        //$rootScope.breadcrumb.clear();
        //$rootScope.breadcrumb.push('Min side', 'MinSideCtrl');
        //$rootScope.breadcrumb.push('Favoritter', 'MyFavoritesCtrl');

        $scope.favorites = favorites.get();

    });
