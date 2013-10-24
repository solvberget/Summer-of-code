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
                controller: 'AnbefalingerCtrl'
            })
            .when('/apningstider', {
                templateUrl: 'views/apningstider.html',
                controller: 'ApningstiderCtrl'
            })
            .when('/blogger', {
                templateUrl: 'views/blogger.html',
                controller: 'BloggerCtrl'
            })
            .when('/kontakt-oss', {
                templateUrl: 'views/kontaktoss.html',
                controller: 'KontaktOssCtrl'
            })
            .when('/nyheter', {
                templateUrl: 'views/nyheter.html',
                controller: 'NyheterCtrl'
            })
            .otherwise({
                redirectTo: '/nyheter'
            });

    }).run(function($rootScope, $location) {

        $rootScope.isViewActive = function (viewLocation) {
            return viewLocation === $location.path();
        };

        $rootScope.pageTitle = 'SØLVBERGET';

    });
