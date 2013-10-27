angular.module('Solvberget.WebApp').
    factory('lists',function($resource){
        return $resource('http://localhost:port/v2/lists', { port: ':39465', limit: 10}, {
            'query':  {method:'GET', isArray:true}
        });
    });