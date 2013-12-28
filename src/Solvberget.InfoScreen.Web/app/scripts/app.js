'use strict';

var $$config =  {
    apiPrefix: 'http://solvbergetapp.cloudapp.net/api/',
    appUrlPrefix : 'http://localhost:9000/',
    apiPrefixEscaped : function(){
        return this.apiPrefix.replace(/:(\d+)/,'\\:$1'); // workaround to escape port number : so it doesn't get interpreted as a variable by $resource
      }
  };

angular.module('solvbergetinfoScreenwebApp', [
  'ngCookies',
  'ngResource',
  'ngSanitize',
  'ngRoute'
])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .when('/:id', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .otherwise({
        redirectTo: '/'
      });
  }).run(function () {
    console.log($$config.apiPrefix);
  });
    
