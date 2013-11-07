'use strict';

angular.module('Solvberget.WebApp')
  .controller('LoginCtrl', function ($scope, $rootScope, $routeParams, $location, login) {

        $rootScope.pageTitle = 'MIN SIDE';

        $scope.login = function(){

            login.get({username : $scope.username, password : $scope.password}, function(){

                if($routeParams.redirect) {
                    $location.search('redirect', null);
                    $location.path($routeParams.redirect);
                }
            });

            $$config.username = $scope.username;
            $$config.password = $scope.password;
        };

  });
