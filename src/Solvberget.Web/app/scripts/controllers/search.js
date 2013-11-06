'use strict';

angular.module('Solvberget.WebApp')
    .controller('SearchCtrl', function ($scope, $rootScope, $filter, $location, documentSearch) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Søk');

        $scope.selectedType = null;

        $scope.selectType = function(type){
            $scope.selectedType = type;

            if(null == type) {
                delete $scope.resultFilter; // no filter
            }
            else if(type == 'Other') {
                $scope.resultFilter = function(item){
                    return item.type != 'Book' && item.type != 'Film' && item.type != 'AudioBook' && item.type != 'CD' && item.type != 'SheetMusic' && item.type != 'Journal' && item.type != 'Game';
                }
            }
            else $scope.resultFilter = function(item){ return item.type == type};
        }

        $scope.search = function(){

            $location.search('query', $scope.searchQuery);
            $location.replace();

            $scope.isSearching = true;

            $scope.results = documentSearch.query({query : $scope.searchQuery}, function(){

                $scope.allCount = $scope.results.length;
                $scope.bookCount = $scope.results.filter(function(item){ return item.type == 'Book'}).length;
                $scope.filmCount = $scope.results.filter(function(item){ return item.type == 'Film'}).length;
                $scope.audioBookCount = $scope.results.filter(function(item){ return item.type == 'AudioBook'}).length;
                $scope.cdCount = $scope.results.filter(function(item){ return item.type == 'CD'}).length;
                $scope.sheetMusicCount = $scope.results.filter(function(item){ return item.type == 'SheetMusic'}).length;
                $scope.journalCount = $scope.results.filter(function(item){ return item.type == 'Journal'}).length;
                $scope.gameCount = $scope.results.filter(function(item){ return item.type == 'Game'}).length;
                $scope.otherCount = $scope.allCount - $scope.bookCount - $scope.filmCount - $scope.audioBookCount - $scope.cdCount - $scope.sheetMusicCount - $scope.journalCount - $scope.gameCount;

                $scope.isSearching = false;
            });
        }

        // initialize search from query string in url, if present
        $scope.searchQuery = $location.search()['query'];
        if($scope.searchQuery) $scope.search();
    });
