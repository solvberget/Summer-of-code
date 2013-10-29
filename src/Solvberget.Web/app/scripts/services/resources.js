angular.module('Solvberget.WebApp')
    .factory('lists',function($resource){
        return $resource('http://localhost:port/v2/lists/:id', { port: ':39465', limit: 10});
    })
    .factory('documents',function($resource){
        return $resource('http://localhost:port/v2/documents/:id', { port: ':39465', limit: 10},{
            'rating' : { method:'GET', url: 'http://localhost:port/v2/documents/:id/rating'}
        });
    })
    .factory('documentRating', function($resource){
        return $resource('http://localhost:port/v2/documents/:id/rating', { port: ':39465'});
    });
