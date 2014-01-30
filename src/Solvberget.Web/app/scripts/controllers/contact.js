'use strict';

angular.module('Solvberget.WebApp')
  .controller('ContactCtrl', function ($scope, $rootScope, contactDetails) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Kontakt oss', 'ContactCtrl');
        $scope.contacts = contactDetails.query();

  });
