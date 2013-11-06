'use strict';

angular.module('Solvberget.WebApp')
  .controller('MinSideCtrl', function ($scope, $rootScope, login) {

        $rootScope.pageTitle = 'MIN SIDE';

        $scope.login = function(){

            login.get({username : $scope.username, password : $scope.password});

            $$config.username = $scope.username;
            $$config.password = $scope.password;
        };

  });
