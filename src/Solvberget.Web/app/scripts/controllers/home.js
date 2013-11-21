'use strict';

angular.module('Solvberget.WebApp')
    .controller('HomeCtrl', function ($scope, $rootScope, $routeParams, $location, $http, $cookies) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Sølvberget');

    });