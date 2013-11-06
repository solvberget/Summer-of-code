'use strict';

angular.module('Solvberget.WebApp')
  .controller('EventsCtrl', function ($scope, $rootScope, events) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Arrangementer');

        $scope.events = events.query();

        $scope.pathFor = function(item){
            return $rootScope.path('EventDetailsCtrl', {id: item.id});
        };
  })
    .controller('EventDetailsCtrl', function ($scope, $routeParams, $rootScope, events) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Arrangementer');

        $scope.event = events.get({id : $routeParams.id}, function(){
            $rootScope.breadcrumb.push($scope.event.name, 'EventDetailsCtrl', {id : $scope.event.id});
        });
    });

