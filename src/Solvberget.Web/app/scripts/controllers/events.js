'use strict';

angular.module('Solvberget.WebApp')
  .controller('EventsCtrl', function ($scope, $rootScope, $filter, events) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Arrangementer');

        $scope.events = events.query();

        $scope.pathFor = function(item){
            return $rootScope.path('EventDetailsCtrl', {id: item.id});
        };

        $scope.getPriceString = function(item){

            if(item.ticketPrice === 0) return "Gratis";
            else return item.ticketPrice + ",-";
        };

        $scope.getSubText = function(item){

            var dateFilter = $filter('date');
            var subtext = dateFilter(item.start, 'fullDate');

            if(item.start.hour > 0 || item.end.hour > 0) subtext += ' kl.' + $filter('date')(item.start, 'hh:mm');

            subtext += "<br/><span style='font-size:0.8em'>" + item.location + ' | ';
            if(item.ticketPrice == 0) subtext += 'Gratis';
            else subtext += item.ticketPrice + ',-';

            subtext += '</span>';

            return subtext;
        }
  });

