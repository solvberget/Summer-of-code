'use strict';

angular.module('Solvberget.WebApp')
    .controller('HomeCtrl', function (events, news, $scope, $rootScope, $location) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Sølvberget');

        $scope.events = events.query();
        $scope.news = news.query();

        $scope.search = function(){

            $location.path('sok').search('query', $scope.searchQuery);;
        }
    });