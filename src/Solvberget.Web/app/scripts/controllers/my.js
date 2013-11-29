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

    }).controller('MyLoansCtrl', function ($scope, $rootScope, $filter, userInfo) {

        $scope.userInfo = userInfo.get();

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');

        var dateFilter = $filter('date');

        $scope.subtextFor = function(loan) {

            return dateFilter(loan.loanDate, 'mediumDate') + " - " + dateFilter(loan.dueDate, 'mediumDate') + ", " + loan.subLibrary;
        };

    }).controller('MyFinesCtrl', function ($scope, $rootScope, userInfo) {

        $scope.userInfo = userInfo.get();

        $scope.getFineTitle = function(fine){

            if(fine.document) return fine.document.title;
            return "Gebyr";
        }

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');


    }).controller('MyReservationsCtrl', function ($scope, $rootScope, $filter, userInfo) {

        $scope.getSubtext = function(reservation){

            var toDate= $filter('date');

            var text = "Reservert: " + toDate(reservation.reserved, 'shortDate') + '<br/>';

            if(reservation.readyForPickup) {
                text += "<strong class='text-success'>Klar til henting nå. Hentefrist: " + toDate(reservation.pickupDeadline, 'shortDate') + "</strong>";
            }
            else text += "Ikke klar til henting.";

            return text;
        }

        $scope.userInfo = userInfo.get();

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Min side');


    });
