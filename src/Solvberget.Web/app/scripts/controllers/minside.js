'use strict';

angular.module('Solvberget.WebApp')
    .controller('MyFavoritesCtrl', function ($scope, $rootScope, favorites) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');

        $scope.favorites = favorites.get();

    }).controller('MyDetailsCtrl', function ($scope, $rootScope, userInfo) {

        $scope.userInfo = userInfo.get();

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');

    }).controller('MyMessagesCtrl', function ($scope, $rootScope, userInfo) {

        $scope.userInfo = userInfo.get();

        $rootScope.newMessagesCount = 0;

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');

    }).controller('MyLoansCtrl', function ($scope, $rootScope, userInfo) {

        $scope.userInfo = userInfo.get();

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');

    }).controller('MyReservationsCtrl', function ($scope, $rootScope, $filter, userInfo) {

        $scope.getSubtext = function(reservation){

            var toDate= $filter('date');

            var text = "Reservert: " + toDate(reservation.reserved, 'shortDate') + '<br/>';

            if(reservation.readyForPickup) {
                text += "Klar til henting: Ja<br/>Hentefrist: " + toDate(reservation.pickupDeadline, 'shortDate');
            }
            else text += "Klar til henting: Nei";

            return text;
        }

        $scope.userInfo = userInfo.get();

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');


    });
