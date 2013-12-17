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
    }).config(function ($httpProvider) {
        
        $httpProvider.interceptors.push(function () {
            return {
                'responseError': function (response) {

                    alert("En feil oppstod (status " + response.status + "). " + response.data);

                    return response;
                }
            };
        });
        
    }).directive('viewLoaded', function () {

    var link = function ($scope, element, attrs) {

        var fn = attrs.viewLoaded;
        var viewLoaded = window[fn];
        if (viewLoaded) viewLoaded($scope, element);
    };

        return { link: link };
    });