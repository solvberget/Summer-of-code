angular.module('Solvberget.WebApp')
    .factory('lists',function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1'); // workaround to escape port number : so it doesn't get interpreted as a variable by $resource
        return $resource(apiPrefix + 'lists/:id', { limit: 10});
    })
    .factory('documents',function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'documents/:id', { limit: 10});
    })
    .factory('documentRating', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'documents/:id/rating');
    });
