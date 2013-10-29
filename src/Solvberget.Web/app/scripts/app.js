'use strict';

angular.module('Solvberget.WebApp', ['globalErrors', 'ngResource'])
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
            .when('/anbefalinger/:id', {
                templateUrl: 'views/anbefalinger.detaljer.html',
                controller:'AnbefalingerDetaljerCtrl'
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
            .when('/bok/:id/:title', {
                templateUrl: 'views/media.bok.html',
                controller: 'BookCtrl'
            })
            .when('/cd/:id/:title', {
                templateUrl: 'views/media.cd.html',
                controller: 'CdCtrl'
            })
            .when('/film/:id/:title', {
                templateUrl: 'views/media.film.html',
                controller: 'FilmCtrl'
            })
            .when('/lydbok/:id/:title', {
                templateUrl: 'views/media.lydbok.html',
                controller: 'LydbokCtrl'
            })
            .when('/notehefte/:id/:title', {
                templateUrl: 'views/media.notehefte.html',
                controller: 'NotehefteCtrl'
            })
            .otherwise({
                redirectTo: '/nyheter'
            });

    }).run(function($rootScope, $location, $route) {

        $rootScope.isViewActive = function (viewLocation) {
            return $location.path().indexOf(viewLocation) === 0;
        };

        $rootScope.range = function(n) {

            if(!n || n === NaN) return 0;
            n = Math.round(1*n);
            return new Array(n);
        };

        $rootScope.path = function(controller, params)
        {
            if(!controller) return undefined;
            // Iterate over all available routes

            for(var path in $route.routes)
            {
                var pathController = $route.routes[path].controller;

                if(pathController == controller) // Route found
                {
                    var result = path;

                    // Construct the path with given parameters in it

                    for(var param in params)
                    {
                        result = result.replace(':' + param, params[param]);
                    }

                    return '#' + result;
                }
            }

            // No such controller in route definitions

            return undefined;
        };

        $rootScope.breadcrumb = {

            crumbs : [],

            last : null,

            push : function(title, ctrl, ctrlParams){

                this.last = {title : title, url: $rootScope.path(ctrl, ctrlParams)};
                this.crumbs.push(this.last);
            },

            pop : function(){
                this.last = this.crumbs.pop();
            },

            clear : function(){
                this.crumbs = [];
                this.last = null;
            }
        }

    });
