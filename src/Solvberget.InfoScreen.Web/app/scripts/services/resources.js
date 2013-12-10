'use strict';

angular.module('solvbergetinfoScreenwebApp')
    .factory('news', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'news/', { limit: 10}).query();
      })
    .factory('events', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'events/').query();
    })
    .factory('slides', function($resource) {
        return function(screenId) { return $resource($$config.apiPrefixEscaped() + 'slides/' + screenId); };
    });