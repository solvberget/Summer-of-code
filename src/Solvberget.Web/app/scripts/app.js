'use strict';

angular.module('Solvberget.WebApp', [])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/min-side', {
                templateUrl: 'views/minside.html',
                controller: 'MinSideCtrl'
            })
            .when('/anbefalinger', {
                templateUrl: 'views/anbefalinger.html',
                controller: ''
            })
            .when('/apningstider', {
                templateUrl: 'views/apningstider.html',
                controller: ''
            })
            .when('/blogger', {
                templateUrl: 'views/blogger.html',
                controller: ''
            })
            .when('/kontakt-oss', {
                templateUrl: 'views/kontaktoss.html',
                controller: ''
            })
            .when('/nyheter', {
                templateUrl: 'views/nyheter.html',
                controller: ''
            })
            .otherwise({
                redirectTo: '/nyheter'
            });

    }).run(function($rootScope, $location) {

        $rootScope.isViewActive = function (viewLocation) {
            return viewLocation === $location.path();
        };

    });
