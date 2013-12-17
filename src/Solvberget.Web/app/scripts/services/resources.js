angular.module('Solvberget.WebApp')
    .factory('lists',function($resource){
        return $resource($$config.apiPrefixEscaped() + 'lists/:id', { limit: 10});
    })
    .factory('documents',function($resource){
        return $resource($$config.apiPrefixEscaped() + 'documents/:id', { limit: 10});
    })
    .factory('documentRating', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'documents/:id/rating');
    })
    .factory('documentReview', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'documents/:id/review');
    })
    .factory('documentSearch', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'documents/search');
    })
    .factory('blogs', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'blogs/:id');
    })
    .factory('news', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'news');
    })
    .factory('openingHours', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'info/opening-hours');
    })
    .factory('contactDetails', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'info/contact');
    })
    .factory('events', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'events/:id');
    })
    .factory('userInfo', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'user/info');
    })
    .factory('favorites', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'favorites/:documentId', { documentId: '@documentId'}, {
            add: {method:'PUT'},
            remove: {method:'DELETE'},
            get: {method:'GET', isArray:true}
        });
    })
    .factory('notificationCount', function ($resource) {
        return $resource($$config.apiPrefixEscaped() + 'user/notifications/count');
    })
    .factory('reservations', function($resource){
        return $resource($$config.apiPrefixEscaped() + 'reservations/:documentId', { documentId: '@documentId'}, {
            add: {method:'PUT'},
            remove: {method:'DELETE'},
            get: {method:'GET', isArray:true}
        });
    })
    .factory('renewals', function ($resource) {
        return $resource($$config.apiPrefixEscaped() + 'documents/renew/:documentId', { documentId: '@documentId' }, {
            add: { method: 'PUT' }
        });
    })
    .config(function($httpProvider){

        $httpProvider.interceptors.push(function() {
            return {
                'request': function(config) {
                    
                    if($$config.username && $$config.password) config.headers.Authorization = $$config.username +  ':' + $$config.password;

                    return config;
                },
                'responseError': function(response) {

                    if (response.status == 401) {
                        window.location = './#/login?redirect=' + window.location.hash.substring(1);
                    }

                    return response;
                }
            };
        });
    });