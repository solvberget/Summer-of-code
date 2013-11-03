'use strict';

angular.module('Solvberget.WebApp')
    .controller('SearchCtrl', function ($scope, $rootScope, documentSearch) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Søk');

        $scope.search = function(){

            console.log("Searching for " + $scope.searchQuery);
            $scope.isSearching = true;

            $scope.results = documentSearch.query({query : $scope.searchQuery}, function(){

                $scope.isSearching = false;
                console.log("search results fetched", $scope.results);
            });
        }

        $scope.pathFor = function(item){
            var title = encodeURIComponent(item.title.replace(' ','-').toLowerCase());
            return $rootScope.path(item.type + 'Ctrl', {id: item.id, title : title});
        };

    });
