'use strict';

angular.module('Solvberget.WebApp')
  .controller('OpeningHoursCtrl', function ($scope, $rootScope, openingHours) {

        $rootScope.breadcrumb.push('Åpningstider', 'OpeningHoursCtrl');
        $scope.items = openingHours.query();
  });
