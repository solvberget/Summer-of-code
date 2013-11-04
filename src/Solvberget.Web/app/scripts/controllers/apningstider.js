'use strict';

angular.module('Solvberget.WebApp')
  .controller('ApningstiderCtrl', function ($scope, $rootScope, openingHours) {

        $rootScope.breadcrumb.push('Åpningstider', 'ApningstiderCtrl');
        $scope.items = openingHours.query();
  });
