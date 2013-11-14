'use strict';

var $$config =  {
    apiPrefix : 'http://localhost:39465/',
    appUrlPrefix : 'http://localhost:9000/',
    apiPrefixEscaped : function(){
        return this.apiPrefix.replace(/:(\d+)/,'\\:$1'); // workaround to escape port number : so it doesn't get interpreted as a variable by $resource
    }
}

angular.module('Solvberget.WebApp', ['globalErrors', 'ngCookies', 'ngResource', 'ngRoute', 'ngSanitize'])
    .config(function ($routeProvider) {
        $routeProvider
            .when('/sok', {
                templateUrl: 'views/search.html',
                controller: 'SearchCtrl',
                reloadOnSearch: false
            })
            .when('/min-side', {
                redirectTo: '/minside/favoritter'
            })
            .when('/min-side/favoritter', {
                templateUrl: 'views/my.favorites.html',
                controller: 'MyFavoritesCtrl'
            })
            .when('/min-side/detaljer', {
                templateUrl: 'views/my.details.html',
                controller: 'MyDetailsCtrl'
            })
            .when('/min-side/meldinger', {
                templateUrl: 'views/my.messages.html',
                controller: 'MyMessagesCtrl'
            })
            .when('/min-side/lan', {
                templateUrl: 'views/my.loans.html',
                controller: 'MyLoansCtrl'
            })
            .when('/min-side/gebyrer', {
                templateUrl: 'views/my.fines.html',
                controller: 'MyFinesCtrl'
            })
            .when('/min-side/reservasjoner', {
                templateUrl: 'views/my.reservations.html',
                controller: 'MyReservationsCtrl'
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
            .when('/blogg/:id', {
                templateUrl: 'views/blog.html',
                controller: 'BlogCtrl'
            })
            .when('/kontakt-oss', {
                templateUrl: 'views/kontaktoss.html',
                controller: 'KontaktOssCtrl'
            })
            .when('/nyheter', {
                templateUrl: 'views/nyheter.html',
                controller: 'NewsCtrl'
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
                templateUrl: 'views/media.bok.html',
                controller: 'AudioBookCtrl'
            })
            .when('/noter/:id/:title', {
                templateUrl: 'views/media.sheetmusic.html',
                controller: 'SheetMusicCtrl'
            })
            .when('/spill/:id/:title', {
                templateUrl: 'views/media.game.html',
                controller: 'GameCtrl'
            })
            .when('/journal/:id/:title', {
                templateUrl : 'views/media.journal.html',
                controller: 'OtherMediaCtrl'
            })
            .when('/annet/:id/:title', {
                templateUrl : 'views/media.other.html',
                controller: 'OtherMediaCtrl'
            })
            .when('/arrangementer', {
                templateUrl : 'views/events.html',
                controller: 'EventsCtrl'
            })
            .when('/login', {
                templateUrl : 'views/login.html',
                controller: 'LoginCtrl'
            })
            .otherwise({
                redirectTo: '/nyheter'
            });

    }).run(function($rootScope, $location, $route, $http, $cookies) {

        $$config.username = $cookies.username;
        $$config.password = $cookies.password;

        $rootScope.newMessagesCount = 2;

        $http({method: 'GET', url: '/app.config.json'}).
            success(function(data) {
                console.log("app.config.json loaded", data);

                $$config.apiPrefix = data.apiPrefix;
                $$config.appUrlPrefix = data.appUrlPrefix;

                $rootScope.apiPrefix = $$config.apiPrefix;
            });

        $rootScope.isViewActive = function (viewLocation) {
            return $location.path().indexOf(viewLocation) === 0;
        };

        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        $rootScope.range = function(n) {

            n = Math.round(1*n);

            if(!isNumber(n)) n = 0;

            var array = [];
            for(var i=0; i<n; i++) array.push(i);
            return array;
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

        $rootScope.pathForDocument = function(document){

            var title = encodeURIComponent(document.title.replace(' ','-').toLowerCase());
            var documentPath = $rootScope.path(document.type + 'Ctrl', {id: document.id, title : title});

            if(!documentPath) documentPath = $rootScope.path('OtherMediaCtrl', {id: document.id, title : title});

            return documentPath;
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
