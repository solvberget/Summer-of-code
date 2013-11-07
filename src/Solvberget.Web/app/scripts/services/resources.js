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
    })
    .factory('documentReview', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'documents/:id/review');
    })
    .factory('documentSearch', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'documents/search');
    })
    .factory('blogs', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'blogs/:id');
    })
    .factory('news', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'news');
    })
    .factory('openingHours', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'info/opening-hours');
    })
    .factory('contactDetails', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'info/contact');
    })
    .factory('events', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'events/:id');
    })
    .factory('favorites', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'favorites/:documentId');
    })
    .factory('login', function($resource){

        var apiPrefix = $$config.apiPrefix.replace(/:(\d+)/,'\\:$1');
        return $resource(apiPrefix + 'login');
    })
    .config(function($httpProvider){

        $httpProvider.interceptors.push(function() {
            return {
                'request': function(config) {

                    if($$config.username && $$config.password) config.headers.Authorization = $$config.username +  ':' + $$config.password;

                    return config;
                },
                'responseError': function(response) {

                    if(response.status == 401){
                        window.location = './#/login?redirect=' + window.location.hash.substring(1);
                    }

                    return response;
                }
            };
        });
    });