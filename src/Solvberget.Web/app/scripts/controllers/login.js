'use strict';

angular.module('Solvberget.WebApp')
  .controller('LoginCtrl', function ($scope, $rootScope, $routeParams, $location, $http, $cookies) {

        $scope.login = function(){

            $http({
                method: 'POST',
                url: $$config.apiPrefix + '/login',
                data : $.param({username : $scope.username, password: $scope.password}),
                headers: {'Content-Type': 'application/x-www-form-urlencoded'}})
                .success(function(data, status, headers, config) {

                    $$config.username = $cookies.username = $scope.username;
                    $$config.password = $cookies.password = $scope.password;

                    if($routeParams.redirect) {
                        $location.search('redirect', null);
                        $location.path($routeParams.redirect);
                    }
                    else $location.path('/');
                }).error(function(data, status, headers, config) {

                });
        };

  });
