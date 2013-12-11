'use strict';

angular.module('solvbergetinfoScreenwebApp')
    .factory('news', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'news/', { limit: 10});
      })
    .factory('events', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'events/');
    })
    .factory('slides', function($resource) {
        return function(screenId) { return $resource($$config.apiPrefixEscaped() + 'slides/' + screenId); };
    });