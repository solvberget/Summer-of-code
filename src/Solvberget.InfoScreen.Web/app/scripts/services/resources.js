'use strict';

angular.module('solvbergetinfoScreenwebApp')
    .factory('news', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'news/', { limit: 10}).query();
      })
    .factory('events', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'events/').query();
    });