'use strict';

angular.module('solvbergetinfoScreenwebApp')
    .factory('news', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'news/', { limit: 10 });
    })
    .factory('tweets', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'infoscreen/tweets/%23s%C3%B8lvberget', { limit: 10 });
    })
    .factory('events', function($resource) {
        return $resource($$config.apiPrefixEscaped() + 'events/');
    })
    .factory('slides', function($resource) {
        return function(screenId) { return $resource($$config.apiPrefixEscaped() + 'slides/' + screenId); };
    }).config(function($httpProvider) {

        $httpProvider.interceptors.push(function() {
            return {
                'responseError': function(response) {

                    console.error("En feil oppstod (status " + response.status + "). " + response.data);

                    return response;
                }
            };
        });

    }).directive('viewLoaded', function() {

        var link = function($scope, element, attrs) {

            var fn = attrs.viewLoaded;
            var viewLoaded = window[fn];
            if (viewLoaded) viewLoaded($scope, element);
        };

        return { link: link };
    })
    .filter('firstParagraph', function() {
        return function(input) {

            var i = input.indexOf("</p>");

            if (i < 0) return input;
            else return input.substring(0, i) + "</p>";
        };
    }).filter('characters', function () {
        return function (input, chars, breakOnWord) {
            if (isNaN(chars)) return input;
            if (chars <= 0) return '';
            if (input && input.length >= chars) {
                input = input.substring(0, chars);

                if (!breakOnWord) {
                    var lastspace = input.lastIndexOf(' ');
                    //get last space
                    if (lastspace !== -1) {
                        input = input.substr(0, lastspace);
                    }
                } else {
                    while (input.charAt(input.length - 1) == ' ') {
                        input = input.substr(0, input.length - 1);
                    }
                }
                return input + '...';
            }
            return input;
        };
    })
    .filter('words', function () {
        return function (input, words) {
            if (isNaN(words)) return input;
            if (words <= 0) return '';
            if (input) {
                var inputWords = input.split(/\s+/);
                if (inputWords.length > words) {
                    input = inputWords.slice(0, words).join(' ') + '...';
                }
            }
            return input;
        };
    });