angular.module('Solvberget.WebApp')
    .factory('lists',function($resource){
        return $resource('http://localhost:port/v2/lists/:id', { port: ':39465', limit: 10});
    })
    .factory('documents',function($resource){
        return $resource('http://localhost:port/v2/documents/:id', { port: ':39465', limit: 10});
    });