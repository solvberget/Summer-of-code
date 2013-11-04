'use strict';

angular.module('Solvberget.WebApp')
  .controller('KontaktOssCtrl', function ($scope, $rootScope, contactDetails) {


        $rootScope.breadcrumb.push('Kontakt oss', 'KontaktOssCtrl');
        $scope.contacts = contactDetails.query();

  });
