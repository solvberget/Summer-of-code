'use strict';

angular.module('Solvberget.WebApp')
    .controller('NewsCtrl', function ($scope, $routeParams, $rootScope, news) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Nyheter', 'NewsCtrl');

        $scope.items = news.query();

        $scope.toDate = function(dateStr){
            return new Date(dateStr).toLocaleString();
        }
    });
